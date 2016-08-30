using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UGM
{
    [CustomEditor(typeof(GameMenu))]
    public class GameMenuInspector : Editor
    {
        SerializedProperty settingsFileName;
        SerializedProperty cancelKey;
        SerializedProperty noneKey;
        SerializedProperty pauseKey;
        SerializedProperty menuSettings;
        SerializedProperty minResolution;
        SerializedProperty exitB;
        SerializedProperty newGameB;
        SerializedProperty controlsScroll;
        SerializedProperty audioChannelScroll;
        SerializedProperty bindPopup;
        SerializedProperty conflictPopup;

        SerializedProperty directLevelLoad;

        SerializedProperty volumeSl;
        SerializedProperty shadowsD;
        SerializedProperty shadowsQualityD;
        SerializedProperty anFilteringD;
        SerializedProperty vSyncD;
        SerializedProperty realTimeRefTog;
        SerializedProperty antiAliasingD;
        SerializedProperty resolutionD;
        SerializedProperty fullScreenTog;
        SerializedProperty optionsBackB;
        SerializedProperty bindTemplate;
        SerializedProperty categoryTemplate;
        SerializedProperty audioChannelTemplate;
        SerializedProperty levelTemplate;
        SerializedProperty menuType;

        SerializedProperty levels;
        SerializedProperty levelsScroll;
        SerializedProperty levelPreview;

        SerializedProperty mainMenuLevel;
        SerializedProperty pauseMenu;
        SerializedProperty gameUI;
        SerializedProperty resumeB;
        SerializedProperty returnToMainB;

        //Expansion Fields
        SerializedProperty initExpanded;
        SerializedProperty uiExpanded;
        SerializedProperty templatesExpanded;
        SerializedProperty menuExpanded;

        int levelIndex = -1;

        Menu menuTypeE;

        bool binding = false;
        SerializedProperty currentBind;
        int bindIndex = -1;

        void OnEnable()
        {
            GetProperties();
            CheckOrAddScenes();
        }

        void CheckOrAddScenes()
        {
            Menu menu = (Menu)menuType.enumValueIndex;

            if(menu == Menu.Main)
            {
                SerializedProperty levelsArr = levels.FindPropertyRelative("levels");
                for (int i = 0; i < levelsArr.arraySize; i++)
                {
                    SerializedProperty levelPath = levelsArr.GetArrayElementAtIndex(i).FindPropertyRelative("scenePath");
                    InspectorTools.AddSceneToBuild(new EditorBuildSettingsScene(levelPath.stringValue, true));
                }
            }
            else
            {
                SerializedProperty mainMenuLevelPath = mainMenuLevel.FindPropertyRelative("scenePath");
                InspectorTools.AddSceneToBuild(new EditorBuildSettingsScene(mainMenuLevelPath.stringValue, true), 0);
            }
        }

        void GetProperties()
        {
            settingsFileName = serializedObject.FindProperty("settingsFileName");
            cancelKey = serializedObject.FindProperty("cancelKey");
            noneKey = serializedObject.FindProperty("noneKey");
            pauseKey = serializedObject.FindProperty("pauseKey");
            menuSettings = serializedObject.FindProperty("menuSettings");
            minResolution = serializedObject.FindProperty("minResolution");
            exitB = serializedObject.FindProperty("exitB");
            newGameB = serializedObject.FindProperty("newGameB");
            controlsScroll = serializedObject.FindProperty("controlsScroll");
            audioChannelScroll = serializedObject.FindProperty("audioChannelScroll");
            bindPopup = serializedObject.FindProperty("bindPopup");
            conflictPopup = serializedObject.FindProperty("conflictPopup");

            directLevelLoad = serializedObject.FindProperty("directLevelLoad");

            volumeSl = serializedObject.FindProperty("volumeSl");
            shadowsD = serializedObject.FindProperty("shadowsD");
            shadowsQualityD = serializedObject.FindProperty("shadowsQualityD");
            anFilteringD = serializedObject.FindProperty("anFilteringD");
            vSyncD = serializedObject.FindProperty("vSyncD");
            realTimeRefTog = serializedObject.FindProperty("realTimeRefTog");
            antiAliasingD = serializedObject.FindProperty("antiAliasingD");
            resolutionD = serializedObject.FindProperty("resolutionD");
            fullScreenTog = serializedObject.FindProperty("fullScreenTog");
            optionsBackB = serializedObject.FindProperty("optionsBackB");
            bindTemplate = serializedObject.FindProperty("bindTemplate");
            categoryTemplate = serializedObject.FindProperty("categoryTemplate");
            audioChannelTemplate = serializedObject.FindProperty("audioChannelTemplate");
            levelTemplate = serializedObject.FindProperty("levelTemplate");
            menuType = serializedObject.FindProperty("menuType");

            levels = serializedObject.FindProperty("levels");
            levelsScroll = serializedObject.FindProperty("levelsScroll");
            levelPreview = serializedObject.FindProperty("levelPreview");

            mainMenuLevel = serializedObject.FindProperty("mainMenuLevel");
            pauseMenu = serializedObject.FindProperty("pauseMenu");
            gameUI = serializedObject.FindProperty("gameUI");
            resumeB = serializedObject.FindProperty("resumeB");
            returnToMainB = serializedObject.FindProperty("returnToMainB");

            initExpanded = serializedObject.FindProperty("initExpanded");
            uiExpanded = serializedObject.FindProperty("uiExpanded");
            templatesExpanded = serializedObject.FindProperty("templatesExpanded");
            menuExpanded = serializedObject.FindProperty("menuExpanded");
        }

        public override void OnInspectorGUI()
        {
            if (binding)
                Bind();

            InspectorTools.Space(1);

            initExpanded.boolValue = EditorGUILayout.Foldout(initExpanded.boolValue, "Init");
            if (initExpanded.boolValue)
            {
                EditorGUI.indentLevel++;
                DrawInitRegion();
                //InspectorTools.Space(2);
                EditorGUI.indentLevel--;
            }

            uiExpanded.boolValue = EditorGUILayout.Foldout(uiExpanded.boolValue, "UI");
            if (uiExpanded.boolValue)
            {
                EditorGUI.indentLevel++;
                DrawUIRegion();
                //InspectorTools.Space(2);
                EditorGUI.indentLevel--;
            }

            templatesExpanded.boolValue = EditorGUILayout.Foldout(templatesExpanded.boolValue, "Templates");
            if (templatesExpanded.boolValue)
            {
                EditorGUI.indentLevel++;
                DrawTemplatesRegion();
                //InspectorTools.Space(2);
                EditorGUI.indentLevel--;
            }

            menuExpanded.boolValue = EditorGUILayout.Foldout(menuExpanded.boolValue, "Menu Properties");

            if (menuExpanded.boolValue)
            {
                EditorGUI.indentLevel++;
                DrawMenuRegion();
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }

        void DrawInitRegion()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Direct Level Loading");
            directLevelLoad.boolValue = EditorGUILayout.Toggle(directLevelLoad.boolValue);

            EditorGUILayout.EndHorizontal();

            DrawSimpleResolution(minResolution, "Minimum Resolution", 0);

            DrawMenuSettings(menuSettings, "Menu Settings");
            DrawKeyCode(cancelKey, "Cancel Key", 1);
            DrawKeyCode(noneKey, "None Key", 2);
            DrawKeyCode(pauseKey, "Pause Key", 3);
        }

        void DrawUIRegion()
        {
            EditorGUILayout.PropertyField(exitB, new GUIContent("Exit"));
            EditorGUILayout.PropertyField(controlsScroll, new GUIContent("Controls"));
            EditorGUILayout.PropertyField(audioChannelScroll, new GUIContent("Audio Channels"));
            EditorGUILayout.PropertyField(bindPopup, new GUIContent("Bind Popup"));
            EditorGUILayout.PropertyField(conflictPopup, new GUIContent("Conflict Popup"));

            EditorGUILayout.PropertyField(volumeSl, new GUIContent("Volume"));
            EditorGUILayout.PropertyField(shadowsD, new GUIContent("Shadows"));
            EditorGUILayout.PropertyField(shadowsQualityD, new GUIContent("Shadows Quality"));
            EditorGUILayout.PropertyField(anFilteringD, new GUIContent("AN Filtering"));
            EditorGUILayout.PropertyField(vSyncD, new GUIContent("V Sync"));
            EditorGUILayout.PropertyField(realTimeRefTog, new GUIContent("RealTime Reflections"));
            EditorGUILayout.PropertyField(antiAliasingD, new GUIContent("AntiAliasing"));
            EditorGUILayout.PropertyField(resolutionD, new GUIContent("Resolution"));
            EditorGUILayout.PropertyField(fullScreenTog, new GUIContent("Full Screen"));
            EditorGUILayout.PropertyField(optionsBackB, new GUIContent("Options Menu Back Button"));
        }

        void DrawTemplatesRegion()
        {
            EditorGUILayout.PropertyField(audioChannelTemplate, new GUIContent("Audio Channel"));
            EditorGUILayout.PropertyField(categoryTemplate, new GUIContent("Bind Category"));
            EditorGUILayout.PropertyField(bindTemplate, new GUIContent("Bind"));
            if(menuTypeE == Menu.Main)
                EditorGUILayout.PropertyField(levelTemplate, new GUIContent("Level Template"));
        }

        void DrawMenuRegion()
        {
            menuTypeE = (Menu)menuType.enumValueIndex;
            menuTypeE = (Menu)EditorGUILayout.EnumPopup("Menu Type", menuTypeE);
            menuType.enumValueIndex = (int)menuTypeE;

            if (menuTypeE == Menu.Main)
            {
                EditorGUILayout.PropertyField(newGameB, new GUIContent("New Game"));

                levels.isExpanded = EditorGUILayout.Foldout(levels.isExpanded, "Levels");

                if(levels.isExpanded)
                {
                    SerializedProperty levelsArray = levels.FindPropertyRelative("levels");
                    SerializedProperty autoCreateUI = levels.FindPropertyRelative("autoCreateUI");

                    autoCreateUI.boolValue = EditorGUILayout.Toggle("Auto Create UI", autoCreateUI.boolValue);

                    levelsArray.arraySize = EditorGUILayout.IntField("Size", levelsArray.arraySize);

                    levelsArray.arraySize = InspectorTools.DrawArraySizeButtons(levelsArray.arraySize, 1, 1);

                    if (levelIndex > levelsArray.arraySize - 1)
                    {
                        levelIndex = -1;
                    }

                    EditorGUI.indentLevel+=2;
                    for (int i = 0; i < levelsArray.arraySize; i++)
                    {
                        SerializedProperty level = levelsArray.GetArrayElementAtIndex(i);
                        string name = level.FindPropertyRelative("name").stringValue;

                        if (name == "")
                            name = "Level Name";

                        DrawGameLevel(level, name, 2, true, i);
                    }
                    EditorGUI.indentLevel-=2;

                    if (levelIndex != -1)
                    {
                        int index = InspectorTools.DrawArryElementModifiers(levelIndex, levelsArray.arraySize, 1, 1);

                        if (index == -1)
                        {
                            levelsArray.DeleteArrayElementAtIndex(levelIndex);
                            levelIndex = -1;
                        }

                        if (index != levelIndex)
                        {
                            levelsArray.MoveArrayElement(levelIndex, index);
                            levelIndex = index;
                        }
                    }
                }

                EditorGUILayout.PropertyField(levelsScroll, new GUIContent("Levels Scroll"));
                EditorGUILayout.PropertyField(levelPreview, new GUIContent("Level Preview"));
            }
            else
            {
                DrawGameLevel(mainMenuLevel, "Main Menu Level");

                EditorGUILayout.PropertyField(resumeB, new GUIContent("Resume Button"));
                EditorGUILayout.PropertyField(returnToMainB, new GUIContent("Main Menu Button", "Return To Main Menu Button"));

                EditorGUILayout.PropertyField(pauseMenu, new GUIContent("Pause Menu", "Pause Menu UI Object"));
                EditorGUILayout.PropertyField(gameUI, new GUIContent("Game UI", "Game UI Holding All UI Related To Gameplay"));
            }
        }

        public void DrawMenuSettings(SerializedProperty sett, string label, uint expandSpace = 0)
        {
            sett.isExpanded = EditorGUILayout.Foldout(sett.isExpanded, label);

            if(sett.isExpanded)
            {
                EditorGUI.indentLevel++;

                settingsFileName.stringValue = EditorGUILayout.TextField("File Name", settingsFileName.stringValue);

                SerializedProperty volume = sett.FindPropertyRelative("volume");
                SerializedProperty shadows = sett.FindPropertyRelative("shadows");
                SerializedProperty shadowsQuality = sett.FindPropertyRelative("shadowsQuality");
                SerializedProperty antiAliasing = sett.FindPropertyRelative("antiAliasing");
                SerializedProperty anisotropicFiltering = sett.FindPropertyRelative("anisotropicFiltering");
                SerializedProperty realTimeReflections = sett.FindPropertyRelative("realTimeReflections");
                SerializedProperty vSync = sett.FindPropertyRelative("vSync");
                SerializedProperty resolution = sett.FindPropertyRelative("resolution");
                SerializedProperty fullScreen = sett.FindPropertyRelative("fullScreen");

                MenuSettings.Shadow shadowsE = (MenuSettings.Shadow)shadows.enumValueIndex;
                MenuSettings.ShadowQuality shadowsQualityE = (MenuSettings.ShadowQuality)shadowsQuality.enumValueIndex;
                MenuSettings.AntiAliasing antiAliasingE = (MenuSettings.AntiAliasing)antiAliasing.enumValueIndex;
                AnisotropicFiltering AF = (AnisotropicFiltering)anisotropicFiltering.enumValueIndex;
                MenuSettings.VSync vSyncE = (MenuSettings.VSync)vSync.enumValueIndex;

                volume.floatValue = EditorGUILayout.Slider("Volume", volume.floatValue, 0, 1);

                shadowsE = (MenuSettings.Shadow)EditorGUILayout.EnumPopup("Shadows", shadowsE);

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("Shadows Quality");
                shadowsQualityE = (MenuSettings.ShadowQuality)EditorGUILayout.EnumPopup(shadowsQualityE);

                EditorGUILayout.EndHorizontal();

                antiAliasingE = (MenuSettings.AntiAliasing)EditorGUILayout.EnumPopup("AntiAliasing", antiAliasingE);

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("Anisotropic Filtering");

                AF = (AnisotropicFiltering)EditorGUILayout.EnumPopup(AF);

                EditorGUILayout.EndHorizontal();

                shadows.enumValueIndex = (int)shadowsE;
                shadowsQuality.enumValueIndex = (int)shadowsQualityE;
                antiAliasing.enumValueIndex = (int)antiAliasingE;
                anisotropicFiltering.enumValueIndex = (int)AF;

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("RealTime Reflections");
                realTimeReflections.boolValue = EditorGUILayout.Toggle(realTimeReflections.boolValue);

                EditorGUILayout.EndHorizontal();
                
                vSyncE = (MenuSettings.VSync)EditorGUILayout.EnumPopup("V-Sync", vSyncE);
                vSync.enumValueIndex = (int)vSyncE;

                resolution.intValue = EditorGUILayout.IntField("Resolution Index", resolution.intValue);
                fullScreen.boolValue = EditorGUILayout.Toggle("Full Screen", fullScreen.boolValue);

                EditorGUI.indentLevel--;

                InspectorTools.Space(expandSpace);
            }
        }

        public void DrawKeyCode(SerializedProperty key, string label, int index)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(key, new GUIContent(label));

            if(bindIndex == index)
            {
                if(GUILayout.Button("Cancel"))
                {
                    currentBind = null;
                    bindIndex = -1;
                }
            }
            else if(GUILayout.Button("Bind"))
            {
                binding = true;
                bindIndex = index;
                currentBind = key;
            }

            EditorGUILayout.EndHorizontal();
        }

        void Bind()
        {
            if (!binding)
                return;

            InspectorBindResponse response = InspectorTools.GetBind();

            if (response.keyCode != KeyCode.None)
            {
                currentBind.enumValueIndex = response.KeyCodeIndex;
                currentBind = null;
                bindIndex = -1;
                binding = false;
            }
        }

        public void DrawGameLevel(SerializedProperty level, string label, uint spaceCount = 0, bool arrayMem = false, int currentIndex = 0)
        {
            EditorGUILayout.BeginHorizontal();

            level.isExpanded = EditorGUILayout.Foldout(level.isExpanded, label);

            InspectorTools.Space(2);

            if(arrayMem)
            {
                if (levelIndex == currentIndex)
                {
                    if (GUILayout.Button("Deselect"))
                    {
                        levelIndex = -1;
                    }
                }
                else if (GUILayout.Button("Select"))
                    levelIndex = currentIndex;
            }

            EditorGUILayout.EndHorizontal();

            if (arrayMem)
                InspectorTools.Space();

            if (level.isExpanded)
            {
                EditorGUI.indentLevel++;
                SerializedProperty name = level.FindPropertyRelative("name");

                SerializedProperty sceneName = level.FindPropertyRelative("sceneName");
                SerializedProperty scenePath = level.FindPropertyRelative("scenePath");

                SerializedProperty image = level.FindPropertyRelative("image");

                SerializedProperty description = level.FindPropertyRelative("description");

                if (scenePath.stringValue == "" || sceneName.stringValue == "")
                {
                    sceneName.stringValue = "";
                    scenePath.stringValue = "";
                }

                SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath.stringValue);

                EditorGUI.BeginChangeCheck();

                scene = (SceneAsset)EditorGUILayout.ObjectField("Scene", scene, typeof(SceneAsset), false);

                if (EditorGUI.EndChangeCheck())
                {
                    if(scene)
                    {
                        scenePath.stringValue = AssetDatabase.GetAssetPath(scene);
                        sceneName.stringValue = scene.name;

                        InspectorTools.AddSceneToBuild(new EditorBuildSettingsScene(scenePath.stringValue, true), 0);
                    }
                    else
                    {
                        sceneName.stringValue = "";
                        scenePath.stringValue = "";
                    }
                }

                EditorGUILayout.PropertyField(image, new GUIContent("Preview"));

                name.stringValue = EditorGUILayout.TextField("Name", name.stringValue);

                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Description");
                description.stringValue = EditorGUILayout.TextArea(description.stringValue);
                EditorGUILayout.EndVertical();

                InspectorTools.Space(spaceCount);
                EditorGUI.indentLevel--;
            }
        }

        public static void DrawSimpleResolution(SerializedProperty res, string label, uint spaceCount = 2)
        {
            res.isExpanded = EditorGUILayout.Foldout(res.isExpanded, label);

            if (res.isExpanded)
            {
                EditorGUI.indentLevel++;
                SerializedProperty width = res.FindPropertyRelative("width");
                SerializedProperty height = res.FindPropertyRelative("height");

                width.intValue = EditorGUILayout.IntField("Width", width.intValue);
                height.intValue = EditorGUILayout.IntField("Height", height.intValue);

                EditorGUI.indentLevel--;

                InspectorTools.Space(spaceCount);
            }
        }
    }
}