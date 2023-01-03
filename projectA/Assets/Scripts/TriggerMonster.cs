using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMonster : MonoBehaviour
{
    [SerializeField] private GameObject _monster;
    [SerializeField] private GameObject _monsterTrigger;
    [SerializeField] private AudioSource _audioSourceAmbient;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RenderSettings.fogDensity = 0.15f;
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            _monster.SetActive(true);
            _monsterTrigger.SetActive(true);
            _audioSourceAmbient.Stop();
        }
    }
}
