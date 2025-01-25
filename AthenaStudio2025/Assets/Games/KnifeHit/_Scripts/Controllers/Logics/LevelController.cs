
using MyUtils;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace KnifeHit
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] LevelSO levelSO;


        [SerializeField] Transform logHolder;
        [SerializeField] Transform knifeHolder;
        [SerializeField] Transform knifeStopPoint;
        [SerializeField] Canvas backgroundCanvas;
        [SerializeField] Image backgroundImage;

        [Header("Runtime Parameter")]
        [SerializeField] Level level;
        [SerializeField] LogController logPrefab;
        [SerializeField] KnifeController knifePrefab;

        [SerializeField] LogController currentLog;
        [SerializeField] KnifeController currentKnife;

        [SerializeField] int totalKnife = 0;
        [SerializeField] int amountKnifeLeft = 0;

        [SerializeField] LevelState levelState = LevelState.Playing;
        [SerializeField] bool isEndGame = false;

        public Transform LogHolder { get => logHolder; }
        public Transform KnifeHolder { get => knifeHolder; }
        public Transform KnifeStopPoint { get => knifeStopPoint; }
        public LogController CurrentLog { get => currentLog; }
        public KnifeController CurrentKnife { get => currentKnife; }
        public bool IsEndGame { get => isEndGame; }

        private void Awake()
        {
            if (levelSO != null)
            {
                level = levelSO.Level.CloneSelf();
                logPrefab = level.LogPrefab;
                knifePrefab = level.KnifePrefab;
                backgroundCanvas.worldCamera = Camera.main;
                backgroundCanvas.sortingLayerName = "Background";
                backgroundImage.sprite = level.Background;
            }
        }
        private void Start()
        {
            levelState = LevelState.Playing;
            isEndGame = false;
            InitLevel();
            Observer.Instance.AddObserver(ObserverConstants.KNIFE_THROWN, (x) => CheckGameOver());
            Observer.Instance.AddObserver(ObserverConstants.LOSE_GAME, (x) =>
            {
                levelState = LevelState.Losing;
                CheckGameOver();
            });
            Observer.Instance.AddObserver(ObserverConstants.WIN_GAME, (x) =>
            {
                levelState = LevelState.Winning;
                CheckGameOver();
            });
            SetUpItem();
        }

        private void SetUpItem()
        {
            foreach (var itemPrefab in level.ItemLists)
            {
                ItemController itemController = PoolingHelper.SpawnObject(itemPrefab.gameObject, currentLog.transform.parent).GetComponent<ItemController>();

                //ItemController itemController = Instantiate(itemPrefab, currentLog.transform.parent);
                itemController.transform.position = currentLog.transform.position;
                itemController.transform.position = new Vector3(itemController.transform.position.x,
                    itemController.transform.position.y + itemController.Item.Distance,
                    itemController.transform.position.z);
                currentLog.transform.rotation = Quaternion.Euler(currentLog.transform.rotation.x,
                    currentLog.transform.rotation.y,
                    -itemController.Item.Degree);
                itemController.transform.parent = currentLog.transform;
                currentLog.transform.rotation = Quaternion.Euler(0, 0, 0);

            }
        }

        private void InitLevel()
        {
            SpawnLog();
            SpawnKnife();
            totalKnife = level.KnifeAmount;
            amountKnifeLeft = totalKnife;
        }
        private void SpawnLog()
        {
            currentLog = PoolingHelper.SpawnObject(logPrefab.gameObject, logHolder).GetComponent<LogController>();
            //currentLog = Instantiate(logPrefab, logHolder);
        }

        private void SpawnKnife()
        {
            currentKnife = PoolingHelper.SpawnObject(knifePrefab.gameObject, knifeHolder).GetComponent<KnifeController>();
            //currentKnife = Instantiate(knifePrefab, knifeHolder);
            amountKnifeLeft -= 1;
        }

        private void CheckGameOver()
        {
            //Debug.Log(levelState);
            if (isEndGame) { return; }
            switch (levelState)
            {
                case LevelState.Playing:
                    StopAllCoroutines();
                    if (!isEndGame)
                    {
                        StartCoroutine(SpawnKnifeAfterCooldown());
                    }
                    break;
                case LevelState.Winning:
                    StopAllCoroutines();
                    //Debug.Log("You win");
                    if (!isEndGame)
                    {
                        AudioManager.Instance.PlayAudio(AudioConstants.HIT_2);
                        isEndGame = true;
                    }
                    break;
                case LevelState.Losing:
                    //StopCoroutine(nameof(SpawnKnifeAfterCooldown));
                    StopAllCoroutines();
                    //Debug.Log("You lose");
                    if (!isEndGame)
                    {
                        AudioManager.Instance.PlayAudio(AudioConstants.HIT_3);
                        isEndGame = true;
                    }
                    break;
            }

        }

        IEnumerator SpawnKnifeAfterCooldown()
        {
            yield return new WaitForSeconds(level.ReloadCooldown);
            SpawnKnife();
        }
        public void AttachKnifeToLog(Transform knife)
        {
            if (currentLog != null)
            {
                knife.parent = currentLog.LogTransform;
                if (amountKnifeLeft <= 1)
                {
                    Observer.Instance.Notify(ObserverConstants.WIN_GAME);
                }
            }
        }

        private void OnDestroy()
        {
            Observer.Instance.RemoveObserver(ObserverConstants.KNIFE_THROWN, (x) => CheckGameOver());
        }
    }
}