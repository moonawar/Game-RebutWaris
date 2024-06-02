using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerBonk : MonoBehaviour
{
    [Header("Bonk Settings")]
    [SerializeField] private float radius = 0.8f;
    [SerializeField] private float stunDuration = 1.2f;
    [SerializeField] public PlayerMovement playerMovement;
    [SerializeField] private Collider2D selfCollider;
    [SerializeField] private float Cooldown;
    private float timer;
    private GameObject cooldownIndicator;
    private bool OnCooldown = false;

    public void SetBonkIndicator(GameObject indicator)
    {
        cooldownIndicator = indicator;
    }

    private void FixedUpdate()
    {
        if(OnCooldown && timer > 0)
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

    public void OnBonk(InputAction.CallbackContext context)
    {
        if (GameplayManager.Instance.Paused) return;
        if (OnCooldown) return;

        gameObject.GetComponent<Animator>().SetTrigger("Bonk");
        if (playerMovement.IsStunned || playerMovement.IsGrabbed) return;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collider in colliders)
        {
            if (collider == selfCollider) continue;


            if (collider.TryGetComponent(out PlayerMovement player))
            {
                player.Stun(stunDuration);
            }
        }

        StartCoroutine(CooldownTimer());
    }
}
