using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        PlayerManager playerManager;

        //slots
        public WeaponHolderSlot rightHandSlot;
        public WeaponHolderSlot leftHandSlot;

        //weapon managers
        [SerializeField] WeaponManager rightWeaponManager;
        [SerializeField] WeaponManager leftWeaponManager;

        //weapon models
        public GameObject rightHandWeaponModel;
        public GameObject leftHandWeaponModel;

        //equipment arrays
        [SerializeField] Equipment[] defaultEquipment;
        [SerializeField] Equipment[] currentEquipment;
        [SerializeField] WeaponItem[] defaultWeapons;
        [SerializeField] WeaponItem[] currentWeapons;

        protected override void Awake()
        {
            base.Awake();

            playerManager = GetComponent<PlayerManager>();

            //get weapon slots
            InitializeWeaponSlots();
        }

        protected override void Start()
        {
            base.Start();

            LoadWeaponsOnBothHands();
        }

        private void InitializeWeaponSlots()
        {
            WeaponHolderSlot[] weaponSlots = GetComponentsInChildren<WeaponHolderSlot>();

            foreach(var weaponSlot in weaponSlots)
            {
                print("found weaponSlot");
                if(weaponSlot.weaponModelSlot == WeaponModelSlot.RightHand)
                {
                    rightHandSlot = weaponSlot;
                }
                else if(weaponSlot.weaponModelSlot == WeaponModelSlot.LeftHand)
                {
                    leftHandSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponsOnBothHands()
        {
            LoadRightWeapon();
            LoadLeftWeapon();
        }

        //right weapon
        public void LoadRightWeapon()
        {
            if(playerManager.playerInventoryManager.currentRightHandWeapon != null)
            {
                //remove old weapon
                rightHandSlot.UnloadWeapon();

                //load model on slot
                rightHandWeaponModel = Instantiate(playerManager.playerInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);
                //assign weapon damage to its collider
                rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
                rightWeaponManager.SetWeaponDamage(playerManager, playerManager.playerInventoryManager.currentRightHandWeapon);               
            }
        }

        public void SwitchRightWeapon()
        {

        }

        //left weapon
        public void LoadLeftWeapon()
        {
            if (playerManager.playerInventoryManager.currentLeftHandWeapon != null)
            {
                //rmove old weapon
                leftHandSlot.UnloadWeapon();

                //load model on slot
                leftHandWeaponModel = Instantiate(playerManager.playerInventoryManager.currentLeftHandWeapon.weaponModel);
                leftHandSlot.LoadWeapon(leftHandWeaponModel);
                //assign weapon damage to its collider
                leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
                leftWeaponManager.SetWeaponDamage(playerManager, playerManager.playerInventoryManager.currentLeftHandWeapon);
            }
        }

        public void SwitchLeftWeapon()
        {

        }
    }
}

