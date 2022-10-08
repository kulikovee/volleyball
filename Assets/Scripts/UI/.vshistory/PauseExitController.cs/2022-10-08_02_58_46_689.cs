using System;
using TMPro;
using UnityEngine;

public class PauseExitController : MonoBehaviour
{
    public static readonly int optionId = 1;
    public TextMeshProUGUI text;

    void Start()
    {
        ActionsContoller.OnSelectPauseOption += UpdateSelection;
    }

    void UpdateSelection(int option)
    {
        text.text = option == optionId ? "<b>Exit Game</b>" : "Exit Game";
        text.fontStyle = option == optionId ? FontStyles.Underline : FontStyles.Normal;
    }
}
