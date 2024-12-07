﻿using System;
using System.Collections;
using System.Collections.Generic;
using GameEvent;
using UnityEngine;

public class SoundManager: UnitySingleton<SoundManager>,
                            IGameEventListener<PlayerAttackEvent>,
                            IGameEventListener<PlayerJumpEvent>,
                            IGameEventListener<PlayerDieEvent>,
                            IGameEventListener<PlayerSkillEvent>
                            
{
    [Header("SFX")]
    [SerializeField] private AudioClip _playerAttackSFX;
    [SerializeField] private AudioClip _playerOnHitSFX;
    [SerializeField] private AudioClip _playerOnDeadSFX;
    [SerializeField] private AudioClip _playerJumpOneSFX;
    [SerializeField] private AudioClip _playerJumpTwoSFX;
    [SerializeField] private AudioClip _playerPerformSkillSFX;

    
    [Header("Audio Sources")]
    private AudioSource _musicSource;
    private AudioSource _sfxSource;
    private Queue<AudioSource> _sfxQueue = new();
    [Header("Audio Settings")]
    [SerializeField] [Range(0f, 1f)]private float _musicVolume = 1f;
    [SerializeField] [Range(0f, 1f)] private float _sfxVolume = 1f;

    [Header("Controlling")]
    [SerializeField] private int _maxSfxPoolSize = 10;


    protected override void SingletonAwake()
    {
        base.SingletonAwake();
        _musicSource = gameObject.AddComponent<AudioSource>();
        for (int i = 0; i < _maxSfxPoolSize; i++) 
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            _sfxQueue.Enqueue(audioSource);
        }
    }

    protected override void SingletonStarted()
    {
        base.SingletonStarted();
        EventAggregator.Register<PlayerAttackEvent>(this);
        EventAggregator.Register<PlayerJumpEvent>(this);
        EventAggregator.Register<PlayerDieEvent>(this);
        EventAggregator.Register<PlayerSkillEvent>(this);
    }

    protected override void SingletonOnDestroy()
    {
        base.SingletonOnDestroy();
        EventAggregator.Unregister<PlayerAttackEvent>(this);
        EventAggregator.Unregister<PlayerJumpEvent>(this);
        EventAggregator.Unregister<PlayerDieEvent>(this);
        EventAggregator.Unregister<PlayerSkillEvent>(this);
    }

    public void PlaySfx(AudioClip audio)
    {
        _sfxSource = _sfxQueue.Dequeue();
        _sfxSource.clip = audio;
        _sfxSource.volume = _sfxVolume;
        _sfxSource.Play();

        StartCoroutine(BackToQueue(audio.length));
    }

    public void PlayMusic(AudioClip music, bool loop = true)
    {
        if (_musicSource.isPlaying)
            _musicSource.Stop();

        _musicSource.clip = music;
        _musicSource.loop = loop;
        _musicSource.volume = _musicVolume;
        _musicSource.Play();
    }

    private IEnumerator BackToQueue(float length)
    {
        var startTime = Time.time;
        while (startTime + length > Time.time) {
            yield return null;
        }
        _sfxQueue.Enqueue(_sfxSource);
    }

    public void SetMusicVolume(float volume)
    {
        _musicVolume = Mathf.Clamp01(volume);
        _musicSource.volume = _musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        _sfxVolume = Mathf.Clamp01(volume);
        foreach (var source in _sfxQueue)
        {
            source.volume = _sfxVolume;
        }
    }

    public void Handle(PlayerAttackEvent @event)
    {
        PlaySfx(_playerAttackSFX);
    }

    public void Handle(PlayerJumpEvent @event)
    {
        if(@event.JumpCount == 1)
        {
            PlaySfx(_playerJumpOneSFX);
        }
        else
        {
            PlaySfx(_playerJumpTwoSFX);
        }
    }

    public void Handle(PlayerDieEvent @event)
    {
        PlaySfx(_playerOnDeadSFX);
    }

    public void Handle(PlayerSkillEvent @event)
    {
        //If another skill perform please implement this again
        PlaySfx(_playerPerformSkillSFX);
    }
}
