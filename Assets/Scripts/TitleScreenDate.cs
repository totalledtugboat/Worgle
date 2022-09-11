using UnityEngine;
using TMPro;
using System;

public class TitleScreenDate : MonoBehaviour
{
    private TMP_Text date;

    // Start is called before the first frame update
    void Start()
    {
        date = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        DateTime localDate = DateTime.Now;

        date.text = localDate.ToString("MM/dd/yyyy");
    }
}





//https://www.turboimagehost.com/album/151771/teenm?p=267
//https://www.turboimagehost.com/album/151771/teenm?p=252