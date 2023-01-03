using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;
using UnityStandardAssets.Characters.FirstPerson;

public class ItemState
{
    protected FirstPersonController _fpc;
    protected HandStateManager _hsm;

    public ItemState(FirstPersonController firstPersonController, HandStateManager handStateManager)
    {
        _fpc = firstPersonController;
        _hsm = handStateManager;
    }

    virtual public ItemState HandleInput()
    {
        return this;
    }
}
