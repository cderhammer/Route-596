using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    
    [SerializeField]
    private AudioClip[] clips;
    private AudioSource audioSource;

    private void Awake(){
        audioSource = GetComponent<AudioSource>();
    }

    private void Step(){
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }
    private void walkStep(){
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }
    private void jump(){
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }
    private void jumpLand(){
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }
    private AudioClip GetRandomClip(){
        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }

}