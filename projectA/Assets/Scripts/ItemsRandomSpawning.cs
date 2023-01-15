using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;

public class ItemsRandomSpawning : MonoBehaviour
{
    [SerializeField] private Vector3 _position1;
    [SerializeField] private Vector3 _position2;
    [SerializeField] private Vector3 _position3;

    private int _randomNum;
    private bool _isSetRandomPosition = false;

    private void Start()
    {
        if (DiffucultyLevelScript.IsRandomItemSpawning)
        {
            GetSavedReferences();

            if (!_isSetRandomPosition)
            {
                _isSetRandomPosition = true;

                _randomNum = Random.Range(1, 3);
                Debug.Log(_randomNum);

                if (_randomNum == 1)
                {
                    this.gameObject.transform.position = _position1;
                }
                else if (_randomNum == 2)
                {
                    this.gameObject.transform.position = _position2;
                }
                else if (_randomNum == 3)
                {
                    this.gameObject.transform.position = _position3;
                }
                SaveCurrentProgress();
            }
        }
    }

    private void SaveCurrentProgress()
    {
        var writer = QuickSaveWriter.Create("ItemRandomSpawning" + this.gameObject.name);
        writer.Write("IsSetRandomPosition", _isSetRandomPosition);

        writer.Commit();
    }

    private void GetSavedReferences()
    {
        try
        {
            var reader = QuickSaveReader.Create("ItemRandomSpawning" + this.gameObject.name);
            if (reader.Exists("IsSetRandomPosition"))
            {
                _isSetRandomPosition = reader.Read<bool>("IsSetRandomPosition");
            }
        }
        catch
        {
            Debug.Log("Root does not exist");
        }
    }
}
