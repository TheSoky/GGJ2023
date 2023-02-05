using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogAudio : MonoBehaviour
{
    [SerializeField]
    private AudioClip musicClip;

    [SerializeField]
    private AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Music")==1)
        {
            musicSource.clip = musicClip;
            musicSource.Play();
        }
        else
        {
            musicSource.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
