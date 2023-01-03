using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class MobileInputScript : MonoBehaviour
{
    public FixedTouchField FixedTouchField;

    private void Update()
    {
        var fps = GetComponent<FirstPersonController>();

        fps.m_MouseLook.LookAxis = FixedTouchField.TouchDist;
    }
}
