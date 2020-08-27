using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadGameClick : MonoBehaviour {

	// Use this for initialization
	public TextMeshProUGUI textName;
	public GameObject saving;
	
	public void LoadGame() {
		saving.GetComponent<ObjectManager>().OnLoad(textName.text);
	}
}
