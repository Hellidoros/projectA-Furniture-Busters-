using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDestroyScript : MonoBehaviour
{
    public GameObject[] GameObjectsDestroy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Yurei_static")
        {
            DestroyWall();
        }
    }

    public void DestroyWall()
    {
        foreach (GameObject gameObject in GameObjectsDestroy)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
