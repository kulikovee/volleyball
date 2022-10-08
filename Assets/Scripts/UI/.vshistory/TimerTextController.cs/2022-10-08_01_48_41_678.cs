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

    // Update is called once per frame
    void UpdateTimer(int seconds)
    {
        
    }
}
