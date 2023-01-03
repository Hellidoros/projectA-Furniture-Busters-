using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Item", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite icon;
    public GameObject Prefab;
    public string AnimationBoolName;

    public virtual void Use()
    {
        Debug.Log(name + "was used.");
    }
    
}
