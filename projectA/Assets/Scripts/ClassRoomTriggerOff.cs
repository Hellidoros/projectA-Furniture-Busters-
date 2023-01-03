using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassRoomTriggerOff : MonoBehaviour
{
    public GameObject[] GameObjectsDisable;
    public GameObject[] GameObjectsEnable;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject gameObject in GameObjectsEnable)
            {
                if(gameObject != null)
                {
                    gameObject.SetActive(true);
                }
            }

            foreach (GameObject gameObject in GameObjectsDisable)
            {
                if(gameObject != null)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
