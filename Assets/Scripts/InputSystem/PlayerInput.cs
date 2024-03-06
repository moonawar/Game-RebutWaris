using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInput", menuName = "Input/PlayerInput")]
class PlayerInput : ScriptableObject
{
    [Header("Movement Keys")]
    public KeyCode Up;
    public KeyCode Down;
    public KeyCode Left;
    public KeyCode Right;
    
    [Header("Skill Keys")]
    public KeyCode Skill;

    public Vector2 MoveInput => new Vector2(
        Convert.ToInt32(Input.GetKey(Right)) - Convert.ToInt32(Input.GetKey(Left)),
        Convert.ToInt32(Input.GetKey(Up)) - Convert.ToInt32(Input.GetKey(Down))
    );
}
