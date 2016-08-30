using UnityEngine;
using System.Collections;

namespace UGM
{
    public class UpdateAudioSource : MonoBehaviour
    {
        [SerializeField]
        new string name;

        AudioSource aud;

        void Start()
        {
            aud = GetComponent<AudioSource>();

            ChangeVolume(AudioManager.GetVolume(name));
            AudioManager.Subscribe(name, ChangeVolume);
        }

        void ChangeVolume(float newVolume)
        {
            aud.volume = newVolume;
        }
    }
}