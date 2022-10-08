using UnityEngine;

public class State : MonoBehaviour
{
    public delegate void OnGameReset();
    public static event OnGameReset onGameReset;

}
