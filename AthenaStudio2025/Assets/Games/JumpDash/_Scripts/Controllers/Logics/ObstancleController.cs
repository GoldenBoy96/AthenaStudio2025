using MyUtils;
using System.Collections;
using UnityEngine;

namespace JumpDash
{
    public class ObstancleController : MonoBehaviour
    {
        [SerializeField] ObstancleSO obstancleSO;
        [SerializeField] ObstancleType obstancleType;

        [Header("Runtime parameter")]
        [SerializeField] Obstancle obstancle;
        [SerializeField] ObstancleState currentState = ObstancleState.Idle;

        public Obstancle Obstancle { get => obstancle; }
        public ObstancleState CurrentState { get => currentState; }
        public ObstancleType ObstancleType { get => obstancleType; }

        private void Awake()
        {
            if (obstancleSO != null)
            {
                obstancle = obstancleSO.Obstancle.CloneSelf();
            }
        }
        private void OnEnable()
        {
            isScored = false; 
            if (obstancleSO != null)
            {
                obstancle = obstancleSO.Obstancle.CloneSelf();
            }
        }
        private void Start()
        {
            Observer.Instance.AddObserver(ObserverConstants.START_GAME, (x) => SwitchToState(ObstancleState.MoveDown));
            Observer.Instance.AddObserver(ObserverConstants.END_GAME, (x) => SwitchToState(ObstancleState.Idle));
            if (GameManager.Instance.CurrentLevel.CurrentState == LevelState.Playing)
            {
                SwitchToState(ObstancleState.MoveDown);
            }
        }
        private void Update()
        {
            UpdateState();
        }

        #region State Machine
        private void SwitchToState(ObstancleState incomingState)
        {
            if (currentState == incomingState) return;
            switch (currentState)
            {
                case ObstancleState.Idle:
                    Exit_Idle();
                    break;
                case ObstancleState.MoveDown:
                    Exit_MoveDown();
                    break;
            }

            switch (incomingState)
            {
                case ObstancleState.Idle:
                    Enter_Idle();
                    break;
                case ObstancleState.MoveDown:
                    Enter_MoveDown();
                    break;
            }

            currentState = incomingState;
        }
        private void UpdateState()
        {
            switch (currentState)
            {
                case ObstancleState.Idle:
                    Update_Idle();
                    break;
                case ObstancleState.MoveDown:
                    Update_MoveDown();
                    break;
            }
        }
        #endregion
        #region State Idle
        private void Enter_Idle()
        {
            StopCoroutine(WaitToDespawnObstancle());
        }
        private void Update_Idle()
        {
        }
        private void Exit_Idle()
        {

        }
        #endregion
        #region State MoveDown
        private void Enter_MoveDown()
        {
            StartCoroutine(WaitToDespawnObstancle());
        }
        private void Update_MoveDown()
        {
            transform.position += new Vector3(0, -obstancle.Speed, 0) * Time.deltaTime;
        }
        private void Exit_MoveDown()
        {
        }
        #endregion


        IEnumerator WaitToDespawnObstancle()
        {
            yield return new WaitForSeconds(10);
            PoolingHelper.ReturnObjectToPool(gameObject);
        }

        bool isScored = false;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log(collision);
            if (collision.TryGetComponent<ScoreAreaController>(out var score))
            {
                if (isScored) { return; }
                Observer.Instance.Notify(ObserverConstants.GAIN_SCORE);
                isScored = true;
            }
        }
    }
}