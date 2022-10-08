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
    private TextMeshProUGUI menuPlayer0Text;
    private TextMeshProUGUI menuPlayer1Text;
    private TextMeshProUGUI menuPlayer2Text;
    private TextMeshProUGUI menuPlayer3Text;
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
        ball = GameObject.Find("Ball").GetComponent<BallController>();
        player0 = GameObject.Find("Player 0").GetComponent<PlayerController>();
        player1 = GameObject.Find("Player 1").GetComponent<PlayerController>();
        player2 = GameObject.Find("Player 2").GetComponent<PlayerController>();
        player3 = GameObject.Find("Player 3").GetComponent<PlayerController>();
        menuPlayer0Text = GameObject.Find("Menu Player 0").GetComponent<TextMeshProUGUI>();
        menuPlayer1Text = GameObject.Find("Menu Player 1").GetComponent<TextMeshProUGUI>();
        menuPlayer2Text = GameObject.Find("Menu Player 2").GetComponent<TextMeshProUGUI>();
        menuPlayer3Text = GameObject.Find("Menu Player 3").GetComponent<TextMeshProUGUI>();
        timerText = GameObject.Find("Menu Timer").GetComponent<TextMeshProUGUI>();
        score = GameObject.Find("Score").GetComponent<ScoreLabelController>();
        pause = GameObject.Find("Pause Menu").GetComponent<PauseMenuController>();
        animator = GetComponent<Animator>();
        ground1 = GameObject.Find("Ground 1").GetComponent<Renderer>();
        ground2 = GameObject.Find("Ground 2").GetComponent<Renderer>();
        mainTheme = GameObject.Find("Main Theme").GetComponent<AudioSource>();
        menuTheme = GameObject.Find("Menu Theme").GetComponent<AudioSource>();

        players = new List<PlayerController> { player0, player1, player2, player3 };

        player0.isLeft = true;
        player1.isLeft = false;
        player2.isLeft = true;
        player3.isLeft = false;

        Cursor.visible = false;
        stopGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (isVisible)
        {
            this.updateTimerText();
            this.checkUserSelection();

            if (isAllDevicesSelected())
            {
                timerFinished();
            }
        }
    }

    public void resetPlayerText()
    {
        menuPlayer0Text.text = "Player 1\nPress any button";
        menuPlayer1Text.text = "Player 2\nPress any button";
        menuPlayer2Text.text = "Player 3\nPress any button";
        menuPlayer3Text.text = "Player 4\nPress any button";
    }

    public void setVisible(bool visible, bool skipSounds = false)
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

    public IEnumerator resetGameDelayed()
    {
        yield return new WaitForSeconds(1f);
        resetGame();
    }

    void resetGame()
    {
        stopGame();
        setVisible(true);
    }

    public void playShowSound()
    {
        showSound.Play();
    }

    public void playHideSound()
    {
        hideSound.Play();
    }

    public void stopGame()
    {
        nextRound();
        ball.StopBall();
        players.ForEach(player => player.ResetPlayer());
        setPlayersFrozen(true);
        resetPlayerText();
    }

    public void nextRound()
    {
        ball.StopBall();
        ball.StartBall();
        setPlayersFrozen(true);
        StartCoroutine(this.unfreezePlayerAfterDelay());
        ground1.material.color = new Color32(255, 160, 129, 1);
        ground2.material.color = new Color32(107, 255, 130, 1);
    }
    public IEnumerator nextRoundDelayed()
    {
        yield return new WaitForSeconds(1f);
        nextRound();
    }

    IEnumerator unfreezePlayerAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        ball.isTouchedGround = false;
        if (!ball.GetComponent<Rigidbody>().isKinematic)
        {
            setPlayersFrozen(false);
        }
    }


    public void setPlayersFrozen(bool isFrozen)
    {
        getPlayers().ForEach((player) => {
            player.ResetPosition();
            player.SetFrozen(isFrozen);
        });
    }

    public List<PlayerController> getPlayers()
    {
        return players;
    }

    void startGame()
    {
        score.resetScore();
        ball.StartBall();
        setPlayersFrozen(false);
        setVisible(false);
    }

    int getGameStartTimer()
    {
        return (int)Mathf.Floor(startGameTimer - (Time.timeSinceLevelLoad - startTimerAt));
    }

    void updateTimerText()
    {
        var timeLeft = getGameStartTimer();
        timerText.text = "Start in " + timeLeft + "...";

        if (timeLeft <= 0)
        {
            timerText.text = "Waiting a player joined...";
            this.timerFinished();
        }
    }

    void timerFinished()
    {
        if (isOnePlayersJoined())
        {
            startGame();
        }
    }

    bool isDeviceSelected(int deviceId)
    {
        return player0.GetDevice().IsEquals(deviceId) || player1.GetDevice().IsEquals(deviceId) || player2.GetDevice().IsEquals(deviceId) || player3.GetDevice().IsEquals(deviceId);
    }

    bool isAllDevicesSelected()
    {
        return player0.HasDevice() && player1.HasDevice() && player2.HasDevice() && player3.HasDevice();
    }

    bool isOnePlayersJoined()
    {
        return player0.HasDevice() || player1.HasDevice() || player2.HasDevice() || player3.HasDevice();
    }

    void checkUserSelection()
    {
        var deviceId = Device.NO_DEVICE;

        KEYBOARD_WASD_KEYS.ForEach((keyCode) => {
            if (Input.GetKeyDown(keyCode))
            {
                deviceId = Device.KEYBOARD_WASD;
            }
        });

        if (Input.GetKeyDown(KeyCode.Escape) && isDeviceSelected(Device.KEYBOARD_WASD))
        {
            pause.updatePauseTimeout();
            timerFinished();
        }

        if (deviceId == Device.NO_DEVICE)
        {
            KEYBOARD_ARROWS_KEYS.ForEach((keyCode) => {
                if (Input.GetKeyDown(keyCode))
                {
                    deviceId = Device.KEYBOARD_ARROWS;
                }
            });
        }

        for (int gamePadId = 0; gamePadId < Gamepad.all.Count && deviceId == Device.NO_DEVICE; gamePadId++)
        {
            if (!isDeviceSelected(gamePadId))
            {
                var gamePad = Gamepad.all[gamePadId];
                if (
                    deviceId == Device.NO_DEVICE && (
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
                    timerFinished();
                }
            }
        }

        if (deviceId != Device.NO_DEVICE && !isDeviceSelected(deviceId) && !isAllDevicesSelected())
        {
            joinSound.Play();
            addSecondsToTimer();
            setPlayerDevice(deviceId);
        }
    }

    private void setPlayerDevice(int deviceId)
    {
        PlayerController player = null;
        TextMeshProUGUI playerText = null;
        int playerNumber = 0;

        if (player0.GetDevice().IsPrevious(deviceId))
        {
            player = player0;
            playerText = menuPlayer0Text;
            playerNumber = 1;
        }

        if (player1.GetDevice().IsPrevious(deviceId))
        {
            player = player1;
            playerText = menuPlayer1Text;
            playerNumber = 2;
        }

        if (player2.GetDevice().IsPrevious(deviceId))
        {
            player = player2;
            playerText = menuPlayer2Text;
            playerNumber = 3;
        }

        if (player3.GetDevice().IsPrevious(deviceId))
        {
            player = player3;
            playerText = menuPlayer3Text;
            playerNumber = 4;
        }

        if (player != null && !player.HasDevice())
        {
            player.GetDevice().SetDeviceId(deviceId);
            playerText.text = "Player " + playerNumber + "\n<b>Joined!</b>";
        } else
        {
            if (!player0.HasDevice())
            {
                player0.SetDeviceId(deviceId);
                menuPlayer0Text.text = "Player 1\n<b>Joined!</b>";
                playerPreviousDevice0 = deviceId;
                return;
            }

            if (!player1.HasDevice())
            {
                player1.SetDeviceId(deviceId);
                menuPlayer1Text.text = "Player 2\n<b>Joined!</b>";
                playerPreviousDevice1 = deviceId;
                return;
            }

            if (!player2.HasDevice())
            {
                player2.SetDeviceId(deviceId);
                menuPlayer2Text.text = "Player 3\n<b>Joined!</b>";
                playerPreviousDevice2 = deviceId;
                return;
            }

            if (!player3.HasDevice())
            {
                player3.SetDeviceId(deviceId);
                menuPlayer3Text.text = "Player 4\n<b>Joined!</b>";
                playerPreviousDevice3 = deviceId;
                return;
            }
        }
    }

    private void addSecondsToTimer()
    {
        if (getGameStartTimer() < 5f)
        {
            startTimerAt = Time.timeSinceLevelLoad - startGameTimer + 6;
        }
    }
}
