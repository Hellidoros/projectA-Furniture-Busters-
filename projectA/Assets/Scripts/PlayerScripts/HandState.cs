using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;
using UnityStandardAssets.Characters.FirstPerson;

public class HandState : MonoBehaviour
{
    protected FirstPersonController _ctx;

    public HandState(FirstPersonController currentContext)
    {
        _ctx = currentContext;
    }

    virtual public HandState HandleInput()
    {
        return this;
    }
}

class GunState : HandState
{
    public GunState(FirstPersonController currentContext)
    : base(currentContext) { }

    public override HandState HandleInput()
    {
        return this;
    }
}