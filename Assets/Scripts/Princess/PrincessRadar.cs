using UnityEngine;
using UnityEngine.UI;

public class PrincessRadar : MonoBehaviour
{   
    private float decreaseTime = 0;
    private float nextPhaseValue;
    private float baseValue = 0;
    private int loveLevel = 0;
    [SerializeField] private float[] PhaseLimits = new float[3];
    [SerializeField] private float[] PhaseIncrease = new float[3];
    [SerializeField] private float radius = 4f;
    [SerializeField] private GamePlayerInput input;
    [SerializeField] private GameObject fill;
    [SerializeField] private Slider loveMeter;
    private float increaseRate;
    [SerializeField] private float decreaseRate;
    [SerializeField] private GameObject heart;

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
        nextPhaseValue = PhaseLimits[loveLevel];
        increaseRate = PhaseIncrease[loveLevel];
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
                nextPhaseValue = PhaseLimits[loveLevel];
                increaseRate = PhaseIncrease[loveLevel];

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
