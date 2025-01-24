using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemManager : MonoBehaviour
{
    private static SystemManager instance;
    public static SystemManager Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(SceneConstants.MainMenu);
    }

    public void JoinKnifeHit()
    {
        SceneManager.LoadScene(SceneConstants.KnifeHit);
    }

    public void JoinJumpDash()
    {
        SceneManager.LoadScene(SceneConstants.JumpDash);
    }
}
