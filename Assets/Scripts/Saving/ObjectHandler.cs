using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandler : MonoBehaviour {

	public ObjectType objectType;
	public ObjectData objectData;


	// Use this for initialization
	void Start () {
		if(string.IsNullOrEmpty(objectData.id)){
			objectData.id = System.DateTime.Now.ToLongDateString() + System.DateTime.Now.ToLongTimeString() + Random.Range(0, int.MaxValue).ToString();
			objectData.objectType = objectType;
			SaveData.current.objects.Add(objectData);
		}

		print("Assign: " + GetInstanceID());
		GameEvents.current.onLoadEvent += DestroyMe;
	}
	
	// Update is called once per frame
	void Update () {
		objectData.position = transform.position;
		objectData.rotation = transform.rotation;
		objectData.flipped = GetComponent<DragObject>().flipped;
		objectData.rot = GetComponent<DragObject>().rot;
		objectData.note = GetComponent<DragObject>().note;
        if ((int)objectType == 0)
            objectData.cardList = GetComponent<CardData>().cardList;
        else
            objectData.cardList = GetComponent<DeckData>().cardList;
    }

	public void DestroyMe(){
		print("Unassign: " + GetInstanceID());

		SaveData.current.objects.Remove(objectData);

		GameEvents.current.onLoadEvent -= DestroyMe;
		Destroy(gameObject.transform.parent.gameObject);
	}
}
