using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new Enemy")]
public class Enemy : ScriptableObject
{
    public float JumpScareSight;
    public float ChaseSpeed;
    public float SimpleSpeed;

    public float HardModeChaseSpeed;
    public float EasyModeChaseSpeed;

}
