using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage")]
    public class TakeStaminaDamageCharacterEffect : InstantCharacterEffect
    {
        public float staminaDamage;

        public override void ProcessEffect(CharacterManager character)
        {
            CalculateStaminaDamage(character);
        }

        private void CalculateStaminaDamage(CharacterManager character)
        {
            if (character.IsOwner)
            {
                character.characterNetworkManager.currentStamina.Value -= staminaDamage;
            }

            //compare the base stamina damage against other player effects/modifiers
            
            //change the value before subtracting/adding it

            //play sound fx or vfx during effect
        }
    }
}

