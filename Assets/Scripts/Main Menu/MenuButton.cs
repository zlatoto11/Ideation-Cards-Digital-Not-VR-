using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour {
	[SerializeField] MenuButtonController menuButtonController;
	[SerializeField] Animator animator;
	[SerializeField] AnimatorFunctions animatorFunctions;
	[SerializeField] int thisIndex;
	bool mouseOver = false;

	// Update is called once per frame
	void Update () {
		//check whether the index of button currently over is this object, index can be changed by keyboard  arrows or by mouse hovering
		if (menuButtonController.index == thisIndex) {
			//start the animation
			animator.SetBool ("selected", true);
			//if enter key is pressed or button is clicked then do the selection animation
			if (Input.GetAxis ("Submit") == 1 || (mouseOver && Input.GetMouseButtonDown(0))) {
				animator.SetBool ("pressed", true);
			}
			//make sure button can only be pressed once 
			else if (animator.GetBool ("pressed")) {
				animator.SetBool ("pressed", false);
				animatorFunctions.disableOnce = true;
			}
		}
		//go back to not selected animation 
		else {
			animator.SetBool ("selected", false);
		}
	}

	public void changeIndexToThis () {
		//change the index of the currently selected button to this
		menuButtonController.index = thisIndex;
		mouseOver = true;
		//Debug.Log(gameObject.name);
	}

	public void setSelectedtoFalse(){
		//unselects the button
		animator.SetBool ("selected", false);
		mouseOver = false;
	}
}
