using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        //instant effects (take damage)

        //timed effects (poison)

        //static effects (buffs from items)

        CharacterManager characterManager;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            // take in effect


            //process effect
            effect.ProcessEffect(characterManager);

        }
    }
}

