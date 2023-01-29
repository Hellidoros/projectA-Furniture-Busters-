using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;
using CI.QuickSave;

public class Monster : MonoBehaviour
{
    public Enemy _enemy;

    //Player
    private GameObject PlayerTransform;
    private GameObject PlayerCamera;
    private GameObject PlayerItemsRendererCamera;

    private GameObject _player;


    public AudioClip[] FootSounds;
    public Transform Eyes;
    public GameObject DeathCam;
    public bool MustHide;
    public AudioSource Growl;
    public float MaxHelath = 3;
    public float Health = 3;
    public float Stunnes = 3;

    public bool MoveOnKill;
    public Vector3 MoveOnKillPosition;

    [SerializeField] private LayerMask _ignorelayerMask;

    public Transform[] _hidingPositions;
    private Transform _cuurentHidingPosition;

    [SerializeField] private GameObject _firstForm;
    [SerializeField] private GameObject[] _monsterForm;
    [SerializeField] private AudioClip _biteSound;

    private NavMeshAgent _nav;
    private AudioSource _sound;
    private Animator _animator;
    private List<Renderer> _renderers =  new List<Renderer>();
    private PlayerHealth _playerHealth;
    [SerializeField]private string _state = "hiding";
    [SerializeField]private bool _alive;
    private float _wait = 0f;
    private bool _highAlert = false;
    [SerializeField] private float _alertness = 20f;
    [SerializeField] private float _calmness;

    [SerializeField] private float _jumpScareSight = 0.7f;
    [SerializeField] private float _chaseSpeed;
    [SerializeField] private float _simpleSpeed;

    private int _diffucultyLevel;

    float elapsedTime = 0;
    float waitTime = 3f;


    [SerializeField] private float _dissolveStartSpeed = 0.002f;
    [SerializeField] private float _dissolveStopSpeed = 0.0004f;

    public float LerpNum;
    public bool StartLerp;

    [SerializeField] private bool _isDead;

    public void Awake()
    {
        if(_enemy != null)
        {
            _jumpScareSight = _enemy.JumpScareSight;
            _simpleSpeed = _enemy.SimpleSpeed;
        }
    }

    public void Start()
    {
        SavePlayerProgress.SaveEvent += SaveCurrentProgress;
        SavePlayerProgress.GetSavedProgressEvent += GetSavedReferences;

        PlayerTransform = PlayerReference.FirstPersonController.gameObject;
        PlayerCamera = PlayerReference.PlayerCam;
        PlayerItemsRendererCamera = PlayerReference.PlayerItemsRendererCamera;

    _diffucultyLevel = DiffucultyLevelScript.DifficultyLevel;

        if (_diffucultyLevel == 0)
        {
            _chaseSpeed = _enemy.EasyModeChaseSpeed;
        }
        if (_diffucultyLevel == 1)
        {
            _chaseSpeed = _enemy.ChaseSpeed;
        }
        if (_diffucultyLevel == 2)
        {
            _chaseSpeed = _enemy.HardModeChaseSpeed;
        }

        foreach (GameObject monster in _monsterForm)
        {
            _renderers.Add(monster.GetComponent<Renderer>());
        }

        StopAllCoroutines();
        _sound = this.GetComponent<AudioSource>();
        _player = PlayerReference.FirstPersonController.gameObject;
        _nav = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _playerHealth = _player.GetComponent<PlayerHealth>();

        if (_state == "hiding")
        {
            if(_firstForm != null)
            {
                _firstForm.SetActive(true);
            }

            foreach (GameObject gameObject in _monsterForm)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (_firstForm != null)
            {
                _firstForm.SetActive(false);
            }
            foreach (GameObject gameObject in _monsterForm)
            {
                gameObject.SetActive(true);
            }
        }

        _nav.speed = 1.2f;
        _animator.speed = 1.2f;

        StartCoroutine(EnableNavMesh());
    }

    private void OnDisable()
    {
        SavePlayerProgress.SaveEvent -= SaveCurrentProgress;
        SavePlayerProgress.GetSavedProgressEvent -= GetSavedReferences;
    }

    public void SaveCurrentProgress()
    {
        var writer = QuickSaveWriter.Create("Monster " + this.gameObject.name);

        writer.Write("State", _state);
        writer.Write("IsDead", _isDead);
        writer.Commit();
    }

    public void GetSavedReferences()
    {
        var reader = QuickSaveReader.Create("Monster " + this.gameObject.name);

        Debug.Log(reader.Read<string>("State"));

        if(reader.Read<string>("State") == "hiding")
        {
            //do nothing
        }
        else
        {
            if (_firstForm != null)
            {
                _firstForm.SetActive(false);
            }
            foreach (GameObject gameObject in _monsterForm)
            {
                gameObject.SetActive(true);
            }

            _state = "idle";
            StartLerp = false;
            StartCoroutine(StartLerpTimer());

            if (MoveOnKill)
            {
                this.transform.position = MoveOnKillPosition;
            }
        }

        _isDead = reader.Read<bool>("IsDead");

        if (_isDead)
        {
            Destroy(this.gameObject);
        }
    }

