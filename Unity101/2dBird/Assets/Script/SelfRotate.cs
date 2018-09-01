using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotate : MonoBehaviour {


	void Update () {
        transform.Rotate(Vector3.forward , 360 * Time.deltaTime);
	}
}
