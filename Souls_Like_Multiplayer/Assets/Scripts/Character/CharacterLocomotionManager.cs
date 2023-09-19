using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UB
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        [Header("Scripts")]
        protected CharacterManager characterManager;
        [HideInInspector] public CharacterStats characterStats;

        [Header("Ground Check, Falling and Jumping")]
        [SerializeField] protected float gravityForce = -5.55f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float groundCheckSphereRaduis = 0.5f;
        [SerializeField] protected Vector3 yVelocity; //force that pulls our character up or down (jump or fall)
        [SerializeField] protected float groundedYVelocity = -20f; //force at which char is sticking to ground while grounded
        [SerializeField] protected float fallStartYVelocity = -5f;
        protected bool fallingVelocityHasBeenSet = false;
        protected float inAirTimer = 0;

        [Header("Movement")]
        [SerializeField] protected float moveSpeed = 4f;

        protected virtual void Awake()
        {
            characterStats = GetComponent<CharacterStats>();
            characterManager = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {
            HandleGroundCheck();
        }

        public void HandleGroundCheck()
        {
            characterManager.isGrounded = Physics.CheckSphere(characterManager.transform.position, groundCheckSphereRaduis, groundLayer);

            if (characterManager.isGrounded)
            {
                //if not attempting to jump or move up run this
                if (yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                //if not jumping and falling velocity not set yet
                if (!characterManager.characterNetworkManager.isJumping.Value && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }

                inAirTimer += Time.deltaTime;
                characterManager.animator.SetFloat("inAirTimer", inAirTimer);

                yVelocity.y += gravityForce * Time.deltaTime;               
            }

            characterManager.characterController.Move(yVelocity * Time.deltaTime);

            //always a down force 
            // if (!characterManager.isInBattle)                         //fix because we want the enemy to fall down holes if blown back
            //  characterManager.characterController.Move(yVelocity * Time.deltaTime);
        }

        public void ApplyjumpingVelocity()
        {
            //apply an upward velocity depending on forces e.g gravity
            yVelocity.y = Mathf.Sqrt(characterManager.characterNetworkManager.jumpHeight.Value * -2 * gravityForce);
        }

        protected void OnDrawGizmosSelected()
        {
           // Gizmos.DrawSphere(transform.position, groundCheckSphereRaduis);
        }

    }
}

