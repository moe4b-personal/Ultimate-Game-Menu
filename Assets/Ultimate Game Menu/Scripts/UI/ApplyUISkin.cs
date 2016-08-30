using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace UGM
{
    [ExecuteInEditMode]
    public class ApplyUISkin : MonoBehaviour
    {
        [SerializeField]
        internal string category;

        [SerializeField]
        bool overrideUI;

        [SerializeField]
        internal UI uiType;

        [SerializeField]
        internal ButtonUI buttonUI;

        [SerializeField]
        internal ToggleUI toggleUI;

        [SerializeField]
        internal SliderUI sliderUI;

        [SerializeField]
        internal ScrollbarUI scrollbarUI;

        [SerializeField]
        internal InputFieldUI inputfieldUI;

        [SerializeField]
        internal DropDownUI dropdownUI;

        [SerializeField]
        internal Image imageUI;

        [SerializeField]
        internal Text textUI;

        void Start()
        {
#if UNITY_EDITOR
            if (!GetComponent<Selectable>() && !GetComponent<Text>() && !GetComponent<Image>())
            {
                Debug.LogError("No Selectable Found, Component Must Be Added To A Selectable UI Element");
                DestroyImmediate(this);
            }

            UpdateUI();

            if(Application.isPlaying)
                SkinManager.Current.ApplySkin(this);
#endif
        }

        public void UpdateUI()
        {
            Button butt = GetComponent<Button>();
            Toggle togg = GetComponent<Toggle>();
            Slider sl = GetComponent<Slider>();
            Scrollbar scroll = GetComponent<Scrollbar>();
            InputField inf = GetComponent<InputField>();
            Dropdown drop = GetComponent<Dropdown>();
            Image img = GetComponent<Image>();
            Text tex = GetComponent<Text>();

            if (butt)
            {
                uiType = UI.Button;
                SetButtonUI(butt);
            }
            else if (togg)
            {
                uiType = UI.Toggle;
                SetToggleUI(togg);
            }
            else if (sl)
            {
                uiType = UI.Slider;
                SetSliderUI(sl);
            }
            else if (scroll)
            {
                uiType = UI.Scrollbar;
                SetScrollbarUI(scroll);
            }
            else if (inf)
            {
                uiType = UI.InputField;
                SetInputFieldUI(inf);
            }
            else if (drop)
            {
                uiType = UI.DropDown;
                SetDropDownUI(drop);
            }
            else if(img)
            {
                uiType = UI.Image;
                imageUI = img;
            }
            else if(tex)
            {
                uiType = UI.Text;
                textUI = tex;
            }
        }

        void SetButtonUI(Button butt)
        {
            buttonUI.button = butt;
            buttonUI.image = GetComponent<Image>();
            buttonUI.text = GetComponentInChildren<Text>();
        }

        void SetToggleUI(Toggle togg)
        {
            toggleUI.toggle = togg;
            toggleUI.background = togg.targetGraphic.GetComponent<Image>();
            toggleUI.checkmark = togg.graphic.GetComponent<Image>();
            toggleUI.label = GetComponentInChildren<Text>();
        }

        void SetSliderUI(Slider sl)
        {
            sliderUI.slider = sl;
            sliderUI.background = transform.GetChild(0).GetComponent<Image>();
            sliderUI.fill = sl.fillRect.GetComponent<Image>();
            sliderUI.handle = sl.handleRect.GetComponent<Image>();
        }

        void SetScrollbarUI(Scrollbar scroll)
        {
            scrollbarUI.scrollbar = scroll;
            scrollbarUI.background = GetComponent<Image>();
            scrollbarUI.handle = scroll.handleRect.GetComponent<Image>();
        }

        void SetInputFieldUI(InputField inf)
        {
            inputfieldUI.inputfield = inf;
            inputfieldUI.background = GetComponent<Image>();
            inputfieldUI.text = inf.textComponent;
            inputfieldUI.placeHolder = inf.placeholder.GetComponent<Text>();
        }

        void SetDropDownUI(Dropdown drop)
        {
            dropdownUI.dropdown = drop;
            dropdownUI.image = drop.GetComponent<Image>();
            dropdownUI.captionText = drop.captionText;
        }

        public enum UI
        {
            Image, Text,Button, Toggle, Slider, Scrollbar, InputField, DropDown
        }

        [Serializable]
        public class ButtonUI
        {
            [SerializeField]
            internal Button button;
            [SerializeField]
            internal Image image;
            [SerializeField]
            internal Text text;
        }

        [Serializable]
        public class ToggleUI
        {
            [SerializeField]
            internal Toggle toggle;
            [SerializeField]
            internal Image background;
            [SerializeField]
            internal Image checkmark;
            [SerializeField]
            internal Text label;
        }

        [Serializable]
        public class SliderUI
        {
            [SerializeField]
            internal Slider slider;
            [SerializeField]
            internal Image background;
            [SerializeField]
            internal Image fill;
            [SerializeField]
            internal Image handle;
        }

        [Serializable]
        public class ScrollbarUI
        {
            [SerializeField]
            internal Scrollbar scrollbar;
            [SerializeField]
            internal Image background;
            [SerializeField]
            internal Image handle;
        }

        [Serializable]
        public class InputFieldUI
        {
            [SerializeField]
            internal InputField inputfield;
            [SerializeField]
            internal Image background;
            [SerializeField]
            internal Text text;
            [SerializeField]
            internal Text placeHolder;
        }

        [Serializable]
        public class DropDownUI
        {
            [SerializeField]
            internal Dropdown dropdown;
            [SerializeField]
            internal Image image;
            [SerializeField]
            internal Text captionText;
        }
    }
}
