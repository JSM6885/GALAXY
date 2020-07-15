using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

//지상 몬스터들의 스크립트

public class Enemy : MonoBehaviour, IDamageable
{
    public static Enemy Instance;
    UnityEngine.AI.NavMeshAgent agent;//길찾기 네비
    Transform tower;//목표물
    public float ATTACK_TIME;//공격시간
    private float attackTime = 0;//공격비교시간
    public AudioSource zombileaudio;//소리
    public AudioClip deathclip;//죽는 소리
    public AudioClip attackclip;//공격 소리
    public AudioClip hitclip;//맞는 소리
    public Animator anim;//애니메이션
    public float ATTACK_DISTANCE;//공격 사정거리
    public float damage;//데미지
    public float HP;//체력
    public float HP_origin;
    public float speed;//속도
    public bool Death;//사망 판정
    public int money;//주는 재화량
    Manage m;//Manage 스크립트
    StageManage sm;//StageManage 스크립트
    public ParticleSystem HitEffect;//피격 이펙트
    public ParticleSystem AttackEffect;//공격 이펙트
    private float speed_temp;//피격 시 경직 후 다시 원래 속도로 돌리기 위한 변수
    private float delayTime;//피격 경직 시간 계산
    private float HP_temp;//피격 시 체력에 변동이 있다는 것을 비교
    //public GameObject DmgText;
    public GameObject MoneyText;
    private int Random_Target;

    public Camera_Shake camerashake;//카메라 흔들기
    public float shakeTime;//흔들 시간
    public float shakePower;//흔드는 정도

    public Slider HP_Slide;

    private float DeathTime;

    public void Start()
    {
        Random_Target = Random.Range(0, 3);
        if (Random_Target == 0)
        { 
            tower = GameObject.Find("Tower").transform; //길찾기 목표 오브젝트
        }
        else if(Random_Target == 1)
        {
            tower = GameObject.Find("Tower (1)").transform; //길찾기 목표 오브젝트
        }
        else if (Random_Target == 2)
        {
            tower = GameObject.Find("Tower (2)").transform; //길찾기 목표 오브젝트
        }
        m = GameObject.Find("Manager").GetComponent<Manage>();//Manage 스크립트
        sm = GameObject.Find("StageManager").GetComponent<StageManage>();//StageManage 스크립트
        camerashake = GameObject.Find("ViveCameraRig").GetComponent<Camera_Shake>();//카메라 흔들 스크립트 불러오기
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>(); //길찾기 AI네비게이션
        agent.destination = tower.position;//목표지점 설정
        agent.speed = speed;//네비 이동속도도 설정
        attackTime = ATTACK_TIME;//공격시간 초기화
        speed_temp= speed;//피격 대비 속도값 저장
        anim = GetComponent<Animator>();//애니메이션
        anim.SetBool("move", true);//이동 애니메이션 시작
        Death = false;//사망 판정 FALSE
        HP_temp = HP;//피격판정을 위한 체력 임시값 저장
        delayTime = 0;//피격 경직시간 초기화
        HP_Slide.maxValue = HP;
        HP_Slide.value = HP;
        HP_origin = HP;
        DeathTime = 0;
    }
    
    void Update()
    {

        
        if(HP>0 && HP_temp != HP)//피격 시 경직 스크립트
        {           
            delayTime += Time.deltaTime;//경직 시간 계산
            if (delayTime >= 0.5f)//경직 해제 
            {
                agent.speed = speed_temp;//속도 원상복귀
                HP_temp = HP;//경직 비교를 위한 체력 재입력
                delayTime = 0;//경직시간 초기화
            }
        }

        if (HP>0)//살아있을 때
        {
            if (agent.remainingDistance <= ATTACK_DISTANCE) //공격 범위 안에 들어오면,
            {
                agent.speed = 0;//이동속도를 0으로 하여 더 이상 이동 제한
                anim.SetBool("move", false);//이동 애니메이션 정지
                attackTime += Time.deltaTime; //경과 시간을 체크해서 공격시간 간격될 때마다 공격
                if (attackTime > ATTACK_TIME)//공격시간이 됬을 때
                {
                    StartCoroutine(camerashake.Shake(shakeTime, shakePower));//카메라 흔들기 작동
                    zombileaudio.clip = attackclip;//공격 소리
                    zombileaudio.Play();//해당 소리 재생
                    anim.SetTrigger("Attack");//공격 애니메이션 재생
                    AttackEffect.Play();//공격 이펙트 재생
                    Tower.Instance.Damage(damage);// 목표의 데미지 함수 실행
                    attackTime = 0;//공격 시간 초기화
                }
            }
        }

        if (HP<=0)
        {
            DeathTime += Time.deltaTime;
            if (DeathTime > 2.0f)
            {
                Death = true;//사망 판정 TRUE
                gameObject.SetActive(false);
            }
        }
        
    }

    public void OnDamage(float damageAmount)//데미지 받을 시 함수
    {
        HP -= damageAmount;//체력을 받은 데미지만큼 감소 
        HP_Slide.value = HP;
        if (HP > 0)//살아있을 경우
        {
            agent.speed = 0;//이동 정지를 위한 속도 0
            HitEffect.Play();//피격 이펙트 재생            
            zombileaudio.clip = hitclip;//피격 소리
            zombileaudio.Play();//해당 소리 재생
            anim.SetTrigger("Hit");//피격 애니메이션 재생           
        }
        else
        {
            if (Death == false)
            {
                if (DeathTime == 0)
                { 
                    GameObject money_t = Instantiate<GameObject>(MoneyText, this.transform.position, this.transform.rotation) as GameObject;
                    money_t.transform.parent = this.transform;
                    m.money += money;//재화 추가
                    agent.speed = 0;
                    anim.SetTrigger("Death");//죽는 애니메이션 재생agent.speed = 0;//이동속도 0으로 정지
                    m.moneyChange();//재화 UI 갱신
                    zombileaudio.clip = deathclip;//죽는 소리
                    zombileaudio.Play();//해당 소리 재생
                    sm.monster_count[sm.stage]--;//해당 스테이지(웨이브) 의 총 몹의 수에서 감소
                    sm.Kill_Count++;//죽인 숫자 추가     
                    DeathTime += Time.deltaTime;
                }
                
                //Destroy(gameObject, 2.0f);//오브젝트 파괴
            }
        }
    }
}
