using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Camera _theCam;

    private void Start()
    {
        _theCam = Camera.main;
    }

    private void LateUpdate()
    {
        transform.LookAt(_theCam.transform);

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}
