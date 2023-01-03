using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunUI : MonoBehaviour
{
    [SerializeField] private Text _magazineSizeText;
    [SerializeField] private Text _storedAmmotText;

    public void UpdateInfo(int magazineSize, int storedAmmo)
    {
        _magazineSizeText.text = magazineSize.ToString();
        _storedAmmotText.text = storedAmmo.ToString();
    }

    public void UpdateGunAmmoUI(int magazineSize, int storedAmmo)
    {
        _magazineSizeText.text = magazineSize.ToString();
        _storedAmmotText.text = storedAmmo.ToString();
    }

    public void EnableGunUI(bool enable)
    {
        _magazineSizeText.gameObject.SetActive(enable);
        _storedAmmotText.gameObject.SetActive(enable);
    }
}
