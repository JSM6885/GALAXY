using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//총 선택 소환 스크립트

public class gunselect : MonoBehaviour
{
    public GameObject[] gun;    //총 프리팹들 저장
    public GunManage gunManage;
    public enum Gun { GUN1=1, GUN2, GUN3 }; //총 선택 변수
    public Gun g;   //위에 enum 받아오기
    public int gun_temp;   //중복 선택 방지
    GameObject g00 = null;
    void Start()
    {
        gun_temp = 0;   //중복 방지 변수 초기화
        g = Gun.GUN3;
        CreateGun();
    }

    void CreateGun()
    {
        switch (g)  //총 선택 스위치문 시작
        {
            case Gun.GUN1: //총1
                g = 0;   //선택 변수 초기화 - 총 중복 생성 방지
                

                if (gun_temp == 2)  //총2 이 있을 시 제거
                {
                    Destroy(GameObject.Find("newSVD(Clone)"));
                }
                else if (gun_temp == 3) //총3 이 있을 시 제거
                {
                    Destroy(GameObject.Find("newM4A1(Clone)"));
                }
                if (gun_temp != 1)  //총1 중복 선택 방지
                {
                    gun_temp = 1;   //중복 방지
                    gunManage.GunInit();
                    g00 = Instantiate<GameObject>(gun[0], GameObject.Find("RightHand").transform.position,
                        GameObject.Find("RightHand").transform.rotation) as GameObject; //총1 오른손 위치에 자식으로 생성
                    g00.transform.parent = GameObject.Find("RightHand").transform;
                    gun g = g00.GetComponent<gun>();
                    g.Init();
                }
                break;
            case Gun.GUN2:  //총2
                g = 0;  //선택 변수 초기화 - 총 중복 생성 방지
                
                if (gun_temp == 1)  //총1 이 있을 시 제거
                {
                    Destroy(GameObject.Find("newGun(Clone)"));
                }
                else if (gun_temp == 3) //총3 이 있을 시 제거
                {
                    Destroy(GameObject.Find("newM4A1(Clone)"));
                }
                if (gun_temp != 2)  //총2 중복 선택 방지
                {
                    gun_temp = 2;   //중복 방지
                    gunManage.GunInit();
                    g00 = //총2 오른손 위치에 자식으로 생성
                        Instantiate<GameObject>(gun[1], GameObject.Find("RightHand").transform.position,
                        GameObject.Find("RightHand").transform.rotation) as GameObject;
                    g00.transform.parent = GameObject.Find("RightHand").transform;

                }
                break;
            case Gun.GUN3: //총3
                g = 0;  //선택 변수 초기화 - 총 중복 생성 방지
                
                if (gun_temp == 1)  //총1 이 있을 시 제거
                {
                    Destroy(GameObject.Find("newGun(Clone)"));
                }
                else if (gun_temp == 2) //총2 이 있을 시 제거
                {
                    Destroy(GameObject.Find("newSVD(Clone)"));
                }

                if (gun_temp != 3)  //총3 중복 선택 방지
                {
                    gun_temp = 3;   //중복 방지
                    gunManage.GunInit();
                    g00 = //총3 오른손 위치에 자식으로 생성
                        Instantiate<GameObject>(gun[2], GameObject.Find("RightHand").transform.position,
                        GameObject.Find("RightHand").transform.rotation) as GameObject;
                    g00.transform.parent = GameObject.Find("RightHand").transform;
                }
                //g = 0;  //선택 변수 초기화 - 총 중복 생성 방지
                //gun_temp = 3;   //중복 방지
                break;
        }
    }

    void Update()
    {
        //CreateGun();


    }

    private void LateUpdate()
    {
        if (g00)
        {
            Debug.Log("g00.GetComponentInChildren<gun>()");
            gun gunScript = g00.GetComponentInChildren<gun>();
            gunScript.Init();
            g00 = null;
        }
    }
}
