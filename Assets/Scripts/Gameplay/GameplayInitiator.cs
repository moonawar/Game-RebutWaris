using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public class PlayerData {
    public Sprite sprite;
    public Sprite arrow;
    public GameObject itemHolder;
    public PlayerMashUIRefs uiRefs;
}

public class GameplayInitiator : MonoBehaviour
{
    [SerializeField] private PlayerData dataPlayer1;
    [SerializeField] private PlayerData dataPlayer2;


    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private Collider2D Arena;
    [SerializeField] private Camera cam;
    [SerializeField] private Throwable throwablePrefab;
    [SerializeField] private CinemachineTargetGroup targetGroup;

    public void OnPlayerJoined(GameObject playerObj, int playerIndex)  {
        PlayerMash radar = playerObj.GetComponent<PlayerMash>();
        SpriteRenderer spriteRenderer = playerObj.GetComponent<SpriteRenderer>();
        targetGroup.AddMember(playerObj.transform, 1, 0);

        if (playerIndex == 0) {
            radar.InitUIs(dataPlayer1.uiRefs);
            spriteRenderer.sprite = dataPlayer1.sprite;
            playerObj.GetComponent<PlayerMovement>().SetArena(Arena);
            playerObj.GetComponent<PlayerMovement>().SetArrow(dataPlayer1.arrow);
            playerObj.GetComponent<PlayerMovement>().SetCamera(cam);
            playerObj.GetComponent<PlayerRangeItem>().SetThrowablePrefab(throwablePrefab);
            playerObj.GetComponent<PlayerRangeItem>().SetItemHolderUI(dataPlayer1.itemHolder);
            GameplayManager.Instance.Players.Add(playerObj);

        } else if (playerIndex == 1) {
            radar.InitUIs(dataPlayer2.uiRefs);
            spriteRenderer.sprite = dataPlayer2.sprite;
            playerObj.GetComponent<PlayerMovement>().SetArena(Arena);
            playerObj.GetComponent<PlayerMovement>().SetArrow(dataPlayer2.arrow);
            playerObj.GetComponent<PlayerMovement>().SetCamera(cam);
            playerObj.GetComponent<PlayerRangeItem>().SetThrowablePrefab(throwablePrefab);
            playerObj.GetComponent<PlayerRangeItem>().SetItemHolderUI(dataPlayer2.itemHolder);
            GameplayManager.Instance.Players.Add(playerObj);
        }
    }

    public void StartGame(PlayerInput p1, PlayerInput p2) {
        p1.SwitchCurrentActionMap("Gameplay");
        p2.SwitchCurrentActionMap("Gameplay");
        PowerUpSpawner.Instance.StartSpawn = true;
        CameraManager.Instance.SetInGameCamActive();
        ClockManager.Instance.SpawnClock();

        gameplayUI.SetActive(true);
    }
}
