using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UGM
{
    [CustomEditor(typeof(AudioManager))]
    public class AudioManagerInspector : Editor
    {
        SerializedProperty fileName;
        SerializedProperty audioChannels;
        SerializedProperty autoCreateUI;

        int channelIndex = -1;

        void OnEnable()
        {
            fileName = serializedObject.FindProperty("fileName");
            audioChannels = serializedObject.FindProperty("audioChannels");
            autoCreateUI = serializedObject.FindProperty("autoCreateUI");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            fileName.stringValue = EditorGUILayout.TextField("File Name", fileName.stringValue);

            autoCreateUI.boolValue = EditorGUILayout.Toggle("Auto Create UI", autoCreateUI.boolValue);

            audioChannels.isExpanded = EditorGUILayout.Foldout(audioChannels.isExpanded, "Audio Channels");

            if (audioChannels.isExpanded)
            {
                EditorGUI.indentLevel++;

                audioChannels.arraySize = EditorGUILayout.IntField("Size", audioChannels.arraySize);
                audioChannels.arraySize = InspectorTools.DrawArraySizeButtons(audioChannels.arraySize, 1, 1);

                if (channelIndex > audioChannels.arraySize - 1)
                {
                    channelIndex = -1;
                }

                for (int i = 0; i < audioChannels.arraySize; i++)
                {
                    SerializedProperty audioChannel = audioChannels.GetArrayElementAtIndex(i);

                    SerializedProperty name = audioChannel.FindPropertyRelative("name");

                    string nameLabel = name.stringValue;

                    if (nameLabel == "")
                        nameLabel = "Channel Name";

                    EditorGUILayout.BeginHorizontal();

                    audioChannel.isExpanded = EditorGUILayout.Foldout(audioChannel.isExpanded, nameLabel);

                    if(channelIndex == i)
                    {
                        if(GUILayout.Button("Deselect"))
                        {
                            channelIndex = -1;
                        }
                    }
                    else if (GUILayout.Button("Select"))
                        channelIndex = i;

                    EditorGUILayout.EndHorizontal();
                    if (audioChannel.isExpanded)
                    {
                        InspectorTools.Space();
                        SerializedProperty volume = audioChannel.FindPropertyRelative("volume");

                        name.stringValue = EditorGUILayout.TextField("Name", name.stringValue);
                        volume.floatValue = EditorGUILayout.Slider("Volume", volume.floatValue, 0, 1);
                    }
                }
                
                if(channelIndex != -1)
                {
                    int index = InspectorTools.DrawArryElementModifiers(channelIndex, audioChannels.arraySize, 1, 1);

                    if(index == -1)
                    {
                        audioChannels.DeleteArrayElementAtIndex(channelIndex);
                        channelIndex = -1;
                    }

                    if (index != channelIndex)
                    {
                        audioChannels.MoveArrayElement(channelIndex, index);
                        channelIndex = index;
                    }
                }

                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
