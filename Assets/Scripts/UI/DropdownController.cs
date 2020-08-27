using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownController : MonoBehaviour {
    GameController gameController;
    List<string> categoriesList;
    Dropdown myDropdown;
    public GameObject stackPrefab;
    public List<bool> availableSpaces;
    public GameObject spawnTable;
    List<List<Cards>> all_cards;

    // Use this for initialization
    void Start () {
        myDropdown = GetComponent<Dropdown>();
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
        all_cards = gameController.getCards ();
        categoriesList = gameController.getCategories();
        PopulateDropdown(categoriesList);

         myDropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(myDropdown);
        });

        myDropdown.value = myDropdown.options.Count - 1;
        //Initialise the Text to say the first state of the Toggle
        Debug.Log(myDropdown.options[myDropdown.value].text);

        availableSpaces = new List<bool>(){true,true,true,true,true,true,true,true,true};
    }
	
	// Update is called once per frame
	void Update () {

	}

    //Output the new state of the Toggle into Text
     //Ouput the new value of the Dropdown into Text
    void DropdownValueChanged(Dropdown change)
    {
         Debug.Log(myDropdown.options[myDropdown.value].text);
         
         for (int i = 0; i < availableSpaces.Count; i++){
            if (availableSpaces[i]) {
                //new Vector3(24,13,18)
                Vector3 pos = spawnTable.transform.Find("Quad (" + i + ")").transform.position;
                GameObject newStack = Instantiate(stackPrefab, new Vector3 (pos.x,pos.y + 1.0f,pos.z), Quaternion.identity);
                availableSpaces[i] = false;
                GameObject deck = newStack.transform.Find("Deck").gameObject;
                deck.GetComponent<DeckData>().catGlobal = myDropdown.options[myDropdown.value].text;
                return;
            }
         }
    }

    public void SetAvailableSpace(int index, bool available){
        availableSpaces[index] = available;
    }

    void PopulateDropdown(List<string> optionsArray)
    {
        myDropdown.ClearOptions();

        myDropdown.AddOptions(optionsArray);
        
        Debug.Log("1");
        foreach (string el in optionsArray)
        {

            Debug.Log("----------------" + el);
        }
        Debug.Log("2");
    }
}
