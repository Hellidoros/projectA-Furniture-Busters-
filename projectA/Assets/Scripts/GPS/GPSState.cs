using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;
using UnityStandardAssets.Characters.FirstPerson;

public class GPSState : ItemState
{
    public GPSState(FirstPersonController firstPersonController, HandStateManager handStateManager)
    : base(firstPersonController, handStateManager) { }

    virtual public GPSState HandleInputGPS()
    {
        return new GPSPickState(_fpc, _hsm);
    }
}

class GPSPickState : GPSState
{
    public GPSPickState(FirstPersonController firstPersonController, HandStateManager handStateManager)
    : base(firstPersonController, handStateManager) { }

    override public GPSState HandleInputGPS()
    {
        return new GPSIdleState(_fpc,_hsm);
    }
}

class GPSIdleState : GPSState
{
    public GPSIdleState(FirstPersonController firstPersonController, HandStateManager handStateManager)
    : base(firstPersonController, handStateManager) { }

    override public GPSState HandleInputGPS()
    {
        if (_hsm.MouseClicked)
        {
            return new GPSUseState(_fpc, _hsm);
        }
        if (_fpc.IsWalking && _fpc.PlayerInput.magnitude > 0.1f)
        {
            return new GPSWalkState(_fpc, _hsm);
        }
        if (_fpc.PlayerInput.magnitude > 0.1f)
        {
            return new GPSRunState(_fpc, _hsm);
        }

        return this;
    }
}

class GPSWalkState : GPSState
{
    public GPSWalkState(FirstPersonController firstPersonController, HandStateManager handStateManager)
    : base(firstPersonController, handStateManager) { }

    override public GPSState HandleInputGPS()
    {
        _hsm.HandsAnimator.SetBool("isWalking", true);


        if (_hsm.MouseClicked)
        {
            return new GPSUseState(_fpc, _hsm);
        }
        if (_fpc.PlayerInput.magnitude < 0.1f)
        {
            _hsm.HandsAnimator.SetBool("isWalking", false);
            return new GPSIdleState(_fpc, _hsm);
        }
        if (!_fpc.IsWalking && _fpc.PlayerInput.magnitude > 0.1f) //&& _hsm.Stamina.CurrentStamina > _hsm.Stamina.MinStamina)
        {
            _hsm.HandsAnimator.SetBool("isWalking", false);
            return new GPSRunState(_fpc, _hsm);
        }

        return this;
    }
}

class GPSRunState : GPSState
{
    public GPSRunState(FirstPersonController firstPersonController, HandStateManager handStateManager)
    : base(firstPersonController, handStateManager) { }

    override public GPSState HandleInputGPS()
    {
        _hsm.HandsAnimator.SetBool("isRunning", true);

        if (_hsm.MouseClicked)
        {
            return new GPSUseState(_fpc, _hsm);
        }
        if (_fpc.PlayerInput.magnitude < 0.1f)
        {
            _hsm.HandsAnimator.SetBool("isRunning", false);
            return new GPSIdleState(_fpc, _hsm);
        }
        if (_fpc.IsWalking && _fpc.PlayerInput.magnitude > 0.1f)
        {
            _hsm.HandsAnimator.SetBool("isRunning", false);
            return new GPSWalkState(_fpc, _hsm);
        }

        return this;
    }
}

class GPSUseState : GPSState
{
    public GPSUseState(FirstPersonController firstPersonController, HandStateManager handStateManager)
   : base(firstPersonController, handStateManager) { }

    override public GPSState HandleInputGPS()
    {
        _hsm.HandsAnimator.SetTrigger("MouseClicked");

        if (_fpc.PlayerInput.magnitude < 0.1f)
        {
            return new GPSIdleState(_fpc, _hsm);
        }

        if (_fpc.IsWalking && _fpc.PlayerInput.magnitude > 0.1f)
        {
            return new GPSIdleState(_fpc, _hsm);
        }

        return new GPSIdleState(_fpc, _hsm);
    }
}

class GPSRemoveState : GPSState
{
    public GPSRemoveState(FirstPersonController firstPersonController, HandStateManager handStateManager)
    : base(firstPersonController, handStateManager) { }

    override public GPSState HandleInputGPS()
    {
        return this;
    }
}

