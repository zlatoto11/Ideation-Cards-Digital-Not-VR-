using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;
using System.Text;
using SmartDLL;

public class GameController : MonoBehaviour {

	GameObject currentCard = null;

    public bool paused = false;

    public GameObject renamePanel;

    public SmartFileExplorer fileExplorer = new SmartFileExplorer();

	Texture2D tex;
    public GameObject prefab;

	private int numOfCategories;
	string path;
	bool inList;

    public Texture mainTexture;

    private List<Cards> cardsOpp, cardsCha, cardsQue;
    private int oLength, cLength, qLength;

	List<string> category_List;
	public List<List<Cards>> categories_List;
	List<Cards> cards;

    string pathOfImage;

    Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        //initialise all lists
		cards = new List<Cards>();
		category_List = new List<string>();
		categories_List = new List<List<Cards>>();
		numOfCategories = 0;

        /*System.Diagnostics.Process p = new System.Diagnostics.Process();
        p.StartInfo = new System.Diagnostics.ProcessStartInfo("explorer.exe");
        p.StartInfo.
        p.Start();*/

        //path = EditorUtility.OpenFilePanel("Choose deck to use", "", "csv");	//Opens File Explorer

        //the initial directory the file explorer will open in
        string initialDir = @"D:\Unity\Ideation Cards Digital (Not VR)\Assets\Resources";
        bool restoreDir = true;
        string title = "Open a csv file";
        string defExt = "csv";
        string filter = "csv files (*.csv)|*.csv";

        //open the file explorer to find the csv
        fileExplorer.OpenExplorer(initialDir,restoreDir,title,defExt,filter);

        //check it got the file ok
        if (fileExplorer.resultOK)
            path = fileExplorer.fileName;

        //Debug.Log(path);

        //read the text in the csv
        string readText = File.ReadAllText(path);
        //splits by line
        string[] data = readText.Split('\n');
        //split the file by ','
        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(',');

            //initialise cards objects with data fro each line
            Cards c = new Cards(row);
            cards.Add(c);
        }

        foreach (Cards c in cards)
        {
            //print (c.Category);
			inList = false;

            //find all the categories in the csv, add them to the list
			foreach (string cat in category_List)
			{
				if (c.Category == cat){
					inList = true;
				}
			}
			if (!inList){
				category_List.Add(c.Category);
				numOfCategories++;
				//Debug.Log("" + c.Category + ", " + numOfCategories);
			}
            //load the images of front and back into textures stored in the cards objects
            pathOfImage = Path.GetDirectoryName(path) + c.Front_Image;
			c.frontImg = loadImages(pathOfImage);

            pathOfImage = Path.GetDirectoryName(path) + c.Back_Image;
            c.backImg = loadImages(pathOfImage);

            //Debug.Log("" + c.ID + " " + c.Label + " " + c.Name + " " + c.Category + " " + c.Type + " " + c.Text + " " + c.Image + " " + c.Use_Count);
        }

        //add each category list to a list
		foreach (string cat in category_List){
			categories_List.Add(cards.FindAll(c => c.Category == cat));
		}
		int num = 0;
		foreach (List<Cards> el in categories_List){
			foreach (Cards c in el){
				//Debug.Log("Game controller " + c.Category);
			}
			num++;
		}

    }

	public List<List<Cards>> getCards(){
		return categories_List;
	}

    //used for loading front and back images
    Texture2D loadImages(string path)
    {
        //reads the file
        byte[] byteArray = File.ReadAllBytes(@path);

        //initialise a texture
        Texture2D sampleTexture = new Texture2D(2,2);

        //load the data from file into texture
        bool isLoaded = sampleTexture.LoadImage(byteArray);
        
        //if loaded correctly then return texture
		if (isLoaded)
        {
			return sampleTexture;
        }
        else
        {
            return null;
        }

        /*GameObject image = GameObject.Find("RawImage");
        if (isLoaded)
        {
            image.GetComponent<RawImage>().texture = sampleTexture;
        }*/
		
    }
	
	// Update is called once per frame
	void Update () {
	}

	public void setCurrentCard(GameObject card) {
		currentCard = card;
	}

	public GameObject getCurrentCard(){
		return currentCard;
	}
    public List<string> getCategories(){
        return category_List;
    }

    //find card in the list, used when loading in sessions
    public Cards FindCard(string ID)
    {
        foreach (Cards c in cards)
        {
            if (ID == c.ID)
            {
                return c;
            }
            
        }
        return null;
    }
}
