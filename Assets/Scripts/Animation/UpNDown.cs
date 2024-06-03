using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UpNDown : MonoBehaviour
{
    [SerializeField] private float offset = 0.5f;
    [SerializeField] private float loopDuration = 1;

    private void Start() {
        Vector3 targetPosition = transform.localPosition + Vector3.up * offset;
        transform.DOLocalMove(targetPosition, loopDuration).SetLoops(-1, LoopType.Yoyo);
    }
}
