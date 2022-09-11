using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string challengeWord;
    public List<string> wordsList;
    private WordDictionary wordDictionary;

    public Color inWordWrongPlace;
    public Color inWordRightPlace;
    public Color usedLetter;
    public Color defaultBoxColor;

    [SerializeField] private Keyboard keyboard;

    [HideInInspector] public string[] wordOfTheDayChars;
    public List<string> allGuessedLetters;

    [HideInInspector] public List<Row> rows;

    [SerializeField] private bool warningDisplayed;

    public TMP_Text wordLengthWarning;
    public TMP_Text wordNotRealWarning;
    [SerializeField] private TMP_Text solutionWas;
    [SerializeField] private TMP_Text solvedWord;
    [SerializeField] private GameObject gameWonPopup;
    [SerializeField] private GameObject gameLostPopup;

    private void Awake()
    {
        bool gameStarted = PlayerPrefs.GetInt("gameStarted") == 1 ? true : false;

        if (gameStarted)
        {
            challengeWord = PlayerPrefs.GetString("challengeWord");

            for (int i = 0; i < rows.Count; i++)
            {
                rows[i].active = false;
                rows[i].inputField.text = PlayerPrefs.GetString(rows[i].ToString());

                for (int j = 0; j < rows[i].inputField.text.Length; j++)
                {
                    allGuessedLetters.Add(rows[i].inputField.text[j].ToString());
                }
            }

            int active = PlayerPrefs.GetInt("activerow");
            rows[active].active = true;

            return;
        }

        rows[0].active = true;

        PlayerPrefs.SetInt("gameStarted", 1);

        int randomWord = Random.Range(0, wordsList.Count);

        for (int i = 0; i < wordsList.Count; i++)
        {
            challengeWord = wordsList[randomWord];
        }

        PlayerPrefs.SetString("challengeWord", challengeWord);
        PlayerPrefs.Save();
    }

    private void Start()
    {
        wordDictionary = GetComponent<WordDictionary>();

        wordOfTheDayChars = new string[challengeWord.Length];

        for (int i = 0; i < wordOfTheDayChars.Length; i++)
        {
            wordOfTheDayChars[i] += challengeWord[i];
        }
    }

    public bool RealWordCheck(string word)
    {
        return wordDictionary.allFiveLetterWords.Contains(word);
    }

    public void BackToTitleScreen()
    {
        bool gameStarted = PlayerPrefs.GetInt("gameStarted") == 1 ? true : false;

        if (gameStarted)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                PlayerPrefs.SetString(rows[i].ToString(), rows[i].inputField.text);

                if (rows[i].active)
                    PlayerPrefs.SetInt("activerow", i);
            }
        }

        PlayerPrefs.Save();

        SceneManager.LoadScene("WorgleTitleScreen", LoadSceneMode.Single);
    }

    public void EndGame(bool gameWon)
    {
        solutionWas.enabled = true;
        solvedWord.text = challengeWord;
        PlayerPrefs.SetString("challengeWord", "");
        solvedWord.enabled = true;
        PlayerPrefs.SetInt("activerow", 0);
        PlayerPrefs.SetInt("gameStarted", 0);

        for (int i = 0; i < rows.Count; i++)
        {
            for (int j = 0; j < rows[i].boxRefs.Count; j++)
            {
                PlayerPrefs.DeleteKey(rows[i].boxRefs[j].gameObject.name + ".box");
                PlayerPrefs.DeleteKey(rows[i].boxRefs[j].gameObject.name);
            }
        }

        PlayerPrefs.Save();

        if (gameWon)
        {
            gameWonPopup.SetActive(true);
            return;
        }

        gameLostPopup.SetActive(true);
    }

    public void Enter()
    {

        if (warningDisplayed) return;

        if (!RealWordCheck(keyboard.typedWord))
        {
            StartCoroutine(WarningText(wordNotRealWarning));
            return;
        }

        if (keyboard.typedWord.Length != 5)
        {
            StartCoroutine(WarningText(wordLengthWarning));
            return;
        }

        keyboard.activeRow.SubmitWord();
    }

    private IEnumerator WarningText(TMP_Text warningText)
    {
        warningDisplayed = true;
        warningText.enabled = true;

        yield return new WaitForSeconds(2f);

        warningText.enabled = false;
        warningDisplayed = false;
    }

    public void OpenPopup(GameObject popup)
    {
        popup.SetActive(true);
    }

    public void ClosePopup(GameObject popup)
    {
        popup.SetActive(false);
    }

    public string ChallengeWord
    {
        get
        {
            return challengeWord;
        }
    }
}