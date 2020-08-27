using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

//used to delete saves
public class DeleteSave : MonoBehaviour {

	public TextMeshProUGUI filepath;
	public GameObject saving;

	// Start is called before the first frame update
	void Start () {
		//get the saving object, this holds vital information
		saving = GameObject.FindWithTag ("Saving");
	}
	public void SaveToDelete () {
		//Debug.Log (Application.persistentDataPath + "/saves/" + filepath.text);
		//delete the file at the specified path
		File.Delete (Application.persistentDataPath + "/saves/" + filepath.text);
		//refreshes the load screen
		saving.GetComponent<SaveManager>().ShowLoadScreen();
	}
}