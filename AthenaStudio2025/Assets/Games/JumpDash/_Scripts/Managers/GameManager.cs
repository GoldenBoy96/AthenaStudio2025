using MyUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JumpDash
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] List<LevelController> levelPrefabs;
        [SerializeField] LevelController currentLevel;
        [SerializeField] int currentLevelIndex = 0;

        public static GameManager Instance { get; private set; }
        public LevelController CurrentLevel { get => currentLevel;}

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
            InitLevel();
            Observer.Instance.AddObserver(GUIConstants.BUTTON_PLAY_CLICK, (x) => StartGame());
        }
        public void StartGame()
        {
            if (currentLevel.CurrentState == LevelState.Pending) {
                Observer.Instance.Notify(ObserverConstants.START_GAME);
                Debug.Log("START_GAME");
            }
        }

        public void InitLevel()
        {
            if (currentLevel != null) Destroy(currentLevel.gameObject);
            currentLevel = Instantiate(levelPrefabs[currentLevelIndex]);
            //currentLevel = PoolingHelper.SpawnObject(levelPrefabs[currentLevelIndex].gameObject, transform).GetComponent<LevelController>();
        }
        public void RestartLevel()
        {
            Debug.Log(currentLevel.ToString());
            InitLevel();
            Observer.Instance.Notify(ObserverConstants.RESTART_GAME);
        }
    }
}