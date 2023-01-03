using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

public class MotherMonsterScript : MonoBehaviour
{
    public GameObject PlayerTransform;
    public GameObject PlayerCamera;
    public GameObject PlayerItemsRendererCamera;
    private GameObject _player;
    private NavMeshAgent _nav;
    private AudioSource _sound;
    private Animator _animator;
    private PlayerHealth _playerHealth;
    public GameObject DeathCam;

    public Collider MonsterCollider;

    public AudioClip[] FootSounds;

    [SerializeField] private float _jumpScareSight = 0.7f;

    private bool _alive;
    [SerializeField] private string _state = "chase";

    // Start is called before the first frame update
    void Start()
    {
        _sound = this.GetComponent<AudioSource>();
        _nav = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _player = PlayerTransform.transform.parent.gameObject;
        _playerHealth = _player.GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Physics.IgnoreCollision(MonsterCollider, other.GetComponent<Collider>(), true);
        }
    }

    public void FootStep(int num)
    {
        if(_sound != null)
        {
            _sound.clip = FootSounds[num];
            _sound.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_state == "chase")
        {
            _nav.destination = _player.transform.position;

            if (_nav.remainingDistance <= _nav.stoppingDistance + _jumpScareSight && !_nav.pathPending)
            {
                if (_player.GetComponent<PlayerHealth>().Alive && !_player.GetComponent<PlayerHealth>().IsHiding)
                {
                    Transform[] MonsterChildren = this.gameObject.GetComponentsInChildren<Transform>();

                    foreach (Transform i in MonsterChildren)
                    {
                        i.gameObject.layer = LayerMask.NameToLayer("NoClip");
                    }
                    _state = "kill";
                    _player.GetComponent<PlayerHealth>().Alive = false;
                    _player.GetComponent<FirstPersonController>().enabled = false;
                    DeathCam.SetActive(true);
                    PlayerCamera.SetActive(false);
                    PlayerItemsRendererCamera.transform.parent = DeathCam.transform;
                    PlayerItemsRendererCamera.transform.position = DeathCam.transform.position;
                    PlayerItemsRendererCamera.transform.rotation = DeathCam.transform.rotation;
                    _animator.speed = 1.2f;
                    _animator.SetTrigger("Kill");
                }
            }
        }

        if (_state == "kill")
        {
            _alive = false;
            _animator.speed = 1.2f;
            StartCoroutine(KillPlayer());
            _state = "stop";
        }
    }


    public IEnumerator KillPlayer()
    {
        _animator.speed = 1.2f;
        yield return new WaitForSeconds(2f);
        _playerHealth.KillPLayer();
    }

}
