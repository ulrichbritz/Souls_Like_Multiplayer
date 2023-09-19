using UnityEngine;

namespace UB
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage; //if damage is caused by another character

        [Header("Damage")]
        public float physicalDamage = 0;    // in the future will be split into standard, piercing etc
        public float psychicDamage = 0;
        public float fireDamage = 0;
        public float coldDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;
        public float necroticDamage = 0;
        public float shadowDamage = 0;

        [Header("Final Damage")]
        private int finalDamageDealt = 0; //damage character takes after all other calculations

        [Header("Poise")]
        public float poiseDamage = 0;
        public bool poiseIsBroken = false;  //if a characters poise is broken, they will be "stunned" and play damage anim

        //to do
        //build ups bleed etc
        // build up effect amounts

        [Header("Animation")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("Sound FX")]
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSoundFX; //used on top of regular sfx if magic damage is present

        [Header("Direction Damage Taken From")]
        public float angleHitFrom;  // used to determine what damage anim to play (to the left, right etc)
        public Vector3 contactPoint;    // used to determine where blood fx instantiates

        

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            //if character is dead, dont process additional damage effects
            if (character.characterNetworkManager.isDead.Value)
                return;

            // check for invulnerability

            //calculate damage
            CalculateDamage(character);

            // check which direction damage came from

            //play damage animation if needed

            //check for build ups if needed (bleed, poison)

            //play damage sound fx

            //player da,age vfx (blood etc)

            //if character is AI, check for new target if character causing damage is present
        }

        private void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            if(characterCausingDamage != null)
            {
                // check for damage modifiers and modify base damage (physical damage buff etc)
                //e.g physical *= physical modifier etc
            }

            //check character for flat damage reduction and subtract them from damage

            //check character for armor absorbtions if free roam battle, and subtract the percentage from the damage

            //add all damage types together and apply the final damage
            finalDamageDealt = Mathf.RoundToInt(physicalDamage + psychicDamage + fireDamage + coldDamage + lightningDamage + holyDamage + necroticDamage + shadowDamage);

            if(finalDamageDealt <= 0)
            {
                finalDamageDealt = 0;      //maybe make one for effect
            }

            Debug.Log("Final Damage Given: " + finalDamageDealt);
            character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
            Debug.Log("Character health: " + character.characterNetworkManager.currentHealth.Value);

            //calculate poise damage to determine if character wukk play damage anim
        }
    }
}

