using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TaskManager;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    public TextMeshProUGUI dialogue;
    public AudioSource audioSource;
    public Animator InventoryAnimator;
    public PlayerHealth PlayerHealth;

    public GameObject VCam;
    public GameObject LightGameObject;
    public Transform DoorTrasnform;
    public NodeParser NodeParser;

    public bool StartDialogueBool;

    Task _typeSentence;
    Task _startTutorial;


    public void Start()
    {
        _startTutorial = new Task(StartTutorial());
    }

    public void NextScene()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }


    IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(2f);
        _typeSentence = new Task(TypeSentence("Press WASD to move"));
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D));


        _typeSentence = new Task(TypeSentence("Press SPACE to jump"));
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => Input.GetKey(KeyCode.Space));


        _typeSentence = new Task(TypeSentence("Press E to equip/interact"));
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => Input.GetKey(KeyCode.E));


        _typeSentence = new Task(TypeSentence("Press Q to drop"));
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => Input.GetKey(KeyCode.Q));


        _typeSentence = new Task(TypeSentence("You also have inventory"));
        InventoryAnimator.SetTrigger("Open");

        yield return new WaitForSeconds(3f);
        _typeSentence = new Task(TypeSentence(""));



        //Waits until he fulls bag
        yield return new WaitUntil(() => StartDialogueBool);


        yield return new WaitForSeconds(2f);
        RenderSettings.fog = true;
        RenderSettings.fogDensity = 0.2f;
        _typeSentence = new Task(TypeSentence("HIDE!!"));
        yield return new WaitUntil(() => PlayerHealth.IsHiding);
        _typeSentence = new Task(TypeSentence(""));

        yield return new WaitForSeconds(3f);
        VCam.SetActive(true);
        RenderSettings.fog = false;
        PlayerHealth.StopHideEffect();
        LightGameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        DoorTrasnform.DOLocalRotate(new Vector3(0f, 114f, 0f), 1);
        yield return new WaitForSeconds(1.5f);
        NodeParser.StartDialogue();

    }


    IEnumerator TypeSentence(string sentence)
    {
        _canvas.SetActive(true);
        dialogue.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            yield return new WaitForSeconds(0.06f);
            dialogue.text += letter;
            if (audioSource != null)
            {
                audioSource.Play();
            }
            yield return null;
        }
        _canvas.SetActive(true);
    }
}
