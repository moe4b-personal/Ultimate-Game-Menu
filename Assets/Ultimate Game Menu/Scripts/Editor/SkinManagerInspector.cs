using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UGM
{
    [CustomEditor(typeof(SkinManager))]
    public class SkinManagerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Apply Skin To Visible"))
            {
                SkinManager man = (SkinManager)target;
                man.ApplyToVisible();
            }
        }
    }
}
