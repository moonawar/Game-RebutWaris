using DG.Tweening;
using UnityEngine;

public class ScaleBounce : MonoBehaviour
{
    [SerializeField] private Range bounceRange = new Range(1.0f, 1.05f);
    [SerializeField] private float bounceDuration = 0.4f;
    void Start()
    {
        ScaleLoop();
    }

    private void ScaleLoop() {
        transform.DOScale(bounceRange.max, bounceDuration).SetEase(Ease.InOutSine).OnComplete(() => {
            transform.DOScale(bounceRange.min, bounceDuration).SetEase(Ease.InOutSine).OnComplete(ScaleLoop);
        });
    }
}
