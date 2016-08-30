using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace UGM
{
    public class GameMenu : MonoBehaviour
    {
        static GameMenu current;
        static public GameMenu Current { get { return current; } }

        [SerializeField]
        string settingsFileName = "Settings.xml";

        [SerializeField]
        bool directLevelLoad = false;

        [SerializeField]
        MenuSettings menuSettings = new MenuSettings();
        static MenuSettings menuSettings_s = new MenuSettings();

        [SerializeField]
        KeyCode cancelKey = KeyCode.Backspace;
        [SerializeField]
        KeyCode noneKey = KeyCode.Delete;
        [SerializeField]
        KeyCode pauseKey = KeyCode.Escape;

        [SerializeField]
        SimpleResolution minResolution;

        [SerializeField]
        Button exitB;
        [SerializeField]
        Button newGameB;
        [SerializeField]
        ScrollRect controlsScroll;
        [SerializeField]
        ScrollRect audioChannelScroll;

        [SerializeField]
        GameObject bindPopup;
        Text popupText;

        [SerializeField]
        GameObject conflictPopup;
        Text conflictPopupText;

        [SerializeField]
        Slider volumeSl;
        [SerializeField]
        Dropdown shadowsD;
        [SerializeField]
        Dropdown shadowsQualityD;
        [SerializeField]
        Dropdown anFilteringD;
        [SerializeField]
        Dropdown vSyncD;
        [SerializeField]
        Toggle realTimeRefTog;
        [SerializeField]
        Dropdown antiAliasingD;
        [SerializeField]
        Dropdown resolutionD;
        [SerializeField]
        Toggle fullScreenTog;
        [SerializeField]
        Button optionsBackB;

        //Templates
        [SerializeField]
        GameObject bindTemplate;
        [SerializeField]
        GameObject categoryTemplate;
        [SerializeField]
        GameObject audioChannelTemplate;
        [SerializeField]
        GameObject levelTemplate;

        [Space()]
        [SerializeField]
        Menu menuType;

        //main menu properties
        [SerializeField]
        Levels levels;
        [SerializeField]
        ScrollRect levelsScroll;
        [SerializeField]
        GameObject levelPreview;

        Text levelNameT;
        Image levelImage;
        Text levelDescriptionT;
        Button playLevelB;

        //in-game menu properties
        [SerializeField]
        GameLevel mainMenuLevel;
        [SerializeField]
        GameObject pauseMenu;
        [SerializeField]
        GameObject gameUI;

        [SerializeField]
        Button resumeB;
        [SerializeField]
        Button returnToMainB;

        //generic values
        static string settingsSavePath;

        static bool init = false; //determines if this is the first time running this script

        static bool paused;
        public static bool Paused { get { return paused; } set { if (value != paused) current.TogglePause(); paused = value; } }

        static LevelManager levelManager;
        public static LevelManager LevelManager { get { return levelManager; } }

        InputManager inputManager;
        AudioManager audioManager;

        //binds UI
        GameObject[] categories;
        bool binding = false;

        int currentLevelIndex;

        BindKeyIndex currentBindKeyIndex;
        KeyCode bindKey;

        Light[] sun = new Light[4];

        static bool directLevelLoad_s;

        static KeyCode cancelKey_s;
        static KeyCode noneKey_s;
        static KeyCode pauseKey_s;

        static SimpleResolution minResolution_s;
        static Resolution[] validResolutions;

        public ConflictResponse conflict;

        //Variables For Saving Expanded Values For The CusomInspector
        [SerializeField]
        bool initExpanded;
        [SerializeField]
        bool uiExpanded;
        [SerializeField]
        bool templatesExpanded;
        [SerializeField]
        bool menuExpanded;

        void Start()
        {
            current = this;

            if (init)
            {

            }
            else
            {
                init = true;
                Init();
            }

            GetSun();
            InitUI();
        }

        void Update()
        {
            if(menuType == Menu.InGame)
            {
                if (Input.GetKeyDown(pauseKey_s))
                {
                    if(!paused)
                        TogglePause();
                    else if(pauseMenu.activeInHierarchy)
                        TogglePause();
                }
            }
        }

        //initilization methods
        void GetSun()
        {
            Transform sunT = GameObject.Find("Sun").transform;

            if(sunT == null)
            {
                Debug.LogError("No Sun Prefab Was Found, Please Make Sure It Exists In The Scene, And That Its Named Sun");
            }

            for (int i = 0; i < 4; i++)
            {
                sun[i] = sunT.GetChild(i).GetComponent<Light>();
            }
        }
        void Init() //init is only called once, its used mainly to load settings
        {
            directLevelLoad_s = directLevelLoad;
            cancelKey_s = cancelKey;
            noneKey_s = noneKey;
            pauseKey_s = pauseKey;
            minResolution_s = minResolution;
            settingsSavePath = Path.Combine(Application.dataPath, settingsFileName);

            validResolutions = GetValidResolutions();

            levelManager = FindObjectOfType<LevelManager>();

            menuSettings.LoadOrSave(settingsSavePath, menuSettings);
            menuSettings_s = menuSettings;

            if (menuType == Menu.Main)
            {
                
            }
            else
            {

            }
        }
        void InitUI() //Set UI Values And Listeners
        {
            volumeSl.value = menuSettings_s.volume;
            shadowsD.value = (int)menuSettings_s.shadows;
            shadowsQualityD.value = (int)menuSettings_s.shadowsQuality;
            antiAliasingD.value = (int)menuSettings_s.antiAliasing;
            anFilteringD.value = (int)menuSettings_s.anisotropicFiltering;
            vSyncD.value = (int)menuSettings_s.vSync;
            realTimeRefTog.isOn = menuSettings_s.realTimeReflections;
            fullScreenTog.isOn = menuSettings_s.fullScreen;

            if (menuSettings_s.resolution == MenuSettings.MaxResolution)
                menuSettings_s.resolution = validResolutions.Length - 1;

            resolutionD.ClearOptions();
            for (int i = 0; i < validResolutions.Length; i++)
            {
                resolutionD.options.Add(new Dropdown.OptionData(validResolutions[i].width + " X " + validResolutions[i].height));
            }
            resolutionD.value = menuSettings_s.resolution;
            MenuTools.SetDropDownText(resolutionD);
            resolutionD.onValueChanged.AddListener(SetResolution);

            volumeSl.onValueChanged.AddListener(SetVolume);
            shadowsD.onValueChanged.AddListener(SetShadows);
            shadowsQualityD.onValueChanged.AddListener(SetShadowsQuality);
            antiAliasingD.onValueChanged.AddListener(SetAntiAliasing);
            anFilteringD.onValueChanged.AddListener(SetAnisotropicFiltering);
            vSyncD.onValueChanged.AddListener(SetVSync);
            realTimeRefTog.onValueChanged.AddListener(SetRealTimeReflections);
            fullScreenTog.onValueChanged.AddListener(SetFullScreen);
            exitB.onClick.AddListener(Quit);
            optionsBackB.onClick.AddListener(() => menuSettings_s.Save(settingsSavePath));

            UpdateAllSettings(menuSettings_s);

            if (menuType == Menu.Main)
            {
                newGameB.onClick.AddListener(LoadNewGame);

                if (levels.AutoCreateUI)
                    CreateLevelUI();
            }
            else
            {
                returnToMainB.onClick.AddListener(LoadMainMenu);
                resumeB.onClick.AddListener(Resume);
            }
        }

        //pause
        void Resume()
        {
            if (paused)
                TogglePause();
        }
        void TogglePause()
        {
            paused = !paused;

            if (paused)
            {
                gameUI.SetActive(false);
                pauseMenu.SetActive(true);

                Time.timeScale = 0;
            }
            else
            {
                gameUI.SetActive(true);
                pauseMenu.SetActive(false);

                Time.timeScale = 1;
            }
        }

        void LoadLevel(GameLevel level) //Load A New Scene
        {
            if (!directLevelLoad_s)
                levelManager.LoadLevel(level);
            else
                SceneManager.LoadScene(level.SceneName);
        }

        public void LoadNewGame() //Load NewGame Level
        {
            LoadLevel(levels[0]);
        }

        public void LoadMainMenu() //Load Main Menu Level
        {
            Resume();
            LoadLevel(mainMenuLevel);
        }

        void LoadCurrentLevel()
        {
            LoadLevel(levels[currentLevelIndex]);
        }

        void CreateLevelUI()
        {
            SetLevelPreview();

            for (int i = 0; i < levels.LevelsCount; i++)
            {
                GameObject levelO = Instantiate(levelTemplate);

                RectTransform levelRt = levelO.GetComponent<RectTransform>();
                levelRt.SetParent(levelsScroll.content, false);
                levelRt.anchoredPosition = new Vector2(0, -i * (levelRt.sizeDelta.y + 10) - 10);

                Button levelB = levelO.GetComponent<Button>();
                SetLevelButton(levelB, i);

                Text levelT = levelRt.GetComponentInChildren<Text>();
                levelT.text = levels[i].Name;
            }

            float levelHeight = levelTemplate.GetComponent<RectTransform>().sizeDelta.y;
            float height = MenuTools.CalcScrollHeight(levels.LevelsCount, levelHeight, 10);
            MenuTools.SetScrollHeight(levelsScroll, height);
        }

        void SetLevelPreview()
        {
            levelNameT = levelPreview.transform.Find("Name").GetComponent<Text>();
            levelImage = levelPreview.transform.Find("Image").GetComponent<Image>();
            levelDescriptionT = levelPreview.transform.Find("Description").GetComponent<Text>();
            playLevelB = levelPreview.transform.Find("Play").GetComponent<Button>();

            playLevelB.onClick.AddListener(LoadCurrentLevel);
        }

        void SetLevelButton(Button butt, int levelIndex)
        {
            butt.onClick.AddListener(() => SetCurrentLevel(levelIndex));
        }

        void SetCurrentLevel(int levelIndex)
        {
            levelPreview.SetActive(true);
            currentLevelIndex = levelIndex;
            levelNameT.text = levels[levelIndex].Name;
            levelImage.sprite = levels[levelIndex].Image;
            levelDescriptionT.text = levels[levelIndex].Description;
        }

        internal void CreateBindsUI(Binds[] binds, InputManager inManger) //Creates Categories
        {
            inputManager = inManger;

            categories = new GameObject[binds.Length];
            RectTransform controlsScrollRt = controlsScroll.GetComponent<RectTransform>();

            conflictPopup.transform.GetChild(0).Find("Cancel").GetComponent<Button>().onClick.AddListener(Cancel);
            conflictPopup.transform.GetChild(0).Find("Switch").GetComponent<Button>().onClick.AddListener(Switch);
            conflictPopupText = conflictPopup.transform.GetChild(0).Find("Text").GetComponent<Text>();

            popupText = bindPopup.transform.GetChild(0).Find("Text").GetComponent<Text>();

            for (int i = 0; i < binds.Length; i++)
            {
                GameObject catH = new GameObject(binds[i].CategoryName + " Category");
                if (i != 0)
                    catH.SetActive(false);

                categories[i] = catH;

                RectTransform catHRt = catH.AddComponent<RectTransform>();
                catHRt.SetParent(controlsScroll.content, false);

                catHRt.anchorMin = Vector2.zero;
                catHRt.anchorMax = Vector2.one;

                catHRt.offsetMin = Vector2.zero;
                catHRt.offsetMax = Vector2.zero;

                GameObject catO = Instantiate(categoryTemplate);

                RectTransform catRt = catO.GetComponent<RectTransform>();
                catRt.name = binds[i].CategoryName;
                catRt.SetParent(controlsScrollRt, false);
                catRt.anchoredPosition = new Vector2(i * (catRt.sizeDelta.x + 10), 0);

                Button catB = catO.GetComponent<Button>();

                float buttonHeight = bindTemplate.GetComponent<RectTransform>().sizeDelta.y;
                float height = MenuTools.CalcScrollHeight(binds[i].bindsCount, buttonHeight, 10);

                if (i == 0)
                    MenuTools.SetScrollHeight(controlsScroll, height);

                SetCategoryButton(catB, i, height);

                Text catT = catRt.GetComponentInChildren<Text>();
                catT.text = binds[i].CategoryName;

                CreateBind(i, binds[i].AllBinds);
            }
        }

        void CreateBind(int index, Bind[] binds) //Creates Binds For A Category
        {
            for (int i = 0; i < binds.Length; i++)
            {
                GameObject bindO = Instantiate(bindTemplate);

                RectTransform bindRt = bindO.GetComponent<RectTransform>();
                bindRt.SetParent(categories[index].transform, false);
                bindRt.anchoredPosition = new Vector2(0, -i * (bindRt.sizeDelta.y + 10) - 10);

                for (int x = 0; x < 3; x++)
                {
                    Button bindB = bindRt.GetChild(x).GetComponent<Button>();
                    SetBindButton(bindB, index, i, x + 1);

                    Text bindBT = bindRt.GetChild(x).GetChild(0).GetComponent<Text>();
                    bindBT.text = binds[i][x + 1].ToString();

                    if (x == 0)
                        InputManager.Binds[index][i].primaryT = bindBT;
                    else if(x == 1)
                        InputManager.Binds[index][i].secondaryT = bindBT;
                    else if(x == 2)
                        InputManager.Binds[index][i].extraT = bindBT;
                }

                Text bindT = bindRt.GetChild(bindRt.childCount - 1).GetComponent<Text>();
                bindT.text = binds[i].Name;
            }
        }

        void SetCategoryButton(Button butt, int index, float height)
        {
            butt.onClick.AddListener(() => OpenCategory(index, height));
        }

        void OpenCategory(int index, float height) //Opens A New Bind Category And Sets The Control Scroll Height
        {
            MenuTools.SetScrollHeight(controlsScroll, height);

            for (int i = 0; i < categories.Length; i++)
            {
                if (i == index)
                    categories[i].SetActive(true);
                else
                    categories[i].SetActive(false);
            }
        }

        void SetBindButton(Button butt, int cat, int bind, int key)
        {
            butt.onClick.AddListener(() => StartBinding(cat, bind, key));
        }

        void StartBinding(int catIndex, int bindIndex, int keyIndex) //Starts Binding Procedure
        {
            binding = true;

            optionsBackB.interactable = false;

            currentBindKeyIndex = new BindKeyIndex(catIndex, bindIndex, keyIndex);

            string[] names = inputManager.GetBindNameAndKey(currentBindKeyIndex);

            popupText.text = "Press a Key To Replace [" + names[1] + "] For The Bind [" + names[0] + ']' ;

            bindKey = InputManager.GetKeyCode(currentBindKeyIndex);

            bindPopup.SetActive(true);
        }

        ConflictResponse CheckConflict() //Checks If bindKey Conflicts With Any Key In InputManager.Binds
        {
            if(bindKey != KeyCode.None)
            {
                for (int y = 0; y < InputManager.Binds[currentBindKeyIndex.Cat].bindsCount; y++)
                {
                    for (int z = 1; z <= 3; z++)
                    {
                        if (InputManager.Binds[currentBindKeyIndex.Cat][y][z] == bindKey)
                            return new ConflictResponse(true, currentBindKeyIndex.Cat, y, z);
                    }
                }
            }

            return new ConflictResponse(false, 0, 0, 0);
        }

        void OnGUI() //Used To Bind Using The Event.Current
        {
            if (!binding)
                return;

            KeyCode key = Event.current.keyCode;

            if (Event.current.isMouse)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (Input.GetMouseButton(i))
                        key = (KeyCode)Enum.Parse(typeof(KeyCode), "Mouse" + i);
                }
            }

            if (Event.current.shift)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                    key = KeyCode.LeftShift;
                if (Input.GetKey(KeyCode.RightShift))
                    key = KeyCode.RightShift;
            }

            for (int x = 1; x < 9; x++)
            {
                for (int y = 0; y < 19; y++)
                {
                    if (Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + x + "Button" + y)))
                        key = (KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + x + "Button" + y);
                }

            }

            if(key != KeyCode.None)
            {
                if (key == cancelKey_s)
                {
                    binding = false;
                    bindPopup.SetActive(false);
                    return;
                }
                else if (key == noneKey_s)
                    key = KeyCode.None;
                else if(bindKey == key)
                {
                    binding = false;
                    bindPopup.SetActive(false);
                    StartCoroutine(UpdateBackButton());
                    return;
                }
                   
                FinishBinding(key);
            }
        }

        void FinishBinding(KeyCode bindedKey) //Method Defining Binding Ending Procedure
        {
            binding = false;
            bindKey = bindedKey;
            bindPopup.SetActive(false);

            conflict = CheckConflict();

            if (conflict.Found)
            {
                string[] names = inputManager.GetBindNameAndKey(conflict);
                conflictPopupText.text = "Key [" + names[1] + "] is Already Assigned to [" + names[0] + ']';
                conflictPopup.SetActive(true);
            }
            else
            {
                StartCoroutine(UpdateBackButton());
                UpdateBind();
            }
        }

        IEnumerator UpdateBackButton()
        {
            while(Input.GetKey(bindKey))
            {                
                yield return new WaitForEndOfFrame();
            }

            optionsBackB.interactable = true;
        }

        void UpdateBind() //Updates The InputManager.Binds And Saves Changes
        {
            inputManager.UpdateBind(currentBindKeyIndex, bindKey);
            InputManager.SaveBinds();
        }

        void Cancel() //Defines The Cancel Event When A Conflict Was Found
        {
            optionsBackB.interactable = true;
            conflictPopup.SetActive(false);
        }

        void Switch() //Method To Switch Between The Conflicting Bindings Keys
        {
            KeyCode temp = InputManager.GetKeyCode(currentBindKeyIndex);

            inputManager.UpdateBind(currentBindKeyIndex, InputManager.GetKeyCode(conflict));
            inputManager.UpdateBind(conflict, temp);

            InputManager.SaveBinds();

            Cancel();
        }

        public void CreateAudioUI(AudioManager newAudioManager) //Create UI For The Audio Manager
        {
            audioManager = newAudioManager;

            for (int i = 0; i < AudioManager.AudioChannels.Length; i++)
            {
                GameObject channelO = Instantiate<GameObject>(audioChannelTemplate);

                RectTransform channelRt = channelO.GetComponent<RectTransform>();
                channelRt.SetParent(audioChannelScroll.content, false);
                channelRt.anchoredPosition = new Vector2(0, -i * (channelRt.sizeDelta.y + 10) - 10);

                Slider sl = channelRt.Find("Slider").GetComponent<Slider>();
                AudioManager.AudioChannels[i].SetUI(sl);

                Text text = channelRt.Find("Text").GetComponent<Text>();
                text.text = AudioManager.AudioChannels[i].Name;
            }

            float channelHeight = audioChannelTemplate.GetComponent<RectTransform>().sizeDelta.y;
            float height = MenuTools.CalcScrollHeight(AudioManager.AudioChannels.Length, channelHeight, 10);
            MenuTools.SetScrollHeight(audioChannelScroll, height);
        }

        public void SetUI(string name, object ui)
        {
            switch (name)
            {
                case "controls scroll":
                    controlsScroll = (ScrollRect)ui;
                    break;
                case "audio channels scroll":
                    audioChannelScroll = (ScrollRect)ui;
                    break;
                case "bind popup":

                    break;
                case "conflict popup":

                    break;
                case "volume slider":

                    break;
                case "shadows dropdown":

                    break;
                case "shadows quality dropdown":

                    break;
                default:
                    Debug.LogError("No UI With The Name " + name + " Was Found, Please Check Name");
                    break;
            }
        }

        public static Resolution[] GetValidResolutions() //Returns All Resolutions Bigger Than The Minimum Resolution
        {
#if UNITY_EDITOR
            return Screen.resolutions;
#else
            List<Resolution> validResolutions = new List<Resolution>();

            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                if (minResolution_s.Compare(Screen.resolutions[i]))
                    validResolutions.Add(Screen.resolutions[i]);
            }

            return validResolutions.ToArray();
