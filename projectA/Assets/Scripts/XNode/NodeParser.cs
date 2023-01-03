using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using XNode;
using TMPro;
using TaskManager;
using UnityStandardAssets.Characters.FirstPerson;

public class NodeParser : MonoBehaviour
{
    public DialogueGraph EnglishGraph;
    public DialogueGraph RussianGraph;

    private DialogueGraph _graph;
    Task _parser;
    Task _typeSentence;
    public TextMeshProUGUI dialogue;
    public TextMeshProUGUI nameText;
    public UnityEvent[] nodeEventsList;
    private UnityEvent nodeEvent;
    public SpriteRenderer spriteRenderer;
    public AudioSource audioSource;
    public FirstPersonController FirstPersonController;
    public PlayerInteractor playerInteractor;
    public bool CanStartDialogue;

    [SerializeField] private PlayerHealth _playerHealth;

    public bool StartOnAwake;

    private GameObject _canvas;
    private bool _typeSentenceRunning;

    private void Start()
    {
        _canvas = dialogue.transform.parent.gameObject;
        CanStartDialogue = true;

        if(SettingMenu.LanguageInt == 0)
        {
            _graph = EnglishGraph;
        }
        else if(SettingMenu.LanguageInt == 1)
        {
            _graph = RussianGraph;
        }

        if (StartOnAwake)
        {
            _canvas.SetActive(true);

            if (_playerHealth != null)
            {
                _playerHealth.DisableCanvas();
            }

            foreach (BaseNode b in _graph.nodes)
            {
                if (b.GetString() == "Start")
                {
                    _graph.current = b;
                    break;
                }
            }
            _parser = new Task(ParseNode());
        }
    }

    public void StartDialogue()
    {
        if (CanStartDialogue)
        {
            _canvas.SetActive(true);

            if (SettingMenu.LanguageInt == 0)
            {
                _graph = EnglishGraph;
            }
            else if (SettingMenu.LanguageInt == 1)
            {
                _graph = RussianGraph;
            }

            foreach (BaseNode b in _graph.nodes)
            {
                if (b.GetString() == "Start")
                {
                    _graph.current = b;
                    break;
                }
            }
            _parser = new Task(ParseNode());
        }
    }


    IEnumerator ParseNode()
    {
        BaseNode b = _graph.current;
        string data = b.GetString();
        string[] dataParts = data.Split('/');

        string color = b.GetColor();

        if (color != null || color == "")
        {
            string[] colorParts = color.Split(',');

            if (colorParts.Length > 1)
            {
                dialogue.color = new Color(float.Parse(colorParts[0]), float.Parse(colorParts[1]), float.Parse(colorParts[2]), float.Parse(colorParts[3]));
                color = null;
                colorParts = null;
            }
        }
        else
        {
            dialogue.color = new Color(255, 255, 255);
            color = null;
        }

        if (dataParts[0] == "Start")
        {
            if(_playerHealth != null)
            {
                _playerHealth.DisableCanvas();
            }

            if(FirstPersonController != null)
            {
                FirstPersonController.CanMove = false;
                playerInteractor.enabled = false;
            }

            NextNode("exit");
        }
        if (dataParts[0] == "DialogueNode")
        {
            //Run dialogue processing
            nameText.text = dataParts[1];
            _typeSentence = new Task(TypeSentence(dataParts[2]));

            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = b.GetSprite();
            }

            foreach(UnityEvent unityEvent in nodeEventsList)
            {
                if (dataParts[3] == unityEvent.GetPersistentMethodName(0))
                {
                    Debug.Log(unityEvent.GetPersistentMethodName(0));
                    nodeEvent = unityEvent;
                }
            }

            if (nodeEvent != null)
            {
                nodeEvent.Invoke();
            }


            yield return new WaitUntil(() => Input.GetMouseButtonUp(0));

            if (_typeSentence.Running)
            {
                _typeSentence.Stop();
                yield return new WaitForSeconds(0.1f);
                dialogue.text = "";
                dialogue.text += dataParts[2];

                yield return new WaitForSeconds(0.1f);
                yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
                NextNode("exit");
            }
            else
            {
                NextNode("exit");
            }
        }
        if (dataParts[0] == "End")
        {
            _canvas.SetActive(false);
            nameText.text = null;

            if (_playerHealth != null)
            {
                _playerHealth.EnableCanvas();
            }

            foreach (UnityEvent unityEvent in nodeEventsList)
            {
                if (dataParts[1] == unityEvent.GetPersistentMethodName(0))
                {
                    Debug.Log(unityEvent.GetPersistentMethodName(0));
                    nodeEvent = unityEvent;
                }
            }

            if (nodeEvent != null)
            {
                nodeEvent.Invoke();
            }

            if (_typeSentence.Running)
            {
                _typeSentence.Stop();
            }

            if(FirstPersonController != null)
            {
                FirstPersonController.CanMove = true;
                playerInteractor.enabled = true;
            }
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogue.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            yield return new WaitForSeconds(0.06f);
            dialogue.text += letter;
            if(audioSource != null)
            {
                audioSource.Play();
            }
            yield return null;
        }
    }

    public void NextNode(string fieldName)
    {
        if(_typeSentence != null)
        {
            _typeSentence.Stop();
        }
        //Find the port with this name
        if (_parser != null)
        {
            _parser.Stop();
            _parser = null;
        }
        foreach (NodePort p in _graph.current.Ports)
        {
            //Check if this is port we are looking for
            if (p.fieldName == fieldName)
            {
                StopCoroutine(TypeSentence(null));
                nameText.text = null;
                dialogue.text = null;
                _graph.current = p.Connection.node as BaseNode;
                break;
            }
        }
        _parser = new Task(ParseNode());
    }
}
