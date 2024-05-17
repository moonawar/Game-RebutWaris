using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRangeItem : MonoBehaviour
{
    private Throwable throwablePrefab;
    private Throwable selected;
    [SerializeField] private List<ScriptableThrowable> scriptableThrowables;
    [SerializeField] public List<int> throwableAmount;
    [SerializeField] private Transform arrow;
    [SerializeField] private int selectedItem = 0;
    private GameObject itemHolderUI;

    private void Awake()
    {
        throwableAmount = new List<int>(scriptableThrowables.Count);
        for (int i = 0; i < scriptableThrowables.Count; i++)
        {
            throwableAmount.Add(scriptableThrowables[i].initialAmount);
        }
        selected = Instantiate(throwablePrefab);
        selected.gameObject.GetComponent<SpriteRenderer>().sprite = scriptableThrowables[selectedItem].sprite;
        selected.GetComponent<Collider2D>().enabled = false;
        selected.transform.SetParent(itemHolderUI.transform);
        selected.transform.localScale = Vector3.one * 10;
        selected.transform.localPosition = Vector3.zero;
        selected.GetComponent<SpriteRenderer>().sortingOrder = 2;


    }

    public void SetItemHolderUI(GameObject holder)
    {
        itemHolderUI = holder;
    }

    public void SetThrowablePrefab(Throwable prefab)
    {
        throwablePrefab = prefab;
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (throwableAmount[selectedItem] > 0)
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
                thrown.SetIndex(selectedItem);
                thrown.SetThrowable(scriptableThrowables[selectedItem]);
                thrown.gameObject.GetComponent<SpriteRenderer>().sprite = scriptableThrowables[selectedItem].sprite;
                thrown.SetArena(gameObject.GetComponent<PlayerMovement>().GetArena());
                Vector2 mousePos = gameObject.GetComponent<PlayerMovement>().getMousePosition();
                Vector2 lookDir = mousePos - new Vector2(arrow.position.x, arrow.position.y);
                float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
                Vector3 throwAngle = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
                thrown.Throw(throwAngle);

                throwableAmount[selectedItem]--;
                arrow.gameObject.SetActive(false);
            }
        }
    }

    public void OnChangeItem(InputAction.CallbackContext context)
    {
        if (context.performed && scriptableThrowables.Count > 0)
        {
            selectedItem++;
            if(selectedItem >= scriptableThrowables.Count)
            {
                selectedItem = 0;
            }

            Destroy(selected.gameObject);
            selected = Instantiate(throwablePrefab);
            selected.GetComponent<Collider2D>().enabled = false;
            selected.GetComponent<SpriteRenderer>().sprite = scriptableThrowables[selectedItem].sprite;
            selected.transform.SetParent(itemHolderUI.transform);
            selected.transform.localScale = Vector3.one * 10;
            selected.transform.localPosition = Vector3.zero;
            selected.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
    }
}
