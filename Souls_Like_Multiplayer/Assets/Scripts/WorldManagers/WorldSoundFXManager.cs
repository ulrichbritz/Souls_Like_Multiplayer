using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class WorldSoundFXManager : MonoBehaviour
    {
        public static WorldSoundFXManager instance;

        [Header("Action Sounds")]
        public AudioClip sprintSoundFX;
        public AudioClip jumpSoundFX;
        public AudioClip rollSoundFX;

        public AudioClip maleHurtSoundFX;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}

