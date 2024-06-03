using System;
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

[System.Serializable]
public class MikaBreadcrumbProps
{
    public Collider2D area;
    public GameObject[] ducks;
    public float duckSpeed;
    public float walkSpeed;
}

public class MikaEvent : MonoBehaviour
{
    [SerializeField] private MikaLeafblowerProps lbProps;
    [SerializeField] private MikaBreadcrumbProps bcProps;
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
            from == PlayerMovement.LEFT ? lbProps.posXLeft : lbProps.posXRight,
            lbProps.posY, 0
        );
        startWalkPos = endWalkPos + new Vector3(from * lbProps.walkOffset, 0, 0);

        transform.position = startWalkPos;
        walkDuration = lbProps.walkOffset / lbProps.walkSpeed;

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
        sequence.AppendInterval(lbProps.leafblowerDuration);
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

    public void StartBreadcrumbEvent() {
        Vector2 dest = GetRandomInsideArea();
        foreach (GameObject duck in bcProps.ducks) {
            duck.transform.position = GetRandomOutsideArea();
        }

        transform.position = dest;
        float walkDuration = Vector2.Distance(transform.position, dest) / bcProps.walkSpeed;
        animator.SetTrigger("Walk");

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(dest, walkDuration).SetEase(Ease.Linear));

    }

   private Vector2 GetRandomInsideArea()
    {
        Vector2 randomDest = new Vector2(
            UnityEngine.Random.Range(bcProps.area.bounds.min.x, bcProps.area.bounds.max.x),
            UnityEngine.Random.Range(bcProps.area.bounds.min.y, bcProps.area.bounds.max.y)
        );

        return randomDest;
    }

    private Vector2 GetRandomOutsideArea()
    {
        float rx = UnityEngine.Random.Range(0, 3);
        float ry = UnityEngine.Random.Range(0, 3);

        while (rx == 3 && ry == 3) {
            ry = UnityEngine.Random.Range(0, 3);
        }

        float x = bcProps.area.bounds.min.x - 5;
        if (rx == 1) {
            x = bcProps.area.bounds.max.x + 5;
        } else if (rx == 2) {
            x = UnityEngine.Random.Range(bcProps.area.bounds.min.x, bcProps.area.bounds.max.x);
        }

        float y = bcProps.area.bounds.min.y - 5;
        if (ry == 1) {
            y = bcProps.area.bounds.max.y + 5;
        } else if (ry == 2) {
            y = UnityEngine.Random.Range(bcProps.area.bounds.min.y, bcProps.area.bounds.max.y);
        }

        Vector2 randomDest = new Vector2(x, y);
        return randomDest;
    }
}
