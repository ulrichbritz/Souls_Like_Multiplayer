using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UB
{
    public class UI_StatBar : MonoBehaviour
    {
        private Slider slider;
        private RectTransform rectTransform;

        //create variable to scale bar size depening on stat
        [Header("Bar Options")]
        [SerializeField] protected bool scaleBarLengthWidthStats = true;
        [SerializeField] protected float widthScaleMultiplier = 1f;

        //later make a secondary polish bar

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
            rectTransform = GetComponent<RectTransform>();
        }

        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;

            if (scaleBarLengthWidthStats)
            {
                // scale the transform of the object based on widthscalemultiplier modifier
                rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);

                //reset the position of bars based on layout group settings
                PlayerUIManager.instance.playerUIHudManager.RefreshHUD();
            }
        }
    }
}

