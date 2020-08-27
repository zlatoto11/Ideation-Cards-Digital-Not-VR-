using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenInstructionsPanel : MonoBehaviour {

	public GameObject InstructionPanel;

	//used to open instruction panel from main menu
	public void OpenPanel () {
		if (InstructionPanel != null) {
			//if its already active then close else open
			if (InstructionPanel.activeSelf == true) {
				InstructionPanel.SetActive (false);
			} else {
				InstructionPanel.SetActive (true);
			}
		}
	}
}