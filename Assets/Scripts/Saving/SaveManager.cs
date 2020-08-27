using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour {

	public GameObject loadButtonPrefab;
	public Transform loadArea;
	public string[] saveFiles;

	public void ShowLoadScreen () {
		GetLoadFiles ();
		foreach (Transform button in loadArea) {
			Destroy (button.gameObject);
		}

		for (int i = 0; i < saveFiles.Length; i++) {
			GameObject buttonObject = Instantiate (loadButtonPrefab);
			buttonObject.transform.SetParent (loadArea, false);

			var index = i;
			buttonObject.GetComponent<Button> ().onClick.AddListener (() => {
				GetComponent<ObjectManager> ().OnLoad (saveFiles[index]);
			});
			buttonObject.GetComponentInChildren<TextMeshProUGUI> ().text = saveFiles[index].Replace (Application.persistentDataPath + "/saves/", "");
		}
	}
		public void GetLoadFiles () {
		if (!Directory.Exists (Application.persistentDataPath + "/saves/")) {
			Directory.CreateDirectory (Application.persistentDataPath + "/saves/");
		}

		saveFiles = Directory.GetFiles (Application.persistentDataPath + "/saves/");
	}

}