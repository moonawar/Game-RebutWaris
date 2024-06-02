using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerRangeItem : MonoBehaviour
{
    private Throwable throwablePrefab;
    private Throwable selected;
    //[SerializeField] private List<ScriptableThrowable> scriptableThrowables;
    [SerializeField] public int throwableAmount;
    [SerializeField] private Transform arrow;
    [SerializeField] private GameObject playerBody;
    //[SerializeField] private int selectedItem = 0;
    //private GameObject itemHolderUI;
    private TMP_Text amountText;

    private void Awake()
    {
        //throwableAmount = new List<int>(scriptableThrowables.Count);
        //for (int i = 0; i < scriptableThrowables.Count; i++)
        //{
        //    throwableAmount.Add(scriptableThrowables[i].initialAmount);
        //}
        //selected = Instantiate(throwablePrefab);
        //selected.gameObject.GetComponent<SpriteRenderer>().sprite = scriptableThrowables[selectedItem].sprite;
        //selected.GetComponent<Collider2D>().enabled = false;
        //selected.transform.SetParent(itemHolderUI.transform);
        //selected.transform.localScale = Vector3.one * 10;
        //selected.transform.localPosition = Vector3.zero;
    }

    //public void SetItemHolderUI(GameObject holder)
    //{
    //    itemHolderUI = holder;
    //}

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

                
                Throwable thrown = Instantiate(throwablePrefab, transform.position, transform.rotation);
                thrown.SetOwner(gameObject.GetComponent<PlayerMovement>());
                thrown.SetOwnerBody(playerBody);
                thrown.SetArena(gameObject.GetComponent<PlayerMovement>().GetArena());
                Vector2 mousePos = gameObject.GetComponent<PlayerMovement>().getMousePosition();
                Vector2 lookDir = mousePos - new Vector2(arrow.position.x, arrow.position.y);
                float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
                Vector3 throwAngle = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
                thrown.Throw(throwAngle);

                DecrementThrowable();
                arrow.gameObject.SetActive(false);
            }
        }
    }
}
