using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;

public class WoodenWallScript : MonoBehaviour
{
    [SerializeField] private HandStateManager _handStateManager;
    [SerializeField] private bool _mustSave;
    private bool _isDestroyed = false;
    public GameObject[] WoodenGameObjects;

    private void Start()
    {
        if (_mustSave)
        {
            SavePlayerProgress.SaveEvent += SaveCurrentProgress;
            SavePlayerProgress.GetSavedProgressEvent += GetSavedReferences;
        }
    }


    public void OnDestroy()
    {
        if (_mustSave)
        {
            SavePlayerProgress.SaveEvent -= SaveCurrentProgress;
            SavePlayerProgress.GetSavedProgressEvent -= GetSavedReferences;
        }
    }

    public void SaveCurrentProgress()
    {
        if (_mustSave)
        {
            var writer = QuickSaveWriter.Create("WoodBlocks");

            writer.Write("Bool", _isDestroyed);
            writer.Commit();
        }
    }

    public void GetSavedReferences()
    {
        if (_mustSave)
        {
            var reader = QuickSaveReader.Create("WoodBlocks");

            _isDestroyed = reader.Read<bool>("Bool");

            if (_isDestroyed)
            {
                StartCoroutine(Timer());
            }
        }
    }

    public void DestroyWoodenWall()
    {
        if (_handStateManager.CurrentItem != null)
        {
            if (_handStateManager.CurrentItem.Name == "Axe")
            {
                _isDestroyed = true;
                _handStateManager.HandsAnimator.SetTrigger("MouseClicked");
                SaveCurrentProgress();
                StartCoroutine(Timer());
            }
        }
    }

    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.3f);
        foreach (GameObject gameObject in WoodenGameObjects)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Rigidbody>().useGravity = true;
        }

        this.gameObject.layer = LayerMask.NameToLayer("Default");
        this.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(1f);
        foreach (GameObject gameObject in WoodenGameObjects)
        {
            gameObject.GetComponent<Collider>().enabled = false;
        }
        yield return new WaitForSeconds(2f);
        foreach (GameObject gameObject in WoodenGameObjects)
        {
            Destroy(gameObject);
        }
    }
}
