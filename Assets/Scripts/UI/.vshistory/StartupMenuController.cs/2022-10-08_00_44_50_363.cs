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
    private AudioSource mainTheme;
    private AudioSource menuTheme;
    private ActionsContoller actions;

    // timer
    private int startGameTimer = 16;
    private float startTimerAt;

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
        mainTheme = GameObject.Find("Main Theme").GetComponent<AudioSource>();
        menuTheme = GameObject.Find("Menu Theme").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isVisible)
        {
            this.UpdateTimerText();
            this.CheckUserSelection();
            this.CheckSkipJoinTimeout();

            if (IsAllDevicesSelected())
            {
                TimerFinished();
            }
        }
    }

    private void CheckSkipJoinTimeout()
    {
        var skipDeviceIds = new List<int>();
        if (InputWASDController.IsSkip() && IsDeviceSelected(DeviceController.KEYBOARD_WASD))
        {
            skipDeviceIds.Add(DeviceController.KEYBOARD_WASD);
        }

        InputGamepadController.GetSkipGamepadIds().ForEach((gamePadId) =>
        {
            if (IsDeviceSelected(gamePadId))
            {
                skipDeviceIds.Add(DeviceController.KEYBOARD_WASD);
            }
        });

        if (skipDeviceIds.Count > 0)
        {
            pause.UpdatePauseTimeout();
            TimerFinished();
        }
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

    public IEnumerator WinGameDelayed()
    {
        yield return new WaitForSeconds(1f);
        winGame();
    }

    void winGame()
    {
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

    public void SetPlayersFrozen(bool isFrozen)
    {
        GetPlayers().ForEach((player) => {
            player.ResetPosition();
            player.SetFrozen(isFrozen);
        });
    }
    public IEnumerator NextRoundDelayed()
    {
        yield return new WaitForSeconds(1f);
        NextRound();
    }


    public List<PlayerController> GetPlayers()
    {
        return players;
    }

    void StartGame()
    {
        score.resetScore();
        NextRound();
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
        } else if (InputNumpadController.IsPressed())
        {
            deviceId = DeviceController.KEYBOARD_NUMPAD;
        } else
        {
            var pressedGamepadIds = InputGamepadController.GetPressedIds();

            if (pressedGamepadIds.Count > 0)
            {
                pressedGamepadIds.ForEach((gamePadId) => {
                    if (!IsDeviceSelected(gamePadId))
                    {
                        deviceId = gamePadId;
                    }
                });
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
