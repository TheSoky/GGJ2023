using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioClip mainmenuMusicClip;

    [SerializeField]
    AudioSource musicSource;

    [SerializeField]
    Button musicButton;

    [SerializeField]
    Button soundButton;

    [SerializeField]
    Sprite[] musicSprites;

    [SerializeField]
    Sprite[] soundSprites;


    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = mainmenuMusicClip;
        musicSource.Play();
        musicButton.onClick.AddListener(() =>
        {
            ToggleMusic();
        });
        soundButton.onClick.AddListener(() =>
        {
            ToggleSound();
        });
        PlayerPrefs.SetInt("Music", 1);
        PlayerPrefs.SetInt("Sound", 1);
    }

    public void ToggleMusic(){
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
            musicButton.image.sprite = musicSprites[1];
            PlayerPrefs.SetInt("Music", 0);
        }
        else
        {
            musicSource.clip = mainmenuMusicClip;
            musicSource.Play();
            musicButton.image.sprite = musicSprites[0];
            PlayerPrefs.SetInt("Music", 1);
        }
    }

    public void ToggleSound()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            soundButton.image.sprite = soundSprites[1];
            PlayerPrefs.SetInt("Sound", 0);
        }
        else
        {
            soundButton.image.sprite = soundSprites[0];
            PlayerPrefs.SetInt("Sound", 1);
        }
    }

}
