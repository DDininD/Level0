using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Pig : MonoBehaviour
{

    public int score;
    public GameObject sprite;
    
    public void Die()
    {
        Destroy(transform.parent.gameObject,2.0f);
        FindObjectOfType<GameMode>().pigCount--;

    }

    private void Update()
    {
        sprite.transform.position = transform.position;
    }

    public void Show()
    {
        FindObjectOfType<HudController>().UpdateScore(score);

        sprite.SetActive(true);
        gameObject.SetActive(false);
    }
}
