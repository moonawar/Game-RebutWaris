using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public class PlayerMashUIRefs {
    public GameObject fill;
    public Slider loveMeter;
    public GameObject heart;
}

public class PlayerMash : MonoBehaviour
{   
    private PhaseManager _phaseM;

    public int PlayerIdx => GetComponent<PlayerInput>().playerIndex;
    
    // Private properties
    private float _increaseRate;
    private float _baseValue = 0;
    private float _nextPhaseValue;
    private float _lastTimePrincessInArea = 0;

    private int _loveLevel = 0;
    
    /**
     * Context : clock is representation of time, and the children's time is what
     * every mother needs. Thus, the player must have the clock (time) to be able
     * to increase the love meter.
    */
    private bool haveClock = false;
    public bool HaveClock {
        get { return haveClock; }
        set { 
            OnHaveClockChanged(value);
        }
    }

    // UI Components
    [SerializeField] private GameObject _fill;
    [SerializeField] private Slider _loveMeter;
    [SerializeField] private GameObject _heart;

    public void InitUIs(PlayerMashUIRefs uiRefs) {
        _fill = uiRefs.fill;
        _loveMeter = uiRefs.loveMeter;
        _heart = uiRefs.heart;

        //_fill.GetComponent<Image>().color = Color.green;
        _fill.SetActive(false);
        _heart.GetComponent<Image>().color = Color.white;
    }

    public void DecreaseLoveHalf()
    {
        _loveMeter.value = _baseValue + (_loveMeter.value - _baseValue) / 2;
    }

    public void DecreaseLove(float value)
    {
        _loveMeter.value -= value;
    }

    private void Start() {
        _phaseM = PhaseManager.Instance;

        _nextPhaseValue = _phaseM.GamePhases[_loveLevel].limit;
        _increaseRate = _phaseM.GamePhases[_loveLevel].increase;
    }

    public void OnMashInput(InputAction.CallbackContext context) {
        if (!context.canceled) return; // We will only register on button pressed, not on releaserd
        if (_loveLevel >= 3) return;   // Max Level, avoid crashing at all cost
        if (gameObject.GetComponent<PlayerMovement>().IsStunned) return;   // Max Level, avoid crashing at all cost

        _heart.GetComponent<Animator>().SetTrigger("Mash");
        if (IsPrincessInArea())
        {
            _loveMeter.value += _increaseRate;
            _fill.SetActive(true);
        }

        if (_loveMeter.value >= _nextPhaseValue)
        {
            ChangePhase();
        }
    }

    private bool IsPrincessInArea()
    {
        Vector3 center = transform.position;
        center.x += _phaseM.DetectOffsetX;
        center.y += _phaseM.DetectOffsetY;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(center, _phaseM.DetectRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Emak")) return true; // Emak is here
        }
        return false;
    }

    void Update()
    {
        if (_loveLevel >= 3) return;

        if (IsPrincessInArea()) {
            _lastTimePrincessInArea = Time.time;
        }

        // If princess out of area long enough, and the love value is not at its lowest
        if (Time.time - _lastTimePrincessInArea >= _phaseM.TimeToStartDecreasing && _loveMeter.value >= _baseValue)
        {
            _loveMeter.value -= _phaseM.DecreaseRate * Time.deltaTime;
        }

        if (_loveMeter.value == 0) { _fill.SetActive(false); }
    }

    void ChangePhase()
    {
        _loveLevel++;

        if (_loveLevel >= 3) {
            _phaseM.OnPlayerAdvancePhase(PlayerIdx, _loveLevel);
            return;
        }
        _baseValue = _nextPhaseValue;
        _nextPhaseValue = _phaseM.GamePhases[_loveLevel].limit;
        _increaseRate = _phaseM.GamePhases[_loveLevel].increase;


        UpdateUI();
    }

    private void UpdateUI() {
        if (_loveLevel == 1)
        {
            _heart.SetActive(true);
            //_heart.GetComponent<Image>().color = Color.green;
            //_fill.GetComponent<Image>().color = Color.red;
        }
        else if (_loveLevel == 2)
        {
            _heart.SetActive(true);
            //_heart.GetComponent<Image>().color = Color.red;
            //_fill.GetComponent<Image>().color = new Color32(224, 55, 204, 255);
        }
        else // Level 3
        {
            _heart.SetActive(true);
            //_heart.GetComponent<Image>().color = new Color32(224, 55, 204, 255);
        }
    }

    private void OnDrawGizmosSelected() {
        Vector3 center = transform.position;
        center.x += _phaseM.DetectOffsetX;
        center.y += _phaseM.DetectOffsetY;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, _phaseM.DetectRadius);
    }

    public float GetIncreaseRate()
    {
        return _increaseRate;
    }

    public void SetIncreaseRate(float value)
    {
        _increaseRate = value;
    }

    public void OnHaveClockChanged(bool newValue)
    {
        if (HaveClock == newValue) return; // Nothing should be updated
    }
}