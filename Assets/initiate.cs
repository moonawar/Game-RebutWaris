using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Initiate : MonoBehaviour
{
    public static Initiate Instance { get; private set; }


    [SerializeField] private GameplayInitiator gameplayInitiator;
    [SerializeField] private PlayerMovement NPC;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void ConnectNPC()
    {

        gameplayInitiator.OnPlayerJoined(NPC.gameObject, 1);
        NPC.gameObject.SetActive(true);
        print("INITIATED");
    }
}
