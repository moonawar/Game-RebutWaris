using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerRangeItem : MonoBehaviour
{
    private Throwable throwablePrefab;
    [SerializeField] public int throwableAmount;
    [SerializeField] private Transform arrow;
    [SerializeField] private GameObject playerBody;
    private TMP_Text amountText;

    public void SetAmountText(TMP_Text text)
    {
        amountText = text;
    }

    public void IncrementThrowable()
    {
        throwableAmount++;
        UpdateAmountText();
    }

    public void DecrementThrowable()
    {
        throwableAmount--;
        UpdateAmountText();
    }

    private void UpdateAmountText()
    {
        amountText.text = throwableAmount.ToString();
    }

    public void SetThrowablePrefab(Throwable prefab)
    {
        throwablePrefab = prefab;
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (GameplayManager.Instance.Paused) return;
        if (GameplayManager.Instance.GameEnded) return;
        if (throwableAmount > 0)
        {
            if (context.performed)
            {
                gameObject.GetComponent<PlayerMovement>().AimMode = true;
                arrow.gameObject.SetActive(true);
            }

            if (context.canceled)
            {
                gameObject.GetComponent<PlayerMovement>().AimMode = false;

                AudioManager.Instance.PlaySFX("SendalThrown");
                Throwable thrown = Instantiate(throwablePrefab, transform.position, transform.rotation);
                thrown.SetOwner(gameObject.GetComponent<PlayerMovement>());
                thrown.SetOwnerBody(playerBody);
                thrown.SetArena(gameObject.GetComponent<PlayerMovement>().GetArena());

                float angle;
                if (GetComponent<PlayerInput>().currentControlScheme == "Joystick" || GetComponent<PlayerInput>().currentControlScheme == "Gamepad")
                {
                    angle = GetComponent<PlayerMovement>().getRangeAngle();
                }
                else
                {
                    Vector2 mousePos = gameObject.GetComponent<PlayerMovement>().getMousePosition();
                    Vector2 lookDir = mousePos - new Vector2(arrow.position.x, arrow.position.y);
                    angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
                }
                
                Vector3 throwAngle = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
                thrown.Throw(throwAngle);

                DecrementThrowable();
                arrow.gameObject.SetActive(false);
            }
        }
    }
}
