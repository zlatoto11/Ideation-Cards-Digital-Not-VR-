using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckData : MonoBehaviour {

    GameController gameController;

    public LinkedList<KeyValuePair<Cards, bool>> cardList;

    public KeyValuePair<Cards, bool> topCard, bottomCard;

    List<List<Cards>> all_cards;
    List<string> categories;

    public float height, MAX_HEIGHT = 1.0f;

    public string catGlobal = "";

    public GameObject cardPrefab;

    void Awake () {
        //initialize the card list
        cardList = new LinkedList<KeyValuePair<Cards, bool>> ();
    }

    // Use this for initialization
    void Start () {
        //Find the gamecontroller object within the scene
        GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
        if (gameControllerObject != null) {
            gameController = gameControllerObject.GetComponent<GameController> ();
        }
        if (gameController == null) {
            Debug.Log ("Cannot find 'GameController' script");
        }

        //Get imported cards from the game controller
        all_cards = gameController.getCards ();
        categories = gameController.getCategories ();

        //Set the category of the card to the catGlobdal string
        setCategory (catGlobal);


        //set the height of the stack to the correct height dependant on lengthof linked list
        height = 0.04f * cardList.Count;
        if (height > MAX_HEIGHT)
            height = MAX_HEIGHT;

        //Needs a try in case object has been destroyed - object may have been destroyed in a specifc case when a category with one card is loaded in from the category dropdown
        try{
            topCard = cardList.First.Value;
            bottomCard = cardList.Last.Value;
            setViewableTopCards ();
        } catch (System.NullReferenceException e){
            print("Card not stack error");
        }

        
    }

    // Update is called once per frame
    void Update () {
        //Update the height of the stack
        height = 0.04f * cardList.Count;
        if (height > MAX_HEIGHT)
            height = MAX_HEIGHT;

        //scales to the correct height
        gameObject.transform.localScale = new Vector3 (gameObject.transform.localScale.x, height, gameObject.transform.localScale.z);

        //change the position of the top and bottom sprite depending on height
        GameObject top = gameObject.transform.Find ("Top").gameObject;
        GameObject bottom = gameObject.transform.Find ("Bottom").gameObject;

        top.transform.localPosition = new Vector3 (0, 0.501f, 0);
        bottom.transform.localPosition = new Vector3 (0, -0.501f, 0);

    }

    public void setCategory (string category) {
        if (category != "") {
            cardList.Clear ();
            int num = 0, catNum = 0;

            //get the index of the category to be loaded into the stack
            foreach (string cat in categories) {
                if (category == cat) {
                    catNum = num;
                    break;
                }
                num++;
            }

            num = 0;

            //add the cards in that category to the cardLIst linked list
            if (all_cards[catNum].Count > 1) {

                foreach (Cards c in all_cards[catNum]) {
                    cardList.AddFirst (new KeyValuePair<Cards, bool> (c, false));
                }

                topCard = cardList.First.Value;
                bottomCard = cardList.Last.Value;
                setViewableTopCards ();
            }
            else {
                //if in the specific case that a category of one card has been loaded, destroy this object and instantiate a card object
                Vector3 position = transform.position;
                GetComponent<ObjectHandler>().DestroyMe();

                Texture2D tf,tb;

                GameObject card = Instantiate(cardPrefab, position, Quaternion.identity);
                GameObject cube = card.transform.Find("Cube").gameObject;
                GameObject front = cube.transform.Find("Front").gameObject;
                GameObject back = cube.transform.Find("Back").gameObject;
                
                foreach (Cards c in all_cards[catNum]) {
                    cube.GetComponent<CardData>().setCard(new KeyValuePair<Cards, bool> (c, false));
                    cube.GetComponent<DragObject>().rot = 1;
                    cube.transform.Rotate(0.0f, 0.0f, 180.0f, Space.Self);
                    tf = c.frontImg;
                    tb = c.backImg;
                    front.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tf, new Rect(0, 0, tf.width, tf.height), new Vector2(0.5f, 0.5f));
                    back.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tb, new Rect(0, 0, tb.width, tb.height), new Vector2(0.5f, 0.5f));
                }

            }
        }

    }

    public void setViewableTopCards () {

        //updates the image on the top and bottom cards
        GameObject top = gameObject.transform.Find ("Top").gameObject;
        GameObject bottom = gameObject.transform.Find ("Bottom").gameObject;

        //gets the front and back of top card
        Texture2D texTopf = topCard.Key.frontImg;
        Texture2D texTopb = topCard.Key.backImg;

        //chooses which to show depending on orientation
        if (topCard.Value)
            top.GetComponent<SpriteRenderer> ().sprite = Sprite.Create (texTopf, new Rect (0, 0, texTopf.width, texTopf.height), new Vector2 (0.5f, 0.5f));
        else
            top.GetComponent<SpriteRenderer> ().sprite = Sprite.Create (texTopb, new Rect (0, 0, texTopb.width, texTopb.height), new Vector2 (0.5f, 0.5f));

        //gets the front and back of bottom card
        Texture2D texBottomf = bottomCard.Key.frontImg;
        Texture2D texBottomb = bottomCard.Key.backImg;

        //chooses which to show depending on orientation
        if (bottomCard.Value)
            bottom.GetComponent<SpriteRenderer> ().sprite = Sprite.Create (texBottomb, new Rect (0, 0, texBottomb.width, texBottomb.height), new Vector2 (0.5f, 0.5f));
        else
            bottom.GetComponent<SpriteRenderer> ().sprite = Sprite.Create (texBottomf, new Rect (0, 0, texBottomf.width, texBottomf.height), new Vector2 (0.5f, 0.5f));

    }

    public void setCardList (LinkedList<KeyValuePair<Cards, bool>> newCards) {

        //sets cardlist linked list to the newCards argument
        cardList = newCards;
        //update top and bottom cards
        topCard = cardList.First.Value;
        bottomCard = cardList.Last.Value;

        //print ("Top = " + topCard.Value + ", bottom = " + bottomCard.Value);
        //update image on top and bottom cards
        setViewableTopCards ();
    }

    //returns top card
    public KeyValuePair<Cards, bool> GetTopCard () {
        return cardList.First.Value;
    }

    //returns bottom card
    public KeyValuePair<Cards, bool> GetBottomCard () {
        return cardList.Last.Value;
    }

    public int Length () {
        return cardList.Count;
    }

    public void RemoveFirst () {
        cardList.RemoveFirst ();
        topCard = cardList.First.Value;
        setViewableTopCards ();
    }

    public void RemoveLast () {
        cardList.RemoveLast ();
        bottomCard = cardList.Last.Value;
        setViewableTopCards ();
    }

    public float GetHeight () {
        return height;
    }

    //shuffles the stack
    public void Shuffle () {
        //Random rand = new Random();
        KeyValuePair<Cards, bool> temp;

        //loops through linked list
        for (LinkedListNode<KeyValuePair<Cards, bool>> n = cardList.First; n != null; n = n.Next) {
            //store the card node
            temp = n.Value;
            //chooses randomly to swap with the last value or not
            if (Random.Range (0, 2) == 1) {
                n.Value = cardList.Last.Value;
                cardList.Last.Value = temp;
            } else {
                n.Value = cardList.First.Value;
                cardList.First.Value = temp;
            }
        }

        //updates top and bottom cards and view of them
        topCard = cardList.First.Value;
        bottomCard = cardList.Last.Value;
        setViewableTopCards ();
    }

}