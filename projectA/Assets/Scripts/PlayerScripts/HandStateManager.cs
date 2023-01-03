using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using CI.QuickSave;
using System;
using UnityStandardAssets.CrossPlatformInput;

public class HandStateManager : MonoBehaviour
{
    public bool GetSaveProgress;
    [Space]

    [SerializeField] private FirstPersonController _firstPersonController;
    [SerializeField] private Transform _itemsHolder;
    [SerializeField] private Transform _itemHolderLeft;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private GunShooting _gunShooting;
    [SerializeField] private GunUI _gunUI;
    [SerializeField] private PlayerInteractor _playerInteractor;
    [SerializeField] private UIManager _uiManager;

    //Items
    private GameObject _flashLight;
    private GameObject _gps;
    private Gun _gun;
    private Animator _handsAnimator;

    private string _currentItemsAnim;

    //Item enabled/disabled
    [Header("Activate Items")]
    public bool _canUseFlashLight;
    public bool _canUseGPS;
    public bool _canUseGun;


    //Booleans
    [Header("Booleans")]
    private bool _enabledFlashlight;
    private bool _enabledGun;
    private bool _enableGPS;
    private bool _mouseClicked;
    private bool _enableItem;

    [Space]

    //Inventory System
    [Header("Inventory System")]
    public GameObject CurrentInstance;
    public Item CurrentItem;
    public GameObject CurrentItemHolder;

    public int ItemInInvetoryChoosed = 1;

    public Item FirstItem;
    public Item SecondItem;
    public Item ThirdItem;
    public Item FourthItem;

    public GameObject FirstItemHolder;
    public GameObject SecondItemHolder;
    public GameObject ThirdItemHolder;
    public GameObject FourthItemHolder;

    [Header("Inventory Canvas")]
    [SerializeField] private InventoryCanvasManager _pcInventoryCanvas;
    [SerializeField] private InventoryCanvasManager _mobileInventoryCanvas;
    private InventoryCanvasManager _inventoryCanvasManager;

    ItemState _itemState;
    GPSState _gpsState;
    FlashLightState _flashlightState;
    PistolState _pistolState;
    GPSState _pickedItemState;

    //Getterr Setters Public fields
    public Stamina Stamina;
    public Animator HandsAnimator { get { return _handsAnimator; } }
    public Transform ItemHolder { get { return _itemsHolder; } }
    public Transform ItemHolderLeft { get { return _itemHolderLeft; } }
    public bool GPSEnabled { get { return _enableGPS; } }
    public bool FlashLightEnabled { get { return _enabledFlashlight; } }
    public bool GunEnabled { get { return _enabledGun; } }
    public bool MouseClicked { get { return _mouseClicked; } }

    //Coroutines

    private void Start()
    {
        SavePlayerProgress.SaveEvent += SaveCurrentProgress;
        SavePlayerProgress.GetSavedProgressEvent += GetSavedReferences;

        _handsAnimator = this.GetComponent<Animator>();
        _itemState = new ItemState(_firstPersonController, this);
        _gpsState = new GPSState(_firstPersonController, this);
        _flashlightState = new FlashLightState(_firstPersonController, this);
        _pistolState = new PistolState(_firstPersonController, this, _gunShooting);
        _pickedItemState = new GPSState(_firstPersonController, this);

        if (UIManager.SmartphoneInput)
        {
            _inventoryCanvasManager = _mobileInventoryCanvas;
        }
        else
        {
            _inventoryCanvasManager = _pcInventoryCanvas;
        }

        if (MenuManager.LoadProgress)
        {
            if (GetSaveProgress)
            {
                var reader = QuickSaveReader.Create("Inventory");

                if (reader.Exists("FirstItem"))
                {
                    FirstItemHolder = GameObject.Find(reader.Read<string>("FirstItemHolder"));
                }

                if (reader.Exists("SecondItem"))
                {
                    SecondItemHolder = GameObject.Find(reader.Read<string>("SecondItemHolder"));
                }

                if (reader.Exists("ThirdItem"))
                {
                    ThirdItemHolder = GameObject.Find(reader.Read<string>("ThirdItemHolder"));
                }

                if (reader.Exists("FourthItem"))
                {
                    FourthItemHolder = GameObject.Find(reader.Read<string>("FourthItemHolder"));
                }
            }
        }
    }


