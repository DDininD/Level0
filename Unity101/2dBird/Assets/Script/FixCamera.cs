using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixCamera : MonoBehaviour {

	void Update ()
	{
		transform.position = FindObjectOfType<CameraFollow>().o_pos;
	}
}
