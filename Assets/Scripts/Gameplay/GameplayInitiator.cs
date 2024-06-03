using Cinemachine;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

[System.Serializable]
public class PlayerData {
    public AnimatorController animator;
    public Sprite arrow;
    public TMP_Text throwableText;
    public GameObject activePowerUps;
    public GameObject bonkIndicator;
    public GameObject grabIndicator;
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
    [SerializeField] private PostProcessVolume postProcessVolume;

    public void OnPlayerJoined(GameObject playerObj, int playerIndex)  {
        PlayerMash radar = playerObj.GetComponent<PlayerMash>();
        targetGroup.AddMember(playerObj.transform, 1, 0);

        if (playerIndex == 0) {
            radar.InitUIs(dataPlayer1.uiRefs);
            playerObj.GetComponent<Animator>().runtimeAnimatorController = dataPlayer1.animator;
            playerObj.GetComponent<PlayerMovement>().SetArena(Arena);
            playerObj.GetComponent<PlayerMovement>().SetArrow(dataPlayer1.arrow);
            playerObj.GetComponent<PlayerMovement>().SetCamera(cam);
            playerObj.GetComponent<PlayerRangeItem>().SetThrowablePrefab(throwablePrefab);
            playerObj.GetComponent<PlayerRangeItem>().SetAmountText(dataPlayer1.throwableText);
            playerObj.GetComponent<PlayerPowerUp>().SetActivePanel(dataPlayer1.activePowerUps);
            playerObj.GetComponent<PlayerBonk>().SetBonkIndicator(dataPlayer1.bonkIndicator);
            playerObj.GetComponent<PlayerGrab>().SetGrabIndicator(dataPlayer1.grabIndicator);
            GameplayManager.Instance.Players.Add(playerObj);

        } else if (playerIndex == 1) {
            radar.InitUIs(dataPlayer2.uiRefs);
            playerObj.GetComponent<Animator>().runtimeAnimatorController = dataPlayer2.animator;
            playerObj.GetComponent<PlayerMovement>().SetArena(Arena);
            playerObj.GetComponent<PlayerMovement>().SetArrow(dataPlayer2.arrow);
            playerObj.GetComponent<PlayerMovement>().SetCamera(cam);
            playerObj.GetComponent<PlayerRangeItem>().SetThrowablePrefab(throwablePrefab);
            playerObj.GetComponent<PlayerRangeItem>().SetAmountText(dataPlayer2.throwableText);
            playerObj.GetComponent<PlayerPowerUp>().SetActivePanel(dataPlayer2.activePowerUps);
            playerObj.GetComponent<PlayerBonk>().SetBonkIndicator(dataPlayer2.bonkIndicator);
            playerObj.GetComponent<PlayerGrab>().SetGrabIndicator(dataPlayer2.grabIndicator);
            GameplayManager.Instance.Players.Add(playerObj);
        }
    }

    public void StartGame(PlayerInput p1, PlayerInput p2) {
        p1.SwitchCurrentActionMap("Gameplay");
        p2.SwitchCurrentActionMap("Gameplay");
        CameraManager.Instance.SetInGameCamActive();
        ClockManager.Instance.SpawnClock();

        gameplayUI.SetActive(true);

        // disable depth of field
        postProcessVolume.profile.TryGetSettings(out DepthOfField dof);
        dof.active = false;
    }
}
