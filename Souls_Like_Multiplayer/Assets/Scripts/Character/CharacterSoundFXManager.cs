using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        private AudioSource audioSource;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySprintingSoundFX()
        {
            
        }

        public void PlayJumpSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.jumpSoundFX);
        }

        public void PlayRollSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSoundFX);
        }

        public void PlayMaleHurtSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.maleHurtSoundFX);
        }
    }
}

