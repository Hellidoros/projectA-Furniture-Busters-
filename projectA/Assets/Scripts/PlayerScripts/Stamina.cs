using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Stamina : MonoBehaviour
{
    [SerializeField] private Slider _staminaBar;
    [SerializeField] private FirstPersonController _player;
    [SerializeField] private float _staminaMinus = 0.05f;

    private int _maxStamina = 100;
    [SerializeField] private float _currentStamina;

    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine regen;

    public static Stamina instace;
    public float CurrentStamina { get { return _currentStamina; } set { _currentStamina = value; } }
    public float MinStamina = 10;

    private void Awake()
    {
        instace = this;
    }

    private void Start()
    {
        _currentStamina = _maxStamina;
        _staminaBar.maxValue = _maxStamina;
        _staminaBar.value = _maxStamina;

    }

    private void Update()
    {
        if(_currentStamina > MinStamina)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                UseStamina(0.05f);
            }
            _player.RunSpeed = 5f;
            _player.WalkSpeed = 2f;
        }
        else if(_currentStamina <= MinStamina)
        {
            _player.IsWalking = true;
            _staminaBar.value = _currentStamina;
            _player.RunSpeed = 1f;
            _player.WalkSpeed = 2f;
        }
    }

    public void UseStamina(float amount)
    {
        if(_currentStamina - amount >= 0)
        {
            _currentStamina -= amount;
            _staminaBar.value = _currentStamina;

            if(regen != null)
            {
                StopCoroutine(regen);
            }

            regen = StartCoroutine(RegenStamina());
        }
    }

    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(3);

        while (_currentStamina < _maxStamina)
        {
            _currentStamina += _maxStamina / 100;
            _staminaBar.value = _currentStamina;
            yield return regenTick;
        }
    }
}
