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
}

public class DeviceManagerUI : MonoBehaviour
{
    [SerializeField] private DeviceManagerScreenUIRefs refsPlayer1;
    [SerializeField] private DeviceManagerScreenUIRefs refsPlayer2;

    [Header("References")]
    [SerializeField] private GameplayInitiator gameplayInitiator;

    private bool listenForReadyPlayer1 = false;
    private bool listenForReadyPlayer2 = false;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerInput.playerIndex == 0)
        {
            refsPlayer1.DeviceNotConnected.SetActive(false);
            refsPlayer1.DeviceConnected.SetActive(true);
            refsPlayer1.DeviceUsedText.text = "Device Used : " + playerInput.devices[0].name;

            gameplayInitiator.OnPlayerJoined(playerInput.gameObject, playerInput.playerIndex);
        }
        else if (playerInput.playerIndex == 1)
        {
            refsPlayer2.DeviceNotConnected.SetActive(false);
            refsPlayer2.DeviceConnected.SetActive(true);
            refsPlayer2.DeviceUsedText.text = "Device Used : " + playerInput.devices[0].name;

            gameplayInitiator.OnPlayerJoined(playerInput.gameObject, playerInput.playerIndex);
        }
    }

    private void Update() {
        
    }
}