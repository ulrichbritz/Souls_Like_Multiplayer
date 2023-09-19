using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
    public class LightAttackWeaponItemAction : WeaponItemAction
    {
        [SerializeField] string light_Attack_01 = "Left_Hand_Light_Attack_01";  //or Main_Light_Attack_01 with Main = main hand
        [SerializeField] string light_Attack_02 = "Right_Hand_Light_Attack_01";  //or Main_Light_Attack_01 with Main = main hand

        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            //check if owner
            if (!playerPerformingAction.IsOwner)
                return;

            // check for stops e.g stamina
            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
                return;

            if (!playerPerformingAction.isGrounded)
                return;

            PerformLightAttack(playerPerformingAction, weaponPerformingAction);
        }

        private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            //check which hand in use
            if (playerPerformingAction.playerNetworkManager.isUsingRightHand.Value)
            {
                playerPerformingAction.playerAnimationManager.PlayTargetAttackActionAnimation(light_Attack_02, true);
            }

            if (playerPerformingAction.playerNetworkManager.isUsingLeftHand.Value)
            {
                playerPerformingAction.playerAnimationManager.PlayTargetAttackActionAnimation(light_Attack_01, true);
            }
        }
    }

}

