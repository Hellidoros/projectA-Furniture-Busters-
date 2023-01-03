using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHidePosition : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _spriteRenderer.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _spriteRenderer.enabled = false;
        }
    }
}
