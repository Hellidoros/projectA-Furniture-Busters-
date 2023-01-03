using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerManager : MonoBehaviour
{
    [SerializeField] private Animator _lockedAnimator;
    [SerializeField] private bool _isOpen;
    [SerializeField] private string _boolName;
    private AnimatorControllerParameter[] parameters;

    public void ManageLocker()
    {
        _isOpen = !_isOpen;

        foreach (AnimatorControllerParameter parameter in _lockedAnimator.parameters)
        {
            _lockedAnimator.SetBool(parameter.name, false);
        }

        _lockedAnimator.SetBool(_boolName, _isOpen);
    }
}
