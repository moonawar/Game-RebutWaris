using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] private ScriptableThrowable item;
    private PlayerMovement owner;
    private float currentSpeed;
    private int hitsLeft;
    private bool isThrown = false;
    private Vector3 direction;
    [SerializeField] private Collider2D arena;
    private int indexInList;



    private void Awake()
    {
        print(item.sprite.name);
        gameObject.GetComponent<SpriteRenderer>().sprite = item.sprite;
    }
    public void SetThrowable(ScriptableThrowable throwable)
    {
        item = throwable;
    }
    public void SetArena(Collider2D arena)
    {
        this.arena = arena;
    }
    public void SetOwner(PlayerMovement player)
    {
        owner = player;
    }

    public void SetIndex(int id)
    {
        indexInList = id;
    }

    private void FixedUpdate()
    {
        if (!isThrown) return;

        if (hitsLeft <= 0 || currentSpeed <= 0.1)
        {
            currentSpeed = 0;
            hitsLeft = 0;
            isThrown = false;
        }

        currentSpeed -= item.deacceleration;
        transform.position += currentSpeed * Time.fixedDeltaTime * direction;
        StayInBounds();
    }

    public void Throw(Vector3 direction)
    {
        this.direction = direction;
        currentSpeed = item.throwSpeed;
        hitsLeft = item.hits;
        transform.position += currentSpeed * Time.fixedDeltaTime * direction;
        isThrown = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isThrown)
        {
            if (collision.gameObject.tag == "Player" && collision != owner.GetComponent<Collider2D>())
            {
                collision.GetComponent<EmakRadar>().DecreaseLove(item.damage);
                currentSpeed = 0;
                hitsLeft = 0;
                isThrown = false;
            }
            else
            {
                direction = Vector3.Reflect(direction.normalized, gameObject.transform.position.normalized);
                StayInBounds();
                currentSpeed -= (item.deacceleration * 2);
                hitsLeft--;
            }
        }
        else
        {
            if (collision.gameObject.tag == "Player" && collision == owner.GetComponent<Collider2D>())
            {
                owner.GetComponent<PlayerRangeItem>().throwableAmount[indexInList]++;
                Destroy(this.gameObject);
            }
        }
        
    }

    private void StayInBounds()
    {
        float newX = transform.position.x;
        float newY = transform.position.y;
        if (transform.position.x <= arena.bounds.min.x)
        {
            newX = arena.bounds.min.x;
            if(direction.x <= 0)
            {
                direction.x *= -1;
            }
        }
        else if (transform.position.x >= arena.bounds.max.x)
        {
            newX = arena.bounds.max.x;
            if (direction.x >= 0)
            {
                direction.x *= -1;
            }
        }

        if (transform.position.y <= arena.bounds.min.y)
        {
            newY = arena.bounds.min.y;
            if (direction.y <= 0)
            {
                direction.y *= -1;
            }
        }
        else if (transform.position.y >= arena.bounds.max.y)
        {
            newY = arena.bounds.max.y;
            if (direction.y >= 0)
            {
                direction.y *= -1;
            }
        }

        transform.position = new Vector2(newX, newY);
    }

}
