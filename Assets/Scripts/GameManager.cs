using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public bool hasGameStarted  =false;
    public bool hasgameEnded = false;
    //panels 
    public GameObject StartPanel;
    public GameObject GameOverPanel;
    public GameObject WinPanel;
    public GameObject GameEndedPanel;
    // 
    public bool finishCamMove = false;

    public int score;
    public string finishOrder;
    
    public bool gameOver;
    public int currentLevelBuildIndex;
    
    public static GameManager instance{set; get;}
     private void Awake() {
        instance = this;
        
    }
    
    private void Start() {
        currentLevelBuildIndex = SceneManager.GetActiveScene().buildIndex;
        
    }
    



    public void GameOverFunc()
    {
        hasgameEnded = true;
        StartCoroutine("GOCoroutine"); 

    }
    IEnumerator GOCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        GameOverPanel.SetActive(true);

    }
    public void RestartFunc()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        

    }
    public void Winfunc()
    {
        
        StartCoroutine("winCoroutine");

    }
    IEnumerator winCoroutine()
    {
        yield return new WaitForSecondsRealtime(1f);
        WinPanel.SetActive(true);
    }

    public void NextLevelButton()
    {
        //RestartFunc();
        if(currentLevelBuildIndex >= 2)
        {
            currentLevelBuildIndex = -1;
        }
        SceneManager.LoadScene(currentLevelBuildIndex+1);


    }
    public void GameEndedFunc()
    {
        GameEndedPanel.SetActive(true);
    }


    
    public void StartButton()
    {
        hasGameStarted = true;
        StartPanel.SetActive(false);

    }


    
}
