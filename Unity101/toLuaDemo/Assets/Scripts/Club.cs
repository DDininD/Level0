using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Club : MonoBehaviour
{

	void Start () {
		LuaUtil.ToLua("Club");
		LuaUtil.CallFunc("Club.Start", gameObject);
	}
	

	void Update () {
		LuaUtil.CallFunc("Club.Update");
	}
}
