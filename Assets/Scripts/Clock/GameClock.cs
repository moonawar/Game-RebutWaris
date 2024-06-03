using System.Collections;
using UnityEngine;

public class GameClock : MonoBehaviour
{
    private Material material;
    private bool taken;

    private void Awake() {
        material = GetComponent<SpriteRenderer>().material;
    }

    private void Start() {
        StartCoroutine(DissolveEnter());
    }

    private IEnumerator DissolveEnter()
    {
        float dissolve = 1f;
        while (dissolve > 0)
        {
            dissolve -= Time.deltaTime * (1 / 3f);
            material.SetFloat("_Value", dissolve);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("PlayerBody") && !taken) {
            PlayerMash player = other.GetComponentInParent<PlayerMash>();
            player.HaveClock = true;
            taken = true;
            Destroy(gameObject);
        }
    }
}
