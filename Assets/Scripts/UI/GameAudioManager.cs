using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameAudioManager : MonoBehaviour
{

    [SerializeField]
    private AudioClip gameMusicClip;

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

    }

    public void ToggleSound()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {

        }
        else
        {

        }
    }
}
