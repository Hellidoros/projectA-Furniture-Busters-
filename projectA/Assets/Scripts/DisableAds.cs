using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using CI.QuickSave;
using UnityEngine.UI;

public class DisableAds : MonoBehaviour
{
    [SerializeField] private Image _removeAdsButton;
    public static bool DontShowAds = false;

    private bool _checkForRestore = false;

    private void Awake()
    {
        GetSavedReferences();
    }

    public void DisableButton()
    {
        if (NeutralAgeScreenManager.DisableProduct)
        {
            _removeAdsButton.enabled = false;
        }
    }

    private void Start()
    {
        if (!_checkForRestore)
        {
            _checkForRestore = true;
            RestoreProducts();
        }

        DisableButton();
    }

    private void OnEnable()
    {
        if (NeutralAgeScreenManager.DisableProduct)
        {
            _removeAdsButton.enabled = false;
        }
    }

    public void SaveCurrentProgress()
    {
        var writer = QuickSaveWriter.Create("Ads");

        writer.Write("ShowAds", DontShowAds);
        writer.Write("Restore", _checkForRestore);
        writer.Commit();
    }

    public void RestoreProducts()
    {
        if (CodelessIAPStoreListener.Instance.StoreController.products.WithID("com.noctum.furniturebusters.noads").hasReceipt)
        {
            DontShowAds = true;
        }
        SaveCurrentProgress();
    }

    public void GetSavedReferences()
    {
        var reader = QuickSaveReader.Create("Ads");

        if (reader.Exists("ShowAds"))
        {
            DontShowAds = reader.Read<bool>("ShowAds");

            if (DontShowAds)
            {
                _removeAdsButton.enabled = false;
            }
        }
        if (reader.Exists("Restore"))
        {
            _checkForRestore = reader.Read<bool>("Restore");
        }
    }

    public void DisableAllAds()
    {
        DontShowAds = true;
        SaveCurrentProgress();
    }
}
