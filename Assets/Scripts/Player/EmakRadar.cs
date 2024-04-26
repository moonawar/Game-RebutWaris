using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public class PlayerRadarUIRefs {
    public GameObject fill;
    public Slider loveMeter;
    public GameObject heart;
}

public class EmakRadar : MonoBehaviour
{   
    [Header("Detection")]
    [SerializeField] private float radius = 4f;
    [SerializeField] private float offsetY = 0.0f;
    [SerializeField] private float offsetX = 0.0f;

    [Header("Phase Properties")]
    [SerializeField] private Phase[] phases = new Phase[3];
    [SerializeField] private float decreaseRate = 0.5f;
    [SerializeField] private float timeToStartDecreasing = 3f;

    // Private properties
    private float increaseRate;
    private float baseValue = 0;
    private float nextPhaseValue;
    private float lastTimePrincessInArea = 0;

    private int loveLevel = 0;

    private int playerIdx;

    // UI Components
    private GameObject fill;
    private Slider loveMeter;
    private GameObject heart;

    public void InitUIs(PlayerRadarUIRefs uiRefs) {
        fill = uiRefs.fill;
        loveMeter = uiRefs.loveMeter;
        heart = uiRefs.heart;

        fill.GetComponent<Image>().color = Color.green;
        fill.SetActive(false);
        heart.GetComponent<Image>().color = Color.white;
    }

    private void Awake()
    {
        nextPhaseValue = phases[loveLevel].limit;
        increaseRate = phases[loveLevel].increase;

        playerIdx = GetComponent<PlayerInput>().playerIndex;
    }

    public void OnMashInput(InputAction.CallbackContext context) {
        if (!context.canceled) return;
        if (loveLevel >= 3) return;
        if (IsPrincessInArea())
        {
            fill.SetActive(true);
            loveMeter.value += increaseRate;
        }

        if (loveMeter.value >= nextPhaseValue)
        {
            ChangePhase();
        }
    }

    private bool IsPrincessInArea()
    {
        Vector3 center = transform.position;
        center.x += offsetX;
        center.y += offsetY;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(center, radius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Emak")) return true;
        }
        return false;
    }

    void Update()
    {
        if (loveLevel >= 3) return;

        if (IsPrincessInArea()) {
            lastTimePrincessInArea = Time.time;
        }

        if (Time.time - lastTimePrincessInArea >= timeToStartDecreasing && loveMeter.value >= baseValue)
        {
            loveMeter.value -= decreaseRate * Time.deltaTime;
        }

        if (loveMeter.value == 0) { fill.SetActive(false); }
    }

    void ChangePhase()
    {
        loveLevel++;

        if (loveLevel >= 3) {
            GameplayManager.Instance.EndTheGame(playerIdx);
            return;
        }
        baseValue = nextPhaseValue;
        nextPhaseValue = phases[loveLevel].limit;
        increaseRate = phases[loveLevel].increase;

        UpdateUI();
    }

    private void UpdateUI() {
        if (loveLevel == 1)
        {
            heart.SetActive(true);
            heart.GetComponent<Image>().color = Color.green;
            fill.GetComponent<Image>().color = Color.red;
        }
        else if (loveLevel == 2)
        {
            heart.SetActive(true);
            heart.GetComponent<Image>().color = Color.red;
            fill.GetComponent<Image>().color = new Color32(224, 55, 204, 255);
        }
        else // Level 3
        {
            heart.SetActive(true);
            heart.GetComponent<Image>().color = new Color32(224, 55, 204, 255);
        }
    }

    private void OnDrawGizmosSelected() {
        Vector3 center = transform.position;
        center.x += offsetX;
        center.y += offsetY;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, radius);
    }
}
