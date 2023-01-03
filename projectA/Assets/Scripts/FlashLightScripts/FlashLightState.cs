using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;
using UnityStandardAssets.Characters.FirstPerson;


public class FlashLightState : ItemState
{
    public FlashLightState(FirstPersonController firstPersonController, HandStateManager handStateManager)
    : base(firstPersonController, handStateManager) { }

    virtual public FlashLightState HandleInputFlash()
    {
        return new FlashLightPickState(_fpc, _hsm);
    }
}

class FlashLightPickState : FlashLightState
{
    public FlashLightPickState(FirstPersonController firstPersonController, HandStateManager handStateManager)
    : base(firstPersonController, handStateManager) { }

    override public FlashLightState HandleInputFlash()
    {
        return new FlashLightIdleState(_fpc,_hsm);
    }
}

class FlashLightIdleState : FlashLightState
{
    public FlashLightIdleState(FirstPersonController firstPersonController, HandStateManager handStateManager)
    : base(firstPersonController, handStateManager) { }


    override public FlashLightState HandleInputFlash()
    {
        if (_fpc.IsWalking && _fpc.PlayerInput.magnitude > 0.1f)
        {
            return new FlashLightWalkState(_fpc, _hsm);
        }
        if (_fpc.PlayerInput.magnitude > 0.1f)
        {
            return new FlashLightRunState(_fpc, _hsm);
        }
        if (!_hsm.FlashLightEnabled)
        {
            return new FlashLightRemoveState(_fpc, _hsm);
        }

        return this;
    }
}

class FlashLightWalkState : FlashLightState
{
    public FlashLightWalkState(FirstPersonController firstPersonController, HandStateManager handStateManager)
    : base(firstPersonController, handStateManager) { }


    override public FlashLightState HandleInputFlash()
    {
        _hsm.HandsAnimator.SetBool("isWalking",true);

        if (_fpc.PlayerInput.magnitude < 0.1f)
        {
            _hsm.HandsAnimator.SetBool("isWalking", false);
            return new FlashLightIdleState(_fpc, _hsm);
        }
        if (!_fpc.IsWalking && _fpc.PlayerInput.magnitude > 0.1f) //&& _hsm.Stamina.CurrentStamina > _hsm.Stamina.MinStamina)
        {
            _hsm.HandsAnimator.SetBool("isWalking", false);
            return new FlashLightRunState(_fpc, _hsm);
        }
        if (!_hsm.FlashLightEnabled)
        {
            _hsm.HandsAnimator.SetBool("isWalking", false);
            return new FlashLightRemoveState(_fpc, _hsm);
        }

        return this;
    }
}

class FlashLightRunState : FlashLightState
{
    public FlashLightRunState(FirstPersonController firstPersonController, HandStateManager handStateManager)
    : base(firstPersonController, handStateManager) { }


    override public FlashLightState HandleInputFlash()
    {
        _hsm.HandsAnimator.SetBool("isRunning", true);

        if (_fpc.PlayerInput.magnitude < 0.1f)
        {
            _hsm.HandsAnimator.SetBool("isRunning", false);
            return new FlashLightIdleState(_fpc, _hsm);
        }
        if (_fpc.IsWalking && _fpc.PlayerInput.magnitude > 0.1f)
        {
            _hsm.HandsAnimator.SetBool("isRunning", false);
            return new FlashLightWalkState(_fpc, _hsm);
        }
        if (!_hsm.FlashLightEnabled)
        {
            _hsm.HandsAnimator.SetBool("isRunning", false);
            return new FlashLightRemoveState(_fpc, _hsm);
        }

        return this;
    }
}

class FlashLightRemoveState : FlashLightState
{
    public FlashLightRemoveState(FirstPersonController firstPersonController, HandStateManager handStateManager)
    : base(firstPersonController, handStateManager) { }

    override public FlashLightState HandleInputFlash()
    {
        Debug.Log("FlashLightExited");
        return null;
    }
}





