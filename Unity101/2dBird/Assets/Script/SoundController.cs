using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    public AudioSource bgm;
    public AudioSource bgmEnd;

    bool played = false;
    

	void Awake () {
        bgm.Play();
        bgmEnd.Stop();
	}

    public void EndGame()
    {
        if (!played)
        {
            bgm.Stop();
            bgmEnd.Play();
            played = true;
            Debug.Log("Play");
        }
    } 
	
}
