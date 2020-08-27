using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : MonoBehaviour {

	public Cards card;
	public LinkedList<KeyValuePair<Cards,bool>> cardList;
    public float height = 0.1f;

	// Use this for initialization
	void Awake () {
		//Initialise the card list and cards array
		cardList = new LinkedList<KeyValuePair<Cards,bool>>();
		card = new Cards();
	}
    
    void Start ()
    {
		//Find the front and back sprite objects of the card

        //GameObject cube = gameObject.transform.Find("Cube").gameObject;
        GameObject front = gameObject.transform.Find("Front").gameObject;
        GameObject back = gameObject.transform.Find("Back").gameObject;


		//Set the front and back textures
        Texture2D tf, tb;

        Cards card = cardList.First.Value.Key;

        tf = card.frontImg;
        tb = card.backImg;

        front.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tf, new Rect(0, 0, tf.width, tf.height), new Vector2(0.5f, 0.5f));
        back.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tb, new Rect(0, 0, tb.width, tb.height), new Vector2(0.5f, 0.5f));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	//Sets the value of card to the one inputted
    public void setCard(KeyValuePair<Cards,bool> cardSet){
		card = cardSet.Key;

		if(cardList == null)
			print("Card is null");
		cardList.AddFirst(cardSet);
	}

	//Flips the card over
	public void switchOrientation(){
		cardList.AddFirst(new KeyValuePair<Cards,bool>(cardList.First.Value.Key,!cardList.First.Value.Value));
		cardList.RemoveLast();
	}

}
