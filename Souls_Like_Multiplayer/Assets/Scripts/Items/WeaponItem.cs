using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    [CreateAssetMenu(menuName = "Items/Equipment/Weapon Item")]
    public class WeaponItem : Item
    {
        // animator Controller ovveride

        [Header("Model")]
        public GameObject weaponModel;

        [Header("Requirements")]
        public int strengthREQ = 0;
        public int intelligenceREQ = 0;
        public int agilityREQ = 0;
        public int charismaREQ = 0;
        public int willpowerREQ = 0;

        [Header("Weapon Base Damage")]
        public int physicalDamage = 0;
        public int psychicDamage = 0;
        public int fireDamage = 0;
        public int coldDamage = 0;
        public int lightningDamage = 0;
        public int holyDamage = 0;
        public int necroticDamage = 0;
        public int shadowDamage = 0;

        [Header("Weapon Base Poise Damage")]
        public float poiseDamage = 10;
        //offensive poise bonus

        [Header("Weapon Modifiers")]
        //weapon modifiers
        //light attack modifiers
        //heavy attack modifiers
        //crit damage mod

        //guard absorbtions (blocking power)

        [Header("Stamina Costs")]
        public int baseStaminaCost = 20;
        //running attack stamina cost mod
        //light attack stamina cost mod
        //heavy attack stamina cost mod

        [Header("Actions")]
        public WeaponItemAction oh_Left_Click_Action;
        public WeaponItemAction oh_rightClick_Action;  //one handed right click action

        //blocking sounds
        
        

        [Header("Type info")]
        public bool isUnarmed = false;
        public bool isTwoHanded = false;

        [Header("Damage")]
        public List<GameObject> weaponDiceList;
        public int damage;

        [Header("isMelee")]
        public bool isMelee = true;

        [Header("Idle Animations")]
        public string right_hand_idle;
        public string left_hand_idle;

        [Header("Attack Animations")]
        public string light_attack_01;
        public string heavy_attack_01;

    }
}

