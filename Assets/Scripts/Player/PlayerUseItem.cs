using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUseItem : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Collider2D selfCollider;
    [SerializeField] private PowerUp powerUp;
    private GameObject activePanel;
    private List<PowerUp> activePowerUps = new List<PowerUp>();
    //public int activePowerUps = 0;

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
                    float panelPosition = -77.0f + activePowerUps.Count * 30;
                    powerUp.transform.SetParent(activePanel.transform);
                    powerUp.transform.localPosition = new Vector2(panelPosition, -36);
                    powerUp.transform.localScale = new Vector3(10, 10, 10);

                    bool alreadyActive = false;
                    foreach(PowerUp power in activePowerUps)
                    {
                        if (power.GetType().Equals(powerUp.GetType()))
                        {
                            powerUp.transform.localPosition = power.transform.localPosition;
                            powerUp.transform.localScale = power.transform.localScale;
                            alreadyActive = true;
                            StopCoroutine(power.PowerUpCoroutine(player));
                            break;
                        }
                    }

                    if (!alreadyActive)
                    {
                        powerUp.PowerUpEnd.AddListener(deactivatePowerUp);
                        activePowerUps.Add(powerUp);
                    }

                    StartCoroutine(powerUp.PowerUpCoroutine(player));
                    // Make event subsriber
                    // handle kalau power up yg sama diaktifin lg
                }
                
            }

            powerUp = null;

        }

    }
}
