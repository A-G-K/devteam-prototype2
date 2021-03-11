using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private void Awake()
    {
        ServiceLocator.Current.Get<AudioManager>().Controller = this;
    }

    public void playSound(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip);
    }
}
