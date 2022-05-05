using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

    public class MusicPlayer : MonoBehavior
    Author: perilldj

*/

public class MusicPlayer : MonoBehaviour {

    public AudioClip[] clips;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start() {
        audioSource.loop = false;

    }

    private AudioClip GetRandomClip() {
        return clips[Random.Range(0, clips.Length)];
    }

    // Update is called once per frame
    void Update() {
        if(!audioSource.isPlaying) {
            audioSource.clip = GetRandomClip();
            audioSource.Play();
        }
    }
}
