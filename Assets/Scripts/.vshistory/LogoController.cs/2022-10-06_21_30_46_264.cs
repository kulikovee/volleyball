using UnityEngine;

public class LogoController : MonoBehaviour
{
    private StartupMenuController startupMenu;

    void Start()
    {
        startupMenu = GameObject.Find("Startup Menu").GetComponent<StartupMenuController>();
    }

    public void Finish()
    {
        startupMenu.setVisible(true, true);
        gameObject.SetActive(false);
    }
}
