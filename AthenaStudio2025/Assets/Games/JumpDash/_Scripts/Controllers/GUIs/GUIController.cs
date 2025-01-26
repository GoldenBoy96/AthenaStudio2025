using JumpDash;
using MyUtils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    [SerializeField] TMP_Text scoreLabel;
    [SerializeField] GameObject pendingScreen;
    [SerializeField] GameObject levelScreen;
    [SerializeField] GameObject loseScreen;

    [Header("Runtime paramete")]
    [SerializeField] List<GameObject> screens = new List<GameObject>();
    public static GUIController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        scoreLabel.text = "0";

    }

    private void Start()
    {
        scoreLabel.text = "0";
        Observer.Instance.AddObserver(ObserverConstants.GAIN_SCORE, (x) => UpdateScore());

        screens.Clear();
        screens.Add(pendingScreen);
        screens.Add(levelScreen);
        screens.Add(loseScreen);
        Observer.Instance.AddObserver(ObserverConstants.START_GAME,
            (x) => ChangeToLevelScreen());
        Observer.Instance.AddObserver(ObserverConstants.END_GAME,
            (x) => StartCoroutine(WaitToEnableLoseScreen()));
        Observer.Instance.AddObserver(ObserverConstants.RESTART_GAME,
            (x) => ChangeToPendingScreen());
    }
    public void OnButtonPlayClick()
    {
        Observer.Instance.Notify(GUIConstants.BUTTON_PLAY_CLICK);
    }

    public void UpdateScore()
    {
        //Debug.Log(GameManager.Instance.CurrentLevel.Score);
        scoreLabel.text = GameManager.Instance.CurrentLevel.Score.ToString();
    }
    private void DisableAllScreen()
    {
        foreach (GameObject screen in screens)
        {
            screen.SetActive(false);
        }
    }
    public void ChangeToPendingScreen()
    {
        DisableAllScreen();
        pendingScreen.SetActive(true);
        scoreLabel.text = "0";
    }
    public void ChangeToLevelScreen()
    {
        DisableAllScreen();
        levelScreen.SetActive(true);
    }
    public void ChangeToLoseScreen()
    {
        DisableAllScreen();
        loseScreen.SetActive(true);
    }

    IEnumerator WaitToEnableLoseScreen()
    {
        yield return new WaitForSeconds(1f);
        ChangeToLoseScreen();
    }
}
