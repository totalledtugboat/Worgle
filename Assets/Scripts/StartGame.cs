using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartGame : MonoBehaviour
{
    [SerializeField] private TMP_Text progressText;

    public void Awake()
    {
        progressText.enabled = false;

        bool gameStarted = PlayerPrefs.GetInt("gameStarted") == 1 ? true : false;

        if (gameStarted)
        {
            progressText.enabled = true;
            return;
        }
    }

    public void Begin()
    {
        SceneManager.LoadScene(1);
    }
}