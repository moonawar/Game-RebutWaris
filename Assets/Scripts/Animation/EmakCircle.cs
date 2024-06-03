using System;
using DG.Tweening;
using UnityEngine;

public class EmakCircle : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float maxAlpha;
    [SerializeField] private float duration = 1;


    private int playerInArea = 0;
    private bool zeroFlag = true;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        maxAlpha = spriteRenderer.color.a;
    }

    private void Start() {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
    }

    public void OnPlayerEnter()
    {
        playerInArea++;
    }

    public void OnPlayerExit()
    {
        playerInArea--;
    }

    private void Update() {
        if (playerInArea > 0 && zeroFlag) {
            zeroFlag = false;
            spriteRenderer.DOFade(maxAlpha, duration);
        } else if (playerInArea == 0 && !zeroFlag) {
            zeroFlag = true;
            spriteRenderer.DOFade(0, duration);
        }
    }
}
