using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

    public GameObject cardPrefab, deckPrefab;

    GameObject[] objects;

    public TextMeshProUGUI saveName;
    public GameObject SavePanel;
    public GameObject LoadPanel;
    // Use this for initialization
    public string noteOnCard;

    GameController gameController;
    void Start () {

        GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
        if (gameControllerObject != null) {
            gameController = gameControllerObject.GetComponent<GameController> ();
        }
        if (gameController == null) {
            Debug.Log ("Cannot find 'GameController' script");
        }

        objects = new GameObject[] { cardPrefab, deckPrefab };
    }

    public void OpenSave () {
        if (SavePanel != null) {
            if (SavePanel.activeSelf == true) {
                gameController.paused = false;
                SavePanel.SetActive (false);
                Time.timeScale = 1;
                
            } else {
                gameController.paused = true;
                SavePanel.SetActive (true);
                LoadPanel.SetActive(false);
                Time.timeScale = 0;
            }
        }
    }
    public void OpenLoad () {
        if (LoadPanel != null) {
            Debug.Log("Pressed");
            if (LoadPanel.activeSelf == true) {
                gameController.paused = false;
                LoadPanel.SetActive (false);
                Time.timeScale = 1;
                
            } else {
                gameController.paused = true;
                LoadPanel.SetActive (true);
                SavePanel.SetActive(false);
                Time.timeScale = 0;
                
            }
        }
    }

    public void OnSave () {
        print ("Count at save = " + SaveData.current.objects.Count);
        SerializationManager.Save (saveName.text, SaveData.current);
        print (saveName.text);

        Time.timeScale = 1;
        gameController.paused = false;
        SavePanel.SetActive (false);
    }

    public void OnLoad (string gameToLoadName) {
        Time.timeScale = 1;
        gameController.paused = false;
        LoadPanel.SetActive (false);

        GameEvents.current.dispatchOnLoadEvent ();

        SaveData.current = (SaveData) SerializationManager.Load (gameToLoadName);

        print ("Count at load = " + SaveData.current.objects.Count);

        //print(SaveData.current.objects.Count);
        for (int i = 0; i < SaveData.current.objects.Count; i++) {
            ObjectData currentObj = SaveData.current.objects[i];
            GameObject obj = Instantiate (objects[(int) currentObj.objectType]);
            GameObject cube;
            if ((int) currentObj.objectType == 0) {
                cube = obj.transform.Find ("Cube").gameObject;
                cube.GetComponent<CardData> ().cardList = currentObj.cardList;
            } else {
                cube = obj.transform.Find ("Deck").gameObject;
                cube.GetComponent<DeckData> ().cardList = currentObj.cardList;
            }

            cube.GetComponent<DragObject> ().flipped = currentObj.flipped;
            cube.GetComponent<DragObject> ().rot = currentObj.rot;
            cube.GetComponent<DragObject> ().note = currentObj.note;

            ObjectHandler objectHandler = cube.GetComponent<ObjectHandler> ();
            objectHandler.objectData = currentObj;
            objectHandler.transform.position = currentObj.position;
            objectHandler.transform.rotation = currentObj.rotation;
            //noteOnCard = currentObj.note;

            //buttonObject.GetComponentInChildren<TextMeshProUGUI> ().text = saveFiles[index].Replace (Application.persistentDataPath + "/saves/", "");

        }
    }
}