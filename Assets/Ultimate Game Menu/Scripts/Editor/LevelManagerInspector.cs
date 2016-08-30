using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UGM
{
    [CustomEditor(typeof(LevelManager))]
    public class LevelManagerInspector : Editor
    {
        SerializedProperty transitionScene;
        SerializedProperty transitionScenePath;

        SerializedProperty loadEndNote;

        void OnEnable()
        {
            transitionScene = serializedObject.FindProperty("transitionScene");
            transitionScenePath = serializedObject.FindProperty("transitionScenePath");

            loadEndNote = serializedObject.FindProperty("loadEndNote");

            if(transitionScenePath.stringValue != "")
                InspectorTools.AddSceneToBuild(new EditorBuildSettingsScene(transitionScenePath.stringValue, true), -1);
        }

        public override void OnInspectorGUI()
        {
            InspectorTools.DrawSceneField("Transition Scene", ref transitionScene, ref transitionScenePath);

            EditorGUILayout.LabelField("Loading End Note");
            loadEndNote.stringValue = EditorGUILayout.TextArea(loadEndNote.stringValue);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
