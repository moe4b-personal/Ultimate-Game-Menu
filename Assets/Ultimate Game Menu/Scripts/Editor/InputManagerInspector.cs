using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UGM
{
    [CustomEditor(typeof(InputManager))]
    public class InputManagerInspector : Editor
    {
        SerializedProperty fileName;
        SerializedProperty binds;
        SerializedProperty autoCreateUI;

        int bindsIndex = -1;
        BindIndex bindIndex = new BindIndex(-1,-1);

        bool binding;
        BindKeyIndex currentIndex = new BindKeyIndex(0, 0, 0);
        SerializedProperty currentBind;

        void OnEnable()
        {
            fileName = serializedObject.FindProperty("fileName");
            binds = serializedObject.FindProperty("binds");
            autoCreateUI = serializedObject.FindProperty("autoCreateUI");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            if (binding)
                Bind();

            fileName.stringValue = EditorGUILayout.TextField("File Name", fileName.stringValue);

            autoCreateUI.boolValue = EditorGUILayout.Toggle("Auto Create UI", autoCreateUI.boolValue);

            binds.isExpanded = EditorGUILayout.Foldout(binds.isExpanded, "Binds");

            if (binds.isExpanded)
            {
                EditorGUI.indentLevel++;

                binds.arraySize = EditorGUILayout.IntField("Size", binds.arraySize);

                binds.arraySize = InspectorTools.DrawArraySizeButtons(binds.arraySize, 1, 1);

                if (bindsIndex > binds.arraySize - 1)
                {
                    bindsIndex = -1;
                }

                for (int x = 0; x < binds.arraySize; x++)
                {
                    SerializedProperty bindsElement = binds.GetArrayElementAtIndex(x);
                    DrawBinds(bindsElement, x);
                }

                if (bindsIndex != -1)
                {
                    int index = InspectorTools.DrawArryElementModifiers(bindsIndex, binds.arraySize, 1, 1);

                    if (index == -1)
                    {
                        binds.DeleteArrayElementAtIndex(bindsIndex);
                        bindsIndex = -1;
                    }

                    if (index != bindsIndex)
                    {
                        binds.MoveArrayElement(bindsIndex, index);
                        bindsIndex = index;
                    }
                }

                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }

        void Bind()
        {
            if (!binding)
                return;

            InspectorBindResponse response = InspectorTools.GetBind();

            if(response.keyCode != KeyCode.None)
            {
                currentBind.enumValueIndex = response.KeyCodeIndex;
                currentBind = null;
                currentIndex = null;
                binding = false;
            }
        }

        public void DrawBinds(SerializedProperty bindsElement, int currentIndex)
        {
            SerializedProperty categoryName = bindsElement.FindPropertyRelative("categoryName");
            SerializedProperty bindsArray = bindsElement.FindPropertyRelative("binds");

            string categoryNameLabel = categoryName.stringValue;
            if (categoryNameLabel == "")
                categoryNameLabel = "Category " + currentIndex;

            EditorGUILayout.BeginHorizontal();

            bindsArray.isExpanded = EditorGUILayout.Foldout(bindsArray.isExpanded, categoryNameLabel);

            if (bindsIndex == currentIndex)
            {
                if (GUILayout.Button("Deselect"))
                {
                    bindsIndex = -1;
                }
            }
            else if (GUILayout.Button("Select"))
                bindsIndex = currentIndex;

            EditorGUILayout.EndHorizontal();

            if (bindsArray.isExpanded)
            {
                InspectorTools.Space();
                categoryName.stringValue = EditorGUILayout.TextField("Name", categoryName.stringValue);

                bindsArray.arraySize = EditorGUILayout.IntField("Size", bindsArray.arraySize);

                bindsArray.arraySize = InspectorTools.DrawArraySizeButtons(bindsArray.arraySize, 1, 1);

                if (bindIndex.bind > bindsArray.arraySize - 1 && bindIndex.binds == currentIndex)
                {
                    bindIndex = new BindIndex(-1,-1);
                }

                EditorGUI.indentLevel+=2;
                for (int i = 0; i < bindsArray.arraySize; i++)
                {
                    SerializedProperty bind = bindsArray.GetArrayElementAtIndex(i);
                    DrawBind(bind, currentIndex ,i);

                    if (i == bindsArray.arraySize - 1)
                        InspectorTools.Space(1);
                }
                EditorGUI.indentLevel-=2;

                if (bindIndex.bind != -1 && bindIndex.binds == currentIndex)
                {
                    int index = InspectorTools.DrawArryElementModifiers(bindIndex.bind, bindsArray.arraySize, 1, 1);

                    if (index == -1)
                    {
                        bindsArray.DeleteArrayElementAtIndex(bindIndex.binds);
                        bindIndex = new BindIndex(-1,-1);
                    }

                    if (index != bindIndex.bind)
                    {
                        bindsArray.MoveArrayElement(bindIndex.bind, index);
                        bindIndex.bind = index;
                    }
                }
            }
        }

        public void DrawBind(SerializedProperty bind, int parentIndex, int currentIndex)
        {
            SerializedProperty name = bind.FindPropertyRelative("name");

            string bindName = name.stringValue;

            if (bindName == "")
                bindName = "Bind Name";

            EditorGUILayout.BeginHorizontal();

            bind.isExpanded = EditorGUILayout.Foldout(bind.isExpanded, bindName);

            InspectorTools.Space(2);

            if (bindIndex.bind == currentIndex && bindIndex.binds == parentIndex)
            {
                if (GUILayout.Button("Deselect"))
                {
                    bindIndex = new BindIndex(-1, -1);
                }
            }
            else if (GUILayout.Button("Select"))
            {
                bindIndex = new BindIndex(currentIndex, parentIndex);
            }

            EditorGUILayout.EndHorizontal();

            if (bind.isExpanded)
            {
                InspectorTools.Space();
                SerializedProperty primary = bind.FindPropertyRelative("primary");
                SerializedProperty secondary = bind.FindPropertyRelative("secondary");
                SerializedProperty extra = bind.FindPropertyRelative("extra");

                name.stringValue = EditorGUILayout.TextField("Name", name.stringValue);

                DrawKeyCode(primary, "Primary", new BindKeyIndex(parentIndex, currentIndex, 1));
                DrawKeyCode(secondary, "Secondary", new BindKeyIndex(parentIndex, currentIndex, 2));
                DrawKeyCode(extra, "Extra", new BindKeyIndex(parentIndex, currentIndex, 3));

                InspectorTools.Space();
            }
        }

        public void DrawKeyCode(SerializedProperty key, string label, BindKeyIndex index)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(key, new GUIContent(label));

            if(currentBind != null && currentIndex != null && index.Cat == currentIndex.Cat && index.Bind == currentIndex.Bind && index.Key == currentIndex.Key)
            {
                if(GUILayout.Button("Cancel"))
                {
                    binding = false;
                    currentBind = null;
                    currentIndex = null;
                }
            }
            else
            {
                if(GUILayout.Button("Bind"))
                {
                    binding = true;
                    currentIndex = index;
                    currentBind = key;
                }
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    public class BindIndex
    {
        public int binds;
        public int bind;

        public BindIndex(int newBind, int newBinds)
        {
            binds = newBinds;
            bind = newBind;
        }
    }
}