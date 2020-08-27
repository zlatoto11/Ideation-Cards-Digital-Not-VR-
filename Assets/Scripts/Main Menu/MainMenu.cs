using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

        public void startNewSession()
    {
        //Debug.Log("STARTNEWSCENE");
        //open main scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void loadGameButton(){

    }
    
    public void loadInstructionMenu(){
        
    }

    //quit the application
    public void quitGame() {
        //Debug.Log("Quit");
        Application.Quit();
    }

}
