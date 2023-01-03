using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private LayerMask _interactableLayerMask = 8;
    [SerializeField] private Interactable _interactable;
    [SerializeField] private Animator _animator;
    [SerializeField] private HandStateManager _handStateManager;
    UnityEvent onInteract;
    [SerializeField] private Image _interactImage;
    [SerializeField] private Text _interactName;
    [SerializeField] private Sprite _defaultIcon;
    [SerializeField] private Vector2 _defaultIconSize;
    [SerializeField] private Vector2 _defaultInteractorSize;
    [SerializeField] private Sprite _defaultInteractIcon;
    [SerializeField] private float _maxDistance = 5;

    [SerializeField] private Transform _holdParent;
    [SerializeField] private Transform _holdItemParent;
    [SerializeField] private float _moveForce = 250;
    public GameObject heldObj;

    public RaycastHit hit;

    //Mobile button image
    [SerializeField] private Image _mobileButtonImage;
    [SerializeField] private Sprite _defaultMobileButtonImage;

    //Interact buttons
    [SerializeField] private Image _interactButton;

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);

            if (Physics.Raycast(ray, out hit, _maxDistance, _interactableLayerMask))
            {
                ManageInteraction(hit);
            }
        }

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, _maxDistance, _interactableLayerMask))
        {
            ManageInteraction(hit);
        }
        else
        {
            if (_interactImage != null && _interactImage.sprite != _defaultIcon)
            {
                _interactImage.sprite = _defaultIcon;
                _interactImage.rectTransform.sizeDelta = _defaultIconSize;
                _interactName.text = null;

                if (_interactButton != null)
                {
                    _interactButton.color = new Color(_interactButton.color.r, _interactButton.color.g, _interactButton.color.b, 0.22f);
                }
            }
            if(_mobileButtonImage != null && _mobileButtonImage.sprite != _defaultMobileButtonImage)
            {
                _mobileButtonImage.sprite = _defaultMobileButtonImage;
            }
        }

        if(heldObj != null)
        {
            MoveObject();
            if (Input.GetKeyDown(KeyCode.Q))
            {
                DropObject();
            }
        }
    }


    public void ManageInteraction(RaycastHit hit)
    {
        if (hit.collider.GetComponent<Interactable>() != false)
        {
            if (_interactable == null || _interactable.ID != hit.collider.GetComponent<Interactable>().ID)
            {
                _interactable = hit.collider.GetComponent<Interactable>();
                Debug.Log("New interactable");
            }
            if (_interactable.InteractableIcon != null)
            {
                _interactImage.sprite = _interactable.InteractableIcon;
                _interactImage.rectTransform.sizeDelta = _defaultInteractorSize;
                _interactName.text = _interactable.Name;
            }
            else
            {
                _interactImage.sprite = _defaultInteractIcon;
                _interactImage.rectTransform.sizeDelta = _defaultInteractorSize;
                _interactName.text = _interactable.Name;
            }

            if (_mobileButtonImage != null)
            {
                if (_interactable.ButtonIconSprite != null)
                {
                    _mobileButtonImage.sprite = _interactable.ButtonIconSprite;
                }
                else
                {
                    _mobileButtonImage.sprite = _defaultMobileButtonImage;
                }

                if (_interactButton != null)
                {
                    _interactButton.color = new Color(_interactButton.color.r, _interactButton.color.g, _interactButton.color.b, 0.80f);
                }
                else
                {

                }
            }


            if (CrossPlatformInputManager.GetButtonDown("Interact") || Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) //Input.GetKeyDown(KeyCode.E) CrossPlatformInputManager.GetButton("Interact")
            {
                if (_interactable.GetComponent<ItemHolder>())
                {
                    //First inventory slot
                    if (_handStateManager.ItemInInvetoryChoosed == 1)
                    {
                        if (_handStateManager.SecondItem != null && _handStateManager.ThirdItem != null && _handStateManager.FourthItem != null)
                        {
                            MakeInteraction(hit, _handStateManager.FirstItemHolder, _handStateManager.FirstItem);
                            _handStateManager.MakeFirstItem();
                        }
                        else if (_handStateManager.FirstItem == null) //&& _handStateManager.SecondItem == null
                        {
                            MakeInteraction(hit, _handStateManager.FirstItemHolder, _handStateManager.FirstItem);
                            _handStateManager.MakeFirstItem();
                        }
                        else if (_handStateManager.FirstItem != null && _handStateManager.SecondItem != null && _handStateManager.ThirdItem == null)
                        {
                            _handStateManager.ItemInInvetoryChoosed = 3;
                        }
                        else if (_handStateManager.FirstItem != null && _handStateManager.SecondItem != null && _handStateManager.ThirdItem != null && _handStateManager.FourthItem == null)
                        {
                            _handStateManager.ItemInInvetoryChoosed = 4;
                        }
                        else
                        {
                            _handStateManager.ItemInInvetoryChoosed = 2;
                        }
                    }

                    //Second inventory slot
                    if (_handStateManager.ItemInInvetoryChoosed == 2)
                    {
                        if (_handStateManager.ThirdItem != null && _handStateManager.FirstItem != null && _handStateManager.FourthItem != null)
                        {
                            MakeInteraction(hit, _handStateManager.SecondItemHolder, _handStateManager.SecondItem);
                            _handStateManager.MakeSecondItem();
                        }
                        else if (_handStateManager.SecondItem == null) //&& _handStateManager.ThirdItem == null
                        {
                            MakeInteraction(hit, _handStateManager.SecondItemHolder, _handStateManager.SecondItem);
                            _handStateManager.MakeSecondItem();
                        }
                        else if (_handStateManager.SecondItem != null && _handStateManager.ThirdItem != null && _handStateManager.FirstItem == null)
                        {
                            _handStateManager.ItemInInvetoryChoosed = 1;

                            MakeInteraction(hit, _handStateManager.FirstItemHolder, _handStateManager.FirstItem);
                            _handStateManager.MakeFirstItem();
                        }
                        else if (_handStateManager.SecondItem != null && _handStateManager.ThirdItem != null && _handStateManager.FirstItem != null && _handStateManager.FourthItem == null)
                        {
                            _handStateManager.ItemInInvetoryChoosed = 4;
                        }
                        else
                        {
                            _handStateManager.ItemInInvetoryChoosed = 3;
                        }
                    }

                    //Third inventory slot
                    if (_handStateManager.ItemInInvetoryChoosed == 3)
                    {
                        if (_handStateManager.FourthItem != null && _handStateManager.SecondItem != null && _handStateManager.FirstItem != null)
                        {
                            MakeInteraction(hit, _handStateManager.ThirdItemHolder, _handStateManager.ThirdItem);
                            _handStateManager.MakeThirdItem();
                        }
                        else if (_handStateManager.ThirdItem == null) //&& _handStateManager.FourthItem == null
                        {
                            MakeInteraction(hit, _handStateManager.ThirdItemHolder, _handStateManager.ThirdItem);
                            _handStateManager.MakeThirdItem();
                        }
                        else if (_handStateManager.ThirdItem != null && _handStateManager.FourthItem != null && _handStateManager.FirstItem != null && _handStateManager.SecondItem == null)
                        {
                            _handStateManager.ItemInInvetoryChoosed = 2;

                            MakeInteraction(hit, _handStateManager.SecondItemHolder, _handStateManager.SecondItem);
                            _handStateManager.MakeSecondItem();
                        }
                        else if (_handStateManager.ThirdItem != null && _handStateManager.FourthItem != null && _handStateManager.SecondItem != null && _handStateManager.FirstItem == null)
                        {
                            _handStateManager.ItemInInvetoryChoosed = 1;

                            MakeInteraction(hit, _handStateManager.FirstItemHolder, _handStateManager.FirstItem);
                            _handStateManager.MakeFirstItem();
                        }
                        else
                        {
                            _handStateManager.ItemInInvetoryChoosed = 4;
                        }
                    }

                    //Fourth inventory slot
                    if (_handStateManager.ItemInInvetoryChoosed == 4)
                    {
                        if (_handStateManager.FirstItem != null && _handStateManager.SecondItem != null && _handStateManager.ThirdItem != null)
                        {
                            MakeInteraction(hit, _handStateManager.FourthItemHolder, _handStateManager.FourthItem);
                            _handStateManager.MakeFourthItem();
                        }
                        else if (_handStateManager.FourthItem == null) //&& _handStateManager.FirstItem == null
                        {
                            MakeInteraction(hit, _handStateManager.FourthItemHolder, _handStateManager.FourthItem);
                            _handStateManager.MakeFourthItem();
                        }
                        else if (_handStateManager.FourthItem != null && _handStateManager.FirstItem != null && _handStateManager.SecondItem == null)
                        {
                            _handStateManager.ItemInInvetoryChoosed = 2;

                            MakeInteraction(hit, _handStateManager.SecondItemHolder, _handStateManager.SecondItem);
                            _handStateManager.MakeSecondItem();
                        }
                        else if (_handStateManager.FourthItem != null && _handStateManager.FirstItem != null && _handStateManager.SecondItem != null && _handStateManager.ThirdItem == null)
                        {
                            _handStateManager.ItemInInvetoryChoosed = 3;

                            MakeInteraction(hit, _handStateManager.ThirdItemHolder, _handStateManager.ThirdItem);
                            _handStateManager.MakeThirdItem();
                        }
                        else
                        {
                            _handStateManager.ItemInInvetoryChoosed = 1;

                            MakeInteraction(hit, _handStateManager.FirstItemHolder, _handStateManager.FirstItem);
                            _handStateManager.MakeFirstItem();
                        }
                    }
                }

                if (_interactable.GetComponent<Rigidbody>() && !_interactable.GetComponent<ItemHolder>() && !_interactable.GetComponent<Rigidbody>().isKinematic && _interactable.GetComponent<Rigidbody>().useGravity)
                {
                    if (heldObj == null)
                    {
                        PickUpObject(hit.transform.gameObject);
                    }
                }

                _interactable.OnInteract.Invoke();
            }

            //Enable on PC build

            //else if (Input.GetKeyDown(KeyCode.Mouse0) && !_interactable.GetComponent<ItemHolder>())
            //{
            //    _interactable.OnInteract.Invoke();
            //}
        }
    }

    public void MakeInteraction(RaycastHit hit, GameObject currentItemHolder, Item currentItem)
    {
        if (currentItem == null)
            {
                currentItem = _interactable.GetComponent<ItemHolder>().Item;  //_handStateManager.CurrentItem = _interactable.GetComponent<ItemHolder>().Item
                _handStateManager.CurrentItem = currentItem;

                if (_interactable.GetComponent<ItemHolder>().Item.AnimationBoolName != null)
                {
                    _handStateManager.EquipItemHandle(_interactable.GetComponent<ItemHolder>().Item);
                }
                else
                {
                    _handStateManager.EquipItemHandle(null);
                }
                Physics.IgnoreCollision(this.transform.parent.GetComponent<Collider>(), _interactable.GetComponent<Collider>(), true);
                _interactable.transform.position = _holdItemParent.position;
                _interactable.transform.parent = _holdItemParent;
                currentItemHolder = _interactable.gameObject; //_handStateManager.CurrentItemHolder = _interactable.gameObject;
                _handStateManager.CurrentItemHolder = currentItemHolder;

                if (heldObj != null)
                    DropObject();
            }
            else
            {
                _handStateManager.ChangeItem();

                currentItem = _interactable.GetComponent<ItemHolder>().Item;
                _handStateManager.CurrentItem = currentItem;
                _interactable.transform.position = _holdItemParent.position;
                _interactable.transform.parent = _holdItemParent;
                currentItemHolder = _interactable.gameObject;
                _handStateManager.CurrentItemHolder = currentItemHolder;
                //_handStateManager.DisableAll();

                Physics.IgnoreCollision(this.transform.parent.GetComponent<Collider>(), _interactable.GetComponent<Collider>(), true);

                if (_interactable.GetComponent<ItemHolder>().Item.AnimationBoolName != null)
                {
                    _handStateManager.WaitEquipItem(_interactable.GetComponent<ItemHolder>().Item);
                }
                else
                {
                    _handStateManager.WaitEquipItem(null);
                }

                if (heldObj != null)
                    DropObject();
            }
    }

    public void MoveObject()
    {
        if(Vector3.Distance(heldObj.transform.position, _holdParent.position) > 0.1f)
        {
            Vector3 moveDirection = (_holdParent.position - heldObj.transform.position);
            heldObj.GetComponent<Rigidbody>().AddForce(moveDirection * _moveForce);
        }
    }

    public void PickUpObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        {

            Rigidbody objRig = pickObj.GetComponent<Rigidbody>();
            objRig.useGravity = false;
            objRig.drag = 10;

            objRig.transform.parent = _holdParent;
            heldObj = pickObj;
            heldObj.layer = LayerMask.NameToLayer("Default");
        }
    }

    public void DropObject()
    {
        Rigidbody heldRig = heldObj.GetComponent<Rigidbody>();
        heldRig.useGravity = true;
        heldRig.drag = 1;
        heldObj.layer = LayerMask.NameToLayer("Interactable");

        heldObj.transform.parent = null;
        heldObj = null;
    }
}
