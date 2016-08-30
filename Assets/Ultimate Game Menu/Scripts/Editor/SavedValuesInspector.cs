using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

namespace UGM
{
    [CustomEditor(typeof(SavedValues))]
    public class SavedValuesInspector : Editor
    {
        SerializedProperty savedValues;

        SavedValues item;

        SavedValue.UI uiTypeE;
        SavedValue.InputFieldData infDataE;
        SavedValue.SliderData slDataE;

        int savedValueIndex = -1;

        void OnEnable()
        {
            savedValues = serializedObject.FindProperty("savedValues");
            item = (SavedValues)target;
        }

        public override void OnInspectorGUI()
        {
            InspectorTools.Space();
            savedValues.isExpanded = EditorGUILayout.Foldout(savedValues.isExpanded, "Saved Values");

            if(savedValues.isExpanded)
            {
                EditorGUI.indentLevel++;

                DrawArraySizer(savedValues);

                savedValues.arraySize = InspectorTools.DrawArraySizeButtons(savedValues.arraySize, 1, 1);

                if(savedValueIndex > savedValues.arraySize - 1)
                {
                    savedValueIndex = -1;
                }

                InspectorTools.Space();

                for (int i = 0; i < savedValues.arraySize; i++)
                {
                    SerializedProperty savedValue = savedValues.GetArrayElementAtIndex(i);
                    SerializedProperty name = savedValue.FindPropertyRelative("name");

                    string savedValueLabel = name.stringValue;
                    if (savedValueLabel == "")
                        savedValueLabel = "Saved Value " + i;

                    EditorGUILayout.BeginHorizontal();
                    savedValue.isExpanded = EditorGUILayout.Foldout(savedValue.isExpanded, savedValueLabel);

                    if (savedValueIndex == i)
                    {
                        if (GUILayout.Button("Deselect"))
                        {
                            savedValueIndex = -1;
                        }
                    }
                    else if (GUILayout.Button("Select"))
                        savedValueIndex = i;

                    EditorGUILayout.EndHorizontal();

                    if(savedValue.isExpanded)
                    {
                        InspectorTools.Space();
                        name.stringValue = EditorGUILayout.TextField(name.stringValue);

                        SerializedProperty uiType = savedValue.FindPropertyRelative("uiType");
                        uiTypeE = (SavedValue.UI)uiType.enumValueIndex;
                        uiTypeE = (SavedValue.UI)EditorGUILayout.EnumPopup("UI", uiTypeE);
                        uiType.enumValueIndex = (int)uiTypeE;

                        SerializedProperty ui = null;

                        SerializedProperty value;
                        switch (uiTypeE)
                        {
                            case SavedValue.UI.InputField: //Input Field
                                ui = savedValue.FindPropertyRelative("inf");

                                EditorGUILayout.PropertyField(ui, new GUIContent("InputField"));

                                SerializedProperty infData = savedValue.FindPropertyRelative("infData");
                                infDataE = (SavedValue.InputFieldData)infData.enumValueIndex;
                                infDataE = (SavedValue.InputFieldData)EditorGUILayout.EnumPopup("Data", infDataE);
                                infData.enumValueIndex = (int)infDataE;

                                switch (infDataE)
                                {
                                    case SavedValue.InputFieldData.Int: //Input Field Int

                                        value = savedValue.FindPropertyRelative("iValue");
                                        value.intValue = EditorGUILayout.IntField("Value", value.intValue);

                                        break;
                                    case SavedValue.InputFieldData.Float: //Input Field Float

                                        value = savedValue.FindPropertyRelative("fValue");
                                        value.floatValue = EditorGUILayout.FloatField("Value", value.floatValue);

                                        break;
                                    case SavedValue.InputFieldData.String:  //Input Field String

                                        value = savedValue.FindPropertyRelative("sValue");
                                        value.stringValue = EditorGUILayout.TextField("Value", value.stringValue);

                                        break;
                                }

                                break;
                            case SavedValue.UI.Slider: //Slider
                                ui = savedValue.FindPropertyRelative("sl");
                                EditorGUILayout.PropertyField(ui, new GUIContent("Slider"));

                                SerializedProperty slData = savedValue.FindPropertyRelative("slData");
                                slDataE = (SavedValue.SliderData)slData.enumValueIndex;
                                slDataE = (SavedValue.SliderData)EditorGUILayout.EnumPopup("Data", slDataE);
                                slData.enumValueIndex = (int)slDataE;

                                switch (slDataE)
                                {
                                    case SavedValue.SliderData.Int: //slider int

                                        value = savedValue.FindPropertyRelative("iValue");
                                        value.intValue = EditorGUILayout.IntField("Value", value.intValue);

                                        break;
                                    case SavedValue.SliderData.Float: //slider float

                                        value = savedValue.FindPropertyRelative("fValue");
                                        value.floatValue = EditorGUILayout.FloatField("Value", value.floatValue);

                                        break;
                                }

                                break;
                            case SavedValue.UI.Toggle:
                                ui = savedValue.FindPropertyRelative("togg");
                                EditorGUILayout.PropertyField(ui, new GUIContent("Toggle"));

                                value = savedValue.FindPropertyRelative("bValue");
                                value.boolValue = EditorGUILayout.Toggle("Value", value.boolValue);

                                break;
                            case SavedValue.UI.DropDown:
                                ui = savedValue.FindPropertyRelative("drop");
                                EditorGUILayout.PropertyField(ui, new GUIContent("DropDown"));

                                value = savedValue.FindPropertyRelative("iValue");
                                value.intValue = EditorGUILayout.IntField("Value", value.intValue);

                                break;
                        }
                        InspectorTools.Space(1);
                    }
                }

                EditorGUI.indentLevel--;

                if (savedValueIndex != -1)
                {
                    int index = InspectorTools.DrawArryElementModifiers(savedValueIndex, savedValues.arraySize, 1, 1);

                    if (index == -1)
                    {
                        savedValues.DeleteArrayElementAtIndex(savedValueIndex);
                        savedValueIndex = -1;
                    }

                    else if (index != savedValueIndex)
                    {
                        savedValues.MoveArrayElement(savedValueIndex, index);
                        savedValueIndex = index;
                    }
                }
            }

            if (GUILayout.Button("Delete Saved Values"))
                item.DeleteAll();

            serializedObject.ApplyModifiedProperties();
        }

        void DrawArraySizer(SerializedProperty property, string label = "Size")
        {
            property.arraySize = EditorGUILayout.IntField(label, property.arraySize);
        }
    }
}
