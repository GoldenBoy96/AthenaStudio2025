using KnifeHit;
using MyUtils;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        currentLog = Instantiate(logPrefab, logHolder);
    }

    private void SpawnKnife()
    {
        currentKnife = Instantiate(knifePrefab, knifeHolder);
        amountKnifeLeft -= 1;
    }

    private void CheckGameOver()
    {
        Debug.Log(levelState);
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
                Debug.Log("You win");
                if (!isEndGame)
                {
                    AudioManager.Instance.PlayAudio(AudioConstants.HIT_2);
                    isEndGame = true;
                }
                break;
            case LevelState.Losing:
                //StopCoroutine(nameof(SpawnKnifeAfterCooldown));
                StopAllCoroutines();
                Debug.Log("You lose");
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
