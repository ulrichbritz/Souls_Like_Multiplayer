using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class CharacterEquipmentManager : MonoBehaviour
    {
        public delegate void OnEquipmentChanged(Equipment newItem = null, Equipment oldItem = null);
        public OnEquipmentChanged onEquipmentChanged;

        protected virtual void Awake()
        {

        }

        protected virtual void Start()
        {

        }
    }
}

