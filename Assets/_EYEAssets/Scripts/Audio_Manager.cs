using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _generalSource;

    int _musicID;
    [SerializeField] private AudioClip[] _musicTracks;


    void Start()
    {
        PlayMusicAudio();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeMusicAudio();

        if (_musicSource.isPlaying == false)
            ChangeMusicAudio();
    }

    void ChangeMusicAudio()
    {
        _musicID++;
        if (_musicID > _musicTracks.Length - 1)
            _musicID = 0;

        PlayMusicAudio();
    }

    void PlayMusicAudio()
    {
        _musicSource.Stop();

        if (_musicTracks.Length > 0)
            _musicSource.PlayOneShot(_musicTracks[_musicID]);
    }
}
