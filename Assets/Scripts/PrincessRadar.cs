using UnityEngine;
using UnityEngine.UI;

public class PrincessRadar : MonoBehaviour
{
    private bool isArea;
    private float increaseTime = 0;
    private float decreaseTime = 0;
    [SerializeField] public GameObject fill;
    [SerializeField] public Slider loveMeter;
    [SerializeField] public float increaseRate;
    [SerializeField] public float decreaseRate;
    [SerializeField] public int loveLevel = 0;
    [SerializeField] public GameObject heart;

    private void Start()
    {
        fill.GetComponent<Image>().color = Color.green;
        fill.SetActive(false);
        heart.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (loveLevel != 3)
        {
            if (isArea)
            {
                if (increaseTime == 0 || Time.time - increaseTime >= 0.1)
                {
                    fill.SetActive(true);
                    increaseTime = Time.time;
                    loveMeter.value += increaseRate;
                    Debug.Log($"Love {loveMeter.value}%");
                }
            }
            else
            {
                if (decreaseTime == 0 || Time.time - decreaseTime >= 0.1)
                {
                    decreaseTime = Time.time;
                    loveMeter.value -= decreaseRate;
                }

                if (loveMeter.value == 0) { fill.SetActive(false); }

            }

            if (loveMeter.value == 100)
            {
                changePhase();
            }
        }
        
    }

    void changePhase()
    {
        loveLevel++;

        if (loveLevel == 1)
        {
            heart.SetActive(true);
            heart.GetComponent<SpriteRenderer>().color = Color.green;
            fill.GetComponent<Image>().color = Color.red;
            increaseRate /= 1.5f;
            loveMeter.value = 0;
        }
        else if (loveLevel == 2)
        {
            heart.SetActive(true);
            heart.GetComponent<SpriteRenderer>().color = Color.red;
            fill.GetComponent<Image>().color = new Color32(224, 55, 204, 255);
            increaseRate /= 1.5f;
            loveMeter.value = 0;
        }
        else
        {
            heart.SetActive(true);
            heart.GetComponent<SpriteRenderer>().color = new Color32(224, 55, 204, 255);
        }

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "NPC")
        {
            decreaseTime = 0;
            Debug.Log("Princess in the area!");
            isArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "NPC")
        {
            increaseTime = 0;
            Debug.Log("Princess leaves the area!");
            isArea = false;
        }
    }

}
