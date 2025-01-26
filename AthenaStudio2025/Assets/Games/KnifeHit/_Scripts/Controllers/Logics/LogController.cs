using MyUtils;
using System.Collections;
using UnityEngine;

namespace KnifeHit
{
    public class LogController : MonoBehaviour, IInterable<Log>
    {
        [SerializeField] private Transform logTransform;
        [SerializeField] private LogSO logSO;

        [Header("Runtime Parameter")]
        [SerializeField] private Log log;
        [SerializeField] private int currentRotationElementIndex = -1;
        [SerializeField] private float currentSpeed;
        [SerializeField] private float currentAcceleration;
        [SerializeField] private LogState currentState;

        public Transform LogTransform { get => logTransform;}

        private void Awake()
        {
            if (logSO != null)
            {
                log = logSO.Log.CloneSelf();
            }

            currentRotationElementIndex = 0;
        }
        private void Start()
        {
            StartNextRotate();
            SwitchToState(LogState.Rotating);
            Observer.Instance.AddObserver(ObserverConstants.LOSE_GAME, (x) => SwitchToState(LogState.Stopping));
            Observer.Instance.AddObserver(ObserverConstants.WIN_GAME, (x) => SwitchToState(LogState.Falling));

        }

        private void FixedUpdate()
        {
            UpdateState();
        }

        #region State Machine
        private void SwitchToState(LogState incomingState)
        {
            if (currentState == incomingState) return;
            switch (currentState)
            {
                case LogState.Rotating:
                    Exit_Rotating();
                    break;
                case LogState.Stopping:
                    Exit_Stopping();
                    break;
                case LogState.Falling:
                    Exit_Falling();
                    break;
            }

            switch (incomingState)
            {
                case LogState.Rotating:
                    Enter_Rotating();
                    break;
                case LogState.Stopping:
                    Enter_Stopping();
                    break;
                case LogState.Falling:
                    Enter_Falling();
                    break;
            }

            currentState = incomingState;
        }
        private void UpdateState()
        {
            switch (currentState)
            {
                case LogState.Rotating:
                    Update_Rotating();
                    break;
                case LogState.Stopping:
                    Update_Stopping();
                    break;
                case LogState.Falling:
                    Update_Falling();
                    break;
            }
        }
        #endregion

        #region State Rotating
        private void Enter_Rotating()
        {
        }
        private void Update_Rotating()
        {
            currentSpeed += currentAcceleration / 60f;
            logTransform.Rotate(new Vector3(0, 0, currentSpeed * Time.deltaTime));
        }
        private void Exit_Rotating()
        {

        }
        #endregion

        #region State Stopping
        private void Enter_Stopping()
        {
            StopAllCoroutines();
        }
        private void Update_Stopping()
        {
        }
        private void Exit_Stopping()
        {

        }
        #endregion
        #region State Falling
        float rotateSpeed = 0;
        private void Enter_Falling()
        {
            StopAllCoroutines();
            if (gameObject.TryGetComponent<Rigidbody2D>(out var rg))
            {
                rg.gravityScale = 1;
                transform.parent = transform.root;
                foreach (Collider2D collider in gameObject.GetComponents<Collider2D>())
                {
                    Destroy(collider);
                }
            }
            rotateSpeed = Random.Range(-500, 500);
        }
        private void Update_Falling()
        {
            transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
        }
        private void Exit_Falling()
        {

        }
        #endregion
        private void StartNextRotate()
        {
            //wait for seconds duration để break và chuyển sang rotation mới
            //wait until đạt max speed để ngưng gia tốc
            StopAllCoroutines();
            ChangeRotationElement();
            currentAcceleration = log.RotationPattern[currentRotationElementIndex].Acceleration;
            if (log.RotationPattern[currentRotationElementIndex].MaxSpeed < currentSpeed)
            {
                currentAcceleration = -currentAcceleration;
            }
            StartCoroutine(WaitToStopRotating());
            StartCoroutine(WaitToStopAccelerating());

        }
        private IEnumerator WaitToStopRotating()
        {
            yield return new WaitForSeconds(log.RotationPattern[currentRotationElementIndex].Duration);
            //Debug.Log("Stop rotate");
            StartNextRotate();
        }
        private IEnumerator WaitToStopAccelerating()
        {
            yield return new WaitUntil(() => currentAcceleration != 0);
            float maxSpeed = log.RotationPattern[currentRotationElementIndex].MaxSpeed;
            if (currentAcceleration > 0)
            {
                yield return new WaitUntil(() => currentSpeed >= maxSpeed);
            }
            else
            {
                yield return new WaitUntil(() => currentSpeed <= maxSpeed);
            }
            

            //Debug.Log("Stop accelerating");
            currentAcceleration = 0;
        }
        private void ChangeRotationElement()
        {
            if (currentRotationElementIndex >= 0 && currentRotationElementIndex < log.RotationPattern.Count - 1)
            {
                currentRotationElementIndex++;
            }
            else
            {
                currentRotationElementIndex = 0;
            }
        }
        public void Interact(string message)
        {
        }
        
    }
}