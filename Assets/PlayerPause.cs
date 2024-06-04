using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPause : MonoBehaviour
{
    public void OnPause(InputAction.CallbackContext context)
    {
        if (GameplayManager.Instance.GameEnded) return;
        if (context.performed)
        {
            if (GameplayManager.Instance.Paused)
            {
                GameplayManager.Instance.UnpauseGame();
            }
            else
            {
                GameplayManager.Instance.PauseGame();
            }
        }
    }
}
