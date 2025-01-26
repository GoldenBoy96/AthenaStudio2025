using JumpDash;
using MyUtils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace KnifeHit
{
    public class KnifeController : MonoBehaviour, IInterable<Knife>
    {
        [SerializeField] private KnifeSO knifeSO;

        [Header("Runtime Parameter")]
        [SerializeField] private Knife knife;
        [SerializeField] private KnifeState currentState;

        private void Awake()
        {
            if (knifeSO != null)
            {
                knife = knifeSO.Knife.CloneSelf();
            }
        }
        private void Start()
        {
            Observer.Instance.AddObserver(ObserverConstants.LOSE_GAME, (x) => SwitchToState(KnifeState.Falling));
            Observer.Instance.AddObserver(ObserverConstants.WIN_GAME, (x) => SwitchToState(KnifeState.Falling));
            Observer.Instance.AddObserver(ObserverConstants.KNIFE_THROWN_BUTTON_INPUT, (x) => ThrowKnife());
        }

        private void Update()
        {

            UpdateState();
        }

        #region State Machine
        private void SwitchToState(KnifeState incomingState)
        {
            if (currentState == incomingState) return;
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
            //Observer.Instance.AddObserver(ObserverConstants.KNIFE_THROWN, (x) => SwitchToState(KnifeState.Flying));
        }
        private void Update_Pending()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ThrowKnife();
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
            transform.position = GameManager.Instance.CurrentLevel.KnifeStopPoint.position;
            GameManager.Instance.CurrentLevel.AttachKnifeToLog(transform);
            AudioManager.Instance.PlayAudio(AudioConstants.HIT_1);
        }
        private void Update_Attaching()
        {

        }
        private void Exit_Attaching()
        {

        }
        #endregion

        #region State Falling

        float rotateSpeed = 0;
        private void Enter_Falling()
        {
            if (gameObject.TryGetComponent<Rigidbody2D>(out var rg))
            {
                rg.gravityScale = 1;
                transform.parent = transform.root;
            }
            foreach (Collider2D collider in gameObject.GetComponents<Collider2D>())
            {
                Destroy(collider);
            }
            rotateSpeed = Random.Range(-300, 300);
        }
        private void Update_Falling()
        {
            transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
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
            if (message == InteractionConstants.KNIFE_INTERACTION)
            {
                if (!GameManager.Instance.CurrentLevel.IsEndGame)
                {
                    Observer.Instance.Notify(ObserverConstants.LOSE_GAME);
                }
            }
        }

        public void ThrowKnife()
        {
            if (currentState == KnifeState.Pending)
            {
                SwitchToState(KnifeState.Flying);
                Observer.Instance.Notify(ObserverConstants.KNIFE_THROWN);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IInterable<Log>>(out var log))
            {
                Interact(InteractionConstants.LOG_INTERACTION);
            }
            if (collision.TryGetComponent<IInterable<Knife>>(out var knife))
            {
                Interact(InteractionConstants.KNIFE_INTERACTION);
            }
        }

        private void OnDestroy()
        {
            Observer.Instance.RemoveObserver(ObserverConstants.LOSE_GAME, (x) => SwitchToState(KnifeState.Falling));
            
        }
    }
}