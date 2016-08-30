using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UGM
{
    public class ButtonKeyApply : MonoBehaviour
    {
        [SerializeField]
        KeyCode key;

        [SerializeField]
        bool applyWhenUnInteractable = false;

        Button butt;

        void Start()
        {
            butt = GetComponent<Button>();
        }

        void Update()
        {
            if (Input.GetKeyDown(key))
            {
                if (applyWhenUnInteractable)
                    butt.onClick.Invoke();
                else if (butt.interactable)
                    butt.onClick.Invoke();
            }
        }
    }
}