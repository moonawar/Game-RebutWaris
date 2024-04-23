using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[System.Serializable]
class DeviceManagerScreenUIRefs
{       
    public GameObject DeviceNotConnected;
    public GameObject DeviceConnected;
    public GameObject PlayerStatusReady;
    public GameObject PlayerStatusNotReady;
    public TextMeshProUGUI DeviceUsedText;
}

public class DeviceManagerUI : MonoBehaviour
{
    // [SerializeField] private GameObject deviceManagerUI;
    [SerializeField] private DeviceManagerScreenUIRefs refsPlayer1;
    [SerializeField] private DeviceManagerScreenUIRefs refsPlayer2;

    [Header("References")]
    [SerializeField] private GameplayInitiator gameplayInitiator;

    private bool listenForReadyPlayer1 = false;
    private bool player1Ready = false;
    private bool listenForReadyPlayer2 = false;
    private bool player2Ready = false;

    private PlayerInput playerInput1;
    private PlayerInput playerInput2;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerInput.playerIndex == 0)
        {
            refsPlayer1.DeviceNotConnected.SetActive(false);
            refsPlayer1.DeviceConnected.SetActive(true);
            refsPlayer1.DeviceUsedText.text = "Device Used : " + playerInput.devices[0].name;

            gameplayInitiator.OnPlayerJoined(playerInput.gameObject, playerInput.playerIndex);
            listenForReadyPlayer1 = true;

            playerInput1 = playerInput;
            playerInput1.SwitchCurrentActionMap("PreGame");
        }
        else if (playerInput.playerIndex == 1)
        {
            refsPlayer2.DeviceNotConnected.SetActive(false);
            refsPlayer2.DeviceConnected.SetActive(true);
            refsPlayer2.DeviceUsedText.text = "Device Used : " + playerInput.devices[0].name;

            gameplayInitiator.OnPlayerJoined(playerInput.gameObject, playerInput.playerIndex);
            listenForReadyPlayer2 = true;

            playerInput2 = playerInput;
            playerInput2.SwitchCurrentActionMap("PreGame");
        }
    }

    private void Update() {
        if (listenForReadyPlayer1 && playerInput1.currentActionMap.name == "PreGame" && playerInput1.actions["Ready"].triggered)
        {
            refsPlayer1.PlayerStatusReady.SetActive(true);
            refsPlayer1.PlayerStatusNotReady.SetActive(false);
            listenForReadyPlayer1 = false;
            player1Ready = true;
        }

        if (listenForReadyPlayer2 && playerInput2.currentActionMap.name == "PreGame" && playerInput2.actions["Ready"].triggered)
        {
            refsPlayer2.PlayerStatusReady.SetActive(true);
            refsPlayer2.PlayerStatusNotReady.SetActive(false);
            listenForReadyPlayer2 = false;
            player2Ready = true;
        }

        if (player1Ready && player2Ready)
        {
            gameObject.SetActive(false);
            gameplayInitiator.StartGame(playerInput1, playerInput2);
        }
    }
}