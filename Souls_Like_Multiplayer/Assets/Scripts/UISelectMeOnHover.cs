using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UB
{
    public class UISelectMeOnHover : MonoBehaviour
    {
        private Button thisButton;

        private void Awake()
        {
            thisButton = GetComponent<Button>();
        }
        public void SelectMeOnHover()
        {
            thisButton.Select();
        }
    }
}

