using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//보스 스크립트

public class BossEnemy : MonoBehaviour, IDamageable
{
    public static Enemy Instance;
    Transform tower;//목표물
    public float ATTACK_TIME;//공격시간
    private float attackTime = 0;//공격시간 비교값
    public AudioSource zombileaudio;//소리
    public AudioClip deathclip;//죽을때 소리
    public AudioClip Rangeattackclip;//원거리 공격 소리
    public AudioClip attackclip;//공격 소리
    public AudioClip Runclip;//달릴 때 소리
    public Animator anim;//애니메이션
    public float ATTACK_DISTANCE;//공격 사거리
    public float damage;//데미지
    public float rangedamage;//원거리 공격 데미지
    public float HP;//현재체력
    public float MAX_HP;//최대체력
    private float speed;//이동 속도
    public bool Death;//죽음 판정
    public int money;//주는 돈
    Manage m;//Manage 스크립트
    StageManage sm;//StageManage 스크립트
    public ParticleSystem HitEffect;//피격 이펙트
    public ParticleSystem AttackEffect;//공격 이펙트
    

    public Camera_Shake camerashake;//카메라 흔들기
    private float delayTime;//원거리 공격할때 딜레이
    //public GameObject DmgText;
    public GameObject MoneyText;

    void Start()
    {
        camerashake = GameObject.Find("ViveCameraRig").GetComponent<Camera_Shake>();//카메라 흔들 스크립트 불러오기

        m = GameObject.Find("Manager").GetComponent<Manage>();//Manage 스크립트
        sm = GameObject.Find("StageManager").GetComponent<StageManage>();//StageManage 스크립트

        anim = GetComponent<Animator>();//애니메이션
        anim.SetBool("move", false);//최초 이동 false
        Death = false;//사망 판별도 false        

        HP = sm.Boss_HP;//StageManager 에서 보스 체력 가져오기
        MAX_HP = sm.Boss_MAXHP;//StageManager 에서 보스 최대 체력 가져오기

        speed = sm.Speed;//StageManager 에서 이동속도 가져오기
        
        tower = GameObject.Find("Tower").transform; //길찾기 목표 오브젝트
        attackTime = 0;//공격시간 비교 변수 초기화
        delayTime = 0;//원거리 공격 딜레이 변수 초기화
    }

