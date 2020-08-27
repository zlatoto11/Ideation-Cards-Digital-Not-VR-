using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMenu : MonoBehaviour
{
    void OnMouseDown()
    {
        MenuManager.displayMenu = true;
        MenuManager.menuTarget = gameObject;
    }
}

