using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

//총 스크립트

public class gun : MonoBehaviour
{
    //public SteamVR_Action_Vibration hapticSignal;//진동

    public Animator m_Animator; // 총의 애니메이터
    public Transform m_FireTransform; //총구의 위치
    public ParticleSystem m_ShellEjectEffect; //탄피 배출 효과 
    public ParticleSystem m_ShellEjectEffect2; //탄피 배출 효과 
    public ParticleSystem m_MuzzleFlashEffect; // 총구 화염 효과
    public ParticleSystem HitEffect; //탄 피격 이펙트

    public AudioSource m_GunAudioPlayer; //총 소리 재생기
    public AudioClip m_ShotClip; // 발사 소리
    public AudioClip m_ReloadClip; // 재장전 소리

    public LineRenderer m_BulletLineRender; //총알 궤적 렌더러

    public GameObject Light;

    //몬스터나 배경에 날 피격 자국이 같으니 이상하고 구분하자니 뒤로 미뤄둘 기능같음
    //public GameObject m_ImpactPrefab; // 피탄 이펙트&데칼 

    public Text m_AmmoText; //남은 탄환수 UI

    [HideInInspector]
    public int m_MaxAmmo; //탄창의 최대 탄약 수
    [HideInInspector]
    public float m_TimeBetFire; //발사 사이 시간간격
    [HideInInspector]
    public float m_Damage;//총이 주는 데미지
    [HideInInspector]
    public float m_ReloadTime;//총의 재장전 시간
    [HideInInspector]
    public float m_FireDistance; //총의 사정거리

    private enum State { Ready, Empty, Reloading}; //총의 3가지 상태, 준비, 탄약 빔, 재장전
    private State m_CurrentState; // 현재 총의 상태

    private float m_LastFireTime; //총을 마지막으로 발사한 시점
    private int m_CurrentAmmo; //탄창에 남은 현재 탄약 수

    public bool shot;//발사 판별 변수 - Recoil.cs 총기반동에 사용

    public Camera_Shake camerashake;//카메라 흔들기
    public float shakeTime;//흔들 시간
    public float shakePower;//흔드는 정도

    public GunManage gunmanage;//GunManage 스크립트

    private float effect_time;

    public Manage m;
    
    void Start()
    {
        m_CurrentState = State.Empty; //
        m_LastFireTime = 0; //마지막으로 총 쏜 시점 초기화

        m_BulletLineRender.positionCount = 2; //라인렌더러가 사용할 정점을 두 개 지정
        m_BulletLineRender.enabled = false; //라인렌더러를 끔

        camerashake = GameObject.Find("ViveCameraRig").GetComponent<Camera_Shake>();//카메라 흔들 스크립트 불러오기

        m = GameObject.Find("Manager").GetComponent<Manage>();
        gunmanage = GameObject.Find("GunManager").GetComponent<GunManage>();//GunManage 스크립트    

        shot = false;//발사 판별 변수 초기화
        //m_CurrentAmmo = gunmanage.Current_Gun_MaxAmmo; //탄약 최대 충전

        Init();
        UpdateUI(); //Ui 를 갱신
        Debug.Log("gun Start()");
    }

    public void Init()
    {
        m_CurrentAmmo = gunmanage.Current_Gun_MaxAmmo; //탄약 최대 충전
        m_MaxAmmo = gunmanage.Current_Gun_MaxAmmo;//GunManage 에서 현재 총의 최대탄창 불러오기
        m_TimeBetFire = gunmanage.Current_Gun_TimeBetFire; //GunManage 에서 현재 총의 공격속도 불러오기
        m_Damage = gunmanage.Current_Gun_Damage;//GunManage 에서 현재 총의 데미지 불러오기
        m_ReloadTime = gunmanage.Current_Gun_ReloadTime;//GunManage 에서 현재 총의 재장전 속도 불러오기
        m_FireDistance = gunmanage.Current_Gun_FireDistance; //GunManage 에서 현재 총의 사정거리 불러오기
    }

    //private void Update()
    //{
    //    m_MaxAmmo = gunmanage.Current_Gun_MaxAmmo;//GunManage 에서 현재 총의 최대탄창 불러오기
    //    m_TimeBetFire = gunmanage.Current_Gun_TimeBetFire; //GunManage 에서 현재 총의 공격속도 불러오기
    //    m_Damage = gunmanage.Current_Gun_Damage;//GunManage 에서 현재 총의 데미지 불러오기
    //    m_ReloadTime = gunmanage.Current_Gun_ReloadTime;//GunManage 에서 현재 총의 재장전 속도 불러오기
    //    m_FireDistance = gunmanage.Current_Gun_FireDistance; //GunManage 에서 현재 총의 사정거리 불러오기
    //}

