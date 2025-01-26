using KnifeHit;
using MyUtils;
using UnityEngine;

namespace JumpDash
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] PlayerSO playerSO;
        [SerializeField] ParticleSystem particleSystem;

        [Header("Runtime parameter")]
        [SerializeField] Player player;
        [SerializeField] PlayerState currentState = PlayerState.Idle;
        [SerializeField] WallController currentWall;
        private void Awake()
        {
            if (playerSO != null)
            {
                player = playerSO.Player.CloneSelf();
            }
        }
        private void Start()
        {
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                Observer.Instance.AddObserver(ObserverConstants.START_GAME, (x) => SwitchToState(PlayerState.MoveLeft));
            }
            else
            {
                Observer.Instance.AddObserver(ObserverConstants.START_GAME, (x) => SwitchToState(PlayerState.MoveRight));
            }
            Observer.Instance.AddObserver(ObserverConstants.END_GAME, (x) => SwitchToState(PlayerState.Idle));
            Observer.Instance.AddObserver(GUIConstants.BUTTON_PLAY_CLICK, (x) =>
            {
                if (GameManager.Instance.CurrentLevel.CurrentState == LevelState.Playing)
                {
                    if (currentState == PlayerState.Idle)
                    {
                        if (currentWall.Type == WallType.Left)
                        {
                            SwitchToState(PlayerState.MoveRight);
                        }
                        else if (currentWall.Type == WallType.Right)
                        {
                            SwitchToState(PlayerState.MoveLeft);
                        }

                    }
                }
            });
        }

        private void Update()
        {
            UpdateState();
        }

        #region State Machine
        private void SwitchToState(PlayerState incomingState)
        {
            if (currentState == incomingState) return;
            switch (currentState)
            {
                case PlayerState.Idle:
                    Exit_Idle();
                    break;
                case PlayerState.MoveLeft:
                    Exit_MoveLeft();
                    break;
                case PlayerState.MoveRight:
                    Exit_MoveRight();
                    break;
            }

            switch (incomingState)
            {
                case PlayerState.Idle:
                    Enter_Idle();
                    break;
                case PlayerState.MoveLeft:
                    Enter_MoveLeft();
                    break;
                case PlayerState.MoveRight:
                    Enter_MoveRight();
                    break;
            }

            currentState = incomingState;
        }
        private void UpdateState()
        {
            switch (currentState)
            {
                case PlayerState.Idle:
                    Update_Idle();
                    break;
                case PlayerState.MoveLeft:
                    Update_MoveLeft();
                    break;
                case PlayerState.MoveRight:
                    Update_MoveRight();
                    break;
            }
        }
        #endregion
        #region State Idle
        private void Enter_Idle()
        {
            particleSystem.Play();
        }
        private void Update_Idle()
        {
        }
        private void Exit_Idle()
        {

        }
        #endregion
        #region State MoveLeft
        private void Enter_MoveLeft()
        {
            AudioManager.Instance.PlayAudio(AudioConstants.JUMP);
        }
        private void Update_MoveLeft()
        {
            transform.position += new Vector3(-player.Speed, 0, 0) * Time.deltaTime;
        }
        private void Exit_MoveLeft()
        {
        }
        #endregion
        #region State MoveRight
        private void Enter_MoveRight()
        {
            AudioManager.Instance.PlayAudio(AudioConstants.JUMP);
        }
        private void Update_MoveRight()
        {
            transform.position += new Vector3(player.Speed, 0, 0) * Time.deltaTime;
        }
        private void Exit_MoveRight()
        {
        }
        #endregion
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<WallController>(out var wall))
            {
                SwitchToState(PlayerState.Idle);
                currentWall = wall;
            }
            if (collision.TryGetComponent<ObstancleController>(out var obstancle))
            {
                Observer.Instance.Notify(ObserverConstants.END_GAME);
            }
        }
    }
}