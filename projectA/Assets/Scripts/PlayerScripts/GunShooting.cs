using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;

public class GunShooting : MonoBehaviour
{
    [SerializeField] private HandStateManager _handStateManager;
    private Inventory _inventory;
    private Camera _camera;
    private ParticleSystem _muzzleFlash;
    private LineRenderer _lineRenderer;

    private float _lashShootTime = 0;

    [SerializeField] private bool _canShoot;
    [SerializeField] private LayerMask _monsterLayerMask;
    [SerializeField] private int _currentAmmo;
    [SerializeField] private int _currentAmmoStorage;
    [SerializeField] private GunUI _gunUI;

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private AudioClip _reloadSound;
    [SerializeField] private AudioClip _noAmmoSound;

    private bool _magazineIsEmpty = false;

    public bool Reloading;
    public bool ReloadFinished;
    public bool FireRateFinished = true;
    public bool CanShoot { get { return _canShoot; } }
    public int CurrentAmmo { get { return _currentAmmo; } }
    public int CurrentAmmoStorage { get { return _currentAmmoStorage; } }

    private void Start()
    {
        SavePlayerProgress.SaveEvent += SaveCurrentProgress;

        ReloadFinished = true;
        _canShoot = true;
        _inventory = this.GetComponent<Inventory>();
        _camera = GetComponentInChildren<Camera>();

        _currentAmmo = _inventory.Gun.MagazineSize;
        _currentAmmoStorage = _inventory.Gun.StoredAmmo;
        _gunUI.UpdateGunAmmoUI(_inventory.Gun.MagazineSize, _inventory.Gun.StoredAmmo);
    }

    private void Update()
    {
        if (_canShoot)
        {
            if (!Reloading && ReloadFinished)
            {
                if (Input.GetMouseButtonDown(0) && _handStateManager.GunEnabled)
                {
                    Shoot();
                }
            }
            else
            {
                _handStateManager.HandsAnimator.ResetTrigger("MouseClicked");
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    private void RaycastShoot(Gun currentGun)
    {
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        float currentGunRange = currentGun.Range;

        if (Physics.Raycast(ray, out hit,currentGunRange, _monsterLayerMask))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.GetComponent<Monster>())
            {
                hit.transform.GetComponent<Monster>().GetStunned();
            }
        }
    }

    private void Shoot()
    {
        CheckCanShoot();

        if (_canShoot && ReloadFinished)
        {
            Gun currentGun = _inventory.Gun;

            if (Time.time > _lashShootTime + currentGun.FireRate)
            {
                FireRateFinished = false;
                Debug.Log("Shoot");
                _lashShootTime = Time.time;

                RaycastShoot(currentGun);
                UseAmmo(1, 0);

                if (_muzzleFlash != null)
                {
                    _muzzleFlash.Play();
                    StartCoroutine(LaserShoot());
                }
                if(_audioSource != null)
                {
                    _audioSource.clip = _shootSound;
                    _audioSource.Play();
                }
            }
            else
            {
                FireRateFinished = true;
            }
        }
    }

    IEnumerator LaserShoot()
    {
        _lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.1f);
        _lineRenderer.enabled = false;
    }

    private void AddAmmo(int currentAmmoAdded, int currentStoredAmmoAdded)
    {
        _currentAmmo += currentAmmoAdded;
        _currentAmmoStorage += currentStoredAmmoAdded;
        _gunUI.UpdateGunAmmoUI(_currentAmmo, _currentAmmoStorage);
    }

    private void Reload()
    {
        //if have enough ammo
        if (_currentAmmoStorage >= _inventory.Gun.MagazineSize)
        {
            int ammoToReload = _inventory.Gun.MagazineSize - _currentAmmo;

            //if magazine is full
            if (_currentAmmo == _inventory.Gun.MagazineSize)
                Reloading = false;
            else
            {
                Reloading = true;
                ReloadFinished = false;
            }


            AddAmmo(ammoToReload, 0);
            UseAmmo(0, ammoToReload);

            _magazineIsEmpty = false;
        }
        else
        {

        }
    }

    public void InitAmmo(Gun gun)
    {
        _currentAmmo = gun.MagazineSize;
        _currentAmmoStorage = gun.StoredAmmo;
    }

    private void CheckCanShoot()
    {
        _muzzleFlash = _handStateManager.GetComponentInChildren<ParticleSystem>();
        _lineRenderer = _handStateManager.GetComponentInChildren<LineRenderer>();


        if (_currentAmmo == 0)
        {
            _magazineIsEmpty = true;
            _canShoot = false;
            Reload();
        }

        if (_magazineIsEmpty)
            _canShoot = false;
        else
            _canShoot = true;

        //if ()
    }

    private void UseAmmo(int currentAmmoUsed, int currentStoredAmmo)
    {
        if(_currentAmmo <= 0)
        {
            _magazineIsEmpty = true;
            CheckCanShoot();
        }
        else
        {
            _currentAmmo -= currentAmmoUsed;
            _currentAmmoStorage -= currentStoredAmmo;
            _gunUI.UpdateGunAmmoUI(_currentAmmo, _currentAmmoStorage);
            CheckCanShoot();
        }
    }

    private void SaveCurrentProgress()
    {
        
    }

    private void GetSavedReferences()
    {
        var reader = QuickSaveReader.Create("Player");
        _currentAmmo = reader.Read<int>("CurrentAmmo");
        _currentAmmoStorage = reader.Read<int>("CurrentAmmoStorage");

        _gunUI.UpdateGunAmmoUI(reader.Read<int>("CurrentAmmo"), reader.Read<int>("CurrentAmmoStorage"));
    }
}
