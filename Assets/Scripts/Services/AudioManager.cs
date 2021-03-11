using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : IGameService
{
    public AudioController Controller { get; set; }

    public void PlaySound(AudioClip audioClip)
    {
        Controller.playSound(audioClip);
    }
}
