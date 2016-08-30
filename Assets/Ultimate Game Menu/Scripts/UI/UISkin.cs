using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace UGM
{
    [Serializable]
    [CreateAssetMenu]
    public class UISkin : ScriptableObject
    {
        [SerializeField]
        internal ImageSkin[] imageSkins;
        [SerializeField]
        internal TextSkin[] textSkins;
        [SerializeField]
        internal ButtonSkin[] buttonSkins;
        [SerializeField]
        internal ToggleSkin[] toggleSkins;
        [SerializeField]
        internal SliderSkin[] sliderSkins;
        [SerializeField]
        internal ScrollbarSkin[] scrollbarSkins;
        [SerializeField]
        internal InputFieldSkin[] inputFieldSkins;
        [SerializeField]
        internal DropDownSkin[] dropDownSkins;
    }

    [Serializable]
    public class ImageSkin
    {
        [SerializeField]
        internal string category;

        [SerializeField]
        internal TogglableImage customImage;
    }

    [Serializable]
    public class TextSkin
    {
        [SerializeField]
        internal string category;

        [SerializeField]
        internal TogglableFontData customText;
    }

    [Serializable]
    public class ButtonSkin
    {
        [SerializeField]
        internal string category;

        [SerializeField]
        internal SelectableProperties selectionProperties;

        [SerializeField]
        internal ButtonSkinData buttonSkinData;
    }

    [Serializable]
    public class ButtonSkinData
    {
        [SerializeField]
        internal TogglableFontData customFontData;

        [SerializeField]
        internal TogglableImage customImage;
    }

    [Serializable]
    public class ToggleSkin
    {
        [SerializeField]
        internal string category;

        [SerializeField]
        internal SelectableProperties selectionProperties;

        [SerializeField]
        internal ToggleSkinData toggleSkinData;
    }

    [Serializable]
    public class ToggleSkinData
    {
        [SerializeField]
        internal TogglableImage customBackground;
        [SerializeField]
        internal TogglableImage customCheckmark;
        [SerializeField]
        internal TogglableFontData customLabelFontData;
    }

    [Serializable]
    public class SliderSkin
    {
        [SerializeField]
        internal string category;

        [SerializeField]
        internal SelectableProperties selectionProperties;

        [SerializeField]
        internal SliderSkinData sliderSkinData;
    }

    [Serializable]
    public class SliderSkinData
    {
        [SerializeField]
        internal TogglableImage customBackground;
        [SerializeField]
        internal TogglableImage customFill;
        [SerializeField]
        internal TogglableImage customHandle;
    }

    [Serializable]
    public class ScrollbarSkin
    {
        [SerializeField]
        internal string category;

        [SerializeField]
        internal SelectableProperties selectionProperties;

        [SerializeField]
        internal ScrollbarSkinData scrollbarSkinData;
    }

    [Serializable]
    public class ScrollbarSkinData
    {
        [SerializeField]
        internal TogglableImage customBackground;
        [SerializeField]
        internal TogglableImage customHandle;
    }

    [Serializable]
    public class InputFieldSkin
    {
        [SerializeField]
        internal string category;

        [SerializeField]
        internal SelectableProperties selectionProperties;

        [SerializeField]
        internal InputFieldSkinData inputfieldSkinData;
    }

    [Serializable]
    public class InputFieldSkinData
    {
        [SerializeField]
        internal TogglableImage customBackground;
        [SerializeField]
        internal TogglableFontData customTextData;
        [SerializeField]
        internal TogglableFontData customPlaceholderTextData;
    }

    [Serializable]
    public class DropDownSkin
    {
        [SerializeField]
        internal string category;

        [SerializeField]
        internal SelectableProperties selectionProperties;

        [SerializeField]
        internal DropDownSkinData dropdownSkinData;
    }

    [Serializable]
    public class DropDownSkinData
    {
        [SerializeField]
        internal TogglableImage image;

        [SerializeField]
        internal TogglableFontData customItemTextData;
    }

    [Serializable]
    public class ScrollRectSkin
    {
        [SerializeField]
        internal string category;
    }

    [Serializable]
    public class TogglableBestSize
    {
        [SerializeField]
        internal bool apply;

        [SerializeField]
        internal bool bestFit;

        [SerializeField]
        internal int minSize;
        [SerializeField]
        internal int maxSize;
    }

    [Serializable]
    public class TogglableImage
    {
        [SerializeField]
        internal TogglableColor customColor;
        [SerializeField]
        internal TogglableSprite customSprite;
    }

    [Serializable]
    public class TogglableColor
    {
        [SerializeField]
        internal bool apply;
        [SerializeField]
        internal Color color;
    }

    [SerializeField]

    [Serializable]
    public class TogglableFont
    {
        [SerializeField]
        internal bool apply;
        [SerializeField]
        internal Font font;
    }

    [Serializable]
    public class TogglableFontData
    {
        [SerializeField]
        internal TogglableFont customFont;
        [SerializeField]
        internal TogglableColor customColor;
        [SerializeField]
        internal TogglableSize customSize;
        [SerializeField]
        internal TogglableBestSize customBestSize;
    }

    [Serializable]
    public class TogglableSize
    {
        [SerializeField]
        internal bool apply;
        [SerializeField]
        internal int size;
    }

    [Serializable]
    public class TogglableSprite
    {
        [SerializeField]
        internal bool apply;
        [SerializeField]
        internal Sprite sprite;
    }

    [Serializable]
    public class SelectableProperties
    {
        [SerializeField]
        internal Selectable.Transition transition;

        [SerializeField]
        internal ColorBlock colors;

        [SerializeField]
        internal SpriteState sprites;

        [SerializeField]
        internal AnimationTriggers animations;
    }
}