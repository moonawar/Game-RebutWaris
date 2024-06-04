using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingText : MonoBehaviour
{
    [SerializeField] private float _loopDuration = 5f;
    private TextMeshProUGUI _text;

    private void Awake() {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (Time.time % _loopDuration < _loopDuration / 4)
        {
            _text.text = "Loading";
        }
        else if (Time.time % _loopDuration < _loopDuration / 4 * 2)
        {
            _text.text = "Loading .";
        } 
        else if (Time.time % _loopDuration < _loopDuration / 4 * 3)
        {
            _text.text = "Loading . .";
        }
        else
        {
            _text.text = "Loading . . .";
        }
    }
}
