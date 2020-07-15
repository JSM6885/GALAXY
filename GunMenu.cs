using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunMenu : MonoBehaviour
{
    public gunselect gs;    //총 고르는 gunselect 불러오기
    public GunManage gm;    //총 일부 기능 관리 불러오기
    public Manage m;
    public StageManage sm;


    // 각 총들 수치 제한들  순서대로 총1(권총), 총2(소총), 총3(돌격소총)
    public int[] Gun_LimitAmmo = new int[] { };                      //각 총 최대 탄수 제한
    public float[] Gun_LimitShotSpeed = new float[] { };       //각 총 공격속도 제한
    public float[] Gun_LimitDamage = new float[] { };            //각 총 데미지 제한
    public float[] Gun_LimitReload = new float[] { };          //각 총 장전속도 제한
    public float[] Gun_LimitDistance = new float[] { }; //각 총 사정거리 제한

    // 한번의 업그레이드 수치들 순서대로 총1(권총), 총2(소총), 총3(돌격소총)
    public int[] Up_Ammo = new int[] { };                     // 1회당 탄수 업그레이드 수치
    public float[] Up_ShotSpeed = new float[] { };       // 1회당 공격속도 업그레이드 수치
    public float[] Up_Damage = new float[] { };            // 1회당 데미지 업그레이드 수치
    public float[] Up_Reload = new float[] { };          // 1회당 장전속도 업그레이드 수치
    public float[] Up_Distance = new float[] { }; // 1회당 사정거리 업그레이드 수치


    public int[,] UPG_money = new int[3, 5];
    public int[] UP_money = new int[] { };

    public Text[] UPMoneyText;

    public int Gun_MeunNum;
    public int Gun_MenuNum_Temp;

    public GameObject[] GunUI;

    public int[,] chargeup = new int[3,5];

    private Vector3 v00;
    private Vector3 v01;
    private Vector3 v02;
    private Vector3 v03;


    public AudioSource menuaudio;//소리
    public AudioClip Clickclip;//
    public AudioClip N_Clickclip;//

    public GameObject Warning_M;
    public GameObject Warning_F;
    private Vector3 v04;//
    private Vector3 v05;//
    public float time;
    private bool W_M;
    public bool W_F;

    private Vector3 v;

    void Start()
    {
        gs = GameObject.Find("RightHand").GetComponent<gunselect>();    //gunselet 스크립트 불러오기
        gm = GameObject.Find("GunManager").GetComponent<GunManage>();   //GunManage 스크립트     
        m = GameObject.Find("Manager").GetComponent<Manage>();
        sm = GameObject.Find("StageManager").GetComponent<StageManage>();


        Gun_LimitAmmo = new int[] { 30, 20, 100 };                      //각 총 최대 탄수 제한
        Gun_LimitShotSpeed = new float[] { 0.2f, 1.0f, 0.05f };       //각 총 공격속도 제한
        Gun_LimitDamage = new float[] { 500.0f, 1000.0f, 200.0f };            //각 총 데미지 제한
        Gun_LimitReload = new float[] { 1.0f, 2.0f, 1.0f };          //각 총 장전속도 제한
        Gun_LimitDistance = new float[] { 200.0f, 1000.0f, 250.0f }; //각 총 사정거리 제한

        Up_Ammo = new int[] { 2, 2, 12 };                     // 1회당 탄수 업그레이드 수치
        Up_ShotSpeed = new float[] { 0.02f, 0.2f, 0.01f };       // 1회당 공격속도 업그레이드 수치
        Up_Damage = new float[] { 98, 180.0f, 32.0f };            // 1회당 데미지 업그레이드 수치
        Up_Reload = new float[] { 0.2f, 0.1f, 0.2f };          // 1회당 장전속도 업그레이드 수치
        Up_Distance = new float[] { 20, 60.0f, 30.0f };     // 1회당 사정거리 업그레이드 수치


        Gun_MeunNum = 0;
        Gun_MenuNum_Temp = -1;
        GunUI[0].SetActive(false);
        GunUI[1].SetActive(false);
        GunUI[2].SetActive(false);
        GunUI[3].SetActive(false);

        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                chargeup[i,j] = 0;
            }
        }

        for (int i = 0; i < 3; i++)     //업그레이드 비용 초기화
        {
            for (int j = 0; j < 5; j++)
            {
                UPG_money[i, j] = 100;
            }
        }

        UP_money = new int[] { 400, 500, 1000, 2000, 4000 };   //업그레이드 비용 증가량

        for (int i = 0; i < 15; i++)
        {            
            for(int j = 0; j < 3; j++)
            {
                for(int k = 0; k < 5; k++)
                {
                    UPMoneyText[i].text = UPG_money[j, k].ToString();
                }
            }
        }
        v00 = GunUI[0].transform.localScale;
        v01 = GunUI[1].transform.localScale;
        v02 = GunUI[2].transform.localScale;
        v03 = GunUI[3].transform.localScale;

        time = 0;
        v04 = Warning_M.transform.localScale;
        v05 = Warning_F.transform.localScale;
        W_F = false;
        W_M = false;

        v = this.transform.localScale;
    }
    
    void Update()
    {
        if (sm.WaveEnd == true)
        {
            if (this.transform.localScale.y < 0.06952192f)
            {
                v.y += 0.002f;
                this.transform.localScale = v;
            }
            else if (this.transform.localScale.y > 0.06952192f)
            {
                v.y = 0f;
                this.transform.localScale = new Vector3(0.06952192f, 0.06952192f, 0.06952192f);
            }
        }
        else if(sm.WaveEnd == false)
        {
            if (this.transform.localScale.y > 0)
            {
                v.y -= 0.002f;
                this.transform.localScale = v;
            }
            else if(this.transform.localScale.y < 0)
            {
                v.y = 0f;
                this.transform.localScale = new Vector3(0.06952192f, 0, 0.06952192f);
            }
        }

            switch (Gun_MeunNum)
            {
                case 0:
                    if (Gun_MenuNum_Temp == 1)
                    {
                        if (GunUI[1].transform.localScale.y >= 0)
                        {
                            v01.y -= 0.002f;//Y축 크기 증가
                            GunUI[1].transform.localScale = v01;//증가한 크기 적용
                        }
                        if (GunUI[1].transform.localScale.y < 0)
                        {
                            v01.y = 0;
                            GunUI[1].transform.localScale = new Vector3(0.02087407f, 0, 0.02087407f);//크기 값 고정
                            GunUI[1].SetActive(false);
                        }
                    }
                    else if (Gun_MenuNum_Temp == 2)
                    {
                        if (GunUI[2].transform.localScale.y >= 0)
                        {
                            v02.y -= 0.002f;//Y축 크기 증가
                            GunUI[2].transform.localScale = v02;//증가한 크기 적용
                        }
                        if (GunUI[2].transform.localScale.y < 0)
                        {
                            v02.y = 0;
                            GunUI[2].transform.localScale = new Vector3(0.02087407f, 0, 0.02087407f);//크기 값 고정
                            GunUI[2].SetActive(false);
                        }
                    }
                    else if (Gun_MenuNum_Temp == 3)
                    {
                        if (GunUI[3].transform.localScale.y >= 0)
                        {
                            v03.y -= 0.002f;//Y축 크기 증가
                            GunUI[3].transform.localScale = v03;//증가한 크기 적용
                        }
                        if (GunUI[3].transform.localScale.y < 0)
                        {
                            v03.y = 0;
                            GunUI[3].transform.localScale = new Vector3(0.02087407f, 0, 0.02087407f);//크기 값 고정
                            GunUI[3].SetActive(false);
                        }
                    }
                    if (Gun_MenuNum_Temp != 0)
                    {
                        GunUI[0].SetActive(true);
                        if (GunUI[0].transform.localScale.y < 0.02087407)
                        {
                            v00.y += 0.001f;//Y축 크기 증가
                            GunUI[0].transform.localScale = v00;//증가한 크기 적용
                        }
                        if (GunUI[0].transform.localScale.y >= 0.02087407)
                        {
                            v00.y = 0.02087407f;
                            GunUI[0].transform.localScale = new Vector3(0.02087407f, 0.02087407f, 0.02087407f);//크기 값 고정
                            Gun_MenuNum_Temp = 0;
                        }
                    }
                    break;
                case 1:
                    if (Gun_MenuNum_Temp == 0)
                    {
                        if (GunUI[0].transform.localScale.y >= 0)
                        {
                            v00.y -= 0.002f;//Y축 크기 증가
                            GunUI[0].transform.localScale = v00;//증가한 크기 적용
                        }
                        if (GunUI[0].transform.localScale.y < 0)
                        {
                            v00.y = 0;
                            GunUI[0].transform.localScale = new Vector3(0.02087407f, 0, 0.02087407f);//크기 값 고정
                            GunUI[0].SetActive(false);
                        }
                    }
                    if (Gun_MenuNum_Temp != 1)
                    {
                        GunUI[1].SetActive(true);
                        if (GunUI[1].transform.localScale.y <= 0.02087407)
                        {
                            v01.y += 0.001f;//Y축 크기 증가
                            GunUI[1].transform.localScale = v01;//증가한 크기 적용
                        }
                        if (GunUI[1].transform.localScale.y > 0.02087407)
                        {
                            v01.y = 0.02087407f;
                            GunUI[1].transform.localScale = new Vector3(0.02087407f, 0.02087407f, 0.02087407f);//크기 값 고정
                            Gun_MenuNum_Temp = 1;
                        }
                    }
                    break;
                case 2:
                    if (Gun_MenuNum_Temp == 0)
                    {
                        if (GunUI[0].transform.localScale.y >= 0)
                        {
                            v00.y -= 0.002f;//Y축 크기 증가
                            GunUI[0].transform.localScale = v00;//증가한 크기 적용
                        }
                        if (GunUI[0].transform.localScale.y < 0)
                        {
                            v00.y = 0;
                            GunUI[0].transform.localScale = new Vector3(0.02087407f, 0, 0.02087407f);//크기 값 고정
                            GunUI[0].SetActive(false);
                        }
                    }
                    if (Gun_MenuNum_Temp != 2)
                    {
                        GunUI[2].SetActive(true);
                        if (GunUI[2].transform.localScale.y <= 0.02087407)
                        {
                            v02.y += 0.001f;//Y축 크기 증가
                            GunUI[2].transform.localScale = v02;//증가한 크기 적용
                        }
                        if (GunUI[2].transform.localScale.y > 0.02087407)
                        {
                            v02.y = 0.02087407f;
                            GunUI[2].transform.localScale = new Vector3(0.02087407f, 0.02087407f, 0.02087407f);//크기 값 고정
                            Gun_MenuNum_Temp = 2;
                        }
                    }
                    break;
                case 3:
                    if (Gun_MenuNum_Temp == 0)
                    {
                        if (GunUI[0].transform.localScale.y >= 0)
                        {
                            v00.y -= 0.002f;//Y축 크기 증가
                            GunUI[0].transform.localScale = v00;//증가한 크기 적용
                        }
                        if (GunUI[0].transform.localScale.y < 0)
                        {
                            v00.y = 0;
                            GunUI[0].transform.localScale = new Vector3(0.02087407f, 0, 0.02087407f);//크기 값 고정
                            GunUI[0].SetActive(false);
                        }
                    }
                    if (Gun_MenuNum_Temp != 3)
                    {
                        GunUI[3].SetActive(true);
                        if (GunUI[3].transform.localScale.y <= 0.02087407)
                        {
                            v03.y += 0.001f;//Y축 크기 증가
                            GunUI[3].transform.localScale = v03;//증가한 크기 적용
                        }
                        if (GunUI[3].transform.localScale.y > 0.02087407)
                        {
                            v03.y = 0.02087407f;
                            GunUI[3].transform.localScale = new Vector3(0.02087407f, 0.02087407f, 0.02087407f);//크기 값 고정
                            Gun_MenuNum_Temp = 3;
                        }
                    }
                    break;
            }

        if(time >= 2)
        {
            time = 0;
            W_M = false;
            W_F = false;
        }

        if (W_M == true)
        {
            Warning_M.SetActive(true);
            if(Warning_M.transform.localScale.y < 0.3168856f)
            {
                v04.y += 0.01f;
                Warning_M.transform.localScale = v04;
            }
            else if (Warning_M.transform.localScale.y >= 0.3168856f)
            {
                v04.y = 0.3168856f;
                Warning_M.transform.localScale = new Vector3(0.3168856f, 0.3168856f, 0.3168856f);
                time += Time.deltaTime;
            }            
        }
        if(W_M==false)
        {
            if (Warning_M.transform.localScale.y >= 0)
            {
                v04.y -= 0.01f;
                Warning_M.transform.localScale = v04;
            }
            else if (Warning_M.transform.localScale.y < 0)
            {
                v04.y = 0;
                Warning_M.transform.localScale = new Vector3(0.3168856f, 0, 0.3168856f);
                Warning_M.SetActive(false);
            }
        }

        if (W_F == true)
        {
            Warning_F.SetActive(true);
            if (Warning_F.transform.localScale.y < 0.3168856f)
            {
                v05.y += 0.01f;
                Warning_F.transform.localScale = v05;
            }
            else if (Warning_F.transform.localScale.y >= 0.3168856f)
            {
                v05.y = 0.3168856f;
                Warning_F.transform.localScale = new Vector3(0.3168856f, 0.3168856f, 0.3168856f);
                time += Time.deltaTime;
            }            
        }
        if (W_F == false)
        {
            if (Warning_F.transform.localScale.y >= 0)
            {
                v05.y -= 0.01f;
                Warning_F.transform.localScale = v05;
            }
            else if (Warning_F.transform.localScale.y < 0)
            {
                v05.y = 0;
                Warning_F.transform.localScale = new Vector3(0.3168856f, 0, 0.3168856f);
                Warning_F.SetActive(false);
            }
        }
    }


    //Update 여기가 끝부분

    public void ReturnBtn()
    {
        Gun_MeunNum = 0;
    }


    public void ClickGun01()    //Gun1 버튼 클릭 시
    {
        gs.g = gunselect.Gun.GUN1;  //gunselect 스크립트에 판별 변수 GUN1 로 변경 
        menuaudio.clip = Clickclip;//
        menuaudio.Play();//
        Gun_MeunNum = 1;
    }
    public void ClickGun02()    //Gun2 버튼 클릭 시
    {
        gs.g = gunselect.Gun.GUN2;  //gunselect 스크립트에 판별 변수 GUN2 로 변경 
        menuaudio.clip = Clickclip;//
        menuaudio.Play();//
        Gun_MeunNum = 2;
    }
    public void ClickGun03()    //Gun3 버튼 클릭 시
    {
        gs.g = gunselect.Gun.GUN3;  //gunselect 스크립트에 판별 변수 GUN3 로 변경
        menuaudio.clip = Clickclip;//
        menuaudio.Play();//
        Gun_MeunNum = 3;
    }

    //=================================총1 업그레이드 버튼
    public void Gun1_AmmoUp()
    {
        if (m.money >= UPG_money[0, 0]&&chargeup[0,0]<5)
        {
            m.money -= UPG_money[0, 0];
            UPG_money[0, 0] += UP_money[chargeup[0, 0]];
            m.moneyChange();
            UPMoneyText[0].text = UPG_money[0, 0].ToString();
            Gun_AmmoUp(0);
            chargeup[0, 0]++;
            menuaudio.clip = Clickclip;//
            menuaudio.Play();//
        }
        else if (m.money <= UPG_money[0, 0])// 돈 부족
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_M = true;
        }
        else if(chargeup[0, 0] >= 5)//업그레이드 최대치
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_F = true;
        }
    }
    public void Gun1_ShotSpeedUp()
    {
        if (m.money >= UPG_money[0, 1] && chargeup[0, 1] < 5)
        {
            m.money -= UPG_money[0, 1];
            UPG_money[0, 1] += UP_money[chargeup[0, 1]];
            m.moneyChange();
            UPMoneyText[1].text = UPG_money[0, 1].ToString();
            Gun_ShotSpeedUp(0);
            chargeup[0, 1]++;
            menuaudio.clip = Clickclip;//
            menuaudio.Play();//
        }
        else if (m.money <= UPG_money[0, 1])// 돈 부족
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_M = true;
        }
        else if (chargeup[0, 1] >= 5)//업그레이드 최대치
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_F = true;
        }
    }
    public void Gun1_DamageUp()
    {
        if (m.money >= UPG_money[0, 2] && chargeup[0, 2] < 5)
        {
            m.money -= UPG_money[0, 2];
            UPG_money[0, 2] += UP_money[chargeup[0, 2]];
            m.moneyChange();
            UPMoneyText[2].text = UPG_money[0, 2].ToString();
            Gun_DamageUp(0);
            chargeup[0, 2]++;
            menuaudio.clip = Clickclip;//
            menuaudio.Play();//
        }
        else if (m.money <= UPG_money[0, 2])// 돈 부족
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_M = true;
        }
        else if (chargeup[0, 2] >= 5)//업그레이드 최대치
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_F = true;
        }
    }
    public void Gun1_ReloadUp()
    {
        if (m.money >= UPG_money[0, 3] && chargeup[0, 3] < 5)
        {
            m.money -= UPG_money[0, 3];
            UPG_money[0, 3] += UP_money[chargeup[0, 3]];
            m.moneyChange();
            UPMoneyText[3].text = UPG_money[0, 3].ToString();
            Gun_ReloadUp(0);
            chargeup[0, 3]++;
            menuaudio.clip = Clickclip;//
            menuaudio.Play();//
        }
        else if (m.money <= UPG_money[0, 3])// 돈 부족
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_M = true;
        }
        else if (chargeup[0, 3] >= 5)//업그레이드 최대치
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_F = true;
        }
    }
    public void Gun1_DistanceUp()
    {
        if (m.money >= UPG_money[0, 4] && chargeup[0, 4] < 5)
        {
            m.money -= UPG_money[0, 4];
            UPG_money[0, 4] += UP_money[chargeup[0, 4]];
            m.moneyChange();
            UPMoneyText[4].text = UPG_money[0, 4].ToString();
            Gun_DistanceUp(0);
            chargeup[0, 4]++;
            menuaudio.clip = Clickclip;//
            menuaudio.Play();//
        }
        else if (m.money <= UPG_money[0, 4])// 돈 부족
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_M = true;
        }
        else if (chargeup[0, 4] >= 5)//업그레이드 최대치
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_F = true;
        }
    }


    //=================================총2 업그레이드 버튼
    public void Gun2_AmmoUp()
    {
        if (m.money >= UPG_money[1, 0] && chargeup[1, 0] < 5)
        {
            m.money -= UPG_money[1,0];
            UPG_money[1, 0] += UP_money[chargeup[1, 0]];
            m.moneyChange();
            UPMoneyText[5].text = UPG_money[1, 0].ToString();
            Gun_AmmoUp(1);
            chargeup[1, 0]++;
            menuaudio.clip = Clickclip;//
            menuaudio.Play();//
        }
        else if (m.money <= UPG_money[1, 0])// 돈 부족
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_M = true;
        }
        else if (chargeup[1, 0] >= 5)//업그레이드 최대치
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_F = true;
        }
    }
    public void Gun2_ShotSpeedUp()
    {
        if (m.money >= UPG_money[1, 1] && chargeup[1, 1] < 5)
        {
            m.money -= UPG_money[1, 1];
            UPG_money[1, 1] += UP_money[chargeup[1, 1]];
            m.moneyChange();
            UPMoneyText[6].text = UPG_money[1, 1].ToString();
            Gun_ShotSpeedUp(1);
            chargeup[1, 1]++;
            menuaudio.clip = Clickclip;//
            menuaudio.Play();//
        }
        else if (m.money <= UPG_money[1, 1])// 돈 부족
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_M = true;
        }
        else if (chargeup[1, 1] >= 5)//업그레이드 최대치
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_F = true;
        }
    }
    public void Gun2_DamageUp()
    {
        if (m.money >= UPG_money[1, 2] && chargeup[1, 2] < 5)
        {
            m.money -= UPG_money[1, 2];
            UPG_money[1, 2] += UP_money[chargeup[1, 2]];
            m.moneyChange();
            UPMoneyText[7].text = UPG_money[1, 2].ToString();
            Gun_DamageUp(1);
            chargeup[1, 2]++;
            menuaudio.clip = Clickclip;//
            menuaudio.Play();//
        }
        else if (m.money <= UPG_money[1, 2])// 돈 부족
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_M = true;
        }
        else if (chargeup[1, 2] >= 5)//업그레이드 최대치
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_F = true;
        }
    }
    public void Gun2_ReloadUp()
    {
        if (m.money >= UPG_money[1, 3] && chargeup[1, 3] < 5)
        {
            m.money -= UPG_money[1, 3];
            UPG_money[1, 3] += UP_money[chargeup[1, 3]];
            m.moneyChange();
            UPMoneyText[8].text = UPG_money[1, 3].ToString();
            Gun_ReloadUp(1);
            chargeup[1, 3]++;
            menuaudio.clip = Clickclip;//
            menuaudio.Play();//
        }
        else if (m.money <= UPG_money[1, 3])// 돈 부족
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_M = true;
        }
        else if (chargeup[1, 3] >= 5)//업그레이드 최대치
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_F = true;
        }
    }
    public void Gun2_DistanceUp()
    {
        if (m.money >= UPG_money[1, 4] && chargeup[1, 4] < 5)
        {
            m.money -= UPG_money[1, 4];
            UPG_money[1, 4] += UP_money[chargeup[1, 4]];
            m.moneyChange();
            UPMoneyText[9].text = UPG_money[1, 4].ToString();
            Gun_DistanceUp(1);
            chargeup[1, 4]++;
            menuaudio.clip = Clickclip;//
            menuaudio.Play();//
        }
        else if (m.money <= UPG_money[1, 4])// 돈 부족
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_M = true;
        }
        else if (chargeup[1, 4] >= 5)//업그레이드 최대치
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_F = true;
        }
    }


    //=================================총3 업그레이드 버튼
    public void Gun3_AmmoUp()
    {
        if (m.money >= UPG_money[2, 0] && chargeup[2, 0] < 5)
        {
            m.money -= UPG_money[2, 0];
            UPG_money[2, 0] += UP_money[chargeup[2, 0]];
            m.moneyChange();
            UPMoneyText[10].text = UPG_money[2, 0].ToString();
            Gun_AmmoUp(2);
            chargeup[2, 0]++;
            menuaudio.clip = Clickclip;//
            menuaudio.Play();//
        }
        else if (m.money <= UPG_money[2, 0])// 돈 부족
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_M = true;
        }
        else if (chargeup[2, 0] >= 5)//업그레이드 최대치
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_F = true;
        }
    }   
    public void Gun3_ShotSpeedUp()
    {
        if (m.money >= UPG_money[2, 1] && chargeup[2, 1] < 5)
        {
            m.money -= UPG_money[2, 1];
            UPG_money[2, 1] += UP_money[chargeup[2, 1]];
            m.moneyChange();
            UPMoneyText[11].text = UPG_money[2, 1].ToString();
            Gun_ShotSpeedUp(2);
            chargeup[2, 1]++;
            menuaudio.clip = Clickclip;//
            menuaudio.Play();//
        }
        else if (m.money <= UPG_money[2, 1])// 돈 부족
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_M = true;
        }
        else if (chargeup[2, 1] >= 5)//업그레이드 최대치
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_F = true;
        }
    }   
    public void Gun3_DamageUp()
    {
        if (m.money >= UPG_money[2, 2] && chargeup[2, 2] < 5)
        {
            m.money -= UPG_money[2, 2];
            UPG_money[2, 2] += UP_money[chargeup[2, 2]];
            m.moneyChange();
            UPMoneyText[12].text = UPG_money[2, 2].ToString();
            Gun_DamageUp(2);
            chargeup[2, 2]++;
            menuaudio.clip = Clickclip;//
            menuaudio.Play();//
        }
        else if (m.money <= UPG_money[2, 2])// 돈 부족
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_M = true;
        }
        else if (chargeup[2, 2] >= 5)//업그레이드 최대치
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_F = true;
        }
    }   
    public void Gun3_ReloadUp()
    {
        if (m.money >= UPG_money[2, 3] && chargeup[2, 3] < 5)
        {
            m.money -= UPG_money[2, 3];
            UPG_money[2, 3] += UP_money[chargeup[2, 3]];
            m.moneyChange();
            UPMoneyText[13].text = UPG_money[2, 3].ToString();
            Gun_ReloadUp(2);
            chargeup[2, 3]++;
            menuaudio.clip = Clickclip;//
            menuaudio.Play();//
        }
        else if (m.money <= UPG_money[2, 3])// 돈 부족
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_M = true;
        }
        else if (chargeup[2, 3] >= 5)//업그레이드 최대치
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_F = true;
        }
    }   
    public void Gun3_DistanceUp()
    {
        if (m.money >= UPG_money[2, 4] && chargeup[2, 4] < 5)
        {
            m.money -= UPG_money[2, 4];
            UPG_money[2, 4] += UP_money[chargeup[2, 4]];
            m.moneyChange();
            UPMoneyText[14].text = UPG_money[2, 4].ToString();
            Gun_DistanceUp(2);
            chargeup[2, 4]++;
            menuaudio.clip = Clickclip;//
            menuaudio.Play();//
        }
        else if (m.money <= UPG_money[2, 4])// 돈 부족
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_M = true;
        }
        else if (chargeup[2, 4] >= 5)//업그레이드 최대치
        {
            menuaudio.clip = N_Clickclip;//
            menuaudio.Play();//
            W_F = true;
        }
    }


    //============================================================업그레이드 함수들
    public void Gun_AmmoUp(int i)
    {
        if (gm.m_MaxAmmo[i] < Gun_LimitAmmo[i])
        {
            gm.m_MaxAmmo[i] += Up_Ammo[i];
        }
    }
    public void Gun_ShotSpeedUp(int i)
    {
        if (gm.m_TimeBetFire[i] > Gun_LimitShotSpeed[i])
        {
            gm.m_TimeBetFire[i] -= Up_ShotSpeed[i];
        }
    }
    public void Gun_DamageUp(int i)
    {
        if (gm.m_Damage[i] < Gun_LimitDamage[i])
        {
            gm.m_Damage[i] += Up_Damage[i];
        }
    }
    public void Gun_ReloadUp(int i)
    {
        if (gm.m_ReloadTime[i] > Gun_LimitReload[i])
        {
            gm.m_ReloadTime[i] -= Up_Reload[i];
        }
    }
    public void Gun_DistanceUp(int i)
    {
        if (gm.m_FireDistance[i] < Gun_LimitDistance[i])
        {
            gm.m_FireDistance[i] += Up_Distance[i];
        }
    }
}
