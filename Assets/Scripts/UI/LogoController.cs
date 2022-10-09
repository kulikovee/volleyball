using UnityEngine;

public class LogoController : MonoBehaviour
{
    private ActionsContoller actions;
    private Animator animator;

    void Start()
    {
        ActionsContoller.OnStartGame += Hide;

        actions = ActionsContoller.GetActions();
        animator = GetComponent<Animator>();
        Time.timeScale = 0;
    }

    public void Hide()
    {
        animator.Play("Logo Hide");
    }

    /** Called from animation: Logo Idle **/
    public void ShowStartupMenu()
    {
        actions.FirstShowStartupMenu();
    }

    /** Called from animation: Logo Hide **/
    public void Hidden()
    {
        gameObject.SetActive(false);
    }
}
