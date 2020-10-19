using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSoure;
    [SerializeField]private AudioClip jumpAudio, hurtAudio, collectAudio,deathAudio;

    private void Awake()
    {
        instance = this;
    }


    public void JumpAudio()
    {
        audioSoure.clip = jumpAudio;
        audioSoure.Play();
    }

    public void HurtAudio()
    {
        audioSoure.clip = hurtAudio;
        audioSoure.Play();
    }

    public void CollectAudio()
    {
        audioSoure.clip = collectAudio;
        audioSoure.Play();
    }

    public void DeathAudio() 
    {
        audioSoure.clip = deathAudio;
        audioSoure.Play();
    }
}
