using System;
using TMPro;
using UnityEngine;

public class PauseResumeController : MonoBehaviour
{
    public static readonly int optionId = 0;
    public TextMeshProUGUI text;

    void Start()
    {
        ActionsContoller.OnSelectPauseOption += UpdateSelection;
    }

    void UpdateSelection(int option)
    {
        if (!isDisabled)
        {
            Navigate();
        }
    }
}
