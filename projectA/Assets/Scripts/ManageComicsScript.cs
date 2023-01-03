using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageComicsScript : MonoBehaviour
{
    public Animator[] Animators;
    public NodeParser NodeParser;
    public GameObject Transition;

    public GameObject[] DisableGameobject;

    public void Start()
    {
        StartCoroutine(ManageComics());
    }

    public IEnumerator ManageComics()
    {
        Animators[0].SetBool("Start", true);
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => Input.GetMouseButton(0));

        Animators[1].SetBool("Start", true);
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => Input.GetMouseButton(0));

        Animators[2].SetBool("Start", true);
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => Input.GetMouseButton(0));

        Animators[3].SetBool("Start", true);
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => Input.GetMouseButton(0));

        Animators[4].SetBool("Start", true);
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => Input.GetMouseButton(0));

        Animators[5].SetBool("Start", true);
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => Input.GetMouseButton(0));

        yield return new WaitForSeconds(3f);

        foreach(GameObject gameObject in DisableGameobject)
        {
            gameObject.SetActive(false);
        }

        Transition.SetActive(true);
        yield return new WaitForSeconds(2f);
        NodeParser.StartDialogue();
    }

}