    public void Fire() //발사 처리를 하는 함수
    {
        //총이 준비된 상태이고 && 현재시간 >= 마지막 발사 시점+발사간격
        if(m.TimeStop==false && m_CurrentState == State.Ready && Time.time  >= m_LastFireTime + m_TimeBetFire)
        {
            m_LastFireTime = Time.time; //마지막으로 총을 쏜 시점이 현재 시점으로 갱신    
            //Handheld.Vibrate(); //컨트롤러 진동?
            // hapticAction.Execute(0, 1, 100, 1,0);// 컨트롤러 진동?
            Shot();//발사 함수 실행
            UpdateUI();//탄 UI 
            shot = true;//총기반동 판별 TRUE            
            StartCoroutine(camerashake.Shake(shakeTime, shakePower));//카메라 흔들기 작동
        }
    }

    public void Shot() //실제 발사 처리를 하는 함수
    {
        RaycastHit hit; //레이캐스트 정보를 저장하는 컨테이너.
        //총을 쏴서 총알이 맞은 곳 : 총구 위치 값+ 총구 위치 앞쪽 방향 * 사정거리
        Vector3 hitPosition = m_FireTransform.position + m_FireTransform.forward * m_FireDistance;

        // 레이캐스트 (시작지점, 방향, 충돌 정보 컨테이너, 사정거리)
        if(Physics.Raycast(m_FireTransform.position, m_FireTransform.forward, out hit, m_FireDistance))
        {
            //상대방이 IDamageable 로서 가져와 진다면,
            //상대방의 OnDamage 함수를 실핼시켜서 데미지를 전달한다.

            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if (target != null)
            {
                target.OnDamage(m_Damage);
            }
            //충돌 위치를 가져오기
            hitPosition = hit.point;
            //피탄 효과 게임 오브젝트를 복제 생성하고, 충돌 지점에 충돌한 표면의 방향으로 생성
           //GameObject decal = Instantiate(m_ImpactPrefab, hitPosition, Quaternion.LookRotation(hit.normal));
           // decal.transform.SetParent(hit.collider.transform);  //피탄 효과는 물체가 움직이면 따라가야 하므로 오브젝트 자식으로 넣음
        }

        
        StartCoroutine(ShotEffect(hitPosition)); //발사 이펙트 재생 시작
        
        m_CurrentAmmo--; //남은 탄환의 수를 -1씩 감소

        if (m_CurrentAmmo <= 0)
        {
            m_CurrentState = State.Empty;
        }

    }

    private IEnumerator ShotEffect(Vector3 hitPosition) //발사 이펙트를 재생하고 총알 궤적 재생
    {
        m_Animator.SetTrigger("Fire"); //Fire 트리거를 당김

        m_BulletLineRender.enabled = true; //총알 궤적 렌더러 켬

        m_BulletLineRender.SetPosition(0, m_FireTransform.position); //직선의 첫번째 점은 총구의 위치

        m_BulletLineRender.SetPosition(1, hitPosition); // 직선의 두번째 점은 입력되는 피탄 위치

        m_MuzzleFlashEffect.Play(); //총구 화염 효과
        m_ShellEjectEffect.Play(); //탄피 배출 효과
        if (m_ShellEjectEffect2 != null)
        { 
            m_ShellEjectEffect2.Play(); //탄피 배출 효과
        }     

        if (m_GunAudioPlayer.clip != m_ShotClip)
        {
            m_GunAudioPlayer.clip = m_ShotClip; //총 발사 소리를 로딩
        }
        Light.SetActive(true);
        m_GunAudioPlayer.Play(); //총 소리 재생

        HitEffect.transform.position = hitPosition;
        HitEffect.Play();

        yield return new WaitForSeconds(0.07f); //코루틴 처리 쉬는 시간

        m_BulletLineRender.enabled = false; //코루틴 잠깐 쉬고 라인렌더러를 꺼주어 궤적이 번쩍 하는 효과생김
        Light.SetActive(false);
        //HitEffect.Emit(100);
    }

    private void UpdateUI() //남은 탄약수를 갱신해서 UI에 표시
    {
        Debug.Log("UpdateUI m_CurrentAmmo = "+ m_CurrentAmmo+ " /m_CurrentState = "+ m_CurrentState);
        if (m_CurrentAmmo ==0 && m_CurrentState == State.Empty)
        {
            m_AmmoText.text = "Empty";
        }
        else if (m_CurrentState == State.Reloading)
        {
            m_AmmoText.text = "Reloading";
        }
        else
        {
            m_AmmoText.text = m_CurrentAmmo.ToString();
            m_CurrentState = State.Ready;
        }
    }

    public void Reload() //재장전
    {
        if (m_CurrentState != State.Reloading)
        {
            StartCoroutine(ReloadRoutine());
        }
    }

    private IEnumerator ReloadRoutine() //실제 재장전 처리가 되는 부분
    {
        m_Animator.SetTrigger("Reload"); //Reload 파라키터 트리거를 당김
        m_CurrentState = State.Reloading; //현재 상태를 재장전 상태로 전환

        m_GunAudioPlayer.clip = m_ReloadClip; //오디오 소스의 클립을 재장전 소리로 교체
        m_GunAudioPlayer.Play();

        UpdateUI();

        yield return new WaitForSeconds(m_ReloadTime); //재장전 시간만큼 처리를 쉰다

        m_CurrentAmmo = m_MaxAmmo; //탄약 최대 충전
        m_CurrentState = State.Ready;
        UpdateUI();
    }
}
