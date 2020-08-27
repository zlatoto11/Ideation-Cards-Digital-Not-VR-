using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// U	pdate is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		//uses the destroyme function to destroy so that object is correctly removed from save data before being removed
		other.GetComponent<ObjectHandler>().DestroyMe();
	}

}
