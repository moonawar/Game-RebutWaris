using DG.Tweening;
using TMPro;
using UnityEngine;

public class EventTextAnim : MonoBehaviour
{
    [SerializeField] private float initOffsetY = -50;
    [SerializeField] private float targetOffsetY = 0;
    private TextMeshProUGUI tmp;
    private Vector3 targetPos;

    private void Awake() {
        tmp = GetComponent<TextMeshProUGUI>();
        targetPos = new Vector3(transform.position.x, transform.position.y + targetOffsetY, transform.position.z);
        transform.position = new Vector3(transform.position.x, transform.position.y + initOffsetY, transform.position.z);
    }

    private void Start()
    {
        transform.DOMoveY(targetPos.y, 3.2f).OnComplete(() =>
        {
            tmp.DOColor(new Color(1, 1, 1, 0), 1.1f).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        });
    }
}
