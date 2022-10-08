using UnityEngine;

public class LogoController : MonoBehaviour
{
    private StartupMenuController startupMenu;

    void Start()
    {
        startupMenu = GameObject.Find("Startup Menu").GetComponent<StartupMenuController>();
    }

    public void FinishAnimation()
    {
        startupMenu.SetVisible(true, true);
        gameObject.SetActive(false);
    }
}
