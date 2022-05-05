using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

    public class MusicPlayer : MonoBehavior
    Author: perilldj
    Description: Used to randomly play songs while the game is in play.

*/

public class MusicPlayer : MonoBehaviour {

    public AudioClip[] clips;
    public AudioSource audioSource;

    void Start() {
        audioSource.loop = false;
    }

    private AudioClip GetRandomClip() {
        return clips[Random.Range(0, clips.Length)];
    }

    void Update() {
        if(!audioSource.isPlaying) {
            audioSource.clip = GetRandomClip();
            audioSource.Play();
        }
    }
    
}
