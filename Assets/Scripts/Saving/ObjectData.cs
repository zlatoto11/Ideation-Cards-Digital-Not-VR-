using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ObjectType
{
	Card,
	Deck
}

[System.Serializable]
public class ObjectData
{

	public string id;

	public ObjectType objectType;

	public Vector3 position;

	public Quaternion rotation;

	public LinkedList<KeyValuePair<Cards,bool>> cardList;

	public int flipped, rot;
	
	public string note;

}