    public IEnumerator EnableNavMesh()
    {
        yield return new WaitForSeconds(3f);
        _nav.enabled = true;
        _nav.speed = 1.2f;

        _alive = true;
    }

    public void FootStep(int num)
    {
        _sound.clip = FootSounds[num];
        _sound.Play();
    }

    public void Bite()
    {
        if(_biteSound != null)
        {
            Growl.clip = _biteSound;
            Growl.Play();
        }
    }

    private IEnumerator KillPlayer()
    {
        _animator.speed = 1.2f;
        yield return new WaitForSeconds(1.5f);
        _playerHealth.KillPLayer();
    }

    public void GetStunned()
    {
        if (_firstForm != null)
        {
            _firstForm.SetActive(false);
        }
        foreach (GameObject gameObject in _monsterForm)
        {
            gameObject.SetActive(true);
        }

        _playerHealth.StartChaseEffect();

        Health -= 1;
        _state = "chase";
        _nav.speed = _chaseSpeed;
        _animator.speed = _chaseSpeed;

        _animator.SetTrigger("getHit");
        _animator.SetBool("isSearching", false);

        if (Health <= 0)
        {
            _animator.SetBool("isSearching", false);
            _animator.speed = _simpleSpeed;
            _sound.pitch = 1;
            _nav.isStopped = true;
            _wait = 10f;

            _playerHealth.StopChasseEffect();
            Eyes.gameObject.SetActive(false);

            if(Stunnes <= 1)
            {
                StartLerp = true;
                StopAllCoroutines();
                StartCoroutine(StartLerpTimer());
                _wait = 2f;
            }

            _state = "stunned";
        }
    }

