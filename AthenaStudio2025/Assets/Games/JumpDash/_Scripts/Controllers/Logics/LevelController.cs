using KnifeHit;
using MyUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpDash
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] LevelSO levelSO;
        [SerializeField] Transform obstancleSpawnerLeft;
        [SerializeField] Transform obstancleSpawnerMid;
        [SerializeField] Transform obstancleSpawnerRight;
        [SerializeField] List<GameObject> obstanclePrefabs;

        [Header("Runtime parameter")]
        [SerializeField] Level level;
        [SerializeField] LevelState currentState = LevelState.Pending;
        [SerializeField] int score = 0;

        public Level Level { get => level; }
        public LevelState CurrentState { get => currentState; }
        public int Score { get => score; }

        private void Awake()
        {
            if (levelSO != null)
            {
                level = levelSO.Level.CloneSelf();
            }
            score = 0;
            Observer.Instance.Notify(ObserverConstants.GAIN_SCORE);

        }
        private void OnEnable()
        {
            if (levelSO != null)
            {
                level = levelSO.Level.CloneSelf();
            }
            score = 0;
            Observer.Instance.Notify(ObserverConstants.GAIN_SCORE);
        }
        private void Start()
        {
            Observer.Instance.AddObserver(ObserverConstants.START_GAME, (x) => SwitchToState(LevelState.Playing));
            Observer.Instance.AddObserver(ObserverConstants.END_GAME, (x) => SwitchToState(LevelState.Ending));
            Observer.Instance.AddObserver(ObserverConstants.GAIN_SCORE, (x) => IncreaseScore()); ;
        }
        private void Update()
        {
            UpdateState();
        }

        #region State Machine
        public void SwitchToState(LevelState incomingState)
        {
            if (currentState == incomingState) return;
            //Debug.Log(incomingState.ToString());
            switch (currentState)
            {
                case LevelState.Pending:
                    Exit_Pending();
                    break;
                case LevelState.Playing:
                    Exit_Playing();
                    break;
                case LevelState.Ending:
                    Exit_Ending();
                    break;
            }

            switch (incomingState)
            {
                case LevelState.Pending:
                    Enter_Pending();
                    break;
                case LevelState.Playing:
                    Enter_Playing();
                    break;
                case LevelState.Ending:
                    Enter_Ending();
                    break;
            }

            currentState = incomingState;
        }
        private void UpdateState()
        {
            switch (currentState)
            {
                case LevelState.Pending:
                    Update_Pending();
                    break;
                case LevelState.Playing:
                    Update_Playing();
                    break;
                case LevelState.Ending:
                    Update_Ending();
                    break;
            }
        }
        #endregion
        #region State Pending
        private void Enter_Pending()
        { }
        private void Update_Pending()
        {
        }
        private void Exit_Pending()
        {

        }
        #endregion

        #region State Playing
        private void Enter_Playing()
        {
            StartCoroutine(WaitToSpawnFirstObstancle());
        }
        private void Update_Playing()
        {
        }
        private void Exit_Playing()
        {
        }

        #endregion
        #region State Ending
        private void Enter_Ending()
        {
            StopAllCoroutines();
        }
        private void Update_Ending()
        {
        }
        private void Exit_Ending()
        {

        }
        #endregion
        IEnumerator WaitToSpawnFirstObstancle()
        {
            yield return new WaitForSeconds(level.BeginDelayTime);
            StartCoroutine(WaitToSpawnObstancle());
        }
        IEnumerator WaitToSpawnObstancle()
        {
            SpawnObstancle();
            yield return new WaitForSeconds(level.DeltaTime);
            StartCoroutine(WaitToSpawnObstancle());
        }
        private void SpawnObstancle()
        {
            int randomObstancleIndex = Random.Range(0, obstanclePrefabs.Count);
            //var obstancle = Instantiate(obstanclePrefabs[randomObstancleIndex], transform);
            GameObject obstancle = PoolingHelper.SpawnObject(obstanclePrefabs[randomObstancleIndex].gameObject, transform);
            if (obstanclePrefabs[randomObstancleIndex].GetComponent<ObstancleController>().ObstancleType == ObstancleType.Mono)
            {
                obstancle.transform.position = obstancleSpawnerMid.position;
            }
            else if (obstanclePrefabs[randomObstancleIndex].GetComponent<ObstancleController>().ObstancleType == ObstancleType.Wall)
            {
                int randomSpawner = Random.Range(0, 3);
                if (randomSpawner == 0)
                {
                    obstancle.transform.position = obstancleSpawnerLeft.position;
                }
                else
                {
                    obstancle.transform.position = obstancleSpawnerRight.position;
                }
            }

            //Debug.Log(randomObstancleIndex + " | " + obstanclePrefabs.Count + " | " + obstancle.GetComponent<Obstancle>());
            float randomRange = Random.Range(
                -obstancle.GetComponent<ObstancleController>().Obstancle.RandomPositionRange,
                obstancle.GetComponent<ObstancleController>().Obstancle.RandomPositionRange);
            //Debug.Log(obstancle.GetComponent<Obstancle>().RandomPositionRange + " | " + randomRange);
            obstancle.transform.position = new Vector3(
                    obstancle.transform.position.x + randomRange,
                    obstancle.transform.position.y,
                    obstancle.transform.position.z);
        }

        private void IncreaseScore()
        {
            Debug.Log(score);
            score++;
        }
    }
}