using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Row : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private Keyboard keyboard;
    [SerializeField] public TMP_Text inputField;
    public List<Image> boxRefs;

    [HideInInspector] public bool active;

    private string[] guess;

    private void Awake()
    {
        active = false;

        int thisIndex = keyboard.rows.IndexOf(this);
        int activeRow = PlayerPrefs.GetInt("activerow");

        if (thisIndex == activeRow)
            active = true;

        for (int i = 0; i < boxRefs.Count; i++)
        {
            string boxName = PlayerPrefs.GetString(boxRefs[i].gameObject.name + ".box");
            int boxIndex = PlayerPrefs.GetInt(boxRefs[i].gameObject.name);

            if (boxName == boxRefs[i].gameObject.name)
            {
                switch (boxIndex)
                {
                    case 0:
                        boxRefs[i].color = gm.usedLetter;
                        break;

                    case 1:
                        boxRefs[i].color = gm.inWordWrongPlace;
                        break;

                    case 2:
                        boxRefs[i].color = gm.inWordRightPlace;
                        break;
                }
            }
        }
    }

    private void Start()
    {
        string keyColorCheck = inputField.text;

        for (int i = 0; i < keyColorCheck.Length; i++)
        {
            if (keyColorCheck[i] == gm.ChallengeWord[i])
                StoredKeyColorValues(keyColorCheck[i].ToString(), gm.inWordRightPlace);
            else if (gm.ChallengeWord.Contains(keyColorCheck[i].ToString()))
                StoredKeyColorValues(keyColorCheck[i].ToString(), gm.inWordWrongPlace);
            else if (gm.allGuessedLetters.Contains(keyColorCheck[i].ToString()))
                StoredKeyColorValues(keyColorCheck[i].ToString(), gm.usedLetter);
        }
    }

    private void Update()
    {
        if (!active)
            return;

        guess = new string[keyboard.guessLength];

        for (int i = 0; i < keyboard.guessLength; i++)
        {
            guess[i] = keyboard.typedWord[i].ToString();
        }

        inputField.text = keyboard.typedWord;
    }

    public void SubmitWord()
    {
        SubmitWordAnim();

        if (keyboard.typedWord == gm.ChallengeWord)
        {
            gm.EndGame(true);
            return;
        }

        int currentRow = gm.rows.IndexOf(this);
        int nextRow = currentRow + 1;

        NextRow(currentRow, nextRow);
    }

    private void NextRow(int currentRow, int nextRow)
    {
        gm.rows[currentRow].active = false;

        if (currentRow == gm.rows.Count - 1)
        {
            gm.EndGame(false);
            return;
        }

        gm.rows[nextRow].active = true;

        string clearWord = keyboard.typedWord;
        clearWord = clearWord.Substring(0, clearWord.Length - 5);

        keyboard.typedWord = clearWord;
        keyboard.guessLength -= 5;
    }

    private void SubmitWordAnim()
    {
        for (int i = 0; i < guess.Length; i++)
        {
            string letter = guess[i].ToString();

            if (!gm.ChallengeWord.Contains(letter))
            {
                StartCoroutine(TextBoxAnim(boxRefs[i], gm.usedLetter, i));
                KeyColorChange(gm.usedLetter, i);
                StoreBoxColorValues(boxRefs[i].gameObject.name, 0);
            } 
            else if (letter != gm.wordOfTheDayChars[i].ToString())
            {
                StartCoroutine(TextBoxAnim(boxRefs[i], gm.inWordWrongPlace, i));
                KeyColorChange(gm.inWordWrongPlace, i);
                StoreBoxColorValues(boxRefs[i].gameObject.name, 1);
            }
            else
            {
                StartCoroutine(TextBoxAnim(boxRefs[i], gm.inWordRightPlace, i));
                KeyColorChange(gm.inWordRightPlace, i);
                StoreBoxColorValues(boxRefs[i].gameObject.name, 2);
            }
        }
    }

    private IEnumerator TextBoxAnim(Image image, Color color, int time)
    {
        float animTimer = (time + 1f) * 2f;

        animTimer /= 10f;

        yield return new WaitForSeconds(animTimer);

        LeanTween.color(image.rectTransform, color, 0.2f);
        LeanTween.scale(image.gameObject, image.rectTransform.localScale * 1.1f, 0.1f);
        LeanTween.scale(image.gameObject, new Vector2(image.rectTransform.localScale.x, image.rectTransform.localScale.y), 0.2f).setDelay(0.1f);
    }

    private void KeyColorChange(Color color, int guessIndex)
    {
        for (int i = 0; i < keyboard.Keys.Count; i++)
        {
            if (guess[guessIndex].ToString() == keyboard.Keys[i].name)
            {
                LeanTween.color(keyboard.Keys[i].rectTransform, color, 0.2f);
            }
        }
    }

    private void StoreBoxColorValues(string boxName, int colorIndex)
    {
        PlayerPrefs.SetString(boxName + ".box", boxName);
        PlayerPrefs.SetInt(boxName, colorIndex);
        PlayerPrefs.Save();
    }

    private void StoredKeyColorValues(string letter, Color color)
    {
        for (int i = 0; i < keyboard.Keys.Count; i++)
        {
            if (letter == keyboard.Keys[i].name)
            {
                keyboard.Keys[i].color = color;
            }
        }
    }
}