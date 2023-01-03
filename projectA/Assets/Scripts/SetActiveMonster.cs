using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveMonster : MonoBehaviour
{
    [SerializeField] private GameObject _monster;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _monster.SetActive(true);
        }
    }
}
