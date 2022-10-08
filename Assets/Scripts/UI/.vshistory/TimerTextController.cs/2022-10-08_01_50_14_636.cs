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
        text.text = "Start in " + seconds + "...";

        if (seconds <= 0)
        {
            text.text = "Waiting a player joined..."
        }
    }
}
