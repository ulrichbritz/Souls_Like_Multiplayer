using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class Item : ScriptableObject
    {
        [Header("Item Information")]
        public string itemName;

        public Sprite itemIcon;

        [TextArea] public string itemDescription;

        public int itemID;
        
        public bool isDefaultItem = false;

        public virtual void Use()
        {
            Debug.Log("Using " + itemName);
        }

        public void RemoveFromInventory()
        {
            Inventory.instance.Remove(this);
        }
    }
}

