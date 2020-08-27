using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stacking : MonoBehaviour
{

    public GameObject stackPrefab;
    LinkedList<KeyValuePair<Cards, bool>> cardFromObject;

    float height;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //get height from either object
        if (gameObject.tag == "Stack")
            height = GetComponent<DeckData>().height;
        else
            height = GetComponent<CardData>().height;

        int layerMask = 1 << 8;

        RaycastHit hit;

        //raycast down to detect hitting another card/stack
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2 * height, layerMask))
        {
            Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.yellow);

            //get the object below
            GameObject objectBelow = hit.transform.gameObject;
            if (objectBelow != null)
            {
                bool flipped;

                //current object is stack
                if (gameObject.tag == "Stack")
                {
                    //print("hello");
                    //object below is stack
                    if (objectBelow.tag == "Stack")
                    {
                        //get orientation of both
                        int currentObjectFlipped = GetComponent<DragObject>().flipped;
                        int objectBelowFlipped = objectBelow.GetComponent<DragObject>().flipped;

                        LinkedList<KeyValuePair<Cards, bool>> currentCards = GetComponent<DeckData>().cardList;
                        LinkedList<KeyValuePair<Cards, bool>> deckBelowCards;
            
                        deckBelowCards = objectBelow.GetComponent<DeckData>().cardList;

                        //depending on orientation, concatenate both card lists in different orders
                        if (currentObjectFlipped == 1)
                        {
                            if (objectBelowFlipped == 1)
                            {
                                foreach (KeyValuePair<Cards, bool> card in deckBelowCards)
                                {
                                    currentCards.AddLast(card);
                                }
                                
                                objectBelow.GetComponent<ObjectHandler>().DestroyMe();
                                GetComponent<DeckData>().setCardList(currentCards);
                                GetComponent<DeckData>().setViewableTopCards();
                            }
                            else
                            {
                                for (int i = 0; i <= deckBelowCards.Count; i++)
                                {
                                    currentCards.AddLast(new KeyValuePair<Cards, bool>(deckBelowCards.Last.Value.Key, !deckBelowCards.Last.Value.Value));
                                    deckBelowCards.RemoveLast();
                                }
                                objectBelow.GetComponent<ObjectHandler>().DestroyMe();
                                GetComponent<DeckData>().setCardList(currentCards);
                                GetComponent<DeckData>().setViewableTopCards();
                            }
                        }
                        else
                        {
                            if (objectBelowFlipped == 1)
                            {
                                foreach (KeyValuePair<Cards, bool> card in currentCards)
                                {
                                    deckBelowCards.AddFirst(new KeyValuePair<Cards, bool>(card.Key, !card.Value));
                                }
                                objectBelow.GetComponent<DeckData>().setCardList(deckBelowCards);
                                objectBelow.GetComponent<DeckData>().setViewableTopCards();
                                GetComponent<ObjectHandler>().DestroyMe();
                            }
                            else
                            {
                                foreach (KeyValuePair<Cards, bool> card in currentCards)
                                {
                                    deckBelowCards.AddLast(card);
                                }
                                objectBelow.GetComponent<DeckData>().setCardList(deckBelowCards);
                                objectBelow.GetComponent<DeckData>().setViewableTopCards();
                                GetComponent<ObjectHandler>().DestroyMe();
                            }
                        }
                    }
                    //object below is a card
                    else {

                        int currentObjectFlipped = GetComponent<DragObject>().flipped;
                        int objectBelowFlipped = objectBelow.GetComponent<DragObject>().flipped;

                        LinkedList<KeyValuePair<Cards, bool>> currentCards = GetComponent<DeckData>().cardList;

                        KeyValuePair<Cards, bool> card = objectBelow.GetComponent<CardData>().cardList.First.Value;

                        //check orientation and add to list
                        if (currentObjectFlipped == 1)
                        {
                            currentCards.AddLast(card);
                            
                        
                            objectBelow.GetComponent<ObjectHandler>().DestroyMe();

                            GetComponent<DeckData>().setCardList(currentCards);
                            GetComponent<DeckData>().setViewableTopCards();

                        }
                        else
                        {
                            currentCards.AddFirst(new KeyValuePair<Cards, bool>(card.Key, !card.Value));
                            

                            objectBelow.GetComponent<ObjectHandler>().DestroyMe();

                            GetComponent<DeckData>().setCardList(currentCards);
                            GetComponent<DeckData>().setViewableTopCards();
                        }

                    }

                }

                //object is a card
                else
                {
                    if (objectBelow.tag != "Stack")
                    {
                        flipped = false;
                        //print("not stack");
                        cardFromObject = objectBelow.GetComponent<CardData>().cardList;
                    }
                    else
                    {
                        print("stack");
                        cardFromObject = objectBelow.GetComponent<DeckData>().cardList;
                        if (objectBelow.GetComponent<DragObject>().flipped == -1)
                        {
                            flipped = true;
                        }
                        else
                        {
                            flipped = false;
                        }
                    }

                    //destroy object below
                    objectBelow.GetComponent<ObjectHandler>().DestroyMe();

                    //instantiate new stack
                    GameObject newStack = Instantiate(stackPrefab, new Vector3(transform.position.x, 11.6f, transform.position.z), Quaternion.identity);
                    GameObject deck = newStack.transform.Find("Deck").gameObject;

                    //add current card to stack list depending on orientation
                    LinkedList<KeyValuePair<Cards, bool>> newCardList = cardFromObject;
                    if (!flipped)
                        newCardList.AddFirst((gameObject.GetComponent<CardData>().cardList).First.Value);
                    else
                    {
                        gameObject.GetComponent<CardData>().switchOrientation();
                        newCardList.AddLast((gameObject.GetComponent<CardData>().cardList).First.Value);
                        deck.GetComponent<DragObject>().rot = 1;
                        deck.GetComponent<DragObject>().flipped = -1;
                        deck.transform.Rotate(0.0f, 0.0f, 180.0f, Space.Self);
                    }
                    deck.GetComponent<DeckData>().setCardList(newCardList);
                    gameObject.GetComponent<ObjectHandler>().DestroyMe();
                }
            }
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.white);
            //Debug.Log("Did not Hit");
        }

    }
}
