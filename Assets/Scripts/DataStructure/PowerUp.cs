using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class DurationPowerUp : PowerUp
{
    [SerializeField] protected float duration;
    protected float timer;
    public float progress;

    protected void FixedUpdate()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            progress = 1 - Mathf.InverseLerp(0, duration, timer);
        }
    }
    private void Update()
    {
        ProgressUpdate.Invoke(progress);
    }
}

public abstract class PowerUp: MonoBehaviour 
{
    protected PlayerMovement target;
    public UnityEvent<PowerUp> PowerUpEnd;
    public UnityEvent<float> ProgressUpdate;
    private Material material;
    private Range lifetime = new Range(10, 24);

    private void Awake() {
        material = GetComponent<SpriteRenderer>().material;
    }

    private void Start() {
        StartCoroutine(DissolveEnter());
        StartCoroutine(Lifetime());
    }

    private IEnumerator DissolveEnter()
    {
        float dissolve = 1f;
        while (dissolve > 0)
        {
            dissolve -= Time.deltaTime * (1 / 1.5f);
            material.SetFloat("_Value", dissolve);
            yield return null;
        }
    }

    private IEnumerator DissolveExit()
    {
        float dissolve = 0;
        while (dissolve < 1f)
        {
            dissolve += Time.deltaTime * (1 / 1.5f);
            material.SetFloat("_Value", dissolve);
            yield return null;
        }

        Destroy(gameObject);
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(lifetime.RandomValue());
        StartCoroutine(DissolveExit());
    }

    public virtual IEnumerator PowerUpCoroutine(PlayerMovement target)
    {
        yield return null;
        PowerUpEnd.Invoke(this);
    }

    protected void PickedUp(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBody") == false) return;
        StopAllCoroutines();
        AudioManager.Instance.PlaySFX("PowerUp_PickUp");
        GameObject player = collision.gameObject.GetComponentInParent<PlayerMovement>().gameObject;
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

        if (playerMovement == null || playerMovement.IsGrabbed || playerMovement.IsStunned) return;

        player.GetComponent<PlayerPowerUp>().EquipItem(this);
        if (player.GetComponent<PlayerInput>().playerIndex == 0)
        {
            transform.SetParent(PowerUpSpawner.Instance.PowerUpPanelP1.transform);
        }
        else
        {
            transform.SetParent(PowerUpSpawner.Instance.PowerUpPanelP2.transform);
        }
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(50f, 50f, 50f);

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Image>().enabled = true;

        gameObject.GetComponent<Collider2D>().enabled = false;
        PowerUpSpawner.powerUpInScene--;
    }

    protected void FindOppositeTarget()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PlayerInput>().playerIndex != target.GetComponent<PlayerInput>().playerIndex)
            {
                target = player.GetComponent<PlayerMovement>();
                return;
            }
        }
    }
}
