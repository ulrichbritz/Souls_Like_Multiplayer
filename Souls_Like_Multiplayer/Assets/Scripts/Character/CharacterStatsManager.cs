using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class CharacterStatsManager : MonoBehaviour
    {
        [Header("Scripts")]
        CharacterManager characterManager;

        [Header("Stamina Regeneration")]
        private float staminaRegenerationTimer = 0;
        [SerializeField] float staminaRegenerationDelay = 2;
        private float staminaTickTimer;
        [SerializeField] int staminaRegenerationAmount = 1;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        protected virtual void Start()
        {

        }

        #region Health
        public int CalculateHealthBasedOnStrength(int strength)
        {
            float health = 0;
            health = 10 + strength;

            return Mathf.RoundToInt(health);
        }

        #endregion

        #region Stamina
        public int CalculateStaminaBasedOnAgility(int agility)
        {
            float stamina = 0;
            stamina = 10 + agility;

            return Mathf.RoundToInt(stamina);
        }

        public virtual void RegenerateStamina()
        {
            //only owners can edit network var
            if (!characterManager.IsOwner)
                return;

            //dont regen if sprinting
            if (characterManager.characterNetworkManager.isSprinting.Value)
                return;

            if (characterManager.isPerformingAction)
                return;

            staminaRegenerationTimer += Time.deltaTime;

            if (staminaRegenerationTimer >= staminaRegenerationDelay)
            {
                if (characterManager.characterNetworkManager.currentStamina.Value < characterManager.characterNetworkManager.maxStamina.Value)
                {
                    staminaTickTimer += Time.deltaTime;

                    if (staminaTickTimer >= 0.1)
                    {
                        staminaTickTimer = 0;
                        characterManager.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                    }
                }
            }
        }

        public virtual void ResetStaminaRegenTimer(float prevStaminaAmount, float currentStaminaAmount)
        {
            //only reset this if action used stamina, else would have to wait everytime it increases even a bit
            if(currentStaminaAmount < prevStaminaAmount)
            {
                staminaRegenerationTimer = 0;
            }          
        }

        #endregion

        #region Move Speed
        public int CalculateMoveSpeedBasedOnStrength(int strength)
        {
            float health = 0;
            health = 10 + strength;

            return Mathf.RoundToInt(health);
        }

        #endregion


    }
}

