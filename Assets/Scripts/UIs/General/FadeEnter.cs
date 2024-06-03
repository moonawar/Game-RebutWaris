using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeEnter : MonoBehaviour
{
    [SerializeField] private Vector2 _startOffset;
    [SerializeField] private float _duration = 0.5f;
    private Image _image;
    private CanvasGroup _canvasGroup;


    private void Awake() {
        _image = GetComponent<Image>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }


    private void Start() {
        Vector2 targetPosition = _image.rectTransform.anchoredPosition;
        _image.rectTransform.anchoredPosition = targetPosition + _startOffset;
        _canvasGroup.alpha = 0;

        _image.rectTransform.DOAnchorPos(targetPosition, _duration).SetEase(Ease.OutSine);
        _canvasGroup.DOFade(1, _duration).SetEase(Ease.OutSine);
    }
}
