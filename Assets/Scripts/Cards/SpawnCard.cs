using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;
using System.Text;


public class SpawnCard : MonoBehaviour
{

    GameController gameController;

    Texture2D tex;
    public GameObject prefab;
    
    int flipped; 

    public Transform newCardTransform;

    DeckData deckData;

    // Start is called before the first frame update
    void Start()
    {
        //find game controller object
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }

        //get flipped data and the deckdata
        flipped = GetComponent<DragObject>().flipped;
        deckData = GetComponent<DeckData>();

    }

    void Update(){
        flipped = GetComponent<DragObject>().flipped;

        //change the new card spawn position depending on flipped variable
        if (flipped == 1){
            newCardTransform.localPosition = new Vector3(-1.2f,0.51f,newCardTransform.localPosition.z);
        }
        else{
            newCardTransform.localPosition = new Vector3(1.2f,-0.51f,newCardTransform.localPosition.z);
        }
    }

    

    void OnMouseOver()
    {
        if (Input.GetKeyDown("space") && !gameController.paused)
        {
            //lower the height of the stack
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y - 0.1f, gameObject.transform.localScale.z);

            //instantiate a new card object
            GameObject card = Instantiate(prefab, newCardTransform.position, Quaternion.identity);
            card.transform.rotation = gameObject.transform.rotation;

            Texture2D tf,tb;

            GameObject cube = card.transform.Find("Cube").gameObject;
            GameObject front = cube.transform.Find("Front").gameObject;
            GameObject back = cube.transform.Find("Back").gameObject;

            KeyValuePair<Cards, bool> topCard = deckData.topCard;
            KeyValuePair<Cards, bool> bottomCard = deckData.bottomCard;

            if (front == null || back == null)
                Debug.Log("Children not found");


            //print("Flipped = " + flipped + ", topCard = " + topCard.Value + ", bottomCard = " + bottomCard.Value);
            
            //set the right card to the newly instantiated card object
            if (flipped == 1){
                //if stack has not flipped then top card is dealt and in normal orientation
                Cards cardFromDeck = topCard.Key;

                cube.GetComponent<CardData>().setCard(topCard);
                
                //depending on orientation in deck set the front and back sprites and rotate accordingly
                if (topCard.Value){
                    cube.GetComponent<DragObject>().rot = 0;
                    tf = cardFromDeck.frontImg;
                    tb = cardFromDeck.backImg;
                }
                else{
                    cube.GetComponent<DragObject>().rot = 1;
                    cube.transform.Rotate(0.0f, 0.0f, 180.0f, Space.Self);
                    tf = cardFromDeck.frontImg;
                    tb = cardFromDeck.backImg;
                }

                deckData.RemoveFirst();
            }
            else{
                //stack is flipped so deal bottom card
                Cards cardFromDeck = bottomCard.Key;
                
                //switch orientation because cards are upside down in deck
                cube.GetComponent<CardData>().setCard(bottomCard);
                cube.GetComponent<CardData>().switchOrientation();
                
                //cube.GetComponent<DragObject>().flipped = 1;

                //depending on orientation in deck set front and back and rotate accordingly
                if (bottomCard.Value){
                    
                    cube.GetComponent<DragObject>().rot = 1;
                    //cube.transform.Rotate(0.0f,0.0f,180.0f,Space.Self);
                    tf = cardFromDeck.frontImg;
                    tb = cardFromDeck.backImg;
                }
                else{
                    cube.GetComponent<DragObject>().rot = 0;
                    cube.transform.Rotate(0.0f,0.0f,180.0f,Space.Self);
                    tf = cardFromDeck.frontImg;
                    tb = cardFromDeck.backImg;
                }
                
                deckData.RemoveLast();
            }

            front.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tf, new Rect(0, 0, tf.width, tf.height), new Vector2(0.5f, 0.5f));
            back.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tb, new Rect(0, 0, tb.width, tb.height), new Vector2(0.5f, 0.5f));

            //if only two cards left then when a card is dealt, the stack must change to a card
            if (deckData.Length() == 1)
            {
                //instantiate a card object for the last card
                GameObject lastCard = Instantiate(prefab, transform.position, Quaternion.identity);
                lastCard.transform.rotation = gameObject.transform.rotation;

                cube = lastCard.transform.Find("Cube").gameObject;
                front = cube.transform.Find("Front").gameObject;
                back = cube.transform.Find("Back").gameObject;

                KeyValuePair<Cards,bool> lastCardObject = deckData.topCard;

                cube.GetComponent<CardData>().setCard(lastCardObject);

                //if the stack is flipped or not
                if (flipped == 1)
                {
                    Cards cardFromDeck = lastCardObject.Key;

                    
                    //cube.GetComponent<CardData>().setCard(lastCardObject);
                    //depending on orientation in deck set front and back and rotate accordingly
                    if (lastCardObject.Value)
                    {
                        cube.GetComponent<DragObject>().rot = 0;
                        tf = cardFromDeck.frontImg;
                        tb = cardFromDeck.backImg;
                    }
                    else
                    {
                        cube.GetComponent<DragObject>().rot = 1;
                        cube.transform.Rotate(0.0f, 0.0f, 180.0f, Space.Self);
                        tf = cardFromDeck.frontImg;
                        tb = cardFromDeck.backImg;
                    }
                }
                else
                {
                    Cards cardFromDeck = lastCardObject.Key;

                    //cube.GetComponent<CardData>().setCard(new KeyValuePair<Cards,bool>(lastCardObject.Key,!lastCardObject.Value));
                    cube.GetComponent<CardData>().switchOrientation();

                    //depending on orientation in deck set front and back and rotate accordingly
                    if (lastCardObject.Value)
                    {
                        cube.GetComponent<DragObject>().rot = 1;
                        
                        tf = cardFromDeck.frontImg;
                        tb = cardFromDeck.backImg;
                    }
                    else
                    {
                        cube.GetComponent<DragObject>().rot = 0;
                        cube.transform.Rotate(0.0f, 0.0f, 180.0f, Space.Self);
                        tf = cardFromDeck.frontImg;
                        tb = cardFromDeck.backImg;
                    }
                }

                front.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tf, new Rect(0, 0, tf.width, tf.height), new Vector2(0.5f, 0.5f));
                back.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tb, new Rect(0, 0, tb.width, tb.height), new Vector2(0.5f, 0.5f));

                GetComponent<ObjectHandler>().DestroyMe();

            }

        }
    }

    /*KeyValuePair<Cards,bool> takeTopCard(){

        KeyValuePair<Cards,bool> tCard = topCard;
        cards.RemoveFirst();
        topCard = cards.First.Value;

        setViewableTopCards();

        return tCard;
    }

    public void AddCardToFront(KeyValuePair<Cards,bool> card){
        cards.AddFirst(card);
        topCard = cards.First.Value;
        setViewableTopCards();
    }

    public void AddCardToBack(KeyValuePair<Cards,bool> card){
        cards.AddLast(card);
        bottomCard = cards.Last.Value;
        setViewableTopCards();
    }
    */
    
}
