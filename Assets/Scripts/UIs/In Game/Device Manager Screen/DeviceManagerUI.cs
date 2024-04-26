using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
class DeviceManagerScreenUIRefs
{       
    public GameObject DeviceNotConnected;
    public GameObject DeviceConnected;
    public GameObject PlayerStatusReady;
    public GameObject PlayerStatusNotReady;
    public TextMeshProUGUI DeviceUsedText;
    public TextMeshProUGUI KeyToReadyText;
    public TextMeshProUGUI KeyToUnreadyText;
}

public class DeviceManagerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameStartText;
    [SerializeField] private DeviceManagerScreenUIRefs refsPlayer1;
    [SerializeField] private DeviceManagerScreenUIRefs refsPlayer2;

    [Header("References")]
    [SerializeField] private GameplayInitiator gameplayInitiator;

    private bool listenForInputPlayer1 = false;
    private bool player1Ready = false;
    private bool listenForInputPlayer2 = false;
    private bool player2Ready = false;

    private PlayerInput playerInput1;
    private PlayerInput playerInput2;

    private Coroutine startGameCoroutine;
    private bool gameStarting = false;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerInput.playerIndex == 0)
        {
            refsPlayer1.DeviceNotConnected.SetActive(false);
            refsPlayer1.DeviceConnected.SetActive(true);
            refsPlayer1.DeviceUsedText.text = "Device Used : " + playerInput.currentControlScheme;

            gameplayInitiator.OnPlayerJoined(playerInput.gameObject, playerInput.playerIndex);
            listenForInputPlayer1 = true;

            playerInput1 = playerInput;
            playerInput1.SwitchCurrentActionMap("PreGame");

            UpdateKeyInstructionText(playerInput1, 0);
        }
        else if (playerInput.playerIndex == 1)
        {
            refsPlayer2.DeviceNotConnected.SetActive(false);
            refsPlayer2.DeviceConnected.SetActive(true);
            refsPlayer2.DeviceUsedText.text = "Device Used : " + playerInput.currentControlScheme;

            gameplayInitiator.OnPlayerJoined(playerInput.gameObject, playerInput.playerIndex);
            listenForInputPlayer2 = true;

            playerInput2 = playerInput;
            playerInput2.SwitchCurrentActionMap("PreGame");

            UpdateKeyInstructionText(playerInput2, 1);
        }
    }

    private void DiscardGameStart()
    {
        if (startGameCoroutine != null)
        {
            StopCoroutine(startGameCoroutine);
            startGameCoroutine = null;
            gameStartText.gameObject.SetActive(false);
        }
        gameStarting = false;
    }
    
    private void UpdateKeyInstructionText(PlayerInput input, int playerIndex) {
        string scheme = input.currentControlScheme;
        if (scheme == "Keyboard+Mouse")
        {
            if (playerIndex == 0)
            {
                refsPlayer1.KeyToReadyText.text = "Press E to Ready";
                refsPlayer1.KeyToUnreadyText.text = "Press Q to Unready";
            }
            else if (playerIndex == 1)
            {
                refsPlayer2.KeyToReadyText.text = "Press E to Ready";
                refsPlayer2.KeyToUnreadyText.text = "Press Q to Unready";
            }
        }
        else if (scheme == "Gamepad")
        {
            if (playerIndex == 0)
            {
                refsPlayer1.KeyToReadyText.text = "Press (X) to Ready";
                refsPlayer1.KeyToUnreadyText.text = "Press (B) to Unready";
            }
            else if (playerIndex == 1)
            {
                refsPlayer2.KeyToReadyText.text = "Press (X) to Ready";
                refsPlayer2.KeyToUnreadyText.text = "Press (B) to Unready";
            }
        }
    }

    private void Update() {
        if (listenForInputPlayer1 && playerInput1.currentActionMap.name == "PreGame")
        {
            if (playerInput1.actions["Ready"].triggered)
            {
                refsPlayer1.PlayerStatusReady.SetActive(true);
                refsPlayer1.PlayerStatusNotReady.SetActive(false);
                player1Ready = true;
            } else if (playerInput1.actions["Unready"].triggered)
            {
                refsPlayer1.PlayerStatusReady.SetActive(false);
                refsPlayer1.PlayerStatusNotReady.SetActive(true);
                player1Ready = false;

                DiscardGameStart();
            }
        }

        if (listenForInputPlayer2 && playerInput2.currentActionMap.name == "PreGame")
        {
            if (playerInput2.actions["Ready"].triggered)
            {
                refsPlayer2.PlayerStatusReady.SetActive(true);
                refsPlayer2.PlayerStatusNotReady.SetActive(false);
                player2Ready = true;
            } else if (playerInput2.actions["Unready"].triggered)
            {
                refsPlayer2.PlayerStatusReady.SetActive(false);
                refsPlayer2.PlayerStatusNotReady.SetActive(true);
                player2Ready = false;

                DiscardGameStart();
            }
        }

        if (player1Ready && player2Ready && !gameStarting)
        {
            gameStarting = true;
            startGameCoroutine = StartCoroutine(StartGame());
        }
    }

    IEnumerator StartGame()
    {
        // gameStartText.text = "Game starts in 3";
        // gameStartText.gameObject.SetActive(true);
        // yield return new WaitForSeconds(1);
        // gameStartText.text = "Game starts in 2";
        // yield return new WaitForSeconds(1);
        // gameStartText.text = "Game starts in 1";
        // yield return new WaitForSeconds(1);
        // gameStartText.gameObject.SetActive(false);
        
        // For faster testing
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
        gameplayInitiator.StartGame(playerInput1, playerInput2);
    }
}