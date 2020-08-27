using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class writeText : MonoBehaviour {

	public GameObject card;
	public GameObject notePanel;
	public TextMeshProUGUI newText;

	GameController gameController;


	// Use this for initialization
	void Start () {
		//get the gamecontroller object for paused boolean
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
        if (gameControllerObject != null) {
            gameController = gameControllerObject.GetComponent<GameController> ();
        }
        if (gameController == null) {
            Debug.Log ("Cannot find 'GameController' script");
        }
	}
	
	// Update is called once per frame
	void Update () {

		//text.GetComponent<Text>().text = inputFieldCo.text;
	}

	public void WriteNote(){
		//when button clicked, set note to the card object and unpause time
		card.GetComponent<DragObject>().note = newText.text;
		notePanel.SetActive(false);
		Time.timeScale = 1;
		gameController.paused = false;


	}

	public void CloseNote(){
		//dont save note but close panel and un pause time
		notePanel.SetActive(false);
		Time.timeScale = 1;
		gameController.paused = false;
	}

}