    //Check if monster can see the player
    public void CheckSight()
    {
        if (_state != "kill")
        {
            if (_alive && !_playerHealth.IsHiding)
            {
                RaycastHit rayHit;
                if (Physics.Linecast(Eyes.position, _player.transform.position, out rayHit, _ignorelayerMask, QueryTriggerInteraction.UseGlobal))
                {
                    //print("hit " + rayHit.collider.gameObject.name);
                    if (rayHit.collider.gameObject.CompareTag("Player"))
                    {
                        if (_state != "kill" && _state != "stunned")
                        {
                            _calmness = 0;
                            if (_firstForm != null)
                            {
                                _firstForm.SetActive(false);
                            }
                            foreach (GameObject gameObject in _monsterForm)
                            {
                                gameObject.SetActive(true);
                            }
                            StartLerp = false;
                            StopAllCoroutines();
                            StartCoroutine(StartLerpTimer());

                            if (_state != "kill")
                            {
                                _state = "chase";
                                _nav.speed = _chaseSpeed;
                                _animator.speed = _chaseSpeed;
                                _sound.pitch = 3;
                                _animator.SetBool("isSearching", false);
                            }

                            if (_state != "stunned")
                            {
                                _playerHealth.StartChaseEffect();
                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator StartLerpTimer()
    {
        float timeRemaining = 60;

        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (StartLerp)
            {
                foreach (Renderer renderer in _renderers)
                {
                    LerpNum = Mathf.Lerp(LerpNum, 1f, _dissolveStartSpeed);
                    renderer.material.SetFloat("_DissolveAmount", LerpNum);
                }
            }
            else
            {
                foreach (Renderer renderer in _renderers)
                {
                    LerpNum = Mathf.Lerp(LerpNum, 0f, _dissolveStopSpeed);
                    renderer.material.SetFloat("_DissolveAmount", LerpNum);
                }
            }
            yield return null;
        }
    }

    private void Update()
    {
        Debug.DrawLine(Eyes.position, _player.transform.position, Color.green);

        //if (StartLerp)
        //{
        //    foreach (Renderer renderer in _renderers)
        //    {
        //        LerpNum = Mathf.Lerp(LerpNum, 1f, _dissolveStartSpeed);
        //        renderer.material.SetFloat("_DissolveAmount", LerpNum);
        //    }
        //}
        //else
        //{
        //    foreach (Renderer renderer in _renderers)
        //    {
        //        LerpNum = Mathf.Lerp(LerpNum, 0f, _dissolveStopSpeed);
        //        renderer.material.SetFloat("_DissolveAmount", LerpNum);
        //    }
        //}

        if (_alive)
        {
            _animator.SetFloat("velocity", _nav.velocity.magnitude);

            //Stunned
            if(_state == "stunned")
            {
                if (_wait > 0f)
                {
                    _wait -= Time.deltaTime;
                    _animator.SetBool("isStunned", true);
                    _animator.ResetTrigger("getHit");
                }
                else
                {
                    Health = MaxHelath;
                    _nav.isStopped = false;
                    _animator.SetBool("isStunned", false);
                    _animator.ResetTrigger("getHit");
                    _playerHealth.StopChasseEffect();
                    Eyes.gameObject.SetActive(true);
                    _state = "idle";

                    Stunnes -= 1;
                    SaveCurrentProgress();

                    if(Stunnes <= 0)
                    {
                        _alive = false;
                        _isDead = true;
                        SaveCurrentProgress();
                        Destroy(this.gameObject);
                    }
                }
            }

            //Idle
            if (_state == "idle")
            {
                //Pick randoms pos
                Vector3 randomPos = Random.insideUnitSphere * _alertness;
                NavMeshHit navHit;
                NavMesh.SamplePosition(transform.position + randomPos, out navHit,20f,NavMesh.AllAreas);

                //go near the player
                if (_highAlert)
                {
                    NavMesh.SamplePosition(PlayerTransform.transform.position + randomPos, out navHit, 20f, NavMesh.AllAreas);
                    //each time, lose awareness of player general position
                    _alertness += 5f;

                    if(_alertness >= 5f)
                    {
                        _highAlert = false;
                        _nav.speed = _simpleSpeed;
                        _animator.speed = _simpleSpeed;
                        _sound.pitch = 1;
                    }
                }

                if (!_highAlert)
                {
                    _calmness += 5;

                    if (_calmness > 10 && MustHide)
                    {
                        _state = "return";
                    }
                    else
                    {
                        _state = "walk";
                        _nav.speed = _simpleSpeed;
                        _animator.speed = _simpleSpeed;
                        _sound.pitch = 1;
                    }
                }

                _nav.SetDestination(navHit.position);
            }
            //Walk
            if(_state == "walk")
            {
                if(_nav.remainingDistance <= _nav.stoppingDistance && !_nav.pathPending)
                {
                    _state = "search";
                    _wait = 5f;
                }
            }
            //Search
            if(_state == "search")
            {
                if(_wait > 0f)
                {
                    _animator.SetBool("isSearching", true);
                    _wait -= Time.deltaTime;
                    transform.Rotate(0f, 120f * Time.deltaTime, 0f);
                }
                else
                {
                    _animator.SetBool("isSearching", false);
                    _state = "idle";
                }
            }


            //Chase
            if(_state == "chase")
            {
                _nav.destination = _player.transform.position;

                //lose sight of player
                float distance = Vector3.Distance(transform.position, _player.transform.position);
                if(distance > 10f)
                {
                    _state = "hunt";
                }
                if(_player.GetComponent<PlayerHealth>().IsHiding)
                {
                    _state = "hunt";
                }
                if(distance < 1f && _playerHealth.IsHiding)
                {
                    _state = "hunt";
                }
                //kill the player
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

            //Kill
            if(_state == "kill")
            {
                _playerHealth.DisableCanvas();
                _alive = false;
                _animator.speed = 1.2f;

                if (Growl != null)
                {
                    Growl.pitch = 1.2f;
                    Growl.Play();
                }

                StartCoroutine(KillPlayer());
                _state = "stop";
            }

            //Hunt
            if(_state == "hunt")
            {
                if(_nav.remainingDistance <= _nav.stoppingDistance && !_nav.pathPending || _player.GetComponent<PlayerHealth>().IsHiding)
                {
                    _nav.speed = _simpleSpeed;
                    _animator.speed = _simpleSpeed;
                    _sound.pitch = 1;

                    _state = "search";
                    _wait = 5f;
                    _highAlert = true;
                    _playerHealth.Chase = false;
                    _playerHealth.StopChasseEffect();
                    _alertness = 5f;
                    CheckSight();
                }
            }

            //Hide
            if(_state == "hiding")
            {
                if (_nav.remainingDistance <= _nav.stoppingDistance && !_nav.pathPending)
                {
                    if (_firstForm != null)
                    {
                        _firstForm.SetActive(true);
                    }
                    foreach (GameObject gameObject in _monsterForm)
                    {
                        gameObject.SetActive(false);
                    }
                    StartLerp = true;
                    StartCoroutine(StartLerpTimer());

                    _wait = 30f;

                    if (MustHide)
                    {
                        _state = "stop";
                    }

                    if (MustHide && _cuurentHidingPosition != null)
                        this.transform.rotation = _cuurentHidingPosition.rotation;
                }
            }

            //Return to random hiding place
            if(_state == "return")
            {
                int randomNum = (int)Random.Range(0f, _hidingPositions.Length);
                _cuurentHidingPosition = _hidingPositions[randomNum];
                _nav.SetDestination(_hidingPositions[randomNum].position);
                _calmness = 0;
                _state = "hiding";
            }



            if (_state == "stop")
            {
                if (_wait > 0f)
                {
                    _wait -= Time.deltaTime;
                }
                else
                {
                    Health = MaxHelath;
                    _nav.isStopped = false;
                    _animator.SetBool("isStunned", false);
                    _animator.ResetTrigger("getHit");
                    _state = "return";
                }
            }

            //_nav.SetDestination(Player.transform.position);
        }
    }
}
