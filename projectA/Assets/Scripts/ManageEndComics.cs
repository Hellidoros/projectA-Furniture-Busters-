using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageEndComics : MonoBehaviour
{

    public Animator[] Animators;
    public GameObject[] GameObjectsToDestroy;
    [SerializeField] private GameObject _endFrame;

    private void Start()
    {
        StartCoroutine(ManageComics());
    }

    private void OnEnable()
    {
        foreach (GameObject gameObject in GameObjectsToDestroy)
        {
            if(gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }

    public IEnumerator ManageComics()
    {
        yield return new WaitForSeconds(2f);
        Animators[0].SetBool("Start", true);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Input.GetMouseButton(0));

        Animators[1].SetBool("Start", true);
        yield return new WaitForSeconds(0.7f);
        yield return new WaitUntil(() => Input.GetMouseButton(0));

        Animators[2].SetBool("Start", true);
        yield return new WaitForSeconds(0.7f);
        yield return new WaitUntil(() => Input.GetMouseButton(0));

        Animators[3].SetBool("Start", true);
        yield return new WaitForSeconds(0.7f);
        yield return new WaitUntil(() => Input.GetMouseButton(0));

        yield return new WaitForSeconds(3f);
        _endFrame.SetActive(true);
    }
}
