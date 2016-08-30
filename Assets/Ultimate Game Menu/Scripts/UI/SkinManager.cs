using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UGM
{
    public class SkinManager : MonoBehaviour
    {
        static SkinManager current;
        public static SkinManager Current { get { return current; } }
        [SerializeField]
        UISkin skin;

        Dictionary<string, int> imageSkins = new Dictionary<string, int>();
        Dictionary<string, int> textSkins = new Dictionary<string, int>();
        Dictionary<string, int> buttonSkins = new Dictionary<string, int>();
        Dictionary<string, int> toggleSkins = new Dictionary<string, int>();
        Dictionary<string, int> sliderSkins = new Dictionary<string, int>();
        Dictionary<string, int> scrollbarskins = new Dictionary<string, int>();
        Dictionary<string, int> inputFieldSkins = new Dictionary<string, int>();
        Dictionary<string, int> dropDownSkins = new Dictionary<string, int>();

        public void Start()
        {
            current = this;
            SetSkins();
        }

        public void ApplyToVisible()
        {
            SetSkins();

            ApplyUISkin[] uiElements = FindObjectsOfType<ApplyUISkin>();

            for (int i = 0; i < uiElements.Length; i++)
            {
                ApplySkin(uiElements[i]);
            }
        }

        void SetSkins()
        {
            imageSkins.Clear();
            textSkins.Clear();
            buttonSkins.Clear();
            toggleSkins.Clear();
            sliderSkins.Clear();
            scrollbarskins.Clear();
            inputFieldSkins.Clear();
            dropDownSkins.Clear();

            for (int i = 0; i < skin.imageSkins.Length; i++)
            {
                imageSkins.Add(skin.imageSkins[i].category, i);
            }

            for (int i = 0; i < skin.textSkins.Length; i++)
            {
                textSkins.Add(skin.textSkins[i].category, i);
            }

            for (int i = 0; i < skin.buttonSkins.Length; i++)
            {
                buttonSkins.Add(skin.buttonSkins[i].category, i);
            }

            for (int i = 0; i < skin.toggleSkins.Length; i++)
            {
                toggleSkins.Add(skin.toggleSkins[i].category, i);
            }

            for (int i = 0; i < skin.sliderSkins.Length; i++)
            {
                sliderSkins.Add(skin.sliderSkins[i].category, i);
            }

            for (int i = 0; i < skin.scrollbarSkins.Length; i++)
            {
                scrollbarskins.Add(skin.scrollbarSkins[i].category, i);
            }

            for (int i = 0; i < skin.dropDownSkins.Length; i++)
            {
                dropDownSkins.Add(skin.dropDownSkins[i].category, i);
            }

            for (int i = 0; i < skin.inputFieldSkins.Length; i++)
            {
                inputFieldSkins.Add(skin.inputFieldSkins[i].category, i);
            }
        }

        public void ApplySkin(ApplyUISkin applyUISkin)
        {
            if (applyUISkin.uiType == ApplyUISkin.UI.Button) //Button 
            {
                ButtonSkin buttonSkin = null;
                if (buttonSkins.ContainsKey(applyUISkin.category))
                    buttonSkin = skin.buttonSkins[buttonSkins[applyUISkin.category]];
                else
                    buttonSkin = skin.buttonSkins[0];

                SetButtonSkin(applyUISkin.buttonUI, buttonSkin);
            }
            else if (applyUISkin.uiType == ApplyUISkin.UI.Toggle) //Toggle 
            {
                ToggleSkin toggleSkin = null;
                if (toggleSkins.ContainsKey(applyUISkin.category))
                    toggleSkin = skin.toggleSkins[toggleSkins[applyUISkin.category]];
                else
                    toggleSkin = skin.toggleSkins[0];

                SetToggleSkin(applyUISkin.toggleUI, toggleSkin);
            }
            else if (applyUISkin.uiType == ApplyUISkin.UI.Slider) //Slider 
            {
                SliderSkin sliderSkin = null;
                if (sliderSkins.ContainsKey(applyUISkin.category))
                    sliderSkin = skin.sliderSkins[sliderSkins[applyUISkin.category]];
                else
                    sliderSkin = skin.sliderSkins[0];

                SetSliderSkin(applyUISkin.sliderUI, sliderSkin);
            }
            else if (applyUISkin.uiType == ApplyUISkin.UI.Scrollbar) //Scrollbar 
            {
                ScrollbarSkin scrollbarSkin = null;
                if (scrollbarskins.ContainsKey(applyUISkin.category))
                    scrollbarSkin = skin.scrollbarSkins[scrollbarskins[applyUISkin.category]];
                else
                    scrollbarSkin = skin.scrollbarSkins[0];

                SetScrollbar(applyUISkin.scrollbarUI, scrollbarSkin);
            }
            else if (applyUISkin.uiType == ApplyUISkin.UI.InputField) //Scrollbar 
            {
                InputFieldSkin inputFieldSkin = null;
                if (inputFieldSkins.ContainsKey(applyUISkin.category))
                    inputFieldSkin = skin.inputFieldSkins[inputFieldSkins[applyUISkin.category]];
                else
                    inputFieldSkin = skin.inputFieldSkins[0];

                SetInputFieldUI(applyUISkin.inputfieldUI, inputFieldSkin);
            }
            else if (applyUISkin.uiType == ApplyUISkin.UI.DropDown) //DropDown 
            {
                DropDownSkin dropDownSkin = null;
                if (dropDownSkins.ContainsKey(applyUISkin.category))
                    dropDownSkin = skin.dropDownSkins[dropDownSkins[applyUISkin.category]];
                else
                    dropDownSkin = skin.dropDownSkins[0];

                SetDropDownSkin(applyUISkin.dropdownUI, dropDownSkin);
            }
            else if (applyUISkin.uiType == ApplyUISkin.UI.Text)
            {
                TextSkin textSkin = null;
                if (textSkins.ContainsKey(applyUISkin.category))
                    textSkin = skin.textSkins[textSkins[applyUISkin.category]];
                else
                    textSkin = skin.textSkins[0];

                SetTextSkin(applyUISkin.textUI, textSkin);
            }
            else if (applyUISkin.uiType == ApplyUISkin.UI.Image)
            {
                ImageSkin imageSkin = null;
                if (imageSkins.ContainsKey(applyUISkin.category))
                    imageSkin = skin.imageSkins[imageSkins[applyUISkin.category]];
                else
                    imageSkin = skin.imageSkins[0];

                SetImageSkin(applyUISkin.imageUI, imageSkin);
            }
        }

        void SetImageSkin(Image img, ImageSkin skin)
        {
            SetImage(img, skin.customImage);
        }

        void SetTextSkin(Text tex, TextSkin skin)
        {
            SetFontData(tex, skin.customText);
        }

        void SetButtonSkin(ApplyUISkin.ButtonUI buttonUI, ButtonSkin skin)
        {
            SetImage(buttonUI.image, skin.buttonSkinData.customImage);

            if(buttonUI.text)
                SetFontData(buttonUI.text, skin.buttonSkinData.customFontData);

            SetSelectableSkin(buttonUI.button, skin.selectionProperties);
        }

        void SetToggleSkin(ApplyUISkin.ToggleUI toggleUI, ToggleSkin skin)
        {
            SetImage(toggleUI.background, skin.toggleSkinData.customBackground);
            SetImage(toggleUI.checkmark, skin.toggleSkinData.customCheckmark);

            if(toggleUI.label)
                SetFontData(toggleUI.label, skin.toggleSkinData.customLabelFontData);

            SetSelectableSkin(toggleUI.toggle, skin.selectionProperties);
        }

        void SetSliderSkin(ApplyUISkin.SliderUI sliderUI, SliderSkin skin)
        {
            SetImage(sliderUI.background, skin.sliderSkinData.customBackground);
            SetImage(sliderUI.fill, skin.sliderSkinData.customFill);
            SetImage(sliderUI.handle, skin.sliderSkinData.customHandle);

            SetSelectableSkin(sliderUI.slider, skin.selectionProperties);
        }

        void SetScrollbar(ApplyUISkin.ScrollbarUI scrollUI, ScrollbarSkin skin)
        {
            SetImage(scrollUI.background, skin.scrollbarSkinData.customBackground);
            SetImage(scrollUI.handle, skin.scrollbarSkinData.customHandle);

            SetSelectableSkin(scrollUI.scrollbar, skin.selectionProperties);
        }

        void SetInputFieldUI(ApplyUISkin.InputFieldUI inputFieldUI, InputFieldSkin skin)
        {
            SetImage(inputFieldUI.background, skin.inputfieldSkinData.customBackground);

            SetFontData(inputFieldUI.text, skin.inputfieldSkinData.customTextData);
            SetFontData(inputFieldUI.placeHolder, skin.inputfieldSkinData.customPlaceholderTextData);

            SetSelectableSkin(inputFieldUI.inputfield, skin.selectionProperties);
        }

        void SetDropDownSkin(ApplyUISkin.DropDownUI dropdownUI, DropDownSkin skin)
        {
            SetImage(dropdownUI.image, skin.dropdownSkinData.image);
            SetFontData(dropdownUI.captionText, skin.dropdownSkinData.customItemTextData);

            SetSelectableSkin(dropdownUI.dropdown, skin.selectionProperties);
        }

        static void SetSelectableSkin(Selectable selectable, SelectableProperties selectableProperties)
        {
            switch (selectableProperties.transition)
            {
                case Selectable.Transition.ColorTint:
                    selectable.colors = selectableProperties.colors;
                    break;
                case Selectable.Transition.SpriteSwap:
                    selectable.spriteState = selectableProperties.sprites;
                    break;
                case Selectable.Transition.Animation:
                    selectable.animationTriggers = selectableProperties.animations;
                    break;
            }
        }

        static void SetFontData(Text text, TogglableFontData togFontData)
        {
            if (togFontData.customColor.apply)
                text.color = togFontData.customColor.color;

            if (togFontData.customFont.apply)
                text.font = togFontData.customFont.font;

            if (togFontData.customSize.apply)
                text.fontSize = togFontData.customSize.size;

            if (togFontData.customBestSize.apply)
            {
                text.resizeTextForBestFit = togFontData.customBestSize.bestFit;

                if(togFontData.customBestSize.bestFit)
                {
                    text.resizeTextMinSize = togFontData.customBestSize.minSize;
                    text.resizeTextMaxSize = togFontData.customBestSize.maxSize;
                }
            }
        }

        static void SetImage(Image img, TogglableImage togImg)
        {
            SetColor(img, togImg.customColor);
            SetSprite(img, togImg.customSprite);
        }

        static void SetColor(Image img, TogglableColor togColor)
        {
            if (togColor.apply)
                img.color = togColor.color;
        }

        static void SetSprite(Image img, TogglableSprite togSprite)
        {
            if (togSprite.apply)
                img.sprite = togSprite.sprite;
        }

        static ColorBlock BlockFromColor(Color main, float colorMultiplier = 1f, float fadeDuration = 0.2f)
        {
            return new ColorBlock
            {
                normalColor = main,
                pressedColor = main,
                highlightedColor = main,
                disabledColor = main,
                colorMultiplier = colorMultiplier,
                fadeDuration = fadeDuration
            };
        }
    }
}