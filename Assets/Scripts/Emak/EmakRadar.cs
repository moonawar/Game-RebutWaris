using UnityEngine;
using UnityEngine.UI;


public class EmakRadar : MonoBehaviour
{   
    [Header("Detection")]
    [SerializeField] private float radius = 4f;

    [Header("Phase Properties")]
    [SerializeField] private float[] phaseLimits = new float[3];
    [SerializeField] private float[] phaseIncrease = new float[3];
    [SerializeField] private Phase[] phases = new Phase[3];
    private float increaseRate;
    [SerializeField] private float decreaseRate;
    private float decreaseTime = 0;
    private float nextPhaseValue;
    private float baseValue = 0;
    private int loveLevel = 0;

    // UI Components
    private GamePlayerInput input;
    private GameObject fill;
    private Slider loveMeter;
    private GameObject heart;

    public void InitUIs(InGamePlayerUIRefs uiRefs) {
        fill = uiRefs.fill;
        loveMeter = uiRefs.loveMeter;
        heart = uiRefs.heart;

        fill.GetComponent<Image>().color = Color.green;
        fill.SetActive(false);
        heart.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void Awake()
    {
        nextPhaseValue = phaseLimits[loveLevel];
        increaseRate = phaseIncrease[loveLevel];
    }

    private bool isPrincessInArea()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "NPC") return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {

        if (loveLevel != 3)
        {
            if (isPrincessInArea())
            {
                if (input.MashInput)
                {
                    fill.SetActive(true);
                    //increaseTime = Time.time;
                    loveMeter.value += increaseRate;
                }

                //if (increaseTime == 0 || Time.time - increaseTime >= 0.1)
                //{
                //    fill.SetActive(true);
                //    increaseTime = Time.time;
                //    loveMeter.value += increaseRate;
                //}
            }
            else
            {
                if ((decreaseTime == 0 || Time.time - decreaseTime >= 0.1) && loveMeter.value >= baseValue)
                {
                    decreaseTime = Time.time;
                    loveMeter.value -= decreaseRate;
                }

                if (loveMeter.value == 0) { fill.SetActive(false); }

            }

            if (loveMeter.value >= nextPhaseValue)
            {
                ChangePhase();
                baseValue = nextPhaseValue;
                nextPhaseValue = phaseLimits[loveLevel];
                increaseRate = phaseIncrease[loveLevel];

            }
        }
        
    }

    void ChangePhase()
    {
        loveLevel++;

        if (loveLevel == 1)
        {
            heart.SetActive(true);
            heart.GetComponent<SpriteRenderer>().color = Color.green;
            fill.GetComponent<Image>().color = Color.red;
        }
        else if (loveLevel == 2)
        {
            heart.SetActive(true);
            heart.GetComponent<SpriteRenderer>().color = Color.red;
            fill.GetComponent<Image>().color = new Color32(224, 55, 204, 255);
        }
        else
        {
            heart.SetActive(true);
            heart.GetComponent<SpriteRenderer>().color = new Color32(224, 55, 204, 255);
        }



    }
}
