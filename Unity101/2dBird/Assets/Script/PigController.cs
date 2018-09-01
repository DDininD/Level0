using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigController : MonoBehaviour {

    Pig pig;

	void Start ()
	{
	    pig = GetComponent<Pig>();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("bird"))
        {
            pig.Show();
            pig.Die();
            
        }
    }

}
