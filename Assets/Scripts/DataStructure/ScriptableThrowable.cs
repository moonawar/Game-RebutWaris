using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThrowableItem", menuName = "Throwable/ThrowableItem")]
public class ScriptableThrowable : ScriptableObject
{
    public string name;
    public float damage;
    public int initialAmount;
    public float throwSpeed;
    public float deacceleration;
    public int hits;
    public Sprite sprite;
}
