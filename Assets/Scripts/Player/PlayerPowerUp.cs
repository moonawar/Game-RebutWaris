using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public struct ActivePowerUp
{
    public Coroutine coroutine;
    public PowerUp powerUp;
}

public class PlayerPowerUp : MonoBehaviour
{
    [Header("Power Up Settings")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Collider2D selfCollider;
    [SerializeField] private PowerUp powerUp;
    [SerializeField] private GameObject indicatorPrefab;
    private GameObject activePanel;
    private List<PowerUp> activePowerUps = new List<PowerUp>();
    private List<Coroutine> activeCoroutines = new List<Coroutine>();

    private void Update()
    {
        if (activePowerUps.Count <= 0) return;

        foreach(PowerUp power in activePowerUps)
        {
            if(power is DurationPowerUp)
            {
                power.transform.GetChild(0).GetComponent<Image>().fillAmount = (power as DurationPowerUp).progress;
            }
        }
    }

    public void SetActivePanel(GameObject panel)
    {
        activePanel = panel;
    }
    public void EquipItem(PowerUp pickedUp)
    {
        if(powerUp != null)
        {
            Destroy(powerUp.gameObject);
        }
        powerUp = pickedUp;
    }

    public void deactivatePowerUp(PowerUp power)
    {
        activePowerUps.Remove(power);
        
        foreach(PowerUp powerUp in activePowerUps)
        {
            powerUp.transform.localPosition -= new Vector3(30, 0, 0);
        }
        Destroy(power.gameObject);
    }

    public void OnUseItem(InputAction.CallbackContext context)
    {
        if (GameplayManager.Instance.Paused) return;
        if (context.performed && powerUp != null)
        {
            if (playerMovement.IsStunned) return;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 100);
            foreach (Collider2D collider in colliders)
            {
                if (collider != selfCollider && collider.TryGetComponent(out PlayerMovement player))
                {
                    foreach(DurationPowerUp power in activePowerUps)
                    {
                        if (power.GetType() == powerUp.GetType())
                        {
                            int index = activePowerUps.IndexOf(power);
                            StopCoroutine(activeCoroutines[index]);
                            activeCoroutines.RemoveAt(index);
                            deactivatePowerUp(power);
                            break;
                        }
                    }

                    float panelPosition = -77.0f + activePowerUps.Count * 30;

                    powerUp.transform.SetParent(activePanel.transform);
                    powerUp.transform.localPosition = new Vector2(panelPosition, -36);
                    powerUp.transform.localScale = new Vector3(10, 10, 10);

                    GameObject radialLoad = Instantiate(indicatorPrefab, powerUp.transform);
                    radialLoad.transform.localPosition = Vector3.zero;
                    radialLoad.transform.localScale = Vector3.one;

                    powerUp.PowerUpEnd.AddListener(deactivatePowerUp);
                    activePowerUps.Add(powerUp);

                    activeCoroutines.Add(StartCoroutine(powerUp.PowerUpCoroutine(player)));
                }
                
            }

            powerUp = null;

        }

    }
}
