using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UGM
{
    public class ButtonTransfer : MonoBehaviour
    {

        [SerializeField]
        GameObject[] enableMenu;
        [SerializeField]
        GameObject[] disableMenu;

        [SerializeField]
        bool disableParent = true;

        Button butt;

        void Start()
        {
            butt = GetComponent<Button>();
            butt.onClick.AddListener(SwitchMenu);
        }

        void SwitchMenu()
        {
            for (int i = 0; i < enableMenu.Length; i++)
            {
                enableMenu[i].SetActive(true);
            }
            for (int i = 0; i < disableMenu.Length; i++)
            {
                disableMenu[i].SetActive(false);
            }

            if (disableParent)
                transform.parent.gameObject.SetActive(false);
        }
    }
}