using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour

{

    public static Menu Instance;
    public AudioSource menuaudio;//소리



    bool gameOver = false; //시작할 때는 게임오버가 아니다.

    void Awake()
    {

        if (Instance == null)
            Instance = this;
    }

    void Start()
    {

        
    }

    private void Update() //만약 gameOver 가 true이면 게임 재시작한다
    {
        if (gameOver == true)
        {

            
        }
    }
    public void gameStart()
    {
        menuaudio.Play();
        SceneManager.LoadScene(1);    //씬매니저에서 1번씬 새게임 로딩
    }
    public void gameReplay()
    {
        menuaudio.Play();
        SceneManager.LoadScene(0);    //씬매니저에서 1번씬 새게임 로딩
    }

    public void Quit()
    {
        menuaudio.Play();
        Application.Quit();
    }

}
