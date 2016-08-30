using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

namespace UGM
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        string transitionScene;
        [SerializeField]
        string transitionScenePath;

        [SerializeField]
        string loadEndNote = "Press Any Key To Continue";

        Slider loadingBar;
        Text loadNotice;

        string levelToLoad;
        System.Action loadFinishAction;

        static AsyncOperation loadLevelOp;
        static public AsyncOperation LoadLevelOp { get { return loadLevelOp; } }

        static bool init = false;

        void Start()
        {
            if(init)
                Destroy(gameObject);
            else
            {
                init = true;
                DontDestroyOnLoad(gameObject);
            }
        }

        internal void LoadLevel(GameLevel level, System.Action finishAction = null)
        {
            levelToLoad = level.SceneName;
            loadFinishAction = finishAction;

            loadLevelOp = SceneManager.LoadSceneAsync(transitionScene);
            StartCoroutine(LoadTransition());
        }

        internal void LoadLevel(string level, System.Action finishAction = null)
        {
            levelToLoad = level;
            loadFinishAction = finishAction;

            loadLevelOp = SceneManager.LoadSceneAsync(transitionScene);
            StartCoroutine(LoadTransition());
        }

        internal void LoadLevelDirect(GameLevel level)
        {
            SceneManager.LoadScene(level.SceneName);
        }

        IEnumerator LoadTransition()
        {
            while(!loadLevelOp.isDone)
            {
                yield return new WaitForFixedUpdate();
            }

            loadLevelOp = SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Single);
            loadLevelOp.allowSceneActivation = false;

            loadingBar = FindObjectOfType<Slider>();
            loadNotice = FindObjectOfType<Text>();
            StartCoroutine(Load());
        }

        IEnumerator Load()
        {
            do
            {
                if (loadLevelOp.progress == 0.9f)
                {
                    if (loadNotice.text != loadEndNote)
                        loadNotice.text = loadEndNote;

                    loadingBar.value = 1;

                    if(Input.anyKey)
                        loadLevelOp.allowSceneActivation = true;
                }
                else
                    loadingBar.value = loadLevelOp.progress;

                yield return new WaitForFixedUpdate();
            } while (loadLevelOp.isDone != true && loadLevelOp.allowSceneActivation == false);

            loadingBar = null;
            loadNotice = null;

            if (loadFinishAction != null)
                loadFinishAction();
        }
    }
}
