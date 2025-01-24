using MyUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KnifeHit
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] List<LevelController> levelPrefabs;
        [SerializeField] LevelController currentLevel;
        [SerializeField] int currentLevelIndex = 0;
        public LevelController CurrentLevel { get => currentLevel; }


        public static GameManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            currentLevel = Instantiate(levelPrefabs[currentLevelIndex]);
            Observer.Instance.AddObserver(ObserverConstants.WIN_GAME, (x) => StartCoroutine(WaitToNextLevel())) ;
        }

        public void InitLevel()
        {
            if (currentLevel != null) Destroy(currentLevel.gameObject);
            currentLevel = Instantiate(levelPrefabs[currentLevelIndex]);
        }
        public void RestartLevel()
        {
            Debug.Log(currentLevel.ToString());
            InitLevel();
        }

        public void NextLevel()
        {
            currentLevelIndex++;
            if (currentLevelIndex >= levelPrefabs.Count)
            {
                currentLevelIndex = 0;
            }
            InitLevel();
        }

        public void PrevLevel()
        {
            currentLevelIndex--;
            if (currentLevelIndex < 0)
            {
                currentLevelIndex = levelPrefabs.Count - 1;
            }
            InitLevel();
        }

        public void ThrowKnife()
        {
            Observer.Instance.Notify(ObserverConstants.KNIFE_THROWN_BUTTON_INPUT);
        }

        IEnumerator WaitToNextLevel()
        {
            yield return new WaitForSeconds(2f);
            NextLevel();
        }
    }
}