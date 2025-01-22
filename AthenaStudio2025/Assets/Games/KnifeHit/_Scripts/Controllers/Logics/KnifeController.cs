using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace KnifeHit
{
    public class KnifeController : MonoBehaviour, IInterable
    {
        [SerializeField] private KnifeSO knifeSO;
        [SerializeField] private Knife knife;
        [SerializeField] private KnifeState currentState;

        private void Awake()
        {
            if (knifeSO != null)
            {
                knife = knifeSO.Knife.CloneSelf();

            }
        }

        private void Update()
        {
            

            UpdateState();
        }

        #region State Machine
        private void SwitchToState(KnifeState incomingState)
        {
            switch (currentState)
            {
                case KnifeState.Pending:
                    Exit_Pending();
                    break;
                case KnifeState.Flying:
                    Exit_Flying();
                    break;
                case KnifeState.Attaching:
                    Exit_Attaching();
                    break;
                case KnifeState.Falling:
                    Exit_Falling();
                    break;
            }

            switch (incomingState)
            {
                case KnifeState.Pending:
                    Enter_Pending();
                    break;
                case KnifeState.Flying:
                    Enter_Flying();
                    break;
                case KnifeState.Attaching:
                    Enter_Attaching();
                    break;
                case KnifeState.Falling:
                    Enter_Falling();
                    break;
            }

            currentState = incomingState;
        }
        private void UpdateState()
        {
            switch (currentState)
            {
                case KnifeState.Pending:
                    Update_Pending();
                    break;
                case KnifeState.Flying:
                    Update_Flying();
                    break;
                case KnifeState.Attaching:
                    Update_Attaching();
                    break;
                case KnifeState.Falling:
                    Update_Falling();
                    break;
            }
        }
        #endregion

        #region State Pending
        private void Enter_Pending()
        {

        }
        private void Update_Pending()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SwitchToState(KnifeState.Flying);
            }
        }
        private void Exit_Pending()
        {

        }
        #endregion

        #region State Flying
        private void Enter_Flying()
        {

        }
        private void Update_Flying()
        {
            transform.position += new Vector3(0, knife.FlyingSpeed, 0) * Time.deltaTime;
        }
        private void Exit_Flying()
        {

        }
        #endregion

        #region State Attaching
        private void Enter_Attaching()
        {

        }
        private void Update_Attaching()
        {

        }
        private void Exit_Attaching()
        {

        }
        #endregion

        #region State Attaching
        private void Enter_Falling()
        {

        }
        private void Update_Falling()
        {

        }
        private void Exit_Falling()
        {

        }
        #endregion

        public void Interact(string message)
        {
            if (message == InteractionConstants.LOG_INTERACTION)
            {
                SwitchToState(KnifeState.Attaching);
            }
        }
    }
}