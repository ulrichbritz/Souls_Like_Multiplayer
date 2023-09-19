using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace UB
{
    public class CharacterAnimationManager : MonoBehaviour
    {
        [Header("Scripts")]
        CharacterManager characterManager;

        [Header("Components")]
        Animator anim;
        [HideInInspector]
        public Animator Anim => anim;

        [Header("NetworkValues")]
        int vertical;
        int horizontal;

        protected virtual void Awake()
        {
            //scripts
            characterManager = GetComponent<CharacterManager>();

            //components
            anim = GetComponent<Animator>();

            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");

        }

        public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue, bool isSprinting)
        {
            float horizontalAmount = horizontalValue;
            float verticalAmount = verticalValue;

            if (isSprinting)
            {
                verticalAmount = 2;
            }

            characterManager.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
            characterManager.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
        }

        public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false)
        {
            characterManager.applyRootMotion = applyRootMotion;
            characterManager.animator.CrossFade(targetAnimation, 0.2f);

            characterManager.isPerformingAction = isPerformingAction;
            characterManager.canRotate = canRotate;
            characterManager.canMove = canMove;

            //tell server/host we played animation and play animation for everybody present            
            characterManager.characterNetworkManager.NotifyTheServerOfActionAnimationServerRPC(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }

        public virtual void PlayTargetAttackActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false)
        {
            //keep track of last attack performed
            //keep track of current attack type (light, heavy etc)
            //update animation set to current weapons anims
            //decide if our attack can be parried
            //tell the network our "is attacking" flag is active (for counter damage etc)

            characterManager.applyRootMotion = applyRootMotion;
            characterManager.animator.CrossFade(targetAnimation, 0.2f);

            characterManager.isPerformingAction = isPerformingAction;
            characterManager.canRotate = canRotate;
            characterManager.canMove = canMove;

            //tell server/host we played animation and play animation for everybody present            
            characterManager.characterNetworkManager.NotifyTheServerOfAttackActionAnimationServerRPC(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }
    }
}

