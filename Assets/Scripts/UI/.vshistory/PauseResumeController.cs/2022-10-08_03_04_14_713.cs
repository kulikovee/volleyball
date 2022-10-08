using System;
using TMPro;
using UnityEngine;

public class PauseResumeController : MonoBehaviour
{
    public const int optionId = 0;

    public TextMeshProUGUI text;

    void Start()
    {
        ActionsContoller.OnSelectPauseOption += UpdateSelection;
    }

    void UpdateSelection(int option)
    {
        text.text = option == optionId ? "<b>Resume</b>" : "Resume";
        text.fontStyle = option == optionId ? FontStyles.Underline : FontStyles.Normal;
    }
}
