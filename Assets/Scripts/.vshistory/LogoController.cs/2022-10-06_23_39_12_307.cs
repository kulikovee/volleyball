using UnityEngine;

public class LogoController : MonoBehaviour
{
    private ActionsContoller actions;

    void Start()
    {
        actions = ActionsContoller.GetActions();
    }

    public void FinishAnimation()
    {
        actions.showStartupMenu();
        // startupMenu.SetVisible(true, true);
        gameObject.SetActive(false);
    }
}