#endif
        }

        //UI Controls
        void SetVolume(float newVolume)
        {
            menuSettings_s.volume = newVolume;
            UpdateVolume(newVolume);
        }
        void SetShadows(int newShadows)
        {
            menuSettings_s.shadows = (MenuSettings.Shadow)newShadows;
            UpdateShadows(menuSettings_s.shadows);
        }
        void SetShadowsQuality(int newShadowsQuality)
        {
            menuSettings_s.shadowsQuality = (MenuSettings.ShadowQuality)newShadowsQuality;
            UpdateShadowsQuality(menuSettings_s.shadowsQuality);
        }
        void SetAntiAliasing(int newAA)
        {
            menuSettings_s.antiAliasing = (MenuSettings.AntiAliasing)newAA;

            UpdateAntiAliasing(menuSettings_s.antiAliasing);
        }
        void SetAnisotropicFiltering(int newAF)
        {
            menuSettings_s.anisotropicFiltering = (AnisotropicFiltering)newAF;
            UpdateAnisotropicFiltering(menuSettings_s.anisotropicFiltering);
        }
        void SetVSync(int newVSync)
        {
            menuSettings_s.vSync = (MenuSettings.VSync)newVSync;
            UpdateVSync(menuSettings_s.vSync);
        }
        void SetRealTimeReflections(bool realTimeRef)
        {
            menuSettings_s.realTimeReflections = realTimeRef;
            UpdateRealTimeReflections(realTimeRef);
        }
        void SetResolution(int resolution)
        {
            menuSettings_s.resolution = resolution;
            SetScreenResolution(resolution, menuSettings_s.fullScreen);
        }
        void SetFullScreen(bool fullscreen)
        {
            menuSettings_s.fullScreen = fullscreen;
            SetScreenResolution(menuSettings_s.resolution, fullscreen);
        }

        //Apply Settings
        public void UpdateVolume(float volume)
        {
            AudioListener.volume = volume;
        }
        public void UpdateShadows(MenuSettings.Shadow shadow)
        {
            for (int i = 0; i < 4; i++)
            {
                sun[i].shadows = (LightShadows)(int)shadow;
            }
        }
        public void UpdateShadowsQuality(MenuSettings.ShadowQuality shadowQuality)
        {
            int lightIndex = (int)shadowQuality;

            for (int i = 0; i < 4; i++)
            {
                if (i == lightIndex)
                    sun[i].gameObject.SetActive(true);
                else
                    sun[i].gameObject.SetActive(false);
            }
        }
        public void UpdateAntiAliasing(MenuSettings.AntiAliasing AA)
        {
            int AAindex = 0;

            switch (AA)
            {
                case MenuSettings.AntiAliasing.None:
                    AAindex = 0;
                    break;
                case MenuSettings.AntiAliasing.X2:
                    AAindex = 2;
                    break;
                case MenuSettings.AntiAliasing.X4:
                    AAindex = 4;
                    break;
                case MenuSettings.AntiAliasing.X8:
                    AAindex = 8;
                    break;
            }

            QualitySettings.antiAliasing = AAindex;
        }
        public void UpdateAnisotropicFiltering(AnisotropicFiltering AF)
        {
            QualitySettings.anisotropicFiltering = AF;
        }
        public void UpdateVSync(MenuSettings.VSync vsync)
        {
            QualitySettings.vSyncCount = (int)vsync;
        }
        public void UpdateRealTimeReflections(bool newRTR)
        {
            QualitySettings.realtimeReflectionProbes = newRTR;
        }
        internal void SetScreenResolution(int index, bool fullScrn)
        {
            Screen.SetResolution(validResolutions[index].width, validResolutions[index].height, fullScrn);
        }
        void UpdateAllSettings(MenuSettings settings)
        {
            UpdateShadows(settings.shadows);
            UpdateShadowsQuality(settings.shadowsQuality);
            UpdateVolume(settings.volume);
            UpdateAntiAliasing(settings.antiAliasing);
            UpdateAnisotropicFiltering(settings.anisotropicFiltering);
            UpdateRealTimeReflections(settings.realTimeReflections);
            UpdateVSync(settings.vSync);
            SetScreenResolution(settings.resolution, settings.fullScreen);
        }

        void Quit() //Quits Application Or Stops Editor
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }

    [Serializable][DataContract]
    public class MenuSettings
    {
        [SerializeField][DataMember][Range(0, 1)]
        internal float volume;

        [SerializeField][DataMember]
        internal Shadow shadows;

        [SerializeField][DataMember]
        internal ShadowQuality shadowsQuality;

        [SerializeField][DataMember]
        internal AntiAliasing antiAliasing;

        [SerializeField][DataMember]
        internal AnisotropicFiltering anisotropicFiltering;

        [SerializeField][DataMember]
        internal bool realTimeReflections;

        [SerializeField][DataMember]
        internal VSync vSync;

        [SerializeField][DataMember]
        internal int resolution;

        [SerializeField][DataMember]
        internal bool fullScreen;

        public const int MaxResolution = -1;

        public void Save(string path)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(MenuSettings));

            var settings = new XmlWriterSettings { Indent = true };

            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                ser.WriteObject(writer, this);
            }
        }

        public static MenuSettings Load(string path)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(MenuSettings));

            using (FileStream file = new FileStream(path, FileMode.OpenOrCreate))
            {
                MenuSettings loadedSettings = null;
                try
                {
                    loadedSettings = (MenuSettings)ser.ReadObject(file);
                }
                catch (Exception)
                {
                    Debug.LogError("Reading Menu Settings Failed, Settings Will Reset");
                }

                return loadedSettings;
            }
        }

        public void LoadOrSave(string path, MenuSettings defaultSettings = null)
        {
            if(File.Exists(path))
            {
                MenuSettings loadedSettings = Load(path);

                if (ValidateSettings(loadedSettings))
                {
                    CopySettings(loadedSettings);
                }
                else
                {
                    if (defaultSettings == null)
                        defaultSettings = new MenuSettings();

                    CopySettings(defaultSettings);
                    Save(path);
                }
            }
            else
            {
                if (defaultSettings == null)
                    defaultSettings = new MenuSettings();

                CopySettings(defaultSettings);

                Save(path);
            }
        }

        public MenuSettings()
        {
            volume = 1;

            shadows = Shadow.Soft;
            shadowsQuality = ShadowQuality.Ultra;

            antiAliasing = AntiAliasing.X8;
            anisotropicFiltering = AnisotropicFiltering.Enable;
            realTimeReflections = true;
            vSync = VSync.Half;
            resolution = MaxResolution;
            fullScreen = true;
        }

        public MenuSettings(MenuSettings settings)
        {
            CopySettings(settings);
        }

        internal void CopySettings(MenuSettings settings)
        {
            volume = settings.volume;

            shadows = settings.shadows;
            shadowsQuality = settings.shadowsQuality;
            antiAliasing = settings.antiAliasing;
            anisotropicFiltering = settings.anisotropicFiltering;
            realTimeReflections = settings.realTimeReflections;
            vSync = settings.vSync;
            resolution = settings.resolution;
            fullScreen = settings.fullScreen;
        }

        public static bool ValidateSettings(MenuSettings settings)
        {
            if (settings == null)
                return false;

            if (settings.volume < 0 || settings.volume > 1)
            {
                Debug.LogError("Volume Was Loaded Beyond The Range Of 0 - 1, Menu Settings Will Reset");
                return false;
            }

            if (settings.resolution > GameMenu.GetValidResolutions().Length - 1 || settings.resolution < -1)
            {
                Debug.LogError("Resolutions Is Invalid, Menu Settings Will Reset");
                return false;
            }

            return true;
        }

        public enum Shadow
        {
            None, Hard, Soft
        }

        public enum ShadowQuality
        {
            Low, Medium, High, Ultra
        }

        public enum AntiAliasing
        {
            None, X2, X4, X8
        }

        public enum VSync
        {
            None, Half, Full
        }
    }

    public class BindKeyIndex
    {
        protected int cat;
        public int Cat { get { return cat; } }

        protected int bind;
        public int Bind { get { return bind; } }

        protected int key;
        public int Key { get { return key; } }

        public BindKeyIndex()
        {
        }

        public BindKeyIndex(int newCat, int newBind, int newKey)
        {
            cat = newCat;
            bind = newBind;
            key = newKey;
        }
    }

    public class ConflictResponse : BindKeyIndex
    {
        bool found = false;
        public bool Found { get { return found; } }

        public ConflictResponse(bool newFound, int newCat, int newBind, int newKey)
        {
            found = newFound;
            cat = newCat;
            bind = newBind;
            key = newKey;
        }
    }

    [Serializable]
    public class Levels
    {
        [SerializeField]
        GameLevel[] levels;
        public int LevelsCount { get { return levels.Length; } }

        [SerializeField]
        bool autoCreateUI = true;
        public bool AutoCreateUI { get { return autoCreateUI; } }

        public GameLevel this[int index]
        {
            get
            {
                return levels[index];
            }
        }
    }

    [Serializable]
    public class GameLevel
    {
        [SerializeField]
        string name;
        public string Name { get { return name; } }

        [SerializeField]
        string sceneName;
        public string SceneName { get { return sceneName; } }

        [SerializeField]
        Sprite image;
        public Sprite Image { get { return image; } }

        [SerializeField]
        string scenePath;

        [SerializeField]
        string description;
        public string Description { get { return description; } }

        public GameLevel(string newSceneName)
        {
            sceneName = newSceneName;
        }

        public GameLevel(string newSceneName, string newName, string newDescription)
        {
            sceneName = newSceneName;
            name = newName;
            description = newDescription;
        }
    }

    [Serializable]
    public class SimpleResolution
    {
        [SerializeField]
        int width;
        public int Width { get { return width; } }

        [SerializeField]
        int height;
        public int Height { get { return height; } }

        public SimpleResolution(int newHeight, int newWidth)
        {
            height = newHeight;
            width = newWidth;
        }

        public SimpleResolution(Resolution resolution)
        {
            height = resolution.height;
            width = resolution.width;
        }

        public bool Compare(Resolution res)
        {
            return res.width >= width && res.height >= height;
        }
    }

    //A static class containing general functions
    public static class MenuTools
    {
        public static bool intToBool(int value)
        {
            if (value == 1)
                return true;

            return false;
        }

        public static int boolToInt(bool value)
        {
            if (value)
                return 1;

            return 0;
        }

        public static float CalcScrollHeight(int elementsCount, float elementSize, float spacing)
        {
            return elementsCount * (elementSize + spacing) + spacing;
        }

        public static void SetDropDownText(Dropdown drop)
        {
            drop.captionText.text = drop.options[drop.value].text;
        }

        public static bool Contains<T>(T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(value))
                    return true;
            }

            return false;
        }

        public static void SetScrollHeight(ScrollRect scroll, float height)
        {
            scroll.content.sizeDelta = new Vector2(scroll.content.sizeDelta.x, height);
        }
    }

    public enum Menu
    {
        Main, InGame
    }
}