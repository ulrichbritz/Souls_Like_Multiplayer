using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using System;

namespace UB
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        PlayerManager playerManager;

        //name
        public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>("Character", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Equipment")]
        public NetworkVariable<int> currentWeaponBeingUsed = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentRightHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentLeftHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isUsingRightHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isUsingLeftHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        protected override void Awake()
        {
            base.Awake();

            playerManager = GetComponent<PlayerManager>();
        }

        public void SetCharacterActionHand(bool rightHandedAction)
        {
            if (rightHandedAction)
            {
                isUsingLeftHand.Value = false;
                isUsingRightHand.Value = true;
            }
            else
            {
                isUsingRightHand.Value = false;
                isUsingLeftHand.Value = true;                
            }
        }

        public void SetNewMaxHealthValue(int oldStrength, int newStrength)
        {
            maxHealth.Value = playerManager.playerStatsManager.CalculateHealthBasedOnStrength(newStrength);
            PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(maxHealth.Value);

            //can add this if we want to fill value when level up or stat change etc
            currentHealth.Value = maxHealth.Value;
        }

        public void SetNewMaxStaminaValue(int oldAgility, int newAgility)
        {
            maxStamina.Value = playerManager.playerStatsManager.CalculateStaminaBasedOnAgility(newAgility);
            PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(maxStamina.Value);

            //can add this if we want to fill value when level up or stat change etc
            currentStamina.Value = maxStamina.Value;
        }

        public void OnCurrentRightHandWeaponIDChange(int oldID, int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
            playerManager.playerInventoryManager.currentRightHandWeapon = newWeapon;
            playerManager.playerEquipmentManager.LoadRightWeapon();
        }

        public void OnCurrentLeftHandWeaponIDChange(int oldID, int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
            playerManager.playerInventoryManager.currentLeftHandWeapon = newWeapon;
            playerManager.playerEquipmentManager.LoadLeftWeapon();
        }

        public void OnCurrentWeaponBeingUsedIDChange(int oldID, int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
            playerManager.playerCombatManager.currentWeaponBeingUsed = newWeapon;
        }

        //Item Actions
        [ServerRpc]
        public void NotifyTheServerOfWeaponActionServerRPC(ulong clientID, int actionID, int weaponID)
        {
            if (IsServer)
            {
                NotifyTheServerOfWeaponActionClientRPC(clientID, actionID, weaponID);
            }
        }

        [ClientRpc]
        private void NotifyTheServerOfWeaponActionClientRPC(ulong clientID, int actionID, int weaponID)
        {
            //dont play action again for player who called it as they are already playing it locally
            if(clientID != NetworkManager.Singleton.LocalClientId)
            {
                PerformWeaponBasedAction(actionID, weaponID);
            }
        }

        private void PerformWeaponBasedAction(int actionID, int weaponID)
        {
            WeaponItemAction weaponAction = WorldActionManager.instance.GetWeaponItemAction(actionID);

            if(weaponAction != null)
            {
                weaponAction.AttemptToPerformAction(playerManager, WorldItemDatabase.instance.GetWeaponByID(weaponID));
            }
            else
            {
                Debug.LogError("Action is NULL, cannot be performed!");
            }
        }
    }
}

