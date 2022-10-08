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
        actions.FirstShowStartupMenu();
        // startupMenu.SetVisible(true, true);
        gameObject.SetActive(false);
    }
}
