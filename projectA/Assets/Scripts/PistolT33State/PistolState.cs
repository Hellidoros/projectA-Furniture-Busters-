using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;
using UnityStandardAssets.Characters.FirstPerson;

public class PistolState
{
    protected FirstPersonController _fpc;
    protected HandStateManager _hsm;
    protected GunShooting _gs;

    public PistolState(FirstPersonController firstPersonController, HandStateManager handStateManager, GunShooting gunShooting)
    {
        _fpc = firstPersonController;
        _hsm = handStateManager;
        _gs = gunShooting;
    }

    virtual public PistolState HandleInputPistol()
    {
        return new PistolPickState(_fpc, _hsm, _gs);
    }
}

class PistolPickState : PistolState
{
    public PistolPickState(FirstPersonController firstPersonController, HandStateManager handStateManager, GunShooting gunShooting)
    : base(firstPersonController, handStateManager, gunShooting) { }

    override public PistolState HandleInputPistol()
    {
        return new PistolIdleState(_fpc, _hsm, _gs);
    }
}

class PistolIdleState : PistolState
{
    public PistolIdleState(FirstPersonController firstPersonController, HandStateManager handStateManager, GunShooting gunShooting)
    : base(firstPersonController, handStateManager, gunShooting) { }


    override public PistolState HandleInputPistol()
    {
        if (_fpc.IsWalking && _fpc.PlayerInput.magnitude > 0.1f)
        {
            return new PistolWalkState(_fpc, _hsm, _gs);
        }
        if (_fpc.PlayerInput.magnitude > 0.1f)
        {
            return new PistolRunState(_fpc, _hsm, _gs);
        }
        if (_hsm.MouseClicked && _gs.CanShoot && !_gs.FireRateFinished && !_gs.Reloading && _gs.ReloadFinished)
        {
            return new PistolShootState(_fpc, _hsm, _gs);
        }
        if (_gs.Reloading)
        {
            return new PistolReloadState(_fpc, _hsm, _gs);
        }

        return this;
    }
}

class PistolWalkState : PistolState
{
    public PistolWalkState(FirstPersonController firstPersonController, HandStateManager handStateManager, GunShooting gunShooting)
    : base(firstPersonController, handStateManager, gunShooting) { }


    override public PistolState HandleInputPistol()
    {
        _hsm.HandsAnimator.SetBool("isWalking", true);

        if (_fpc.PlayerInput.magnitude < 0.1f)
        {
            _hsm.HandsAnimator.SetBool("isWalking", false);
            return new PistolIdleState(_fpc, _hsm, _gs);
        }
        if (!_fpc.IsWalking && _fpc.PlayerInput.magnitude > 0.1f) //&& _hsm.Stamina.CurrentStamina > _hsm.Stamina.MinStamina)
        {
            _hsm.HandsAnimator.SetBool("isWalking", false);
            return new PistolRunState(_fpc, _hsm, _gs);
        }
        if (_hsm.MouseClicked && _gs.CanShoot && !_gs.FireRateFinished && !_gs.Reloading && _gs.ReloadFinished)
        {
            return new PistolShootState(_fpc, _hsm, _gs);
        }
        if (_gs.Reloading)
        {
            _hsm.HandsAnimator.SetBool("isWalking", false);
            return new PistolReloadState(_fpc, _hsm, _gs);
        }

        return this;
    }
}

class PistolRunState : PistolState
{
    public PistolRunState(FirstPersonController firstPersonController, HandStateManager handStateManager, GunShooting gunShooting)
    : base(firstPersonController, handStateManager, gunShooting) { }


    override public PistolState HandleInputPistol()
    {
        _hsm.HandsAnimator.SetBool("isRunning", true);

        if (_fpc.PlayerInput.magnitude < 0.1f)
        {
            _hsm.HandsAnimator.SetBool("isRunning", false);
            return new PistolIdleState(_fpc, _hsm, _gs);
        }
        if (_fpc.IsWalking && _fpc.PlayerInput.magnitude > 0.1f)
        {
            _hsm.HandsAnimator.SetBool("isRunning", false);
            return new PistolWalkState(_fpc, _hsm, _gs);
        }
        if (_hsm.MouseClicked && _gs.CanShoot && !_gs.FireRateFinished && !_gs.Reloading && _gs.ReloadFinished)
        {
            return new PistolShootState(_fpc, _hsm, _gs);
        }
        if (_gs.Reloading)
        {
            _hsm.HandsAnimator.SetBool("isRunning", false);
            return new PistolReloadState(_fpc, _hsm, _gs);
        }

        return this;
    }
}

class PistolShootState : PistolState
{
    public PistolShootState(FirstPersonController firstPersonController, HandStateManager handStateManager, GunShooting gunShooting)
    : base(firstPersonController, handStateManager, gunShooting) { }

    override public PistolState HandleInputPistol()
    {
        _hsm.HandsAnimator.SetTrigger("MouseClicked");

        if (_fpc.PlayerInput.magnitude < 0.1f)
        {
            return new PistolIdleState(_fpc, _hsm, _gs);
        }
        if (_fpc.IsWalking && _fpc.PlayerInput.magnitude > 0.1f)
        {
            return new PistolIdleState(_fpc, _hsm, _gs);
        }
        if (_fpc.IsWalking && _fpc.PlayerInput.magnitude > 0.1f)
        {
            _hsm.HandsAnimator.SetBool("isRunning", false);
            return new PistolWalkState(_fpc, _hsm, _gs);
        }
        if (!_fpc.IsWalking && _fpc.PlayerInput.magnitude > 0.1f) //&& _hsm.Stamina.CurrentStamina > _hsm.Stamina.MinStamina)
        {
            _hsm.HandsAnimator.SetBool("isWalking", false);
            return new PistolRunState(_fpc, _hsm, _gs);
        }
        if (_hsm.MouseClicked && _gs.CanShoot && !_gs.FireRateFinished && !_gs.Reloading && _gs.ReloadFinished)
        {
            return new PistolShootState(_fpc, _hsm, _gs);
        }
        if (_gs.Reloading)
        {
            _hsm.HandsAnimator.SetBool("isWalking", false);
            return new PistolReloadState(_fpc, _hsm, _gs);
        }

        return new PistolIdleState(_fpc, _hsm, _gs);
    }
}

class PistolReloadState : PistolState
{
    public PistolReloadState(FirstPersonController firstPersonController, HandStateManager handStateManager, GunShooting gunShooting)
    : base(firstPersonController, handStateManager, gunShooting) { }

    override public PistolState HandleInputPistol()
    {
        _hsm.HandsAnimator.SetBool("isReloading", true);

        if (_fpc.PlayerInput.magnitude < 0.1f && _gs.ReloadFinished)
        {
            _hsm.HandsAnimator.SetBool("isReloading", false);
            return new PistolIdleState(_fpc, _hsm, _gs);
        }
        if (_fpc.IsWalking && _fpc.PlayerInput.magnitude > 0.1f && _gs.ReloadFinished)
        {
            _hsm.HandsAnimator.SetBool("isReloading", false);
            return new PistolWalkState(_fpc, _hsm, _gs);
        }
        if (!_fpc.IsWalking && _fpc.PlayerInput.magnitude > 0.1f && _gs.ReloadFinished)
        {
            _hsm.HandsAnimator.SetBool("isReloading", false);
            return new PistolRunState(_fpc, _hsm, _gs);
        }

        return this;
    }
}