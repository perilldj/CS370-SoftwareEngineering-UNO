using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

/*
    Static Class: SoundID
    Author: perilldj
    Description: Used to index sounds in the SFX array.
*/

public static class SoundID {

    public const int CARD_HANDLE_ONE = 0;
    public const int CAND_HANDLE_TWO = 1;

    public const int REVERSE_SOUND = 2;
    public const int SKIP_SOUND = 3;
    public const int PLUS_TWO = 4;
    public const int PLUS_FOUR = 5;
    public const int COLOR_CHANGE = 6;
    public const int WIN_SOUND = 7;

}

/*
    public class SoundManager : MonoBehaviour
    Author: perilldj
    Description: Manages the playing of sound effects throughout the game.
*/

public class SoundManager : MonoBehaviour {
    
    [SerializeField]
    private List<Sound> sounds = new List<Sound>();

    private System.Random ran = new System.Random();

    void Awake() {
        foreach(Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void playSound(int id) {
        Sound sound = sounds[id];
        if(sound != null)
            sound.source.Play();
    }

    /* Used for some variety for the sound of playing a standard card to the deck.
       Even though it's only two sounds, it sounds a lot more natural than the same one over and over */
    public void playRandomCardSound() {
        playSound(ran.Next(0, 2));
    }

}
