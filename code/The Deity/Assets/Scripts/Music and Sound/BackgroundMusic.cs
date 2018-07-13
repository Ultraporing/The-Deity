using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour {
    //Lea Kohl
    //script to randomize our playlist
    //add all the songs to this array
    public AudioClip[] m_BackgroundMusic;
    AudioSource m_MySong;
    int currentSong;
	
	void Start ()
    {
        currentSong = 0;
        m_MySong = GetComponent<AudioSource>();
        m_MySong.loop = false;
	}

	void Update () {
        if (!m_MySong.isPlaying)
        {
            m_MySong.clip = GetRandomClip();
            m_MySong.Play();
        }
	}

    private AudioClip GetRandomClip()
    {
        return m_BackgroundMusic[Random.Range(0, m_BackgroundMusic.Length - 1)];
    }
}
