using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        [Header("Components")]
        //scripts
        [HideInInspector] public PlayerManager playerManager;

        [SerializeField] GameObject inventoryUIObject;
        [SerializeField] GameObject equipmentUIObject;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
               // Destroy(gameObject);
            }
        }

        public void ToggleInventory()
        {
            inventoryUIObject.SetActive(!inventoryUIObject.activeSelf);
            CheckIfNeedMouse();
        }

        public void ToggleEquipment()
        {
            equipmentUIObject.SetActive(!equipmentUIObject.activeSelf);
            CheckIfNeedMouse();
        }

        private void CheckIfNeedMouse()
        {
            if(inventoryUIObject.activeSelf == true || equipmentUIObject == true)
            {
                MouseControls.instance.ShowCursor();
                playerManager.uiFlag = true;
            }
            
            if(inventoryUIObject.activeSelf == false || equipmentUIObject == false)
            {
                MouseControls.instance.HideCursor();
                playerManager.uiFlag = false;
            }
        }
    }
}

