using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace UGM
{
    public class SavedValues : MonoBehaviour
    {
        [SerializeField]
        SavedValue[] savedValues;

        static SavedValue[] savedValues_s;

        static Dictionary<string, int> savedValuesIndex; 

        void Start()
        {
            savedValues_s = savedValues;
            savedValuesIndex = new Dictionary<string, int>();

            for (int i = 0; i < savedValues.Length; i++)
            {
                savedValues[i].Load(); //Load Values
                savedValues[i].SetUI(); //Sets All Listeners
                savedValues[i].UpdateUI(); //Updates UI With Values

                savedValuesIndex.Add(savedValues[i].Name, i);
            }
        }

        static SavedValue GetSavedValue(string savedValueName)
        {
            if(!savedValuesIndex.ContainsKey(savedValueName))
            {
                Debug.Log("Saved Value With Name " + savedValueName + " Couldn't be Found");
                return null;
            }

            return savedValues_s[savedValuesIndex[savedValueName]];
        }

        public void DeleteAll()
        {
            for (int i = 0; i < savedValues.Length; i++)
            {
                savedValues[i].Delete();
            }
        }
    }

    [System.Serializable]
    public class SavedValue
    {
        [SerializeField]
        string name;
        public string Name { get { return name; } }

        [SerializeField]
        int iValue;
        public int IntValue { get { return iValue; } set { iValue = value; UpdateUI(); } }

        [SerializeField]
        float fValue;
        public float FloatValue { get { return fValue; } set { fValue = value; UpdateUI(); } }

        [SerializeField]
        bool bValue;
        public bool BoolValue { get { return bValue; } set { bValue = value; UpdateUI(); } }

        [SerializeField]
        string sValue;
        public string StringValue { get { return sValue; } set { sValue = value; UpdateUI(); } }

        [SerializeField]
        UI uiType;

        [SerializeField]
        InputField inf;
        [SerializeField]
        Slider sl;
        [SerializeField]
        Toggle togg;
        [SerializeField]
        Dropdown drop;

        [SerializeField]
        InputFieldData infData;
        [SerializeField]
        SliderData slData;

        public delegate void IValueChanged(int value);
        public IValueChanged OnIValueChanged;

        public delegate void FValueChanged(float value);
        public FValueChanged OnFValueChanged;

        public delegate void BValueChanged(bool value);
        public BValueChanged OnBValueChanged;

        public delegate void SValueChanged(string value);
        public SValueChanged OnSValueChanged;

        public void SetUI()
        {
            switch (uiType)
            {
                case UI.InputField: //inputField
                    switch (infData)
                    {
                        case InputFieldData.Int: //InputField Int

                            inf.onValueChanged.AddListener(
                                delegate
                                {
                                    if (inf.text == "")
                                        iValue = 0;
                                    else
                                        iValue = int.Parse(inf.text);

                                    OnIntChanged();
                                    Save();
                                }
                                );

                            break;
                        case InputFieldData.Float: //InputField Float

                            inf.onValueChanged.AddListener(
                                delegate
                                {
                                    if (inf.text == "")
                                        fValue = 0f;
                                    else
                                        fValue = float.Parse(inf.text);

                                    OnFloatChanged();
                                    Save();
                                }
                                );

                            break;
                        case InputFieldData.String: //InputField String

                            inf.onValueChanged.AddListener(
                                delegate
                                {
                                    sValue = inf.text;
                                    OnStringChanged();
                                    Save();
                                }
                                );

                            break;
                    }
                    break;

                case UI.Slider: //Slider
                    switch (slData)
                    {
                        case SliderData.Int: //Slider Int

                            sl.onValueChanged.AddListener(
                                delegate
                                {
                                    iValue = (int)sl.value;
                                    OnIntChanged();
                                    Save();
                                }
                                );

                            break;
                        case SliderData.Float: //Slider Float

                            sl.onValueChanged.AddListener(
                                delegate
                                {
                                    fValue = sl.value;
                                    OnFloatChanged();
                                    Save();
                                }
                                );

                            break;
                    }
                    break;

                case UI.Toggle: //Toggle

                    togg.onValueChanged.AddListener(
                        delegate
                        {
                            bValue = togg.isOn;
                            OnBoolChanged();
                            Save();
                        }
                        );

                    break;

                case UI.DropDown: //DropDown

                    drop.onValueChanged.AddListener(
                        delegate
                        {
                            iValue = drop.value;
                            OnIntChanged();
                            Save();
                        }
                        );

                    break;
            }
        }

        void OnBoolChanged()
        {
            if (OnBValueChanged != null)
                OnBValueChanged(bValue);
        }

        void OnIntChanged()
        {
            if (OnIValueChanged != null)
                OnIValueChanged(iValue);
        }

        void OnFloatChanged()
        {
            if (OnFValueChanged != null)
                OnFValueChanged(fValue);
        }

        void OnStringChanged()
        {
            if (OnSValueChanged != null)
                OnSValueChanged(sValue);
        }

        public void UpdateUI()
        {
            switch (uiType)
            {
                case UI.InputField: //InputField
                    switch (infData)
                    {
                        case InputFieldData.Int: //InputField Int
                            inf.text = iValue + "";
                            break;
                        case InputFieldData.Float: //InputField Float
                            inf.text = fValue + "";
                            break;
                        case InputFieldData.String: //InputField String
                            inf.text = sValue;
                            break;
                    }
                    break;
                case UI.Slider: //Slider
                    switch (slData)
                    {
                        case SliderData.Int: //Slider Int
                            sl.value = iValue;
                            break;
                        case SliderData.Float: //Slider Float
                            sl.value = fValue;
                            break;
                    }
                    break;
                case UI.Toggle: //Toggle
                    togg.isOn = bValue;
                    break;
                case UI.DropDown: //DropDown
                    drop.value = iValue;
                    break;
            }
        }

        internal void Save()
        {
            switch (uiType)
            {
                case UI.InputField: //InputField
                    switch (infData)
                    {
                        case InputFieldData.Int: //InputField Int
                            PlayerPrefs.SetInt(name, iValue);
                            break;
                        case InputFieldData.Float: //InputField Float
                            PlayerPrefs.SetFloat(name, fValue);
                            break;
                        case InputFieldData.String: //InputField String
                            PlayerPrefs.SetString(name, sValue);
                            break;
                    }
                    break;
                case UI.Slider: //Slider
                    switch (slData)
                    {
                        case SliderData.Int: //Slider Int
                            PlayerPrefs.SetInt(name, iValue);
                            break;
                        case SliderData.Float: //Slider Float
                            PlayerPrefs.SetFloat(name, fValue);
                            break;
                    }
                    break;
                case UI.Toggle: //Toggle
                    PlayerPrefs.SetInt(name, MenuTools.boolToInt(bValue));
                    break;
                case UI.DropDown: //DropDown
                    PlayerPrefs.SetInt(name, iValue);
                    break;
            }
        }

        internal void Load()
        {
            switch (uiType)
            {
                case UI.InputField: //InputField
                    switch (infData)
                    {
                        case InputFieldData.Int: //InputField Int
                            iValue = PlayerPrefs.GetInt(name, iValue);
                            break;
                        case InputFieldData.Float: //InputField Float
                            fValue = PlayerPrefs.GetFloat(name, fValue);
                            break;
                        case InputFieldData.String: //InputField String
                            sValue = PlayerPrefs.GetString(name, sValue);
                            break;
                    }
                    break;
                case UI.Slider: //Slider
                    switch (slData)
                    {
                        case SliderData.Int: //Slider Int
                            iValue = PlayerPrefs.GetInt(name, iValue);
                            break;
                        case SliderData.Float: //Slider Float
                            fValue = PlayerPrefs.GetFloat(name, fValue);
                            break;
                    }
                    break;
                case UI.Toggle: //Toggle
                    bValue = MenuTools.intToBool(PlayerPrefs.GetInt(name, MenuTools.boolToInt(bValue)));
                    break;
                case UI.DropDown: //DropDown
                    iValue = PlayerPrefs.GetInt(name, iValue);
                    break;
            }

            UpdateUI();
        }

        public void Delete()
        {
            PlayerPrefs.DeleteKey(name);
        }

        public enum UI
        {
            InputField, Slider, Toggle, DropDown
        }

        public enum Value
        {
            Int, Float, String
        }

        public enum InputFieldData
        {
            Int, Float, String
        }

        public enum SliderData
        {
            Int, Float
        }
    }
}