using UnityEngine;
using System.Collections;

namespace UGM
{
    [ExecuteInEditMode]
    public class MultiPanel : MonoBehaviour
    {

        [SerializeField]
        GameObject[] panels;

        [SerializeField]
        int index = 0;
        public int Index { get { return index; } set { index = value; SwapPanels(); } }

        void Start()
        {
            SwapPanels();
        }

#if UNITY_EDITOR
        void Update()
        {
            SwapPanels();
        }
#endif

        void SwapPanels()
        {
            for (int i = 0; i < panels.Length; i++)
            {
                if (i == index)
                    panels[i].SetActive(true);
                else
                    panels[i].SetActive(false);
            }
        }
    }
}