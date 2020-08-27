using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleMenu : MonoBehaviour
{
    //class not used anymore, adds a menu to objects when clicked
    public Transform target;
    Camera camera;

    void Start()
    {
        camera = GetComponent<Camera>();
    }

    void OnGUI()
    {
        //display menu at location and check for button presses
        if (MenuManager.displayMenu && MenuManager.menuTarget)
        {
            Vector3 position = camera.WorldToScreenPoint(MenuManager.menuTarget.transform.position);
            Rect rect = new Rect(position.x, Screen.height - position.y, 128, 80);
            GUILayout.BeginArea(rect, GUI.skin.box);

            GUILayout.Label("Commands");

            if (GUILayout.Button("Attack"))
            {
                MenuManager.displayMenu = false;

                MenuManager.menuTarget = null;
            }

            if (GUILayout.Button("Cancel"))
            {
                MenuManager.displayMenu = false;
                MenuManager.menuTarget = null;
            }

            GUILayout.EndArea();
        }
    }
}