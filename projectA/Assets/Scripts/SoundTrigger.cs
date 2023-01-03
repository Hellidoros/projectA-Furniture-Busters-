using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSourceSound;
    [SerializeField] private AudioSource _audioSourceAmbient;
    public GameObject[] GameObjectsToEnable;
    public GameObject[] GameObjectsToDestroy;

    [SerializeField] private GameObject _monstertrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _audioSourceSound.Play();
            foreach(GameObject gameObject in GameObjectsToEnable)
            {
                gameObject.SetActive(true);
            }
            foreach(GameObject gameObject in GameObjectsToDestroy)
            {
                Destroy(gameObject);
            }
            this.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(ScarySceneCoroutine());
        }
    }

    public IEnumerator ScarySceneCoroutine()
    {
        yield return new WaitForSeconds(2f);
        _monstertrigger.SetActive(true);
        _audioSourceAmbient.Play();
    }
}