    public void OnDestroy()
    {
        SavePlayerProgress.SaveEvent -= SaveCurrentProgress;
        SavePlayerProgress.GetSavedProgressEvent -= GetSavedReferences;
    }


    public void SaveCurrentProgress()
    {
        if (GetSaveProgress)
        {
            var writer = QuickSaveWriter.Create("Inventory");

            if (FirstItem != null)
            {
                writer.Write("FirstItem", FirstItem.name);
                writer.Write("FirstItemHolder", FirstItemHolder.name);

                Debug.Log("Saved first");
            }
            else
            {
                writer.Delete("FirstItem");
                writer.Delete("FirstItemHolder");
            }
            if (SecondItem != null)
            {
                writer.Write("SecondItem", SecondItem.name);
                writer.Write("SecondItemHolder", SecondItemHolder.name);
            }
            else
            {
                writer.Delete("SecondItem");
                writer.Delete("SecondItemHolder");
            }
            if (ThirdItem != null)
            {
                writer.Write("ThirdItem", ThirdItem.name);
                writer.Write("ThirdItemHolder", ThirdItemHolder.name);
            }
            else
            {
                writer.Delete("ThirdItem");
                writer.Delete("ThirItemHolder");
            }
            if (FourthItem != null)
            {
                writer.Write("FourthItem", FourthItem.name);
                writer.Write("FourthItemHolder", FourthItemHolder.name);
            }
            else
            {
                writer.Delete("FourthItem");
                writer.Delete("FourthItemHolder");
            }

            //writer.Write("FirstItem", FirstItem.name);
            //writer.Write("SecondItem", SecondItem.name);
            //writer.Write("ThirdItem", ThirdItem.name);
            //writer.Write("FourthItem", FourthItem.name);

            writer.Commit();
        }
    }

    public void GetSavedReferences()
    {
        if (GetSaveProgress)
        {
            var reader = QuickSaveReader.Create("Inventory");

            //if (reader.Read<Item>("FirstItem") != null) {
            //}

            if (reader.Exists("FirstItem"))
            {
                Debug.Log(reader.Read<string>("FirstItem"));
                Debug.Log("Loaded First");
                //FirstItem = (Item)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Items/" + reader.Read<string>("FirstItem") + ".asset", typeof(Item));

                //Resources.Load("Assets/Scripts/Items/" + reader.Read<string>("FirstItem") + ".asset", typeof(Item));

                FirstItem = Resources.Load<Item>("Items/" + reader.Read<string>("FirstItem"));
            }

            if (reader.Exists("SecondItem"))
            {
                //SecondItem = (Item)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Items/" + reader.Read<string>("SecondItem") + ".asset", typeof(Item));
                //Debug.Log(reader.Read<string>("SecondItem"));

                SecondItem = Resources.Load<Item>("Items/" + reader.Read<string>("SecondItem"));
            }

            if (reader.Exists("ThirdItem"))
            {
                //ThirdItem = (Item)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Items/" + reader.Read<string>("ThirdItem") + ".asset", typeof(Item));
                //Debug.Log(reader.Read<string>("ThirdItem"));

                ThirdItem = Resources.Load<Item>("Items/" + reader.Read<string>("ThirdItem"));
            }

            if (reader.Exists("FourthItem"))
            {
                //FourthItem = (Item)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Items/" + reader.Read<string>("FourthItem") + ".asset", typeof(Item));
                //Debug.Log(reader.Read<string>("FourthItem"));

                FourthItem = Resources.Load<Item>("Items/" + reader.Read<string>("FourthItem"));
            }

            if (FirstItem != null)
            {
                ItemInInvetoryChoosed = 1;
                CurrentItem = FirstItem;
                CurrentItemHolder = FirstItemHolder;

                _inventoryCanvasManager.FirstBorder.interactable = true;
                _inventoryCanvasManager.SecondBorder.interactable = false;
                _inventoryCanvasManager.ThirdBorder.interactable = false;
                _inventoryCanvasManager.FourthBorder.interactable = false;

                _inventoryCanvasManager.FirstSlot.enabled = true;
                _inventoryCanvasManager.FirstSlot.sprite = FirstItem.icon;

                EquipItemHandle(FirstItem);
            }
            if (SecondItem != null)
            {
                //_inventoryCanvasManager.FirstBorder.interactable = false;
                //_inventoryCanvasManager.SecondBorder.interactable = true;
                //_inventoryCanvasManager.ThirdBorder.interactable = false;
                //_inventoryCanvasManager.FourthBorder.interactable = false;

                _inventoryCanvasManager.SecondSlot.enabled = true;
                _inventoryCanvasManager.SecondSlot.sprite = SecondItem.icon;
            }
            if (ThirdItem != null)
            {
                //_inventoryCanvasManager.FirstBorder.interactable = false;
                //_inventoryCanvasManager.SecondBorder.interactable = false;
                //_inventoryCanvasManager.ThirdBorder.interactable = true;
                //_inventoryCanvasManager.FourthBorder.interactable = false;

                _inventoryCanvasManager.ThirdSlot.enabled = true;
                _inventoryCanvasManager.ThirdSlot.sprite = ThirdItem.icon;
            }
            if (FourthItem != null)
            {
                //_inventoryCanvasManager.FirstBorder.interactable = false;
                //_inventoryCanvasManager.SecondBorder.interactable = false;
                //_inventoryCanvasManager.ThirdBorder.interactable = false;
                //_inventoryCanvasManager.FourthBorder.interactable = true;

                _inventoryCanvasManager.FourthSlot.enabled = true;
                _inventoryCanvasManager.FourthSlot.sprite = FourthItem.icon;
            }
        }
    }

