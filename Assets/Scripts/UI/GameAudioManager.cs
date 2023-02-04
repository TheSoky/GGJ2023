using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameAudioManager : MonoBehaviour
{

    [SerializeField]
    private AudioClip gameMusicClip;

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioSource soundSource;

    [SerializeField]
    private Button musicButton;

    [SerializeField]
    private Button soundButton;

    [SerializeField]
    private Sprite[] musicSprites;

    [SerializeField]
    private Sprite[] soundSprites;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            musicSource.clip = gameMusicClip;
            musicSource.Play();
        }
        
        musicButton.onClick.AddListener(() =>
        {
            ToggleMusic();
        });
        soundButton.onClick.AddListener(() =>
        {
            ToggleSound();
        });
    }

    public void ToggleMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
            musicButton.image.sprite = musicSprites[1];
            PlayerPrefs.SetInt("Music", 0);
        }
        else
        {
            musicSource.clip = gameMusicClip;
            musicSource.Play();
            musicButton.image.sprite = musicSprites[0];
            PlayerPrefs.SetInt("Music", 1);
        }
    }

    public void ToggleSound()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            soundSource.Stop();
            soundButton.image.sprite = soundSprites[1];
            PlayerPrefs.SetInt("Sound", 0);
        }
        else
        {
            //add sound into clip
            //soundSource.clip = gameMusicClip;
            soundSource.Play();
            soundButton.image.sprite = soundSprites[0];
            PlayerPrefs.SetInt("Sound", 1);
        }
    }
}
