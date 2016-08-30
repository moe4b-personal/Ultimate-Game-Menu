using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UGM
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        string fileName = "Audio.xml";

        [SerializeField]
        AudioChannel[] audioChannels;

        static AudioChannel[] audioChannels_s;
        static public AudioChannel[] AudioChannels { get { return audioChannels_s; } }

        static Dictionary<string, int> channelsIndex = new Dictionary<string, int>();

        [SerializeField]
        bool autoCreateUI;

        static bool init = false;
        static string savePath;

        void Start()
        {
            if (init)
            {
                audioChannels = audioChannels_s;

                if (audioChannels_s.Length == 0)
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
                    menu.CreateAudioUI(this);
                else
                    Debug.LogError("Trying To Create Audio Channels UI But No GameMenu Was Founded !!");
            }
        }

        void Init()
        {
            savePath = Path.Combine(Application.dataPath, fileName);
            audioChannels_s = audioChannels;

            LoadOrSave();

            for (int i = 0; i < audioChannels_s.Length; i++)
            {
                channelsIndex.Add(audioChannels_s[i].Name, i);
            }
        }

        public static void Save()
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(AudioChannel[]));

            var settings = new XmlWriterSettings { Indent = true };

            using (XmlWriter writer = XmlWriter.Create(savePath, settings))
            {
                ser.WriteObject(writer, audioChannels_s);
            }
        }

        public AudioChannel[] Load()
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(AudioChannel[]));
            using (FileStream file = new FileStream(savePath, FileMode.OpenOrCreate))
            {
                AudioChannel[] loadedChannels = null;

                try
                {
                    loadedChannels = (AudioChannel[])ser.ReadObject(file);
                }
                catch (Exception)
                {
                    Debug.LogError("Reading Audio Channels Failed, Channels Will Reset");
                }

                return loadedChannels;
            }
        }

        void LoadOrSave()
        {
            if (File.Exists(savePath))
            {
                AudioChannel[] loadedChannels = Load();

                if (ValidateChannels(loadedChannels))
                {
                    audioChannels = loadedChannels;
                    audioChannels_s = loadedChannels;
                }
                else
                {
                    if(audioChannels_s.Length > 0)
                        Save();
                    else
                    {
                        Debug.LogError("No Audio Channels Defind And No Save Was Found Neither, Disabling AudioManager");
                        enabled = false;
                    }
                }
            }
            else
                Save();
        }


        bool ValidateChannels(AudioChannel[] channels)
        {
            if (channels == null)
                return false;
            if (audioChannels_s.Length == 0)
            {
                if (channels.Length > 0)
                {
                    Debug.LogWarning("No Audio Channels Defind, But A Save Was Found, Using Save Without Vaidating it");
                    return true;
                }
                    
                else
                {
                    return false;
                }
            }

            if (channels.Length != audioChannels_s.Length)
            {
                Debug.Log("OutDated Audio Chanels, Saving New Copy");
                return false;
            }

            for (int i = 0; i < channels.Length; i++)
            {
                if (channels[i].Name != audioChannels_s[i].Name)
                    return false;

                if (channels[i].Volume < 0 || channels[i].Volume > 1)
                {
                    Debug.LogError(channels[i].Name + " Volume Was Loaded Beyond The Range Of 0 - 1, Channels Will Reset");
                    return false;
                } 
            }

            return true;
        }


        public static void Subscribe(string channelName, AudioChannel.VolumeChanged method)
        {
            if (!channelsIndex.ContainsKey(channelName))
            {
                Debug.LogError("No Channel With The Name " + channelName + " Was Found");
                return;
            }

            audioChannels_s[channelsIndex[channelName]].OnVolumeChanged += method;
        }

        public static void UnSubscribe(string channelName, AudioChannel.VolumeChanged method)
        {
            if (!channelsIndex.ContainsKey(channelName))
            {
                Debug.LogError("No Channel With The Name " + channelName + " Was Found");
                return;
            }

            audioChannels_s[channelsIndex[channelName]].OnVolumeChanged -= method;
        }

        public static float GetVolume(string channelName)
        {
            if(!channelsIndex.ContainsKey(channelName))
            {
                Debug.LogError("No Channel With The Name " + channelName + " Was Found");
                return 0;
            }

            return audioChannels_s[channelsIndex[channelName]].Volume;
        }
    }

    [Serializable][DataContract]
    public class AudioChannel
    {
        [SerializeField][DataMember]
        string name;
        public string Name { get { return name; } }

        [SerializeField][DataMember]
        float volume;
        public float Volume
        {
            get
            {
                return volume;
            }
            internal set
            {
                value = Mathf.Clamp01(value);
                volume = value;

                if (volumeSlider)
                    volumeSlider.value = value;
            }
        }

        [IgnoreDataMember]
        Slider volumeSlider;

        public delegate void VolumeChanged(float newVolume);
        public VolumeChanged OnVolumeChanged;

        public void SetUI(Slider sl)
        {
            volumeSlider = sl;
            volumeSlider.value = volume;
            sl.onValueChanged.AddListener(ChangeVolume);
        }

        void ChangeVolume(float newVolume)
        {
            volume = newVolume;

            AudioManager.Save();

            if(OnVolumeChanged != null)
                OnVolumeChanged(volume);
        }
    }
}