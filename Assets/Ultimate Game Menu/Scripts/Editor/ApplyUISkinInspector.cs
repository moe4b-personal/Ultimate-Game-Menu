using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UGM
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ApplyUISkin))]
    public class ApplyUISkinInspector : Editor
    {
        SerializedProperty category;
        SerializedProperty overrideUI;
        SerializedProperty uiType;

        SerializedProperty imageUI;
        SerializedProperty textUI;
        SerializedProperty buttonUI;
        SerializedProperty toggleUI;
        SerializedProperty sliderUI;
        SerializedProperty scrollbarUI;
        SerializedProperty inputFieldUI;
        SerializedProperty dropDownUI;

        ApplyUISkin comp;

        void OnEnable()
        {
            category = serializedObject.FindProperty("category");
            overrideUI = serializedObject.FindProperty("overrideUI");
            uiType = serializedObject.FindProperty("uiType");

            textUI = serializedObject.FindProperty("textUI");
            imageUI = serializedObject.FindProperty("imageUI");

            buttonUI = serializedObject.FindProperty("buttonUI");
            toggleUI = serializedObject.FindProperty("toggleUI");
            sliderUI = serializedObject.FindProperty("sliderUI");
            scrollbarUI = serializedObject.FindProperty("scrollbarUI");
            inputFieldUI = serializedObject.FindProperty("inputfieldUI");
            dropDownUI = serializedObject.FindProperty("dropdownUI");

            comp = (ApplyUISkin)target;

            if (!overrideUI.boolValue)
                comp.UpdateUI();
        }

        public override void OnInspectorGUI()
        {
            category.stringValue = EditorGUILayout.TextField("Category", category.stringValue);

            overrideUI.boolValue = EditorGUILayout.Toggle("Override UI", overrideUI.boolValue);

            if (overrideUI.boolValue)
            {
                if (targets.Length == 1)
                {
                    ApplyUISkin.UI uiTypeE = (ApplyUISkin.UI)uiType.enumValueIndex;
                    uiTypeE = (ApplyUISkin.UI)EditorGUILayout.EnumPopup(new GUIContent("UI"), uiTypeE);
                    uiType.enumValueIndex = (int)uiTypeE;

                    switch (uiTypeE)
                    {
                        case ApplyUISkin.UI.Image:
                            EditorGUILayout.PropertyField(imageUI, new GUIContent("Button UI"), true);
                            break;
                        case ApplyUISkin.UI.Text:
                            EditorGUILayout.PropertyField(textUI, new GUIContent("Button UI"), true);
                            break;
                        case ApplyUISkin.UI.Button:
                            EditorGUILayout.PropertyField(buttonUI, new GUIContent("Button UI"), true);
                            break;
                        case ApplyUISkin.UI.Toggle:
                            EditorGUILayout.PropertyField(toggleUI, new GUIContent("Toggle UI"), true);
                            break;
                        case ApplyUISkin.UI.Slider:
                            EditorGUILayout.PropertyField(sliderUI, new GUIContent("Slider UI"), true);
                            break;
                        case ApplyUISkin.UI.Scrollbar:
                            EditorGUILayout.PropertyField(scrollbarUI, new GUIContent("ScrollBar UI"), true);
                            break;
                        case ApplyUISkin.UI.InputField:
                            EditorGUILayout.PropertyField(inputFieldUI, new GUIContent("InputField UI"), true);
                            break;
                        case ApplyUISkin.UI.DropDown:
                            EditorGUILayout.PropertyField(dropDownUI, new GUIContent("DropDown UI"), true);
                            break;
                    }
                }

                if (GUILayout.Button("Update UI"))
                {
                    for (int i = 0; i < targets.Length; i++)
                    {
                        ApplyUISkin comp = (ApplyUISkin)targets[i];
                        comp.UpdateUI();
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}