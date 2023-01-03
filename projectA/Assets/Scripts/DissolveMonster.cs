using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveMonster : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    public bool StartDissolve;
    private float _lerpNum;

    private void Update()
    {
        if (StartDissolve)
        {
            _renderer.material.SetFloat("_VertexAnimationIntensity", 1f);
            _lerpNum = Mathf.Lerp(_lerpNum, 1f, 0.009f);
            _renderer.material.SetFloat("_DissolveAmount", _lerpNum);
        }
    }
}
