using KnifeHit;
using MyUtils;
using UnityEngine;

namespace KnifeHit
{
    public class ItemController : MonoBehaviour, IInterable<Item>
    {
        //[SerializeField] private ItemSO itemSO;

        [SerializeField] private Item item;

        [Header("Runtime Parameter")]
        [SerializeField] private ItemState currentState;


        public Item Item { get => item; }

        private void Start()
        {
            Observer.Instance.AddObserver(ObserverConstants.WIN_GAME, (x) => SwitchToState(ItemState.Falling));
        }

        private void Update()
        {
            UpdateState();
        }

        #region State Machine
        private void SwitchToState(ItemState incomingState)
        {
            if (currentState == incomingState) return;
            switch (currentState)
            {
                case ItemState.Pending:
                    Exit_Pending();
                    break;
                case ItemState.Falling:
                    Exit_Falling();
                    break;
            }

            switch (incomingState)
            {
                case ItemState.Pending:
                    Enter_Pending();
                    break;
                case ItemState.Falling:
                    Enter_Falling();
                    break;
            }

            currentState = incomingState;
        }
        private void UpdateState()
        {
            switch (currentState)
            {
                case ItemState.Pending:
                    Update_Pending();
                    break;
                case ItemState.Falling:
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
        }
        private void Exit_Pending()
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
            //Destroy(gameObject);
            PoolingHelper.ReturnObjectToPool(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log(collision.gameObject.name);
            if (collision.TryGetComponent<IInterable<Knife>>(out var item))
            {
                Interact(InteractionConstants.KNIFE_INTERACTION);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log(collision.gameObject.name);
            if (collision.gameObject.TryGetComponent<IInterable<Knife>>(out var item))
            {
                Interact(InteractionConstants.KNIFE_INTERACTION);
            }

        }
       
    }
}
