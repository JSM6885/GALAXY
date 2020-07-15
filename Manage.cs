using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

//Manage 스크립트
//플레이어의 체력, 재화 를 관리
//이후에 다른 요소 추가 가능성 있음

public class Manage : MonoBehaviour
{
    public float HeartPoint;//현재 체력
    public float MaxHeartPoint;//최대 체력
    public float HeartPercent;//체력 퍼센트로 변환할 변수
    public int money;//현재 재화
    public Text[] AllMoney;//각종 재화 UI들
    public Text Heart;//체력 UI
    public Slider HP_Slide;
    public GameObject TimePauseUI;
    public bool TimeStop;
    public Text Score;
    private AudioSource _audio;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        TimeStop = false;
        MaxHeartPoint = 5000;//최대 체력
        HeartPoint = MaxHeartPoint;//현재 체력을 최대체력으로 초기화
        money = 0;//시작 시 돈 초기화
        for (int i = 0; i < 5; i++)//재화 UI들 갱신
        {
            moneyChange();
        }
        HP_Slide.maxValue = MaxHeartPoint;
        HP_Slide.value = HeartPoint;
    }
    
    void Update()
    {
        HeartPercent = (HeartPoint / MaxHeartPoint) * 100;//체력을 퍼센트로 표시하기 위해 변환
        Heart.text = Math.Round(HeartPercent, 0).ToString() + " %";//체력UI에 표시
        HP_Slide.value = HeartPoint;
        if (HeartPoint <= 0)
        {
            SceneManager.LoadScene(3);
        }
    }

    public void moneyChange() //현재 재화 갱신 함수
    {
        for (int i = 0; i < 4; i++)
        {
            AllMoney[i].text = money.ToString();
        }
        Score.text = money.ToString();
    }

    public void PasueGame()
    {
        if (TimeStop == false)
        {
            TimePauseUI.SetActive(true);
            Time.timeScale = 0;
            _audio.Stop();
            TimeStop = true;
        }
        else if(TimeStop ==  true)
        {
            TimePauseUI.SetActive(false);
            Time.timeScale = 1;
            _audio.Play();
            TimeStop = false;
        }
    }
}
