using MyUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnifeHit
{
    public class LevelUIController : MonoBehaviour
    {
        [SerializeField] GameObject levelScreen;
        [SerializeField] GameObject loseScreen;

        [Header("Runtime paramete")]
        [SerializeField] List<GameObject> screens = new List<GameObject>();

        private static LevelUIController instance;
        public static LevelUIController Instance { get => instance; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            screens.Clear();
            screens.Add(levelScreen);
            screens.Add(loseScreen);
            Observer.Instance.AddObserver(ObserverConstants.START_GAME,
                (x) => ChangeToLevelScreen());
            Observer.Instance.AddObserver(ObserverConstants.LOSE_GAME, 
                (x) => StartCoroutine(WaitToEnableLoseScreen()));
        }

        private void DisableAllScreen()
        {
            foreach (GameObject screen in screens)
            {
                screen.SetActive(false);
            }
        }

        public void ChangeToLevelScreen()
        {
            DisableAllScreen();
            levelScreen.SetActive(true);
        }
        public void ChangeToLoseScreen()
        {
            DisableAllScreen();
            loseScreen.SetActive(true);
        }

        IEnumerator WaitToEnableLoseScreen()
        {
            yield return new WaitForSeconds(1f);
            ChangeToLoseScreen();
        }
        public void OnRestartLevelClick()
        {
            GameManager.Instance.RestartLevel();
            ChangeToLevelScreen();
        }

        public void OnNextLevelClick()
        {
            GameManager.Instance.NextLevel();
        }

        public void OnPrevLevelClick()
        {
            GameManager.Instance.PrevLevel();
        }

        public void OnThrowKnifeClick()
        {
            GameManager.Instance.ThrowKnife();
        }
    }
}