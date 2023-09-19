using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace UB
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager playerManager;

        public WeaponItem currentWeaponBeingUsed;

        protected override void Awake()
        {
            base.Awake();

            playerManager = GetComponent<PlayerManager>();
        }

        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
        {
            if (playerManager.IsOwner)
            {
                // perform the action
                weaponAction.AttemptToPerformAction(playerManager, weaponPerformingAction);

                //notify the server to perform action on all clients as well.
                playerManager.playerNetworkManager.NotifyTheServerOfWeaponActionServerRPC(NetworkManager.Singleton.LocalClientId, weaponAction.actionID, weaponPerformingAction.itemID);
            }
        }
    }
}

