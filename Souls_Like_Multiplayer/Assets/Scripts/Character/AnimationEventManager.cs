using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class AnimationEventManager : MonoBehaviour
    {
        public CharacterManager characterManager;
        public CharacterLocomotionManager characterLocomotionManager;

        private void Awake()
        {
            characterManager = GetComponent<PlayerManager>();
            characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
        }
        public void ApplyJumpingVelocityFromEvent()
        {
            characterLocomotionManager.ApplyjumpingVelocity();
        }
    }
}

