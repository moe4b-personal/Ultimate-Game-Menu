using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UGM
{
    public class SliderValueText : MonoBehaviour
    {
        [SerializeField]
        Slider slider;

        Text text;

        void Start()
        {
            text = GetComponent<Text>();

            if(!text)
            {
                Debug.LogError("No Text Component Attached, Please Attach This Script To A Game Object With A Text Component");
                return;
            }

            slider.onValueChanged.AddListener(SetText);
            SetText(slider.value);
        }

        void SetText(float value)
        {
            text.text = value + "";
        }
    }
}