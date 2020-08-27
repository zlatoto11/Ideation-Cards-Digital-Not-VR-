using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCardsPanel : MonoBehaviour {

	public int index;
	public GameObject deckDropdown;
	public bool available;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//if something is in the space then its availabily is false
	private void OnCollisionEnter(Collision other) {
		deckDropdown.GetComponent<DropdownController>().SetAvailableSpace(index,false);
		available = false;
	}

	//if something isnt in the space then its availabily is true
	private void OnCollisionExit(Collision other) {
		deckDropdown.GetComponent<DropdownController>().SetAvailableSpace(index,true);
		available = true;
	}
}
