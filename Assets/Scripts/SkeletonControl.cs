using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonControl : MonoBehaviour
{
    //몬스터 AI에 따른 이동 스크립트
    //감지 레이더 반경과 관련 스크립트는 몬스터 머리에 부착

    // 몬스터의 이동 및 상태 타입설정
    public enum E_SKELETON_TYPE
    {
        None,
        Patrol,//순찰 모드
        Trace,//추격모드
        Return, // 처음위치로 돌아가기
        Max
    }


    public E_SKELETON_TYPE m_eSkeletonType = E_SKELETON_TYPE.None;
    //E_SKELETON_TYPE m_ePriviousSkeletonType = E_SKELETON_TYPE.None;

    Vector3 m_vecStartPosition = Vector3.zero;
    float m_fMoveSpeed = 0;
    Vector3 m_vecMoveDirection = Vector3.zero;

    //MosterRoom에서 벗어날시 공격을 멈추고 되돌아 가게 하기위해
    [SerializeField]
    Transform m_trRoomCenter = null;

    //순찰모드일때 좌우로 순찰한다.
    float m_fPatrolMaxX = 4;
    float m_fPatrolMinX = -4;
    bool m_isPatrolMaxX = false;
    bool m_isPatrolMinX = false;

    //정찰시 뒤로 돌 때
    bool m_isTurnning = false;
    Vector3 m_vecTurnBack = Vector3.zero;

    // Trace 모드일시 navigation 사용
    NavMeshAgent m_navmeshAgent = null;
    GameObject m_objPlayer = null;


    //애니메이션 관련
    Animator m_animator = null;
    bool m_isRunning = false;


    //오디오 관련
    AudioSource m_SkeletonAudioSource = null;
    //오디오 클립들
    [SerializeField]
    AudioClip m_clipSkeletonWalk = null;
    [SerializeField]
    AudioClip m_clipSkeletonRun = null;


    // Start is called before the first frame update
    void Start()
    {
        if (m_eSkeletonType == E_SKELETON_TYPE.None)
        {
            Debug.LogError("Skeleton의 열거형 상수타입을 지정해주세요");
        }

        m_vecStartPosition = transform.position;
        m_vecMoveDirection = Vector3.forward;
        m_vecTurnBack = transform.forward * -1;


        if(m_navmeshAgent == null)
        {
            m_navmeshAgent = GetComponent<NavMeshAgent>();
        }

        m_objPlayer = GameObject.Find("Player");

        if(m_animator == null)
        {
            m_animator = GetComponent<Animator>();
        }
        else
        {

        }

        if(m_SkeletonAudioSource == null)
        {
            m_SkeletonAudioSource= GetComponent<AudioSource>();
        }
        else
        {

        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale != 0)
        {
            if (m_eSkeletonType == E_SKELETON_TYPE.Patrol)
            {
                PatrolMode();
            }
            else if (m_eSkeletonType == E_SKELETON_TYPE.Trace)
            {
                TraceMode();
            }
            else if (m_eSkeletonType == E_SKELETON_TYPE.Return)
            {
                BackToStart();
            }
            else
            {

            }
        }
        else //timeScale이 0일 시 멈춤
        {
            m_fMoveSpeed = 0;
            m_navmeshAgent.isStopped = true;
            m_SkeletonAudioSource.Stop();
            m_SkeletonAudioSource.clip = null;
            m_SkeletonAudioSource.loop = false;
        }

    }

    private void LateUpdate()
    {
        if(Time.timeScale != 0)
        {
            SetAnimator();
            SetSkeletonAudio();
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(m_vecMoveDirection * m_fMoveSpeed);
    }

    //순찰모드 함수
    void PatrolMode()
    {
        Vector3 vecCurrentPos = transform.position;

        if (m_isTurnning == false)
        {
            //순찰중일때에는 걷기
            m_fMoveSpeed = 0.01f; //회전중이 아닐 때에만 걷는다.
            // 순찰 거리에 도달하면 방향을 바꾸어준다.
            if (vecCurrentPos.x >= m_fPatrolMaxX && m_isPatrolMaxX == false)
            {
                m_vecTurnBack = transform.forward * -1;
                m_isPatrolMaxX = true;
                m_isPatrolMinX = false;
                m_isTurnning = true;
            }
            else if (vecCurrentPos.x <= m_fPatrolMinX && m_isPatrolMinX == false)
            {
                m_vecTurnBack = transform.forward * -1;
                m_isPatrolMaxX = false;
                m_isPatrolMinX = true;
                m_isTurnning = true;
            }
            else
            {
            }

        }
        else if (m_isTurnning == true)
        {
            //회전시에는 멈춘다.
            m_fMoveSpeed = 0;
            int nTrunSpeed = 5;
            //선형보간회전하기위해
            //PlayerControl의 CameraRotate 처럼 사원수의 LookRotation함수로 해당 좌표를 바라보는 회전값을 받아온다.
            Quaternion qTurnRotation = Quaternion.LookRotation(m_vecTurnBack);
            //Quaternion의 Lerp함수를 통하여 선형보간하며 회전
            transform.rotation = Quaternion.Lerp(transform.rotation, qTurnRotation, Time.deltaTime * nTrunSpeed);
            //Debug.Log(transform.rotation.ToString());
            // qTurnRotation 과 현재 rotation의 오일러각 y축 차이가 5이하라면 목표 뒤 방향으로 바꾸어준다.
            // -> 완전히 180로 돌리고 m_isTurnning을 flase로 바꾸어 다시 앞으로 움직이게 한다.
            float fDiffRotate = Mathf.Abs(transform.rotation.eulerAngles.y - qTurnRotation.eulerAngles.y);
            if (fDiffRotate <= 5f)
            {
                transform.rotation = Quaternion.LookRotation(m_vecTurnBack);
                m_isTurnning = false;
            }
        }

        // 레이더로 플레이어 감시
        // 자식오브젝트 head에 달린 SkeletonRader의 Trigger내에 있는 플레이어를 향해 레이를 쏘아
        // 맞을시 추격모드로 변경
        bool isLookPlayer = GetComponentInChildren<SkeletonRader>().IsDetectPlayer();
        if (isLookPlayer)
        {
            m_fMoveSpeed = 0;
            m_eSkeletonType = E_SKELETON_TYPE.Trace;
        }
        else
        {

        }

    }

    //추격모드 함수
    void TraceMode()
    {
        if (m_navmeshAgent != null)
        {
            
            if(m_trRoomCenter != null)
            {
                m_isRunning = true;
                Vector3 vecPlayerPos = m_objPlayer.transform.position;
                //플레이어와 출/입구의 위치 파악
                float fSafeDistance = Vector3.Distance(vecPlayerPos, m_trRoomCenter.position);
                //m_navmeshAgent 목적지를 플레이어로 설정
                m_navmeshAgent.SetDestination(vecPlayerPos);
                m_navmeshAgent.isStopped = false; //nevmeshAgent를 활성화
                                                  //Debug.Log(vecPlayerPos);

                //플레이어가 출/입구에 다다르면 돌아간다.
                if (fSafeDistance >= 10f)
                {
                    m_eSkeletonType = E_SKELETON_TYPE.Return;
                }
                else
                {

                }
            }
            else
            {
                Debug.LogError("m_trRoomCenter이 없습니다.");
            }
        }
        else
        {
            Debug.LogError("m_navmeshAgent가 없습니다.");
        }
    }

    // 시작위치로 다시 돌아가는 함수
    void BackToStart()
    {
        float fTargetDistance = Vector3.Distance(transform.position, m_vecStartPosition);
        m_navmeshAgent.SetDestination(m_vecStartPosition);
        if (fTargetDistance <= 0.5f)
        {
            m_isRunning = false;
            //시작위치로 초기화 후 정찰모드로 변경
            //navmeshAgent를 비활성화한다.-> 정찰모드일때에는 navigation을 사용하지 않기때문
            m_navmeshAgent.isStopped = true;
            transform.position = m_vecStartPosition;
            m_isTurnning = true;
            m_eSkeletonType = E_SKELETON_TYPE.Patrol;
        }
        else
        {
            
        }
    }

    void SetAnimator()
    {
        if(m_animator != null)
        {
            m_animator.SetBool("isRun", m_isRunning);
        }
        else
        {
            Debug.LogError("m_animator이 없습니다.");
        }
    }

    void SetSkeletonAudio()
    {
        if (m_eSkeletonType == E_SKELETON_TYPE.Patrol)
        {
            if(m_clipSkeletonWalk != null)
            {
                //현재 오디오 클립이 해당로직에서 필요한 클립이 아닐시 한번만 실행
                // 해당조건이 없으면 오디오가 끝나기 전에 계속 Play하기에 문제가 생긴다.
                if (m_SkeletonAudioSource.clip != m_clipSkeletonWalk)
                {
                    m_SkeletonAudioSource.clip = m_clipSkeletonWalk;
                    //walk나 run은 조건을 만족할 시 반복하여 재생 
                    m_SkeletonAudioSource.loop = true;
                    m_SkeletonAudioSource.Play();
                }
            }
            else
            {
                Debug.LogError("m_clipSkeletonWalk가 없습니다.");
            }
        }
        else //정찰모드가 아닐시는 뛰는 오디오 클립 재생
        {
            if (m_clipSkeletonRun != null)
            {
                if (m_SkeletonAudioSource.clip != m_clipSkeletonRun)
                {
                    m_SkeletonAudioSource.clip = m_clipSkeletonRun;
                    //walk나 run은 조건을 만족할 시 반복하여 재생 
                    m_SkeletonAudioSource.loop = true;
                    m_SkeletonAudioSource.Play();
                }
            }
            else
            {
                Debug.LogError("m_clipSkeletonWalk가 없습니다.");
            }
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        //플레이어와 충돌시
        if (collision.collider.CompareTag("Player"))
        {
            //공격소리 재생후 Return으로 Type변경
            GetComponentInChildren<SkeletonRader>().SetAttackAudio();
            m_eSkeletonType = E_SKELETON_TYPE.Return;
        }
        else
        {

        }
    }
}
