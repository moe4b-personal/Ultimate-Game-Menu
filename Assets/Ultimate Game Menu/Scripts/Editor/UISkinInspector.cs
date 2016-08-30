using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

namespace UGM
{
    //[CustomEditor(typeof(UISkin))]
    public class UISkinInspector : Editor
    {
        SerializedProperty buttonSkins;
        SerializedProperty toggleSkins;
        SerializedProperty sliderSkins;
        SerializedProperty scrollbarSkins;
        SerializedProperty dropDownSkins;

        void OnEnable()
        {
            buttonSkins = serializedObject.FindProperty("buttonSkins");
            toggleSkins = serializedObject.FindProperty("toggleSkins");
            sliderSkins = serializedObject.FindProperty("sliderSkins");
            scrollbarSkins = serializedObject.FindProperty("scrollbarSkins");
            dropDownSkins = serializedObject.FindProperty("dropDownSkins");
        }

        public override void OnInspectorGUI()
        {
            DrawButtonSkins();
            DrawToggleSkins();
            DrawSliderSkins();
            DrawScrollbarSkins();
            DrawDropDownSkins();

            serializedObject.ApplyModifiedProperties();
        }

        void DrawButtonSkins()
        {
            buttonSkins.isExpanded = EditorGUILayout.Foldout(buttonSkins.isExpanded, "Button Skins");

            if (buttonSkins.isExpanded)
            {
                EditorGUI.indentLevel++;

                buttonSkins.arraySize = EditorGUILayout.IntField("Size", buttonSkins.arraySize);
                buttonSkins.arraySize = InspectorTools.DrawArraySizeButtons(buttonSkins.arraySize);

                for (int i = 0; i < buttonSkins.arraySize; i++)
                {
                    EditorGUI.indentLevel++;

                    SerializedProperty buttonSkin = buttonSkins.GetArrayElementAtIndex(i);
                    DrawButtonSkin(buttonSkin, "Button Skin " + i);

                    EditorGUI.indentLevel--;
                }

                EditorGUI.indentLevel--;
            }
        }

        void DrawButtonSkin(SerializedProperty buttonSkin, string ovverideLabel = "")
        {
            SerializedProperty category = buttonSkin.FindPropertyRelative("category");

            if(category.stringValue != "")
            {
                ovverideLabel = category.stringValue;
            }

            buttonSkin.isExpanded = EditorGUILayout.Foldout(buttonSkin.isExpanded, ovverideLabel);

            if(buttonSkin.isExpanded)
            {
                EditorGUI.indentLevel++;

                category.stringValue = EditorGUILayout.TextField("Category", category.stringValue);

                DrawSelectableProperties(buttonSkin.FindPropertyRelative("selectionProperties"), "Selectable Properties", true, true);

                DrawButtonTextSkin(buttonSkin.FindPropertyRelative("textSkin"));

                EditorGUI.indentLevel--;
            }
        }

        void DrawToggleSkins()
        {

        }

        void DrawSliderSkins()
        {
            sliderSkins.isExpanded = EditorGUILayout.Foldout(sliderSkins.isExpanded, "Slider Skins");

            if (sliderSkins.isExpanded)
            {
                EditorGUI.indentLevel++;

                sliderSkins.arraySize = EditorGUILayout.IntField("Size", sliderSkins.arraySize);
                sliderSkins.arraySize = InspectorTools.DrawArraySizeButtons(sliderSkins.arraySize);

                for (int i = 0; i < sliderSkins.arraySize; i++)
                {
                    EditorGUI.indentLevel++;

                    SerializedProperty sliderSkin = sliderSkins.GetArrayElementAtIndex(i);
                    DrawSliderSkin(sliderSkin, "Slider Skin " + i);

                    EditorGUI.indentLevel--;
                }

                EditorGUI.indentLevel--;
            }
        }

        void DrawSliderSkin(SerializedProperty sliderSkin, string label)
        {
            sliderSkin.isExpanded = EditorGUILayout.Foldout(sliderSkin.isExpanded, label);

            if(sliderSkin.isExpanded)
            {
                EditorGUI.indentLevel++;
                DrawSelectableProperties(sliderSkin.FindPropertyRelative("selectionProperties"));
                EditorGUI.indentLevel--;
            }
        }

        void DrawScrollbarSkins()
        {

        }

        void DrawDropDownSkins()
        {

        }

