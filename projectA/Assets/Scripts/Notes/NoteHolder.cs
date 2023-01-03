using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class NoteHolder : MonoBehaviour
{
    [SerializeField] private Note _note;
    [SerializeField] private GameObject _noteObject;
    [SerializeField] private Text _canvasText;
    [SerializeField] private FirstPersonController _firstPersonController;

    public void ShowNote()
    {
        _firstPersonController.enabled = false;

        _noteObject.SetActive(true);
        _canvasText.text = _note.NoteText;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseNote()
    {
        _firstPersonController.enabled = true;

        _noteObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
