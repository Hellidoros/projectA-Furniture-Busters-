using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrangePersonScript : MonoBehaviour
{
    [SerializeField] private Sprite _normalSprite;
    [SerializeField] private Sprite _scarySprite;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _spriteRenderer.sprite = _scarySprite;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _spriteRenderer.sprite = _normalSprite;
        }
    }
}