        void DrawSelectableProperties(SerializedProperty selectionProperties, string label = "Selectable Properties", bool overrideColor = false, bool overrideSprite = false)
        {
            selectionProperties.isExpanded = EditorGUILayout.Foldout(selectionProperties.isExpanded, label);

            if(selectionProperties.isExpanded)
            {
                EditorGUI.indentLevel++;

                SerializedProperty transition = selectionProperties.FindPropertyRelative("transition");
                Selectable.Transition transitionE = (Selectable.Transition)transition.enumValueIndex;
                transitionE = (Selectable.Transition)EditorGUILayout.EnumPopup("Transition", transitionE);
                transition.enumValueIndex = (int)transitionE;

                if (transitionE == Selectable.Transition.None)
                {
                    if (overrideColor)
                    {
                        SerializedProperty customColor = selectionProperties.FindPropertyRelative("customColor");

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Custom Color");
                        customColor.boolValue = EditorGUILayout.Toggle(customColor.boolValue);
                        EditorGUILayout.EndHorizontal();

                        if (customColor.boolValue)
                        {
                            EditorGUI.indentLevel++;
                            SerializedProperty color = selectionProperties.FindPropertyRelative("color");
                            EditorGUILayout.PropertyField(color, new GUIContent("Color"));
                            InspectorTools.Space();
                            EditorGUI.indentLevel--;
                        }
                    }
                    if (overrideSprite)
                    {
                        SerializedProperty customSprite = selectionProperties.FindPropertyRelative("customSprite");

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Custom Sprite");
                        customSprite.boolValue = EditorGUILayout.Toggle(customSprite.boolValue);
                        EditorGUILayout.EndHorizontal();

                        if (customSprite.boolValue)
                        {
                            EditorGUI.indentLevel++;
                            SerializedProperty sprite = selectionProperties.FindPropertyRelative("sprite");
                            EditorGUILayout.PropertyField(sprite, new GUIContent("Sprite"));
                            EditorGUI.indentLevel--;
                        }
                    }
                }
                else
                {
                    EditorGUIUtility.labelWidth += 50;

                    if (transitionE == Selectable.Transition.ColorTint)
                    {
                        SerializedProperty colors = selectionProperties.FindPropertyRelative("colors");
                        EditorGUILayout.PropertyField(colors, new GUIContent("Colors"));

                        InspectorTools.Space();

                        SerializedProperty customSprite = selectionProperties.FindPropertyRelative("customSprite");

                        customSprite.boolValue = EditorGUILayout.Toggle("Custom Sprite", customSprite.boolValue);

                        if (customSprite.boolValue)
                        {
                            EditorGUI.indentLevel++;
                            SerializedProperty sprite = selectionProperties.FindPropertyRelative("sprite");
                            EditorGUILayout.PropertyField(sprite, new GUIContent("Sprite"));
                            EditorGUI.indentLevel--;
                        }
                    }
                    else if (transitionE == Selectable.Transition.Animation)
                    {
                        SerializedProperty animations = selectionProperties.FindPropertyRelative("animations");
                        EditorGUILayout.PropertyField(animations, new GUIContent("Animations"));
                    }
                    else if (transitionE == Selectable.Transition.SpriteSwap)
                    {
                        SerializedProperty Sprites = selectionProperties.FindPropertyRelative("sprites");
                        EditorGUILayout.PropertyField(Sprites, new GUIContent("Sprites"));
                    }

                    EditorGUIUtility.labelWidth -= 50;
                }

                EditorGUI.indentLevel--;
            }
        }

        void DrawButtonTextSkin(SerializedProperty textSkin, string label = "Button Text Skin")
        {
            textSkin.isExpanded = EditorGUILayout.Foldout(textSkin.isExpanded, label);

            if(textSkin.isExpanded)
            {
                EditorGUI.indentLevel++;
                EditorGUIUtility.labelWidth += 40;

                SerializedProperty customFont = textSkin.FindPropertyRelative("customFont");
                customFont.boolValue = EditorGUILayout.Toggle("Custom Font", customFont.boolValue);
                if(customFont.boolValue)
                {
                    EditorGUI.indentLevel++;
                    SerializedProperty font = textSkin.FindPropertyRelative("font");
                    EditorGUILayout.PropertyField(font, new GUIContent("Font"));
                    EditorGUI.indentLevel--;
                }

                SerializedProperty customColor = textSkin.FindPropertyRelative("customColor");
                customColor.boolValue = EditorGUILayout.Toggle("Custom Color", customColor.boolValue);
                if (customColor.boolValue)
                {
                    EditorGUI.indentLevel++;
                    SerializedProperty color = textSkin.FindPropertyRelative("color");
                    EditorGUILayout.PropertyField(color, new GUIContent("Color"));
                    EditorGUI.indentLevel--;
                }

                SerializedProperty customSize = textSkin.FindPropertyRelative("customSize");
                SerializedProperty bestFit = textSkin.FindPropertyRelative("bestFit");

                customSize.boolValue = EditorGUILayout.Toggle("Custom Size", customSize.boolValue);
                if (customSize.boolValue)
                {
                    if (bestFit.boolValue)
                        bestFit.boolValue = false;

                    EditorGUI.indentLevel++;
                    SerializedProperty size = textSkin.FindPropertyRelative("size");
                    EditorGUILayout.PropertyField(size, new GUIContent("Size"));
                    EditorGUI.indentLevel--;
                }

                bestFit.boolValue = EditorGUILayout.Toggle("Best Fit Size", bestFit.boolValue);
                if (bestFit.boolValue)
                {
                    if (customSize.boolValue)
                        customSize.boolValue = false;

                    EditorGUI.indentLevel++;
                    SerializedProperty bestFitSize = textSkin.FindPropertyRelative("bestFitSize");
                    DrawBestFitSize(bestFitSize);
                    EditorGUI.indentLevel--;
                }

                EditorGUIUtility.labelWidth -= 40;
                EditorGUI.indentLevel--;
            }
        }

        void DrawBestFitSize(SerializedProperty bestFit)
        {
            SerializedProperty min = bestFit.FindPropertyRelative("min");
            SerializedProperty max = bestFit.FindPropertyRelative("max");

            min.intValue = EditorGUILayout.IntField("Minimum", min.intValue);
            max.intValue = EditorGUILayout.IntField("Maximum", max.intValue);
        }

        void DrawTogglableValue(SerializedProperty togglable)
        {

        }
    }
}