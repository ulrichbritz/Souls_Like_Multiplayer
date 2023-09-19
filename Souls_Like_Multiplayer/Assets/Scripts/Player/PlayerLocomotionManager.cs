using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        [Header("Scripts")]
        private PlayerInputManager inputHandler;
        private PlayerManager playerManager;

        [Header("Roaming Movement")]
        //settings
        [SerializeField] float rotationSpeed = 0.25f;

        [Header("Grounded Movement")]
        Vector3 targetRotationDirection;
        public float verticalMovement;
        public float horizontalMovement;
        public float moveAmount;
        private Vector3 moveDirection;
        [SerializeField] int sprintingStaminaCost = 2;

        [Header("Jump")]
        [SerializeField] private float jumpStaminaCost = 4f;
        private Vector3 jumpDirection;

        [Header("Dodge")]
        [SerializeField] private float backStepStaminaCost = 2f;
        [SerializeField] private float rollStaminaCost = 3f;
        private Vector3 rollDirection;


        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            inputHandler = PlayerInputManager.instance;
            playerManager = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();

            //network animation information
            if (playerManager.IsOwner)
            {
                playerManager.characterNetworkManager.verticalMovementParameter.Value = verticalMovement;
                playerManager.characterNetworkManager.animatorHorizontalParameter.Value = horizontalMovement;
                playerManager.characterNetworkManager.networkMoveAmountParameter.Value = moveAmount;
            }
            else
            {
                verticalMovement = playerManager.characterNetworkManager.verticalMovementParameter.Value;
                horizontalMovement = playerManager.characterNetworkManager.animatorHorizontalParameter.Value;
                moveAmount = playerManager.characterNetworkManager.networkMoveAmountParameter.Value;

                //if not locked on
                playerManager.playerAnimationManager.UpdateAnimatorMovementParameters(0, moveAmount, playerManager.playerNetworkManager.isSprinting.Value);

                //if locked on, pass horizontal and vertical
            }
        }

        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleJumpingMovement();
            HandleRotation();
            HandleFreeFallMovement();
        }

        private void GetMovementInputs()
        {
            verticalMovement = inputHandler.VerticalInput;
            horizontalMovement = inputHandler.HorizontalInput;
            moveAmount = inputHandler.moveAmount;

            //clamp movements
        }

        public void HandleGroundedMovement()
        {
            if (playerManager.canMove == false)
                return;

            GetMovementInputs();

            moveDirection = CameraController.instance.transform.forward * verticalMovement;
            moveDirection = moveDirection + CameraController.instance.transform.right * horizontalMovement;

            moveDirection.Normalize();
            moveDirection.y = 0;

            if (playerManager.playerNetworkManager.isSprinting.Value)
            {
                playerManager.characterController.Move(moveDirection * (moveSpeed * 1.5f) * Time.deltaTime);
            }
            else
            {
                if (inputHandler.moveAmount > 0.5f)
                {
                    //move at running speed
                    playerManager.characterController.Move(moveDirection * (moveSpeed) * Time.deltaTime);
                }
                else if (inputHandler.moveAmount <= 0.5f)
                {
                    //walking speed
                    playerManager.characterController.Move(moveDirection * (moveSpeed / 2) * Time.deltaTime);
                }
            }

            //if locked on pass horizontal 
        }

        private void HandleJumpingMovement()
        {
            if (playerManager.characterNetworkManager.isJumping.Value == true)
            {
                playerManager.characterController.Move(jumpDirection * (moveSpeed) * Time.deltaTime);
            }
        }

        private void HandleFreeFallMovement()
        {
            if (!playerManager.isGrounded)
            {
                Vector3 freeFallDirection;
                freeFallDirection = CameraController.instance.transform.forward * inputHandler.VerticalInput;
                freeFallDirection += CameraController.instance.transform.right * inputHandler.HorizontalInput;
                freeFallDirection.y = 0;

                playerManager.characterController.Move(freeFallDirection * (moveSpeed / 2) * Time.deltaTime);
            }
        }

        private void HandleRotation()
        {
           // if (playerManager.canRotate == false)
           //    return;

            targetRotationDirection = Vector3.zero;
            targetRotationDirection = CameraController.instance.camObj.transform.forward * verticalMovement;
            targetRotationDirection = targetRotationDirection + (CameraController.instance.camObj.transform.right * horizontalMovement);
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;

            if (targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }

            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }

        public void HandleSprinting()
        {
            if (playerManager.isPerformingAction)
            {
                playerManager.playerNetworkManager.isSprinting.Value = false;
            }

            //if out of stamina set sprinting to false
            if (playerManager.playerNetworkManager.currentStamina.Value <= 0)
            {
                playerManager.playerNetworkManager.isSprinting.Value = false;
                return;
            }

            //if moving set sprinting to true
            if (moveAmount >= 0.5f)
            {
                playerManager.playerNetworkManager.isSprinting.Value = true;
            }
            else
            {
                //if stationary set sprinting to false
                playerManager.playerNetworkManager.isSprinting.Value = false;
            }

            if (playerManager.playerNetworkManager.isSprinting.Value)
            {
                playerManager.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
            }
        }

        public void AttemptToPerformJump()
        {
            if (playerManager.isPerformingAction)
                return;

            //stamina check
            if (playerManager.playerNetworkManager.currentStamina.Value <= 0)
                return;

            if (playerManager.characterNetworkManager.isJumping.Value)
                return;

            if (!playerManager.isGrounded)
                return;

            //if using 2 handed use 2 handed jump anim

            //if using one handed weapon use one handed jump anim
            playerManager.animationManager.PlayTargetActionAnimation("Jump_Start_01", false, false);

            playerManager.characterNetworkManager.isJumping.Value = true;

            //minus stamina cost
            playerManager.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;

            jumpDirection = CameraController.instance.transform.forward * inputHandler.VerticalInput;
            jumpDirection += CameraController.instance.transform.right * inputHandler.HorizontalInput;

            jumpDirection.y = 0;

            if (jumpDirection != Vector3.zero)
            {
                //jump distance based on speed
                if (playerManager.playerNetworkManager.isSprinting.Value)
                {
                    jumpDirection *= 1;
                }
                else if (inputHandler.moveAmount > 0.5f)
                {
                    jumpDirection *= 0.5f;
                }
                else if (inputHandler.moveAmount <= 0.5)
                {
                    jumpDirection *= 0.25f;
                }
            }
        }

        public void AttemptToPerformDodge()
        {
            if (playerManager.isPerformingAction)
                return;

            //stamina check
            if (playerManager.playerNetworkManager.currentStamina.Value <= 0)
                return;

            if (playerManager.characterNetworkManager.isJumping.Value)
                return;

            if (!playerManager.isGrounded)
                return;

            if (moveAmount > 0)
            {
                //perform roll
                rollDirection = CameraController.instance.camObj.transform.forward * PlayerInputManager.instance.VerticalInput;
                rollDirection += CameraController.instance.camObj.transform.right * PlayerInputManager.instance.HorizontalInput;
                rollDirection.y = 0;
                rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                playerManager.transform.rotation = playerRotation;

                playerManager.animationManager.PlayTargetActionAnimation("Roll_Forward_01", true, true, false, false);

                playerManager.characterController.Move(rollDirection * 8f * Time.deltaTime);

                //minus stamina cost
                playerManager.playerNetworkManager.currentStamina.Value -= rollStaminaCost;
            }
            else
            {
                //perform a backstep
                playerManager.animationManager.PlayTargetActionAnimation("Back_Step_01", true, true, false, false);

                //playerManager.characterController.Move((playerManager.transform.forward * -1) * 2f);

                //minus stamina cost
                playerManager.playerNetworkManager.currentStamina.Value -= backStepStaminaCost;
            }
            
        }

    }
}

