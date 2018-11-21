using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerCharacter : MonoBehaviour
{
	
	public float Speed;
	public float JumpForce;
	public GameObject Club;
	private void Start()
	{
		LuaUtil.ToLua("Player");
		LuaUtil.CallFunc("Player.Start",gameObject,Speed,JumpForce,Club);
	}
	private void Update()
	{
		LuaUtil.CallFunc("Player.Update");
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		LuaUtil.CallFunc("Player.CheckCollision",other);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		LuaUtil.CallFunc("Player.CheckCollision",other);

	}
}
