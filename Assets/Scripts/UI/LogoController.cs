using UnityEngine;

public class LogoController : MonoBehaviour
{
    private ActionsContoller actions;

    void Start()
    {
        actions = ActionsContoller.GetActions();
        Time.timeScale = 0;
    }

    public void FinishAnimation()
    {
        actions.FirstShowStartupMenu();
        gameObject.SetActive(false);
    }
}
