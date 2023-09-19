using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager playerManager;

        protected override void Awake()
        {
            base.Awake();

            playerManager = GetComponent<PlayerManager>();       
        }

        protected override void Start()
        {
            base.Start();

            //calculating values here for when we make a new character during character creation and set stats, this will be calculated
            //until we make character creation we can do it here, but if a save file exists they will be ovverwitten
            CalculateHealthBasedOnStrength(playerManager.playerNetworkManager.strength.Value);
            CalculateStaminaBasedOnAgility(playerManager.playerNetworkManager.agility.Value);
        }


    }
}

