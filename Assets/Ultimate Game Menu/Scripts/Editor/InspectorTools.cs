using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Collections.Generic;

namespace UGM
{
    public static class InspectorTools
    {
        public static Array keyCodes;

        public static void Space(uint count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                EditorGUILayout.Space();
            }
        }

        public static int DrawArraySizeButtons(int arraySize, uint ButtonSpacing = 1, uint bottomSpace = 1)
        {
            EditorGUILayout.BeginHorizontal();

            Space(ButtonSpacing);

            if (GUILayout.Button("-"))
            {
                if (arraySize > 0)
                    arraySize--;
            }
            if (GUILayout.Button("+"))
            {
                arraySize++;
            }

            Space(ButtonSpacing);

            EditorGUILayout.EndHorizontal();

            Space(bottomSpace);

            return arraySize;
        }

        public static int DrawArryElementModifiers(int currentIndex, int arraySize, uint ButtonSpacing = 1, uint bottomSpace = 1)
        {
            EditorGUILayout.BeginHorizontal();

            Space(ButtonSpacing);

            if (GUILayout.Button("Up"))
            {
                if (currentIndex != 0)
                    currentIndex--;
            }
            if (GUILayout.Button("Down"))
            {
                if (currentIndex != arraySize - 1)
                    currentIndex++;
            }
            if (GUILayout.Button("X"))
            {
                currentIndex = -1;
            }

            Space(ButtonSpacing);

            EditorGUILayout.EndHorizontal();

            Space(bottomSpace);

            return currentIndex;
        }

        public static InspectorBindResponse GetBind()
        {
            if (keyCodes == null)
                keyCodes = System.Enum.GetValues(typeof(KeyCode));

            KeyCode keyCode = Event.current.keyCode;
            int keyCodeIndex = 0;

            if(keyCode != KeyCode.None)
            {
                for (int i = 0; i < keyCodes.Length; i++)
                {
                    if ((KeyCode)keyCodes.GetValue(i) == keyCode)
                        keyCodeIndex = i;
                }
            }

            return new InspectorBindResponse(keyCode, keyCodeIndex);
        }

        public static void AddSceneToBuild(EditorBuildSettingsScene scene, int index = -1)
        {
            if (!SceneValid(scene))
                return;

            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();

            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                if (i == index)
                    scenes.Add(scene);

                scenes.Add(EditorBuildSettings.scenes[i]);
            }

            if (index == -1)
                scenes.Add(scene);

            EditorBuildSettings.scenes = scenes.ToArray();
        }

        public static bool BuildContainsScene(EditorBuildSettingsScene scene)
        {
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                if (scene.path == EditorBuildSettings.scenes[i].path)
                    return true;
            }

            return false;
        }

        public static bool SceneValid(EditorBuildSettingsScene scene)
        {
            if (BuildContainsScene(scene) || scene.path == "")
                return false;

            SceneAsset asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);

            if (asset == null)
                return false;

            return true;
        }

        public static bool DrawSceneField(string label, ref SerializedProperty name, ref SerializedProperty path, int preferedAddIndex = -1)
        {
            SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(path.stringValue);

            EditorGUI.BeginChangeCheck();

            scene = (SceneAsset)EditorGUILayout.ObjectField(label, scene, typeof(SceneAsset), false);

            if (EditorGUI.EndChangeCheck())
            {
                if (scene != null)
                {
                    name.stringValue = scene.name;
                    path.stringValue = AssetDatabase.GetAssetPath(scene);

                    AddSceneToBuild(new EditorBuildSettingsScene(path.stringValue, true), preferedAddIndex);
                }
                else
                {
                    name.stringValue = "";
                    path.stringValue = "";
                }

                return true;
            }

            return false;
        }
    }

    public class  InspectorBindResponse
    {
        public KeyCode keyCode;
        public int KeyCodeIndex;

        public InspectorBindResponse(KeyCode newKeyCode, int newKeyCodeIndex)
        {
            keyCode = newKeyCode;
            KeyCodeIndex = newKeyCodeIndex;
        }
    }
}