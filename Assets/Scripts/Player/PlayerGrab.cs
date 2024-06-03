using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerGrab : MonoBehaviour
{
    [Header("Grab Settings")]
    [SerializeField] private float radius = 4;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Collider2D selfCollider;
    [SerializeField] private Transform arrow;
    [SerializeField] private float Cooldown;
    private float timer;
    private GameObject cooldownIndicator;
    private bool OnCooldown = false;
    private Animator animator;

    public void SetGrabIndicator(GameObject indicator)
    {
        cooldownIndicator = indicator;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (OnCooldown && timer > 0)
        {
            timer -= Time.deltaTime;
            cooldownIndicator.GetComponent<Image>().fillAmount = Mathf.InverseLerp(0, Cooldown, timer);
        }

    }

    private IEnumerator CooldownTimer()
    {
        OnCooldown = true;
        timer = Cooldown;
        yield return new WaitForSeconds(Cooldown);
        OnCooldown = false;
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (GameplayManager.Instance.Paused) return;
        if (OnCooldown) return; 

        if (context.performed)
        {
            if (playerMovement.IsStunned || playerMovement.IsGrabbed) return;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D collider in colliders)
            {
                if (collider != selfCollider && collider.TryGetComponent(out PlayerMovement player))
                {
                    if (player.IsGrabbed) return;
                    animator.SetTrigger("Grab");
                    player.Grabbed(gameObject.transform.position);
                    arrow.gameObject.SetActive(true);
                    playerMovement.GrabMode = true;

                    if (player.GetComponent<PlayerMash>().HaveClock)
                    {
                        player.GetComponent<PlayerMash>().HaveClock = false;
                        ClockManager.Instance.SpawnClock(player.transform.position);
                    }

                    break;
                }
            }
        }

        if (context.canceled && playerMovement.GrabMode)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D collider in colliders)
            {
                if (collider == selfCollider) continue;

                if (playerMovement.IsStunned || playerMovement.IsGrabbed) return;
                if (collider.TryGetComponent(out PlayerMovement player))
                {
                    if (!player.IsGrabbed) return;

                    animator.SetTrigger("Throw");
                    playerMovement.GrabMode = false;
                    if (Mathf.Abs(transform.rotation.eulerAngles.y) == 180)
                    {
                        player.Ungrab(arrow.transform.rotation.eulerAngles.z, -1);

                    }
                    else
                    {
                        player.Ungrab(arrow.transform.rotation.eulerAngles.z, 1);

                    }

                    arrow.gameObject.SetActive(false);
                }
            }
            StartCoroutine(CooldownTimer());
        }

    }
}
