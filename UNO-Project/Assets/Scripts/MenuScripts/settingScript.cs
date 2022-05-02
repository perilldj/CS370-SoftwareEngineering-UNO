using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class settingScript : MonoBehaviour
{
    public AudioMixer volumeController;
    public void Back () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    public void SetVolume(float volume) {
        volumeController.SetFloat("volume", volume);
    }
}
