using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Gun _gun;
    [SerializeField] private GameObject _flashLight;
    [SerializeField] private GameObject _gps;
    [SerializeField] private GunUI _gunUI;

    private GunShooting _gunShooting;

    public Gun Gun { get { return _gun; } }
    public GameObject FlashLight { get { return _flashLight; } }
    public GameObject GPS { get { return _gps; } }

    private void Start()
    {
        _gunShooting = this.GetComponent<GunShooting>();
    }

    public void ChangeGun(Gun newGun)
    {
        _gun = newGun;
        _gunShooting.InitAmmo(_gun);
    }
    
}
