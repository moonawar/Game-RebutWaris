using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[System.Serializable]
public class MikaLeafblowerProps
{
    public float posXLeft;
    public float posXRight;
    public float posY;
    public float walkSpeed;
    public float walkOffset;
    public float leafblowerDuration = 5f;
}

public class MikaEvent : MonoBehaviour
{
    [SerializeField] private MikaLeafblowerProps leafblowerProps;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Action leafblowerOnCallback;
    private Action leafblowerOffCallback;

    private Vector3 startWalkPos;
    private Vector3 endWalkPos;
    private float walkDuration;

    private void Awake() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartLeafblowerEvent(int from, Action onLeafblowerOn, Action onLeafblowerOff) {
        endWalkPos = new Vector3(
            from == PlayerMovement.LEFT ? leafblowerProps.posXLeft : leafblowerProps.posXRight,
            leafblowerProps.posY, 0
        );
        startWalkPos = endWalkPos + new Vector3(from * leafblowerProps.walkOffset, 0, 0);

        transform.position = startWalkPos;
        walkDuration = leafblowerProps.walkOffset / leafblowerProps.walkSpeed;

        animator.SetTrigger("Walk");
        CameraManager.Instance.SetMikaEventCamActive();
        spriteRenderer.flipX = from == PlayerMovement.LEFT;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(endWalkPos, walkDuration).SetEase(Ease.Linear));
        sequence.AppendCallback(() => {
            animator.SetTrigger("LeafblowerOn");
        });

        leafblowerOnCallback = onLeafblowerOn;
        leafblowerOffCallback = onLeafblowerOff;
    }

    public void MikaLeafblowerOnFrame() {
        Sequence sequence = DOTween.Sequence();
        leafblowerOnCallback();
        CameraManager.Instance.SetInGameCamActive();
        sequence.AppendInterval(leafblowerProps.leafblowerDuration);
        sequence.AppendCallback(() => {
            CameraManager.Instance.SetMikaEventCamActive();
            animator.SetTrigger("Idle");
            leafblowerOffCallback();
        });
        sequence.AppendInterval(0.5f);
        sequence.AppendCallback(() => {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            animator.SetTrigger("Walk");
        });
        sequence.Append(transform.DOMove(startWalkPos, walkDuration).SetEase(Ease.Linear));
        sequence.AppendCallback(() => {
            CameraManager.Instance.SetInGameCamActive();
        });

    }
}
