using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class StartupMenuController : MonoBehaviour
{
    // configurable params
    public List<PlayerController> players;

    // sounds
    public AudioSource joinSound;
    public AudioSource showSound;
    public AudioSource hideSound;

    // animation
    public bool isVisible = false;
    private Animator animator;

    // links
    private TextMeshProUGUI timerText;
    private BallController ball;
    private ScoreLabelController score;
    private PauseMenuController pause;
    private Renderer ground1;
    private Renderer ground2;
    private AudioSource mainTheme;
    private AudioSource menuTheme;
    private ActionsContoller actions;

    // timer
    private int startGameTimer = 16;
    private float startTimerAt;

    public static List<KeyCode> KEYBOARD_ARROWS_KEYS = new List<KeyCode>
    {
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.RightArrow,
        KeyCode.LeftArrow,
        KeyCode.Keypad4,
        KeyCode.Keypad5,
        KeyCode.Keypad6,
        KeyCode.Keypad8,
        KeyCode.Keypad0,
    };

    void Start()
    {
        ActionsContoller.OnShowStartupMenu += ShowStartupMenu;
        ActionsContoller.OnFirstShowStartupMenu += FirstShowStartupMenu;

        actions = ActionsContoller.GetActions();

        Cursor.visible = false;

        ball = GameObject.Find("Ball").GetComponent<BallController>();
        timerText = GameObject.Find("Menu Timer").GetComponent<TextMeshProUGUI>();
        score = GameObject.Find("Score").GetComponent<ScoreLabelController>();
        pause = GameObject.Find("Pause Menu").GetComponent<PauseMenuController>();
        animator = GetComponent<Animator>();
        ground1 = GameObject.Find("Ground 1").GetComponent<Renderer>();
        ground2 = GameObject.Find("Ground 2").GetComponent<Renderer>();
        mainTheme = GameObject.Find("Main Theme").GetComponent<AudioSource>();
        menuTheme = GameObject.Find("Menu Theme").GetComponent<AudioSource>();

        StartCoroutine(LateStart());
    }

    // Update is called once per frame
    void Update()
    {
        if (isVisible)
        {
            this.UpdateTimerText();
            this.CheckUserSelection();

            if (IsAllDevicesSelected())
            {
                TimerFinished();
            }
        }
    }

    private IEnumerator LateStart()
    {
        yield return new WaitForSeconds(1);
        StopGame();
    }

    private void FirstShowStartupMenu()
    {
        SetVisible(true, true);
    }

    private void ShowStartupMenu()
    {
        SetVisible(true);
    }

    public void ResetPlayerText()
    {
        actions.ResetPlayersText();
    }

    public void SetVisible(bool visible, bool skipSounds = false)
    {
        if (isVisible != visible)
        {
            isVisible = visible;

            if (visible)
            {
                startTimerAt = Time.timeSinceLevelLoad;
                animator.Play("Startup Menu Show");

                if (!skipSounds)
                {
                    mainTheme.Stop();
                    menuTheme.Play();
                }
            }
            else
            {
                menuTheme.Stop();
                mainTheme.Play();
                animator.Play("Startup Menu Hide");
            }

        }
    }

    public IEnumerator ResetGameDelayed()
    {
        yield return new WaitForSeconds(1f);
        resetGame();
    }

    void resetGame()
    {
        StopGame();
        SetVisible(true);
    }

    public void PlayShowSound()
    {
        showSound.Play();
    }

    public void PlayHideSound()
    {
        hideSound.Play();
    }

    public void StopGame()
    {
        NextRound();
        ball.FreezeBall();
        players.ForEach(player => player.ResetPlayer());
        SetPlayersFrozen(true);
        ResetPlayerText();
    }

    public void NextRound()
    {
        ball.FreezeBall();
        ball.UnfreezeBall();
        SetPlayersFrozen(true);
        StartCoroutine(this.UnfreezePlayerAfterDelay());
        ground1.material.color = new Color32(255, 160, 129, 1);
        ground2.material.color = new Color32(107, 255, 130, 1);
    }

    public IEnumerator NextRoundDelayed()
    {
        yield return new WaitForSeconds(1f);
        NextRound();
    }

    IEnumerator UnfreezePlayerAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        ball.UnfreezeBall();
        if (!ball.GetComponent<Rigidbody>().isKinematic)
        {
            SetPlayersFrozen(false);
        }
    }


    public void SetPlayersFrozen(bool isFrozen)
    {
        GetPlayers().ForEach((player) => {
            player.ResetPosition();
            player.SetFrozen(isFrozen);
        });
    }

    public List<PlayerController> GetPlayers()
    {
        return players;
    }

    void StartGame()
    {
        score.resetScore();
        ball.UnfreezeBall();
        SetPlayersFrozen(false);
        SetVisible(false);
    }

    int GetGameStartTimer()
    {
        return (int)Mathf.Floor(startGameTimer - (Time.timeSinceLevelLoad - startTimerAt));
    }

    void UpdateTimerText()
    {
        var timeLeft = GetGameStartTimer();
        timerText.text = "Start in " + timeLeft + "...";

        if (timeLeft <= 0)
        {
            timerText.text = "Waiting a player joined...";
            this.TimerFinished();
        }
    }

    void TimerFinished()
    {
        if (IsOnePlayersJoined())
        {
            StartGame();
        }
    }

    bool IsDeviceSelected(int deviceId)
    {
        var foundIndex = GetPlayers().FindIndex(_player => _player.GetDevice().IsEquals(deviceId));
        return foundIndex > -1;
    }

    bool IsAllDevicesSelected()
    {
        var players = GetPlayers();
        var selectedPlayers = players.FindAll(_player => _player.GetDevice().IsSelected());
        return selectedPlayers.Count == players.Count;
    }

    bool IsOnePlayersJoined()
    {
        var players = GetPlayers();
        var foundIndex = players.FindIndex(_player => _player.GetDevice().IsSelected());
        return foundIndex > -1;
    }

    void CheckUserSelection()
    {
        var deviceId = DeviceController.NO_DEVICE;

        if (InputWASDController.IsPressed())
        {
            deviceId = DeviceController.KEYBOARD_WASD;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && IsDeviceSelected(DeviceController.KEYBOARD_WASD))
        {
            pause.updatePauseTimeout();
            TimerFinished();
        }

        if (deviceId == DeviceController.NO_DEVICE)
        {
            
        }

        for (int gamePadId = 0; gamePadId < Gamepad.all.Count && deviceId == DeviceController.NO_DEVICE; gamePadId++)
        {
            if (!IsDeviceSelected(gamePadId))
            {
                var gamePad = Gamepad.all[gamePadId];
                if (
                    deviceId == DeviceController.NO_DEVICE && (
                        gamePad.aButton.isPressed 
                        || gamePad.bButton.isPressed 
                        || gamePad.yButton.isPressed 
                        || gamePad.xButton.isPressed
                    )
                )
                {
                    deviceId = gamePadId;
                }
            } else {
                if (Gamepad.all[gamePadId].startButton.isPressed)
                {
                    pause.updatePauseTimeout();
                    TimerFinished();
                }
            }
        }

        if (deviceId != DeviceController.NO_DEVICE && !IsDeviceSelected(deviceId) && !IsAllDevicesSelected())
        {
            joinSound.Play();
            AddSecondsToTimer();
            SetNextPlayerDevice(deviceId);
        }
    }

    private void SetNextPlayerDevice(int deviceId)
    {
        PlayerController player = null;
        var players = GetPlayers();

        foreach (var _player in players)
        {
            if (_player.GetDevice().IsPrevious(deviceId) && !_player.GetDevice().IsSelected())
            {
                player = _player;
                break;
            }
        }

        if (player != null)
        {
            player.GetDevice().SetId(deviceId);
            actions.JoinedPlayersText();
        } else
        {
            foreach (var _player in players)
            {
                if (!_player.GetDevice().IsSelected())
                {
                    _player.GetDevice().SetId(deviceId);
                    actions.JoinedPlayersText();
                    break;
                }
            }
        }
    }

    private void AddSecondsToTimer(float seconds = 5f)
    {
        if (GetGameStartTimer() < seconds)
        {
            startTimerAt = Time.timeSinceLevelLoad - startGameTimer + seconds + 1;
        }
    }
}
