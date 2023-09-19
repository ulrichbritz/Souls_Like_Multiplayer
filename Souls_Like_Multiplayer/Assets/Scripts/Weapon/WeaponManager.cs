using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField] MeleeWeaponDamageCollider meleeDamageCollider;

        private void Awake()
        {
            meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
        }

        public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon)
        {
            meleeDamageCollider.characterCausingDamage = characterWieldingWeapon;

            //assign all damages
            meleeDamageCollider.physicalDamage = weapon.physicalDamage;
            meleeDamageCollider.psychicDamage = weapon.psychicDamage;
            meleeDamageCollider.fireDamage = weapon.fireDamage;
            meleeDamageCollider.coldDamage = weapon.coldDamage;
            meleeDamageCollider.lightningDamage = weapon.lightningDamage;
            meleeDamageCollider.holyDamage = weapon.holyDamage;
            meleeDamageCollider.necroticDamage = weapon.necroticDamage;
            meleeDamageCollider.shadowDamage = weapon.shadowDamage;
        }
    }
}

