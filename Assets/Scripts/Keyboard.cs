using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    [HideInInspector] public string typedWord;
    [HideInInspector] public int guessLength;

    public Row activeRow;
    public List<Row> rows;

    [SerializeField] private List<Image> keys;

    public void Update()
    {
        for (int i = 0; i < rows.Count; i++)
        {
            if (rows[i].active)
                activeRow = rows[i];
        }
    }

    public void KeyPress(string key)
    {
        if (guessLength >= 5) return;

        guessLength++;
        typedWord += key;
    }

    public void Delete()
    {
        if (guessLength == 0) return;

        string deletedChar = typedWord;
        deletedChar = deletedChar.Substring(0, deletedChar.Length - 1);

        typedWord = deletedChar;
        guessLength--;
    }

    public List<Image> Keys
    {
        get
        {
            return keys;
        }
    }
}