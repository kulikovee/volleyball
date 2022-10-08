using TMPro;
using UnityEngine;

public class TimerTextController : MonoBehaviour
{
    private TextMeshProUGUI text;

    void Start()
    {
        ActionsContoller.OnTimerUpdate += UpdateTimer;
        text = GetComponent<TextMeshProUGUI>();
    }

    void UpdateTimer(int seconds)
    {
        timerText.text = "Start in " + timeLeft + "...";

        if (timeLeft <= 0)
        {
            timerText.text = "Waiting a player joined...";
            this.TimerFinished();
        }
    }
}
