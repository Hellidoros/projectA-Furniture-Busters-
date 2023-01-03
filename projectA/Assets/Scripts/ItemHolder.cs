using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;
using System;

public class ItemHolder : MonoBehaviour
{
    public Item Item;
    private string ItemName;
    private GameObject ItemHold;
    [SerializeField] private bool _dontSaveItem;


    public void OnDestroy()
    {
        SavePlayerProgress.SaveEvent -= SaveCurrentProgress;
        SavePlayerProgress.GetSavedProgressEvent -= GetSavedReferences;
    }

    public void Awake()
    {
        SavePlayerProgress.SaveEvent += SaveCurrentProgress;
        SavePlayerProgress.GetSavedProgressEvent += GetSavedReferences;

        ItemHold = GameObject.Find("ItemHold");
        ItemName = Item.name;
    }

    public void SaveCurrentProgress()
    {
        if (!_dontSaveItem)
        {
            var writer = QuickSaveWriter.Create("Item" + ItemName);
            writer.Write("Position", this.transform.position);
            if (this.transform.parent != null)
            {
                if (this.gameObject.transform.parent.name == "ItemHold")
                {
                    writer.Write("IsParent", true);
                }
                else
                {
                    writer.Write("IsParent", false);
                }
            }
            else
            {
                writer.Write("IsParent", false);
            }
            writer.Write("IsActive", this.gameObject.activeSelf);
            writer.Commit();
        }
    }

    private void GetSavedReferences()
    {
        if (!_dontSaveItem)
        {
            var reader = QuickSaveReader.Create("Item" + ItemName);

            if (reader.Read<bool>("IsParent") == true)
            {
                this.gameObject.transform.SetParent(ItemHold.transform);
                this.gameObject.transform.position = ItemHold.transform.position;
            }
            else
            {
                this.gameObject.transform.position = reader.Read<Vector3>("Position");
            }

            this.gameObject.SetActive(reader.Read<bool>("IsActive"));
        }
    }

    public void EnableGravity()
    {
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<Rigidbody>().useGravity = true;
    }
}
