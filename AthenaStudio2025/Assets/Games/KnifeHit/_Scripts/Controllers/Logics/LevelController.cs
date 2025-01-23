using KnifeHit;
using MyUtils;
using System.Collections;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] LevelSO levelSO;


    [SerializeField] Transform logHolder;
    [SerializeField] Transform knifeHolder;
    [SerializeField] Transform knifeStopPoint;

    [Header("Runtime Parameter")]
    [SerializeField] Level level;
    [SerializeField] LogController logPrefab;
    [SerializeField] KnifeController knifePrefab;

    [SerializeField] LogController currentLog;
    [SerializeField] KnifeController currentKnife;

    [SerializeField] int totalKnife = 0;
    [SerializeField] int amountKnifeLeft = 0;

    public Transform LogHolder { get => logHolder;}
    public Transform KnifeHolder { get => knifeHolder;}
    public Transform KnifeStopPoint { get => knifeStopPoint; }

    private void Awake()
    {
        if (levelSO != null)
        {
            level = levelSO.Level.CloneSelf();
            logPrefab = level.LogPrefab;
            knifePrefab = level.KnifePrefab;
        }
    }
    private void Start()
    {
        InitLevel();
        Observer.Instance.AddObserver(ObserverConstants.KNIFE_THROWN, (x) => CheckGameOver());
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
        if (amountKnifeLeft > 0)
        {
            StartCoroutine(SpawnKnifeAfterCooldown());
        }
        else
        {
            Debug.Log("You win");
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
        }
    }

    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(ObserverConstants.KNIFE_THROWN, (x) => CheckGameOver());
    }
}
