using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class PlayerInventoryManager : CharacterEquipmentManager
    {
        public WeaponItem currentRightHandWeapon;
        public WeaponItem currentLeftHandWeapon;

        public delegate void OnItemChanged();
        public OnItemChanged onItemChangedCallback;

        public int inventorySpace = 30;

        public List<Item> items = new List<Item>();

        public bool Add(Item item)
        {
            if (!item.isDefaultItem)
            {
                if (items.Count >= inventorySpace)
                {
                    Debug.Log("Not enough space in inventory");
                    return false;
                }

                items.Add(item);

                if (onItemChangedCallback != null)
                    onItemChangedCallback.Invoke();
            }

            return true;
        }

        public void Remove(Item item)
        {
            items.Remove(item);

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
    }
}

