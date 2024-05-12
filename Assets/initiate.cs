using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class initiate : MonoBehaviour
{

    [SerializeField] private GameplayInitiator gameplayInitiator;
    [SerializeField] private PlayerInput player1;
    [SerializeField] private PlayerInput player2;
    private void Awake()
    {
        gameplayInitiator.OnPlayerJoined(player1.gameObject, 0);
        gameplayInitiator.OnPlayerJoined(player2.gameObject, 1);
    }

    //private void Start()
    //{
    //    gameplayInitiator.OnPlayerJoined(player1.gameObject, 0);
    //    gameplayInitiator.OnPlayerJoined(player2.gameObject, 1);
    //}
}
