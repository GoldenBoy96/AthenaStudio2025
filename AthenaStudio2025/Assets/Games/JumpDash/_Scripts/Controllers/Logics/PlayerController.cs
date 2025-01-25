using KnifeHit;
using UnityEngine;

namespace JumpDash
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] PlayerSO playerSO;

        [Header("Runtime parameter")]
        [SerializeField] Player player;
        [SerializeField] PlayerState currentState = PlayerState.Idle; private void Awake()
        {
            if (playerSO != null)
            {
                player = playerSO.Player.CloneSelf();
            }
        }

        private void Update()
        {

            UpdateState();
        }

        #region State Machine
        private void SwitchToState(PlayerState incomingState)
        {
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
        { }
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
            }
        }
    }
}