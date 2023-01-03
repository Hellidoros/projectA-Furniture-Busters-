using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public UnityEvent OnInteract;
    public int ID;
    public Sprite InteractableIcon;
    public Sprite ButtonIconSprite;
    public string Name;

    private void Start()
    {
        ID = Random.Range(0, 999999);
    }
}
