using System.Collections;
using UnityEngine;

namespace KnifeHit
{
    public class LogController : MonoBehaviour
    {
        [SerializeField] private LogSO logSO;
        [SerializeField] private Log log;
        [SerializeField] private int currentRotationElementIndex;
        [SerializeField] private float currentSpeed;

        private void Awake()
        {
            if (logSO != null)
            {
                log = logSO.Log.CloneSelf();

            }

            currentRotationElementIndex = 0;
        }

        private void StartRotate()
        {
            //wait for seconds duration để break và chuyển sang rotation mới
            //wait until đạt max speed để ngưng gia tốc
        }

        private void StopRotate()
        {
            //wait for seconds duration để break và chuyển sang rotation mới
            //wait until đạt max speed để ngưng gia tốc
        }

        private IEnumerator WaitToStopRotating()
        {
            yield return new WaitForSeconds(log.RotationPattern[currentRotationElementIndex].Duration);
            Debug.Log("Stop rotate");
        }

        private IEnumerator WaitToStopAccelerating()
        {
            yield return new WaitUntil(() => currentSpeed == log.RotationPattern[currentRotationElementIndex].MaxSpeed);
            Debug.Log("Stop accelerating");
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IInterable>(out var target))
            {
                target.Interact(InteractionConstants.LOG_INTERACTION);
            }
        }
    }
}