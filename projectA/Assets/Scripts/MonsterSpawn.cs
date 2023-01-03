using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CI.QuickSave;

public class MonsterSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _clockMonster;
    [SerializeField] private GameObject _sofaMonster;

    private int _randomNumber;
    [SerializeField]private int _lastNumber;

    private void NewRandomNumber()
    {
        _randomNumber = Random.Range(0,3);
        if (_randomNumber == _lastNumber)
        {
            _randomNumber = Random.Range(0,3);
        }
        _lastNumber = _randomNumber;
    }


    private void Start()
    {
        if (ManageCurrentScene.CurrentSceneSave == 1)
        {

        }
    }

}
