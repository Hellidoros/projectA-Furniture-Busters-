using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassRoomTriggerOn : MonoBehaviour
{
    public GameObject[] ClassRoomEnable;
    public GameObject[] GameObjecstDisable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach(GameObject gameObject in GameObjecstDisable)
            {
                if(gameObject != null)
                {
                    gameObject.SetActive(false);
                }
            }

            foreach (GameObject gameObject in ClassRoomEnable)
            {
                if(gameObject != null)
                {
                    gameObject.SetActive(true);
                }
            }

        }
    }
}
