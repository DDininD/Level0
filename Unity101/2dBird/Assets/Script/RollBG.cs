using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollBG : MonoBehaviour
{
	private Vector2 oPosition;

	public Transform trans1;
	public Transform trans2;

	private void Awake()
	{
		oPosition = trans2.position;
	}

	void Update ()
	{
		transform.position += new Vector3(Time.deltaTime * 2,0,0);

		if (trans1.position.x >= 31.37f)
		{
			trans1.position = oPosition;
		}
		
		if (trans2.position.x >= 31.37f)
		{
			trans2.position = oPosition;
		}
		
		
	}
}
