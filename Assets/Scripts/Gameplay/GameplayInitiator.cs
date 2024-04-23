using UnityEngine;
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
    [SerializeField] private PlayerData data_player1;
    [SerializeField] private PlayerData data_player2;

    public void OnPlayerJoined(GameObject playerObj, int playerIndex)  {
        PrincessRadar radar = playerObj.GetComponent<PrincessRadar>();
        SpriteRenderer spriteRenderer = playerObj.GetComponent<SpriteRenderer>();

        if (playerIndex == 0) {
            radar.InitUIs(data_player1.uiRefs);
            spriteRenderer.sprite = data_player1.sprite;
        } else if (playerIndex == 1) {
            radar.InitUIs(data_player2.uiRefs);
            spriteRenderer.sprite = data_player2.sprite;
        }
    }
}
