using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class RenameFile : MonoBehaviour {

	public TextMeshProUGUI originalName;
	public TextMeshProUGUI newName;
	GameObject renamePanel;
	public GameObject saving;

	GameController gameController;
	// Start is called before the first frame update
	void Start () {

		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
        if (gameControllerObject != null) {
            gameController = gameControllerObject.GetComponent<GameController> ();
        }
        if (gameController == null) {
            Debug.Log ("Cannot find 'GameController' script");
        }

		renamePanel = gameController.renamePanel;
		saving = GameObject.FindWithTag ("Saving");
	}
	public void FileToRename () {
		print("OG name in rename: " + originalName.text);
		File.Move (Application.persistentDataPath + "/saves/" + originalName.text, Application.persistentDataPath + "/saves/" + newName.text + ".save");
		renamePanel.SetActive(false);
		saving.GetComponent<SaveManager> ().ShowLoadScreen ();
	}
	public void OpenRenamePanel(){
		if(renamePanel != null){
			if (renamePanel.activeSelf == true) {
                renamePanel.SetActive (false);
                Time.timeScale = 1;
            } else {
                renamePanel.SetActive (true);
                Time.timeScale = 0;
				print("OG name in load: " + originalName.text);
				renamePanel.transform.Find("Button").GetComponent<RenameFile>().originalName = originalName;
            }
		}else{
			print("Rename panel null");
		}
	}
} 