using TMPro;
using UnityEngine;

public class TextPopUpManager : MonoBehaviour
{
    [SerializeField] private GameObject eventTextPrefab;

    public static TextPopUpManager Instance;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void SpawnEventText(string text)
    {
        GameObject eventText = Instantiate(eventTextPrefab, transform);
        eventText.GetComponent<TextMeshProUGUI>().text = text;
    }
}
