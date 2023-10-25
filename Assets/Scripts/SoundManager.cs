using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public GameObject BGSound;

    private void Awake()
    {
        Instance = this;
    }

    public void SFXPlay(string SFXName, AudioClip clip,float vol)
    {
        GameObject go = new GameObject(SFXName);
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.clip = clip;
        audioSource.volume = 0.1f;
        audioSource.Play();

        Destroy(go,clip.length);
    }

    public void BGPlay(string BGName, AudioClip clip)
    {
        GameObject go = new GameObject(BGName);
        BGSound = go;
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.volume = 0.1f;
        audioSource.Play();
        
    }
}
