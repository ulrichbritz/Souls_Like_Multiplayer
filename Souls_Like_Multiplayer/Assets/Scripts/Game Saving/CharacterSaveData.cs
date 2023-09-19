using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UB
{
    // template for everything we want to load
    [System.Serializable]
    public class CharacterSaveData
    {
        [Header("Scene index")]
        public int sceneIndex = 1;

        [Header("Character Name")]
        public string characterName = "Character";

        [Header("Time Played")]
        public float secondsPlayed;

        [Header("World Position/Co-ordinates")]  //using floats because can only save/serialize basic variable types
        public float xPosition;
        public float yPosition;
        public float zPosition;

        [Header("Resources")]
        public int currentHealth;
        public float currentStamina;

        [Header("Stats")]
        public int strength;
        public int agility;
    }
}