    void Update()
    {
        if (GetInput())
        {
            _mouseClicked = true;
        }
        else
        {
            _mouseClicked = false;
        }

        if (_canUseGun)
        {
            GunHandle();
        }
        //GPSHandle();
        PickHandle();


        if(CurrentItem != null && CurrentItemHolder != null)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                DropItem();
            }
        }


        if (UIManager.SmartphoneInput)
        {
            if (CrossPlatformInputManager.GetButtonDown("Slot1"))
            {
                ItemInInvetoryChoosed = 1;
                CurrentItem = FirstItem;
                CurrentItemHolder = FirstItemHolder;

                _inventoryCanvasManager.CanvasAnimator.SetTrigger("Open");

                _inventoryCanvasManager.FirstBorder.interactable = true;
                _inventoryCanvasManager.SecondBorder.interactable = false;
                _inventoryCanvasManager.ThirdBorder.interactable = false;
                _inventoryCanvasManager.FourthBorder.interactable = false;

                if (FirstItem != null && FirstItem.Name == "TT33")
                {
                    EnableGun();
                }
                else
                {
                    DisableGun();
                }

                EquipItemHandle(FirstItem);
            }

            if (CrossPlatformInputManager.GetButtonDown("Slot2"))
            {
                ItemInInvetoryChoosed = 2;
                CurrentItem = SecondItem;
                CurrentItemHolder = SecondItemHolder;


                _inventoryCanvasManager.CanvasAnimator.SetTrigger("Open");

                _inventoryCanvasManager.FirstBorder.interactable = false;
                _inventoryCanvasManager.SecondBorder.interactable = true;
                _inventoryCanvasManager.ThirdBorder.interactable = false;
                _inventoryCanvasManager.FourthBorder.interactable = false;

                if (SecondItem != null && SecondItem.Name == "TT33")
                {
                    EnableGun();
                }
                else
                {
                    DisableGun();
                }

                EquipItemHandle(SecondItem);

            }

            if (CrossPlatformInputManager.GetButtonDown("Slot3"))
            {
                ItemInInvetoryChoosed = 3;
                CurrentItem = ThirdItem;
                CurrentItemHolder = ThirdItemHolder;

                _inventoryCanvasManager.CanvasAnimator.SetTrigger("Open");

                _inventoryCanvasManager.FirstBorder.interactable = false;
                _inventoryCanvasManager.SecondBorder.interactable = false;
                _inventoryCanvasManager.ThirdBorder.interactable = true;
                _inventoryCanvasManager.FourthBorder.interactable = false;

                if (ThirdItem != null && ThirdItem.Name == "TT33")
                {
                    EnableGun();
                }
                else
                {
                    DisableGun();
                }

                EquipItemHandle(ThirdItem);
            }

            if (CrossPlatformInputManager.GetButtonDown("Slot4"))
            {
                ItemInInvetoryChoosed = 4;
                CurrentItem = FourthItem;
                CurrentItemHolder = FourthItemHolder;

                _inventoryCanvasManager.CanvasAnimator.SetTrigger("Open");

                _inventoryCanvasManager.FirstBorder.interactable = false;
                _inventoryCanvasManager.SecondBorder.interactable = false;
                _inventoryCanvasManager.ThirdBorder.interactable = false;
                _inventoryCanvasManager.FourthBorder.interactable = true;

                if (FourthItem != null && FourthItem.Name == "TT33")
                {
                    EnableGun();
                }
                else
                {
                    DisableGun();
                }

                EquipItemHandle(FourthItem);
            }
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ItemInInvetoryChoosed = 1;
            CurrentItem = FirstItem;
            CurrentItemHolder = FirstItemHolder;

            _inventoryCanvasManager.CanvasAnimator.SetTrigger("Open");

            _inventoryCanvasManager.FirstBorder.interactable = true;
            _inventoryCanvasManager.SecondBorder.interactable = false;
            _inventoryCanvasManager.ThirdBorder.interactable = false;
            _inventoryCanvasManager.FourthBorder.interactable = false;

            if(FirstItem != null && FirstItem.Name == "TT33")
            {
                EnableGun();
            }
            else
            {
                DisableGun();
            }

            EquipItemHandle(FirstItem);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ItemInInvetoryChoosed = 2;
            CurrentItem = SecondItem;
            CurrentItemHolder = SecondItemHolder;


            _inventoryCanvasManager.CanvasAnimator.SetTrigger("Open");

            _inventoryCanvasManager.FirstBorder.interactable = false;
            _inventoryCanvasManager.SecondBorder.interactable = true;
            _inventoryCanvasManager.ThirdBorder.interactable = false;
            _inventoryCanvasManager.FourthBorder.interactable = false;

            if (SecondItem != null && SecondItem.Name == "TT33")
            {
                EnableGun();
            }
            else
            {
                DisableGun();
            }

            EquipItemHandle(SecondItem);

            //CurrentItem = SecondItem;
            //SecondItem = CurrentItem;

            //CurrentItemHolder = SecondItemHolder;
            //SecondItemHolder = CurrentItemHolder;

            //EquipItemHandle(SecondItem);

            //if (CurrentItem != null && CurrentItemHolder != null)
            //    DropItem();

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ItemInInvetoryChoosed = 3;
            CurrentItem = ThirdItem;
            CurrentItemHolder = ThirdItemHolder;

            _inventoryCanvasManager.CanvasAnimator.SetTrigger("Open");

            _inventoryCanvasManager.FirstBorder.interactable = false;
            _inventoryCanvasManager.SecondBorder.interactable = false;
            _inventoryCanvasManager.ThirdBorder.interactable = true;
            _inventoryCanvasManager.FourthBorder.interactable = false;

            if (ThirdItem != null && ThirdItem.Name == "TT33")
            {
                EnableGun();
            }
            else
            {
                DisableGun();
            }

            EquipItemHandle(ThirdItem);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ItemInInvetoryChoosed = 4;
            CurrentItem = FourthItem;
            CurrentItemHolder = FourthItemHolder;

            _inventoryCanvasManager.CanvasAnimator.SetTrigger("Open");

            _inventoryCanvasManager.FirstBorder.interactable = false;
            _inventoryCanvasManager.SecondBorder.interactable = false;
            _inventoryCanvasManager.ThirdBorder.interactable = false;
            _inventoryCanvasManager.FourthBorder.interactable = true;

            if (FourthItem != null && FourthItem.Name == "TT33" )
            {
                EnableGun();
            }
            else
            {
                DisableGun();
            }

            EquipItemHandle(FourthItem);
        }
    }

    public void MakeFirstItem()
    {
        FirstItem = CurrentItem;
        FirstItemHolder = CurrentItemHolder;

        _inventoryCanvasManager.FirstBorder.interactable = true;
        _inventoryCanvasManager.SecondBorder.interactable = false;
        _inventoryCanvasManager.ThirdBorder.interactable = false;
        _inventoryCanvasManager.FourthBorder.interactable = false;

        _inventoryCanvasManager.FirstSlot.enabled = true;
        _inventoryCanvasManager.FirstSlot.sprite = FirstItem.icon;


        _inventoryCanvasManager.CanvasAnimator.SetTrigger("Open");

        if (CurrentItem != null && CurrentItem.Name == "TT33")
        {
            EnableGun();
        }
        else
        {
            DisableGun();
        }
    }

    public void MakeSecondItem()
    {
        SecondItem = CurrentItem;
        SecondItemHolder = CurrentItemHolder;

        _inventoryCanvasManager.FirstBorder.interactable = false;
        _inventoryCanvasManager.SecondBorder.interactable = true;
        _inventoryCanvasManager.ThirdBorder.interactable = false;
        _inventoryCanvasManager.FourthBorder.interactable = false;

        _inventoryCanvasManager.SecondSlot.enabled = true;
        _inventoryCanvasManager.SecondSlot.sprite = SecondItem.icon;

        _inventoryCanvasManager.CanvasAnimator.SetTrigger("Open");

        if (CurrentItem != null && CurrentItem.Name == "TT33")
        {
            EnableGun();
        }
        {
            DisableGun();
        }
    }

    public void MakeThirdItem()
    {
        ThirdItem = CurrentItem;
        ThirdItemHolder = CurrentItemHolder;

        _inventoryCanvasManager.FirstBorder.interactable = false;
        _inventoryCanvasManager.SecondBorder.interactable = false;
        _inventoryCanvasManager.ThirdBorder.interactable = true;
        _inventoryCanvasManager.FourthBorder.interactable = false;

        _inventoryCanvasManager.ThirdSlot.enabled = true;
        _inventoryCanvasManager.ThirdSlot.sprite = ThirdItem.icon;


        _inventoryCanvasManager.CanvasAnimator.SetTrigger("Open");

        if (CurrentItem != null && CurrentItem.Name == "TT33")
        {
            EnableGun();
        }
        {
            DisableGun();
        }
    }

    public void MakeFourthItem()
    {
        FourthItem = CurrentItem;
        FourthItemHolder = CurrentItemHolder;

        _inventoryCanvasManager.FirstBorder.interactable = false;
        _inventoryCanvasManager.SecondBorder.interactable = false;
        _inventoryCanvasManager.ThirdBorder.interactable = false;
        _inventoryCanvasManager.FourthBorder.interactable = true;

        _inventoryCanvasManager.FourthSlot.enabled = true;
        _inventoryCanvasManager.FourthSlot.sprite = FourthItem.icon;


        _inventoryCanvasManager.CanvasAnimator.SetTrigger("Open");

        if (CurrentItem != null && CurrentItem.Name == "TT33")
        {
            EnableGun();
        }
        {
            DisableGun();
        }
    }

    public void ReEnableHand() //ResetsHandAnimations and Handle
    {
        if (ItemInInvetoryChoosed == 1)
        {
            ExitAllStates();
            ItemInInvetoryChoosed = 1;
            CurrentItem = FirstItem;
            CurrentItemHolder = FirstItemHolder;

            _inventoryCanvasManager.CanvasAnimator.SetTrigger("Open");

            _inventoryCanvasManager.FirstBorder.interactable = true;
            _inventoryCanvasManager.SecondBorder.interactable = false;
            _inventoryCanvasManager.ThirdBorder.interactable = false;
            _inventoryCanvasManager.FourthBorder.interactable = false;

            if (FirstItem != null && FirstItem.Name == "TT33")
            {
                EnableGun();
            }
            else
            {
                DisableGun();
            }

            EquipItemHandle(FirstItem);
        }

        if (ItemInInvetoryChoosed == 2)
        {
            ItemInInvetoryChoosed = 2;
            CurrentItem = SecondItem;
            CurrentItemHolder = SecondItemHolder;


            _inventoryCanvasManager.CanvasAnimator.SetTrigger("Open");

            _inventoryCanvasManager.FirstBorder.interactable = false;
            _inventoryCanvasManager.SecondBorder.interactable = true;
            _inventoryCanvasManager.ThirdBorder.interactable = false;
            _inventoryCanvasManager.FourthBorder.interactable = false;

            if (SecondItem != null && SecondItem.Name == "TT33")
            {
                EnableGun();
            }
            else
            {
                DisableGun();
            }

            EquipItemHandle(SecondItem);

        }

        if (ItemInInvetoryChoosed == 3)
        {
            ItemInInvetoryChoosed = 3;
            CurrentItem = ThirdItem;
            CurrentItemHolder = ThirdItemHolder;

            _inventoryCanvasManager.CanvasAnimator.SetTrigger("Open");

            _inventoryCanvasManager.FirstBorder.interactable = false;
            _inventoryCanvasManager.SecondBorder.interactable = false;
            _inventoryCanvasManager.ThirdBorder.interactable = true;
            _inventoryCanvasManager.FourthBorder.interactable = false;

            if (ThirdItem != null && ThirdItem.Name == "TT33")
            {
                EnableGun();
            }
            else
            {
                DisableGun();
            }

            EquipItemHandle(ThirdItem);
        }

        if (ItemInInvetoryChoosed == 4)
        {
            ItemInInvetoryChoosed = 4;
            CurrentItem = FourthItem;
            CurrentItemHolder = FourthItemHolder;

            _inventoryCanvasManager.CanvasAnimator.SetTrigger("Open");

            _inventoryCanvasManager.FirstBorder.interactable = false;
            _inventoryCanvasManager.SecondBorder.interactable = false;
            _inventoryCanvasManager.ThirdBorder.interactable = false;
            _inventoryCanvasManager.FourthBorder.interactable = true;

            if (FourthItem != null && FourthItem.Name == "TT33")
            {
                EnableGun();
            }
            else
            {
                DisableGun();
            }

            EquipItemHandle(FourthItem);
        }
    }

    public void ExitAllStates()
    {
        if(ItemInInvetoryChoosed == 1)
        {
            if (SecondItem != null)
            {
                _handsAnimator.SetBool(SecondItem.AnimationBoolName, false);
            }

            if (ThirdItem != null)
            {
                _handsAnimator.SetBool(ThirdItem.AnimationBoolName, false);
            }

            if (FourthItem != null)
            {
                _handsAnimator.SetBool(FourthItem.AnimationBoolName, false);
            }
        }
        else if(ItemInInvetoryChoosed == 2)
        {
            if(FirstItem != null)
            {
                _handsAnimator.SetBool(FirstItem.AnimationBoolName, false);
            }

            if(ThirdItem != null)
            {
                _handsAnimator.SetBool(ThirdItem.AnimationBoolName, false);
            }

            if(FourthItem != null)
            {
                _handsAnimator.SetBool(FourthItem.AnimationBoolName, false);
            }
        }
        else if(ItemInInvetoryChoosed == 3)
        {
            if (FirstItem != null)
            {
                _handsAnimator.SetBool(FirstItem.AnimationBoolName, false);
            }

            if (SecondItem != null)
            {
                _handsAnimator.SetBool(SecondItem.AnimationBoolName, false);
            }

            if (FourthItem != null)
            {
                _handsAnimator.SetBool(FourthItem.AnimationBoolName, false);
            }
        }
        else if(ItemInInvetoryChoosed == 4)
        {
            if (FirstItem != null)
            {
                _handsAnimator.SetBool(FirstItem.AnimationBoolName, false);
            }

            if (SecondItem != null)
            {
                _handsAnimator.SetBool(SecondItem.AnimationBoolName, false);
            }

            if (ThirdItem != null)
            {
                _handsAnimator.SetBool(ThirdItem.AnimationBoolName, false);
            }

        }
    }

    private void GunHandle()
    {
        if (CurrentItem != null && CurrentItem.Name == "TT33")
        {
            _pistolState = _pistolState.HandleInputPistol();
        }
    }

    private void GPSHandle()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3) && _canUseGPS)
        {
            _gps = _inventory.GPS;
            if (_playerInteractor.heldObj != null)
                _playerInteractor.DropObject();

            if (_gun != null)
            {
                _enabledGun = false;
                _handsAnimator.SetBool(_gun.Animation, false);
                _gunUI.EnableGunUI(false);
            }
            _enabledFlashlight = false;
            _handsAnimator.SetBool("FlashLightEnabled", false);

            if (CurrentItem != null && CurrentItemHolder != null)
                DropItem();

            _enableGPS = !_enableGPS;
            _handsAnimator.SetBool("GPSEnabled", _enableGPS);
        }

        if (_enableGPS)
        {
            _gpsState = _gpsState.HandleInputGPS();
        }
    }

    private void PickHandle()
    {
        if (_enableItem)
        {
            _gpsState = _gpsState.HandleInputGPS();
        }
    }

    public void EquipItemHandle(Item currentItem)
    {

        if (currentItem != null)
        {
            if (currentItem.AnimationBoolName != null)
            {
                _currentItemsAnim = currentItem.AnimationBoolName;
                _handsAnimator.SetBool(_currentItemsAnim, true);
            }
            else
            {
                print("Put animation string in item");
                _handsAnimator.SetBool("isPicking", true);
            }

            ExitAllStates();
            _enableItem = true;
        }
        else
        {
            ExitAllStates();
        }
    }

    public void WaitEquipItem(Item currentItem)
    {
        StartCoroutine(WaitEquipItemCoroutine(currentItem));
    }

    public IEnumerator WaitEquipItemCoroutine(Item currentItem)
    {
        yield return new WaitForSeconds(0.2f);
        if (currentItem.AnimationBoolName != null)
        {
            _currentItemsAnim = currentItem.AnimationBoolName;
            _handsAnimator.SetBool(_currentItemsAnim, true);
        }
        else
        {
            print("Put animation string in item");
            _handsAnimator.SetBool("isPicking", true);
        }
        ExitAllStates();
    }

    public IEnumerator DropItemCoroutine()
    {

        if (_playerInteractor.heldObj != null)
            _playerInteractor.DropObject();

        if (_currentItemsAnim != null)
        {
            _handsAnimator.SetBool(_currentItemsAnim, false);
        }
        else
        {
            print("Cant find Anim in Item, Put in string");
            _handsAnimator.SetBool("isPicking", false);
        }

        yield return new WaitForSeconds(0.2f);

        if(CurrentItemHolder != null)
        {
            CurrentItemHolder.SetActive(true);
            CurrentItemHolder.transform.parent = null;
            CurrentItemHolder = null;
        }

        _enableItem = false;
        CurrentItem = null;

        if (ItemInInvetoryChoosed == 1)
        {
            FirstItemHolder = null;
            FirstItem = null;

            _inventoryCanvasManager.FirstSlot.sprite = null;
            _inventoryCanvasManager.FirstSlot.enabled = false;
        }
        else if (ItemInInvetoryChoosed == 2)
        {
            SecondItemHolder = null;
            SecondItem = null;

            _inventoryCanvasManager.SecondSlot.sprite = null;
            _inventoryCanvasManager.SecondSlot.enabled = false;
        }
        else if (ItemInInvetoryChoosed == 3)
        {
            ThirdItem = null;
            ThirdItemHolder = null;

            _inventoryCanvasManager.ThirdSlot.sprite = null;
            _inventoryCanvasManager.ThirdSlot.enabled = false;
        }
        else if (ItemInInvetoryChoosed == 4)
        {
            FourthItem = null;
            FourthItemHolder = null;

            _inventoryCanvasManager.FourthSlot.sprite = null;
            _inventoryCanvasManager.FourthSlot.enabled = false;
        }

        _handsAnimator.SetBool("isRunning", false);
    }

    public void ChangeItem()
    {
        if (_playerInteractor.heldObj != null)
            _playerInteractor.DropObject();

        if (_currentItemsAnim != null)
        {
            _handsAnimator.SetBool(_currentItemsAnim, false);
        }
        else
        {
            _handsAnimator.SetBool("isPicking", false);
        }
        CurrentItemHolder.SetActive(true);
        CurrentItemHolder.transform.parent = null;
        CurrentItemHolder = null;

        //if (ItemInInvetoryChoosed == 1)
        //{
        //    FirstItemHolder = null;
        //    FirstItem = null;
        //}
        //else if (ItemInInvetoryChoosed == 2)
        //{
        //    SecondItemHolder = null;
        //    SecondItem = null;
        //}

        CurrentItem = null;
        _handsAnimator.SetBool("isRunning", false);
    }

    public void DropItem()
    {
        if (CurrentItem != null && CurrentItem.Name == "TT33")
        {
            DisableGun();
        }

        ExitAllStates();

        StartCoroutine(DropItemCoroutine());
    }

    private bool GetInput()
    {
        if (_uiManager.EnableSmartPhoneInput)
        {
            return CrossPlatformInputManager.GetButtonDown("Interact");
        }
        else
        {
            return Input.GetMouseButtonDown(0);
        }
    }

    public void EnableGun()
    {
        _canUseGun = true;
        _enabledGun = true;

        _gun = _inventory.Gun;
        _gunUI.EnableGunUI(true);
    }

    public void DisableGun()
    {
        if(_gun != null)
        {
            _canUseGun = false;
            _enabledGun = false;

            _gunUI.EnableGunUI(false);
        }
    }

    public void EnableGPS()
    {
        _enabledFlashlight = false;
        _handsAnimator.SetBool("FlashLightEnabled", false);

        _enableGPS = true;
        _handsAnimator.SetBool("GPSEnabled", true);

        _canUseGPS = true;
    }


}