    void Update()
    {
        if (anim.GetBool("move") == true)//애니메이션이 달리는 모션일 때 
        {
            StartCoroutine(camerashake.Shake(0.7f, 0.5f));
            transform.position = new Vector3(transform.position.x, -10, transform.position.z);//높이 고정? 이게 왜 있지
            this.transform.position = Vector3.Lerp(transform.position, tower.transform.position, speed * Time.deltaTime);//목표지점까지 이동
        }
        if (HP > (MAX_HP/2))//보스의 체력이 절반 이상일 때
        {
            //Debug.Log("HP FULL");
            if (Vector3.Distance(transform.position, tower.position) <= ATTACK_DISTANCE) //보스 1페이즈 피가 절반 이상일 때
            {
              // Debug.Log("attack_avail");
                //Debug.Log(Vector3.Distance(transform.position, tower.position));
                speed = 0;
                attackTime += Time.deltaTime; //경과 시간을 체크해서 좀비 공격시간 간격될 때마다 공격
                if (attackTime > ATTACK_TIME)//지정한 공격시간마다
                {
                    if (delayTime == 0)//원거리 공격 딜레이 시간이 0이면
                    {
                        anim.SetTrigger("Range_Attack");//원거리 공격 애니메이션을 재생
                    }
                    delayTime += Time.deltaTime;//지연 시간을 증가 시켜
                    if (delayTime > 1.0f)//지정한 값보다 지연시간이 높아질 경우
                    {
                        zombileaudio.clip = Rangeattackclip;//원거리 공격 소리로 변경하고
                        zombileaudio.Play();//해당 소리를 재생
                        StartCoroutine(camerashake.Shake(0.5f, 0.2f));//카메라 흔들기 함수 실행
                        Tower.Instance.Damage(rangedamage);// 목표의 데미지 함수 실행
                        delayTime = 0;//지연 시간 초기화
                       attackTime = 0;//공격비교시간 초기화
                    }
                }
            }
        }
        else if (HP< (MAX_HP / 2) && HP>0)//보스의 체력이 절반 이하이고 살아있을 경우
        {
           // Debug.Log(Vector3.Distance(transform.position, tower.position));
            if (sm.Pase_Change == 1)//페이즈 전환
            {

                PaseChange();//StageMange 에서 PaseChange() 함수를 실행
                if (Vector3.Distance(transform.position, tower.position) >= ATTACK_DISTANCE)
                { 
                    zombileaudio.clip = Runclip;//달리는 소리로 교체
                    zombileaudio.Play();//해당 소리를 재생
                    StartCoroutine(camerashake.Shake(0.5f, 0.05f));//카메라 흔들기 함수 실행
                }
                                                           //Debug.Log("Run " + Vector3.Distance(transform.position, tower.position)); //달려갈 때 보스와 목표까지 거리 로그
                
            }
            else if (sm.Pase_Change == 2 && Vector3.Distance(transform.position, tower.position) <= ATTACK_DISTANCE) //공격 범위 안에 들어오면,
            {
                //Debug.Log(Vector3.Distance(transform.position, tower.position));//보스와 목표까지 거리 로그
                speed = 0;//이동을 정지
                anim.SetBool("move", false);//이동 애니메이션 정지
                attackTime += Time.deltaTime; //경과 시간을 체크해서 좀비 공격시간 간격될 때마다 공격
                if (attackTime > 2.0f)//공격 시간마다 
                {
                    anim.SetTrigger("Attack");//공격 애니메이션 재생
                    StartCoroutine(camerashake.Shake(1.5f, 0.2f));//카메라 흔들기 함수 실행
                    zombileaudio.clip = attackclip;//공격하는 소리로 교체
                    zombileaudio.Play();//해당 소리 재생
                    AttackEffect.Play();//공격 이펙트 재생
                    Tower.Instance.Damage(damage);//목표의 데미지 함수 실행
                    attackTime = 0;//공격시간 초기화
                }
            }

        }
      
    }
    public void OnDamage(float damageAmount)//데미지 함수
    {
        HP -= damageAmount;//받은 데미지 만큼 체력 감소
        sm.Boss_HP = HP;
        if (Death == false)
        {
            //GameObject dmg = Instantiate<GameObject>(DmgText, this.transform.position, this.transform.rotation) as GameObject;
            //dmg.transform.parent = this.transform;
        }
        if (HP > 0)//살아 있을 때
        {
            HitEffect.Play();//피격 이펙트 재생           
        }
        else//보스가 죽었을 때
        {
            if (Death == false)
            {
                GameObject money_t = Instantiate<GameObject>(MoneyText, this.transform.position, this.transform.rotation) as GameObject;
                money_t.transform.parent = this.transform;
                speed = 0;//이동속도 0
                m.money += money;//Manage에서 돈 갱신
                m.moneyChange();//각종 UI들 돈 표시 갱신
                anim.SetTrigger("Death");//죽는 애니메이션 재생
                zombileaudio.clip = deathclip;//죽는 소리로 교체
                zombileaudio.Play();//해당 소리 재생
                sm.Kill_Count++;//StageManage에서 킬 카운트 추가
                sm.Boss_HP = HP;
                Destroy(gameObject, 2.0f);//오브젝트 제거
                Death = true;//죽음 판정
            }
        }
    }
    public void PaseChange()//페이즈 전환
    {
        ATTACK_DISTANCE = 10;//공격 사거리 변경
        speed = 1.0f;//이동속도 변경
        attackTime = 0;//공격비교시간 초기화
        anim.SetBool("move", true);//이동 애니메이션 재생
        sm.Pase_Change = 2;//페이즈 판별 변수 변경
    }
}
