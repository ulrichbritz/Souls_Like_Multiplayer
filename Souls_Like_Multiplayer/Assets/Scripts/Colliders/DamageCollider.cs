using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("Collider")]
        protected Collider damageCollider;

        [Header("Damage")]
        public float physicalDamage = 0;    // in the future will be split into standard, piercing etc
        public float psychicDamage = 0;
        public float fireDamage = 0;
        public float coldDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;
        public float necroticDamage = 0;
        public float shadowDamage = 0;

        [Header("Contact Point")]
        protected Vector3 contactPoint;

        [Header("Characters Damaged")]
        protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

        public int currentWeaponDamage;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            // check if object is a character
            CharacterManager damageTarget = other.GetComponent<CharacterManager>();

            if(damageTarget != null)
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // check if we can damage this target based on friendly fire

                // check if target is blocking

                //check if target is invulnerable

                //damage
                DamageTarget(damageTarget);
            }
        }

        protected virtual void DamageTarget(CharacterManager damageTarget)
        {
            // we dont want to damage the same target more than once in a single attack
            //so we add them to a list that checks before applying damage

            if (charactersDamaged.Contains(damageTarget))
                return;

            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.psychicDamage = psychicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.coldDamage = coldDamage;           
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.necroticDamage = necroticDamage;
            damageEffect.shadowDamage = shadowDamage;

            damageEffect.contactPoint = contactPoint;

            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }

        public virtual void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public virtual void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            charactersDamaged.Clear();  //reset characters that have been hit so we can damage them again next swing
        }
    }
}

