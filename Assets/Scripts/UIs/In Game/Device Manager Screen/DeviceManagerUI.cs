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
    [SerializeField] private DeviceManagerScreenUIRefs refs_player1;
    [SerializeField] private DeviceManagerScreenUIRefs refs_player2;

    [Header("References")]
    [SerializeField] private GameplayInitiator gameplayInitiator;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerInput.playerIndex == 0)
        {
            refs_player1.DeviceNotConnected.SetActive(false);
            refs_player1.DeviceConnected.SetActive(true);
            refs_player1.DeviceUsedText.text = "Device Used : " + playerInput.devices[0].name;

            gameplayInitiator.OnPlayerJoined(playerInput.gameObject, playerInput.playerIndex);
        }
        else if (playerInput.playerIndex == 1)
        {
            refs_player2.DeviceNotConnected.SetActive(false);
            refs_player2.DeviceConnected.SetActive(true);
            refs_player2.DeviceUsedText.text = "Device Used : " + playerInput.devices[0].name;

            gameplayInitiator.OnPlayerJoined(playerInput.gameObject, playerInput.playerIndex);
        }
    }
}