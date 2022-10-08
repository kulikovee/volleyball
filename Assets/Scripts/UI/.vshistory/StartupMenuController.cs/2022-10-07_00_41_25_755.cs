using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class StartupMenuController : MonoBehaviour
{

    // sounds
    public AudioSource joinSound;
    public AudioSource showSound;
    public AudioSource hideSound;

    // animation
    public bool isVisible = false;
    private Animator animator;

    // links
    private TextMeshProUGUI timerText;
    private PlayerController player0;
    private PlayerController player1;
    private PlayerController player2;
    private PlayerController player3;
    private BallController ball;
    private ScoreLabelController score;
    private PauseMenuController pause;
    private List<PlayerController> players;
    private Renderer ground1;
    private Renderer ground2;
    private AudioSource mainTheme;
    private AudioSource menuTheme;
    private ActionsContoller actions;

    // timer
    private int startGameTimer = 16;
    private float startTimerAt;

    private static List<KeyCode> KEYBOARD_WASD_KEYS = new List<KeyCode>
    {
        KeyCode.A,
        KeyCode.D,
        KeyCode.W,
        KeyCode.S,
        KeyCode.Space,
    };

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

        ball = GameObject.Find("Ball").GetComponent<BallController>();
        player0 = GameObject.Find("Player 0").GetComponent<PlayerController>();
        player1 = GameObject.Find("Player 1").GetComponent<PlayerController>();
        player2 = GameObject.Find("Player 2").GetComponent<PlayerController>();
        player3 = GameObject.Find("Player 3").GetComponent<PlayerController>();
        timerText = GameObject.Find("Menu Timer").GetComponent<TextMeshProUGUI>();
        score = GameObject.Find("Score").GetComponent<ScoreLabelController>();
        pause = GameObject.Find("Pause Menu").GetComponent<PauseMenuController>();
        animator = GetComponent<Animator>();
        ground1 = GameObject.Find("Ground 1").GetComponent<Renderer>();
        ground2 = GameObject.Find("Ground 2").GetComponent<Renderer>();
        mainTheme = GameObject.Find("Main Theme").GetComponent<AudioSource>();
        menuTheme = GameObject.Find("Menu Theme").GetComponent<AudioSource>();

        players = new List<PlayerController> { player0, player1, player2, player3 };

        Cursor.visible = false;
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
        
        return -1 < GetPlayers().FindIndex(
            _player => _player.GetDevice().IsEquals(deviceId)
        );
    }

    bool IsAllDevicesSelected()
    {
        return 
            player0.GetDevice().IsSelected() 
            && player1.GetDevice().IsSelected() 
            && player2.GetDevice().IsSelected() 
            && player3.GetDevice().IsSelected();
    }

    bool IsOnePlayersJoined()
    {
        return 
            player0.GetDevice().IsSelected() 
            || player1.GetDevice().IsSelected() 
            || player2.GetDevice().IsSelected() 
            || player3.GetDevice().IsSelected();
    }

    void CheckUserSelection()
    {
        var deviceId = DeviceController.NO_DEVICE;

        KEYBOARD_WASD_KEYS.ForEach((keyCode) => {
            if (Input.GetKeyDown(keyCode))
            {
                deviceId = DeviceController.KEYBOARD_WASD;
            }
        });

        if (Input.GetKeyDown(KeyCode.Escape) && IsDeviceSelected(DeviceController.KEYBOARD_WASD))
        {
            pause.updatePauseTimeout();
            TimerFinished();
        }

        if (deviceId == DeviceController.NO_DEVICE)
        {
            KEYBOARD_ARROWS_KEYS.ForEach((keyCode) => {
                if (Input.GetKeyDown(keyCode))
                {
                    deviceId = DeviceController.KEYBOARD_NUMPAD;
                }
            });
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
