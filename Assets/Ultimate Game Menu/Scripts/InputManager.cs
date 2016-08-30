using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine.UI;
using System.Xml;

namespace UGM
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField]
        string fileName = "Binds.xml";

        [SerializeField]
        Binds[] binds;
        static Binds[] binds_s;
        public static Binds[] Binds { get { return binds_s; } }

        static Dictionary<string, Bind> BindsDirectory = new Dictionary<string, Bind>();

        public delegate void BindsChanged(string name);

        public static BindsChanged OnBindsChanged;

        [SerializeField]
        bool autoCreateUI = true;

        static bool init = false;
        static string savePath;

        void Start()
        {
            if(init)
            {
                binds = binds_s;

                if (binds_s.Length == 0)
                {
                    enabled = false;
                    return;
                }
            }
            else
            {
                init = true;

                Init();
            }

            if (autoCreateUI)
            {
                GameMenu menu = FindObjectOfType<GameMenu>();
                if (menu)
                    menu.CreateBindsUI(binds, this);
                else
                    Debug.LogError("Trying To Create Controls UI But No GameMenu Was Founded !!");
            }
        }

        void Init()
        {
            savePath = Path.Combine(Application.dataPath, fileName);

            binds_s = binds;
            LoadOrSave(savePath, binds);

            RegisterBinds();
        }


        public static void SaveBinds()
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(Binds[]));

            var settings = new XmlWriterSettings { Indent = true };

            using (XmlWriter writer = XmlWriter.Create(savePath, settings))
            {
                ser.WriteObject(writer, binds_s);
            }
        }

        Binds[] LoadBinds()
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(Binds[]));

            using (FileStream file = new FileStream(savePath, FileMode.OpenOrCreate))
            {
                Binds[] loadedBinds = null;

                try
                {
                    loadedBinds = (Binds[])ser.ReadObject(file);
                }
                catch (Exception)
                {
                    Debug.LogError("Reading Binds Failed, Binds Will Reset");
                }

                return loadedBinds;
            }
        }

        void LoadOrSave(string path, Binds[] defaultBinds)
        {
            if (File.Exists(path))
            {
                Binds[] loadedBinds = LoadBinds();

                if (ValidateBinds(loadedBinds))
                {
                    binds = loadedBinds;
                    binds_s = loadedBinds;
                }
                else
                {
                    if (binds_s.Length > 0)
                        SaveBinds();
                    else
                    {
                        Debug.LogError("No Binds Defind And No Save Was Found Neither, Disabling Input Manager");
                        enabled = false;
                    }
                }
                    
            }
            else
                SaveBinds();
        }


        bool ValidateBinds(Binds[] loadedBinds)
        {
            if (loadedBinds == null)
                return false;

            if (binds_s.Length == 0)
            {
                if (loadedBinds.Length > 0)
                {
                    Debug.LogWarning("Binds Not Defind, But A Save Was Found, Using Save Without Vaidating it");

                    if (!ValidateOverlapControls(loadedBinds))
                        return false;

                    return true;
                }

                else
                {
                    return false;
                }
            }

            if (loadedBinds.Length != binds_s.Length)
            {
                Debug.Log("OutDated Binds, Saving New Copy");
                return false;
            }

            for (int x = 0; x < loadedBinds.Length; x++)
            {
                if (loadedBinds[x].CategoryName != binds_s[x].CategoryName)
                {
                    Debug.LogError("Category Name Mismatch, Local Binds Say " + binds_s[x].CategoryName + "While Loaded Binds Provide The Name " + loadedBinds[x].CategoryName + " Binds Will Reset");
                    return false;
                }

                if (loadedBinds[x].bindsCount != binds_s[x].bindsCount)
                {
                    Debug.Log("OutDated Binds, Saving New Copy");
                    return false;
                }

                for (int y = 0; y < loadedBinds[x].bindsCount; y++)
                {
                    if (loadedBinds[x][y].Name != binds_s[x][y].Name)
                        return false;
                }
            }

            if (!ValidateOverlapControls(loadedBinds))
            {
                Debug.LogError("Controls Overlaping Found, Binds Will Reset");
                return false;
            }

            return true;
        }

        bool ValidateOverlapControls(Binds[] binds)
        {
            for (int x = 0; x < binds.Length; x++)
            {
                KeyCode[] defindKeys = new KeyCode[binds[x].AllBinds.Length * 3];

                int index = 0;
                for (int y = 0; y < binds[x].AllBinds.Length; y++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        if (binds[x].AllBinds[y][z + 1] != KeyCode.None && MenuTools.Contains<KeyCode>(defindKeys, binds[x].AllBinds[y][z + 1]))
                        {
                            return false;
                        }
                        else
                            defindKeys[index] = binds[x].AllBinds[y][z + 1];

                        index++;
                    }
                }
            }

            return true;
        }


        internal void UpdateBind(BindKeyIndex index, KeyCode newKey)
        {
            binds_s[index.Cat].UpdateBind(index.Bind, index.Key, newKey);
            binds = binds_s;
        }


        internal static KeyCode GetKeyCode(BindKeyIndex index)
        {
            return binds_s[index.Cat][index.Bind][index.Key];
        }

        public static Bind GetBindValue(string accessor)
        {
            return BindsDirectory[accessor];
        }

        public string[] GetBindNameAndKey(BindKeyIndex index)
        {
            string[] resault = new string[2];

            resault[0] = binds_s[index.Cat][index.Bind].Name;

            resault[1] = binds_s[index.Cat][index.Bind][index.Key].ToString();

            return resault;
        }

        public static void RegisterBinds()
        {
            BindsDirectory.Clear();

            for (int x = 0; x < binds_s.Length; x++)
            {
                for (int y = 0; y < binds_s[x].bindsCount; y++)
                {
                    BindsDirectory.Add(binds_s[x].CategoryName + "." + binds_s[x][y].Name, binds_s[x][y]);
                }
            }
        }

        public static void UpdateBind(string updatedAccessor, Bind updatedBind)
        {
            BindsDirectory[updatedAccessor] = updatedBind;

            if (OnBindsChanged != null)
                OnBindsChanged(updatedAccessor);
        }

        public static bool GetBindUp(Bind bind)
        {
            for (int i = 1; i <= 3; i++)
            {
                if (Input.GetKeyUp(bind[i]))
                    return true;
            }

            return false;
        }

        public static bool GetBindDown(Bind bind)
        {
            for (int i = 1; i <= 3; i++)
            {
                if (Input.GetKeyDown(bind[i]))
                    return true;
            }

            return false;
        }

        public static bool GetBind(Bind bind)
        {
            for (int i = 1; i <= 3; i++)
            {
                if (Input.GetKey(bind[i]))
                    return true;
            }

            return false;
        }

        public static bool GetBindUp(string bindName)
        {
            Bind bind = BindsDirectory[bindName];

            for (int i = 1; i <= 3; i++)
            {
                if (Input.GetKeyUp(bind[i]))
                    return true;
            }

            return false;
        }

        public static bool GetBindDown(string bindName)
        {
            Bind bind = BindsDirectory[bindName];

            for (int i = 1; i <= 3; i++)
            {
                if (Input.GetKeyDown(bind[i]))
                    return true;
            }

            return false;
        }

        public static bool GetBind(string bindName)
        {
            Bind bind = BindsDirectory[bindName];

            for (int i = 1; i <= 3; i++)
            {
                if (Input.GetKey(bind[i]))
                    return true;
            }

            return false;
        }
    }

    [Serializable][DataContract]
    public class Binds
    {
        [SerializeField][DataMember(Order = 0)]
        string categoryName;
        public string CategoryName { get { return categoryName; } }

        [SerializeField][DataMember(Order = 1)]
        Bind[] binds;
        public int bindsCount { get { return binds.Length; } }
        public Bind[] AllBinds { get { return binds; } }

        public Bind this[int index]
        {
            get
            {
                return binds[index];
            }
        }

        internal void UpdateBind(int bindIndex, int keyIndex, KeyCode newKey)
        {
            binds[bindIndex][keyIndex] = newKey;
            InputManager.UpdateBind(categoryName + "." + binds[bindIndex], binds[bindIndex]);
        }
    }

    [Serializable][DataContract]
    public class Bind
    {
        [SerializeField][DataMember(Order = 0)]
        string name;
        public string Name { get { return name; } }

        [SerializeField][DataMember(Order = 1)]
        KeyCode primary;
        [SerializeField][DataMember(Order = 2)]
        KeyCode secondary;
        [SerializeField][DataMember(Order = 3)]
        KeyCode extra;

        public Text primaryT;
        public Text secondaryT;
        public Text extraT;

        public KeyCode GetKey(BindKey key)
        {
            if (key == BindKey.Primary)
                return primary;
            if (key == BindKey.Secondary)
                return secondary;
            if (key == BindKey.Extra)
                return extra;

            return KeyCode.None;
        }

        public KeyCode this[int index]
        {
            get
            {
                if (index == 1)
                    return primary;
                if (index == 2)
                    return secondary;
                if (index == 3)
                    return extra;

                Debug.LogError("Trying To Access Non Existing Key : " + index + ", Range is 1-3");
                return KeyCode.None;
            }

            internal set
            {
                if (index == 1)
                    primary = value;
                else if (index == 2)
                    secondary = value;
                else if (index == 3)
                    extra = value;
                else
                    Debug.LogError("Trying To Set Non Existing Key, Max Is 3");

                UpdateKeyText();
            }
        }

        public Bind(KeyCode newPrimary, bool CopyToAll = false)
        {
            primary = newPrimary;

            if(CopyToAll)
            {
                secondary = newPrimary;
                extra = newPrimary;
            }
        }

        public Bind(KeyCode newPrimary, KeyCode newSecondary, KeyCode newExtra)
        {
            primary = newPrimary;
            secondary = newSecondary;
            extra = newExtra;
        }

        public void UpdateKeyText()
        {
            primaryT.text = primary.ToString();
            secondaryT.text = secondary.ToString();
            extraT.text = extra.ToString();
        }
    }

    [Serializable]
    public class Axis
    {
        [SerializeField]
        string positiveName;
        [SerializeField]
        string negativeName;

        Bind positiveBind;
        Bind negativeBind;

        [SerializeField]
        bool snap = true;
        [SerializeField]
        float sens = 3;
        [SerializeField]
        float gravity = 3;

        [SerializeField]
        float value;
        [SerializeField]
        int rawValue;

        bool dualInput;
        public bool DualInput { get { return dualInput; } }

        bool posVal;
        bool negVal;

        bool posRVal;
        bool negRVal;

        public Axis(string posName, string negName)
        {
            positiveName = posName;
            negativeName = negName;
        }

        public void SetUp()
        {
            InputManager.OnBindsChanged += UpdateBinds;

            positiveBind = InputManager.GetBindValue(positiveName);
            negativeBind = InputManager.GetBindValue(negativeName);
        }

        public void UpdateBinds(string bindAccessor)
        {
            if (bindAccessor == positiveName)
                positiveBind = InputManager.GetBindValue(positiveName);

            if (bindAccessor == negativeName)
                negativeBind = InputManager.GetBindValue(negativeName);
        }

        public float GetValue()
        {
            posVal = InputManager.GetBind(positiveBind);
            negVal = InputManager.GetBind(negativeBind);

            if(posVal || negVal)
            {
                if(posVal && negVal)
                {
                    if(value == 0)
                        dualInput = true;
                }
                else if(posVal)
                {
                    dualInput = false;

                    if (snap && value < 0)
                        value = 0;

                    value = Mathf.MoveTowards(value, 1, sens * Time.deltaTime);

                    GetRawInput();

                    return value;
                }
                else if(negVal)
                {
                    dualInput = false;

                    if (snap && value > 0)
                        value = 0;

                    value = Mathf.MoveTowards(value, -1, sens * Time.deltaTime);

                    GetRawInput();

                    return value;
                }
            }
            else
            {
                dualInput = false;
            }

            value = Mathf.MoveTowards(value, 0, gravity * Time.deltaTime);
            GetRawInput();

            return value;
        }

        public int GetRawValue()
        {
            posRVal = InputManager.GetBind(positiveBind);
            negRVal = InputManager.GetBind(negativeBind);

            if(posRVal || negRVal)
            {
                if (posRVal && negRVal)
                {
                    dualInput = true;
                }
                else if (posRVal)
                {
                    dualInput = false;

                    value = 1;
                    rawValue = 1;

                    return rawValue;
                }
                else if (negRVal)
                {
                    dualInput = false;

                    value = -1;
                    rawValue = -1;

                    return rawValue;
                }
            }
            else
            {
                dualInput = false;
            }

            value = 0;
            rawValue = 0;

            return rawValue;
        }

        void GetRawInput()
        {
            if (value > 0)
                rawValue = 1;
            else if (value < 0)
                rawValue = -1;
            else
                rawValue = 0;
        }
    }

    public enum BindKey
    {
        Primary, Secondary, Extra
    }
}