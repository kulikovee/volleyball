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
        actions.ShowStartupMenu();
        // startupMenu.SetVisible(true, true);
        gameObject.SetActive(false);
    }
}
