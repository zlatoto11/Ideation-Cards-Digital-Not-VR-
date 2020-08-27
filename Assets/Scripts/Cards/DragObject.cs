using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class DragObject : MonoBehaviour {

    private Vector3 mOffset;
    private float mZCoord;
    public Vector3 worldPosition;

    public GameObject noteSymbolf, noteSymbolb;

    Plane planeC = new Plane(Vector3.up, -13.5f);
    Plane planeS = new Plane(Vector3.up, -16.0f);
    float distance;
    Ray ray;

    public GameObject notePanel;
    public TMP_InputField textInBox;
    public string note = "";

    private Vector3 prevMousePos, mouse;

    private float mDir;
    private Vector3 deltaMpos = Vector3.zero;
    private Vector3 prevDeltaMpos = Vector3.zero;
    private int shakeCount = 0;
    private int zeroCount = 0;
    public int flipped = 1;
    public int rot = 0;

    private bool objectSelected = false, mouseDown = false;

    float zoomSize = 1.0f;

    GameController gameController;
    void Start () {
        
        //finds the gamecontroller object

        GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
        if (gameControllerObject != null) {
            gameController = gameControllerObject.GetComponent<GameController> ();
        }
        if (gameController == null) {
            Debug.Log ("Cannot find 'GameController' script");
        }

        //get mouse position for use later on
        prevMousePos = GetMouseWorldPos();

        //freeze most physics elements of cards and stacks other than movement up and down
        gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ |
            RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezeRotationZ;

        //determine whether to display the note symbol or not
        if (note != "")
        {
            noteSymbolf.SetActive(true);
            noteSymbolb.SetActive(true);
        }

    }

    void Update () {

        

        //print(note.Length);

        //determine whether to display the note symbol or not
        if (note.Length > 1){
            noteSymbolf.SetActive(true);
            noteSymbolb.SetActive(true);
        }
        else{
            noteSymbolf.SetActive(false);
            noteSymbolb.SetActive(false);
        }

        //main update loop when card is selected
        if (objectSelected && !gameController.paused) {
            //rotate the object depending on its rot value - this is changed when the card is flipper
            GetComponent<Rigidbody> ().MoveRotation (Quaternion.Euler (0, 0, rot * 180.0f));

            //dont limit movement or rotation of the object
            gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;

            //calculate difference in current and previous mouse positions to workout acceleration
            mouse = GetMouseWorldPos();
            //print(mouse + ", Shakecount = " + shakeCount + ", Delta mouse = " + deltaMpos );
            deltaMpos = mouse - prevMousePos;
            prevMousePos = mouse;

            //Max mouse speed is 15
            if (deltaMpos.x > 15)
                deltaMpos.x = 15;
            else if (deltaMpos.x < -15)
                deltaMpos.x = -15;
            if (deltaMpos.z > 15)
                deltaMpos.z = 15;
            else if (deltaMpos.z < -15)
                deltaMpos.z = -15;

            //gameObject.transform.position = new Vector3(mouse.x + mOffset.x, gameObject.transform.position.y, mouse.z + mOffset.z);
            //print(deltaMpos + ", " + prevDeltaMpos);
            //print((deltaMpos.x > 0.01f && prevDeltaMpos.x < -0.01f) + ", " + (deltaMpos.x < -0.01f && prevDeltaMpos.x > 0.01f));

            //if the direction of mouse has changed from previous to current add one to the shake count
            if ((deltaMpos.x > 0.01f && prevDeltaMpos.x < -0.01f) || (deltaMpos.x < -0.01f && prevDeltaMpos.x > 0.01f)) {
                if (zeroCount <= 10) {
                    shakeCount++;
                    zeroCount = 0;
                }
            //zero count is added to to make sure movements are quick enough
            } else if (shakeCount > 0) {
                zeroCount++;
                if (zeroCount > 10) {
                    shakeCount = 0;
                    zeroCount = 0;
                }
            }

            //once three shakes that have been completed fast enough are done then the stack is shuffled
            if (shakeCount >= 3) {
                //print ("shake");

                if (gameObject.name == "Deck") {
                    GetComponent<DeckData>().Shuffle ();
                }

                shakeCount = 0;
            }

            prevDeltaMpos = deltaMpos;
            
            //code for calculating mouse position and if it is over an object
            if (gameObject.transform.parent.name == "Card(Clone)") {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (planeC.Raycast(ray, out distance)){
                    worldPosition = ray.GetPoint(distance);
                }
                GetComponent<Rigidbody> ().MovePosition (new Vector3 (worldPosition.x, worldPosition.y , worldPosition.z));
            } else {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (planeS.Raycast(ray, out distance)){
                    worldPosition = ray.GetPoint(distance);
                }
                GetComponent<Rigidbody> ().MovePosition (new Vector3 (worldPosition.x, worldPosition.y , worldPosition.z));
            }

            //deselects the object
            if (!mouseDown && Input.GetMouseButtonDown (0)) {
                GetComponent<BoxCollider>().isTrigger = false;
                
                //resets the size of object
                if (gameObject.transform.parent.name == "Card(Clone)")
                    gameObject.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
                else
                    gameObject.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);

                //print ("update");
                //freezes physics elements
                objectSelected = false;
                gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY |
                    RigidbodyConstraints.FreezeRotationZ;

                gameController.setCurrentCard (null);
                GetComponent<Rigidbody> ().isKinematic = false;

                //transform.rotation = Quaternion.Euler(0, 0, rot * 180.0f);
                GetComponent<Rigidbody> ().MoveRotation (Quaternion.Euler (0, 0, rot * 180.0f));

            }

            //zooming feature
            if (Input.GetAxis ("Mouse ScrollWheel") != 0f) // forward
            {
                //how is the mouse wheel scrolled
                zoomSize = Input.GetAxis ("Mouse ScrollWheel");
                float xScale = gameObject.transform.localScale.x + Input.GetAxis ("Mouse ScrollWheel");
                //change scale depending on scroll
                if (xScale >= 1.0f && xScale < 10.0f)
                    gameObject.transform.localScale = new Vector3 (xScale, 1.0f, xScale);
            }

            //note feature
            if (Input.GetKeyDown("t")){
                //stop background interaction
                Time.timeScale = 0;
                gameController.paused = true;
                //open the note panel
                notePanel.SetActive(true);
                //set the text to be the note on the object
                textInBox.text = note;
                //set the card object in the button to this object so it can set the note on this object
                notePanel.transform.Find("Button").GetComponent<writeText>().card = gameObject;
                //notePanel.transform.Find("Button").GetComponent<writeText>().text = text;
            }

            mouseDown = false;
        }

    }

    //flip object is right click while mouse is over
    void OnMouseOver () {
        if (Input.GetMouseButtonDown (1) && !gameController.paused) {
            Flip ();
        }

    }

    void Flip () {
        //set the flipped variable to reverse
        flipped = flipped * -1;
        if (gameObject.transform.parent.name == "Card(Clone)")
            gameObject.GetComponent<CardData> ().switchOrientation ();

        //change the rotation value which is use when transforming
        if (rot == 1)
            rot = 0;
        else
            rot = 1;

        //actually rotate the object
        gameObject.transform.Rotate (0.0f, 0.0f, 180.0f, Space.Self);
    }

    void OnMouseDown () {
        //select the object if it isnt already selected and the game isnt paused
        if (!objectSelected && !gameController.paused) {
            //stop it colliding with objects while selected
            GetComponent<BoxCollider>().isTrigger = true;

            //print ("mouse down");

            //set current card in game controller
            gameController.setCurrentCard (gameObject);

            GetComponent<Rigidbody> ().isKinematic = true;

            mouseDown = true;
            objectSelected = true;
        }

    }

    void OnMouseUp () {

    }

    //gets the mouse position
    private Vector3 GetMouseWorldPos () {
        //pixel coords (x,y)
        Vector3 mousePoint = Input.mousePosition;
        //print(mousePoint);

        // z coord of game object
        mousePoint.z = 13.5f;

        return (Camera.main.ScreenToWorldPoint(mousePoint));
    }

    void OnMouseDrag () {

    }

    //sets the flipped variable
    public void setFlipped (int f) {
        flipped = f;
    }

}