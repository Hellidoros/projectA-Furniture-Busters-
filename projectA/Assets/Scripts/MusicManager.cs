using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField] private AudioClip _atmosphereSound1;
    [SerializeField] private AudioClip _monsterNearSound1;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void StartMonsterNearSound()
    {
        _audioSource.clip = _monsterNearSound1;
        _audioSource.Play();
    }

    public void StartAtmosphereSound()
    {
        _audioSource.clip = _atmosphereSound1;
        _audioSource.Play();
    }


}
