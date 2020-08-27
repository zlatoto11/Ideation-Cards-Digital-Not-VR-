using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

//serializing linked list data
public class LinkedListSerializationSurrogate : ISerializationSurrogate {

    GameController gameController;

    void Start()
    {
        
    }

	public void GetObjectData(object obj, SerializationInfo info, StreamingContext context){
        //Debug.Log("In LL serialization...");
        //initialise a new card list
        LinkedList<KeyValuePair<Cards,bool>> cardsList = (LinkedList<KeyValuePair<Cards,bool>>) obj;
		int num = 0;

        //Debug.Log("Count 1 = " + cardsList.Count);
        info.AddValue("count", cardsList.Count);
		foreach (KeyValuePair<Cards,bool> el in cardsList){
			info.AddValue(num + "id", el.Key.ID);
            //Debug.Log(el.Key.ID);
			info.AddValue(num + "faceUp", el.Value);
            num++;

            //Debug.Log("Get value" + (string)info.GetValue(num + "id", typeof(string)));
		}

        //Debug.Log("FINAL NUM = " + num);

	}
	
	public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector){

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }

        LinkedList<KeyValuePair<Cards,bool>> linkedList = new LinkedList<KeyValuePair<Cards,bool>>();

        int count = (int)info.GetValue("count", typeof(int));
        //Debug.Log("Count 2 = " + count);
        string id = "";
        bool faceUp;

        for (int i = 0; i < count; i++)
        {
            //Debug.Log(i + " before");
            id = (string)info.GetValue(i + "id", typeof(string));
            faceUp = (bool)info.GetValue(i + "faceUp", typeof(bool));
            //Debug.Log("id = " + id + ", faceUp = " + faceUp);
            //Debug.Log("Category = " + gameController.FindCard(id).Category);
            linkedList.AddLast(new KeyValuePair<Cards,bool>(gameController.FindCard(id), faceUp));
            //Debug.Log(i + " after");
        }

		obj = linkedList;
		return obj;

	}
}
