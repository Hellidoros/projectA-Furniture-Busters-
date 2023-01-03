using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new Gun", menuName = "Items/Gun")]
public class Gun : Item
{
    public int MagazineSize;
    public int StoredAmmo;
    public float FireRate;
    public float Range;
    public string Animation;
}
