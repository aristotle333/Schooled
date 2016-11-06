using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Timer : MonoBehaviour
{
    public static Timer S;
    public float timeInSeconds;
    public GameObject ui;

    // Do not change this in the Inspector
    public float timeRemaining;

    private Text timerText;
    private Image timerBar;

    private float endTime;

    void Awake()
    {
        S = this;
        endTime = Time.time + timeInSeconds;
        FindPieces();
    }

    void Update()
    {
        timeRemaining = endTime - Time.time;

        if (timeRemaining < 0) {
            timeRemaining = 0;
            Time.timeScale = 0;
            UI.S.DisplayPrompt(false);
        }

        SetTimerText(timeRemaining);
        SetBar(timeRemaining);
    }

    private void FindPieces()
    {
        Transform timerParent = ui.transform.Find("Timer");
        this.timerText = timerParent.Find("Text").gameObject.GetComponent<Text>();
        this.timerBar = timerParent.Find("Image").gameObject.GetComponent<Image>();
    }

    private void SetTimerText(float timeRemaining)
    {
        int milliseconds = Mathf.FloorToInt(timeRemaining * 100) % 100;
        int seconds = Mathf.FloorToInt(timeRemaining) % 60;
        int minutes = Mathf.FloorToInt(timeRemaining) / 60;

        timerText.text = String.Format("{0}:{1:d2}.{2:d2}", minutes, seconds, milliseconds);
    }

    private void SetBar(float timeRemaining)
    {
        float ratio = timeRemaining / timeInSeconds;
        timerBar.transform.localScale = new Vector3(ratio, 1, 1);
    }

}
