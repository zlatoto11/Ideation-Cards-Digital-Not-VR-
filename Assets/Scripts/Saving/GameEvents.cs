using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//events used with saving/loading
public class GameEvents : MonoBehaviour {

	public static GameEvents current;

	private void Awake(){
		current = this;
	}

	//object added to current game objects
	public event Action onLoadEvent;
	
	public void dispatchOnLoadEvent(){
		if (onLoadEvent != null)
		{
			onLoadEvent();
		}
	}

	//object removed from current game objects
	public event Action DestroyMe;
	

}
