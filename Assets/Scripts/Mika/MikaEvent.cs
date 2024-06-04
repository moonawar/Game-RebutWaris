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
    public Transform geese;
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
    private GameObject[] geese;

    private void Awake() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        geese = new GameObject[bcProps.geese.childCount];
        for (int i = 0; i < bcProps.geese.childCount; i++) {
            geese[i] = bcProps.geese.GetChild(i).gameObject;
        }
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
        endWalkPos = new Vector3(
            dest.x < 0 ? lbProps.posXLeft : lbProps.posXRight,
            lbProps.posY, 0
        );
        startWalkPos = endWalkPos + new Vector3(Mathf.Sign(dest.x) * lbProps.walkOffset, 0, 0);
        endWalkPos = dest;
        spriteRenderer.flipX = dest.x < 0;

        int half = Mathf.CeilToInt(geese.Length / 2);
        int i = 0;
        foreach (GameObject goose in geese) {
            if (i < half) goose.transform.position = GetRandomOutsideArea(PlayerMovement.RIGHT);
            else goose.transform.position = GetRandomOutsideArea(PlayerMovement.LEFT);
            i++;
        }

        transform.position = startWalkPos;
        walkDuration = Vector2.Distance(transform.position, dest) / bcProps.walkSpeed;
        animator.SetTrigger("Walk");

        Sequence sequence = DOTween.Sequence();
        CameraManager.Instance.SetMikaEventCamActive();
        sequence.Append(transform.DOMove(dest, walkDuration).SetEase(Ease.Linear));
        sequence.AppendCallback(() => {
            animator.SetTrigger("Breadcrumb");
        });
    }

    public void MikaBreadcrumbEndFrame() {
      animator.SetTrigger("Idle");
      CameraManager.Instance.SetInGameCamActive();
      InitiateGeeseAttack();
      Sequence sequence = DOTween.Sequence();
      sequence.AppendInterval(0.5f);
      sequence.AppendCallback(() => {
          spriteRenderer.flipX = !spriteRenderer.flipX;
          animator.SetTrigger("Walk");
      });
      sequence.Append(transform.DOMove(startWalkPos, walkDuration).SetEase(Ease.Linear));
    //   sequence.AppendCallback(() => {
    //       CameraManager.Instance.SetInGameCamActive();
    //   });
    }

    private void InitiateGeeseAttack() {
        foreach (GameObject goose in geese) {
            Vector3 dest = goose.transform.position + (endWalkPos - goose.transform.position).normalized * 100;
            dest = dest + new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0);
            float duration = Vector3.Distance(goose.transform.position, dest) / bcProps.duckSpeed;
            goose.GetComponent<SpriteRenderer>().flipX = !(dest.x < 0);
            goose.transform.DOMove(dest, duration).SetEase(Ease.Linear).SetDelay(UnityEngine.Random.Range(0, 1));
        }
    }

   private Vector2 GetRandomInsideArea()
    {
        Vector2 randomDest = new Vector2(
            UnityEngine.Random.Range(bcProps.area.bounds.min.x, bcProps.area.bounds.max.x),
            UnityEngine.Random.Range(bcProps.area.bounds.min.y, bcProps.area.bounds.max.y)
        );

        return randomDest;
    }

    private Vector2 GetRandomOutsideArea(int dir = 0)
    {
        float rx = UnityEngine.Random.Range(0, 2);
        if (dir != 0) {
            rx = dir;
        }
        float x = bcProps.area.bounds.min.x - 10;
        if (rx == 1) {
            x = bcProps.area.bounds.max.x + 10;
        } 
        float y = UnityEngine.Random.Range(bcProps.area.bounds.min.y, bcProps.area.bounds.max.y);

        Vector2 randomDest = new Vector2(x, y);
        return randomDest;
    }
}
