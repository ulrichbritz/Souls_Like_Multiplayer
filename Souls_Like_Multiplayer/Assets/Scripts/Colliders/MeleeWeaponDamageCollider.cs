using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage; //helps when calculating attack modifiers
    }
}

