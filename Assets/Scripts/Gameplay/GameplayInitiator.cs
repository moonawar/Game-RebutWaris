using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public class InGamePlayerUIRefs {
    public GameObject fill;
    public Slider loveMeter;
    public GameObject heart;
}

[System.Serializable]
public class PlayerData {
    public Sprite sprite;
    public InGamePlayerUIRefs uiRefs;
}

public class GameplayInitiator : MonoBehaviour
{
    [SerializeField] private PlayerData dataPlayer1;
    [SerializeField] private PlayerData dataPlayer2;

    [SerializeField] private GameObject gameplayUI;

    public void OnPlayerJoined(GameObject playerObj, int playerIndex)  {
        EmakRadar radar = playerObj.GetComponent<EmakRadar>();
        SpriteRenderer spriteRenderer = playerObj.GetComponent<SpriteRenderer>();

        if (playerIndex == 0) {
            radar.InitUIs(dataPlayer1.uiRefs);
            spriteRenderer.sprite = dataPlayer1.sprite;
        } else if (playerIndex == 1) {
            radar.InitUIs(dataPlayer2.uiRefs);
            spriteRenderer.sprite = dataPlayer2.sprite;
        }
    }

    public void StartGame(PlayerInput p1, PlayerInput p2) {
        p1.SwitchCurrentActionMap("Gameplay");
        p2.SwitchCurrentActionMap("Gameplay");

        gameplayUI.SetActive(true);
    }
}
