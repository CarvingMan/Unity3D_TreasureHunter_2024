using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonRader : MonoBehaviour
{
    /*
     * ArmedSkeleton의 PT_Male_Armor_Skeleton_01_head(머리)에 추가한 스크립트
     * 스켈레톤의 Head 좌표에서 Trigger 콜라이더를 통해 플레이어를 감지하고 감지가 되었다면 플레이어 몸통방향으로 
        레이캐스트를 쏘아 앞에 엄폐물이 있다면 그대로, 플레이어의 Body가 검출된다면 SkeletonControl의 타입을 Attack으로 바꾸어준다.
        위와같은 방법으로 플레이어가 엄폐물뒤에 웅크려 가려져 있다면 레이더에 걸리지않는다.
     */

    //플레이어가 현재 시야에 있는지 확인
    bool m_isInPlayer = false; 

    //플레이어 몸통의 Vector
    Vector3 m_vecPlayerBody = Vector3.zero;

    //audio 관련
    //스켈레톤 PT_Spine의 Head에 달려있는 오디오 소스 
    AudioSource m_HeadAudioSource = null;
    [SerializeField]
    AudioClip m_clipSkeletonRoar = null;
    [SerializeField]
    AudioClip m_clipSkeletonAttack = null;

    private void Start()
    {
        if(m_HeadAudioSource == null)
        {
            m_HeadAudioSource=GetComponent<AudioSource>();
        }
        else
        {

        }
    }

    public bool IsDetectPlayer()
    {
        if (m_isInPlayer)
        {
            RaycastHit hit;
            float fDistance = 6f;
            //쉬프트 연산 하여 각각의 레이어에 해당하는 비트를 1로하고 비트 or연산자를 통해 레이어마스크 생성
            //이를 통해 해당 레이어를 사용한 오브젝트만 레이캐스트를 맞춘다.(엄폐물과 플레이어 몸통)
            //->엄폐물에서 navigation레이어로 변경: NavMesh Surface에서 navigation 레이어 사용하기 때문
            int nHitLayer = (1 << LayerMask.NameToLayer("PlayerBody")) | (1 << LayerMask.NameToLayer("Navigation"));
            
            // 플레이어body position(종점)에서 transform.position(시점)을 한 후 normalized를 통하여
            // 크기가 1인 방향 벡터를 구하여 해당방향으로 레이를 쏜다.
            Vector3 vecDirectionToPlayer = (m_vecPlayerBody - transform.position).normalized;
            bool isCast =  Physics.Raycast(transform.position, vecDirectionToPlayer, out hit, fDistance, nHitLayer);
            //Debug.DrawRay(transform.position, vecDirectionToPlayer * 5, Color.red);
            if (isCast)
            {
                //만약 엄폐물 뒤에 숨으면 Obstacle 태그가 먼저 맞을것이다.
                if (hit.collider.CompareTag("PlayerBody"))
                {
                    //Debug.Log("플레이어 감지");
                    //울음소리 재생
                    SetAudioRoar();
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false; //만약 감지레이더 안에 플레이어가 없을시 레이캐스트를 쏘지 않는다.
        }
    }

    //IsDetectPlayer()에서 플레이어 를 감지했을시 스켈레톤의 울음소리(Roar) 재생
    void SetAudioRoar()
    {
        //clip이 m_clipSkeletonRoar가 아닐시 오디오 세팅
        if(m_HeadAudioSource.clip != m_clipSkeletonRoar)
        {
            //오디오 클립 변경/ 루프false/ Play
            m_HeadAudioSource.clip = m_clipSkeletonRoar;
            m_HeadAudioSource.loop = false;
            m_HeadAudioSource.Play();
            //오디오 클립 시간
            float fClipTime = m_clipSkeletonRoar.length;
            //클립시간 이후 초기화
            StartCoroutine(CorAudioInit(fClipTime));
        }
        else
        {
        }
    }

    //공격소리 클립 재생 // 호출은 SkeletonControl에서 하기에 pubic
    public void SetAttackAudio()
    {
        if (m_clipSkeletonAttack != null)
        {
            if (m_HeadAudioSource.clip != m_clipSkeletonAttack)
            {
                m_HeadAudioSource.clip = m_clipSkeletonAttack;
                m_HeadAudioSource.loop = false;
                m_HeadAudioSource.Play();
                //오디오 클립 시간
                float fClipTime = m_clipSkeletonAttack.length;
                //클립시간 이후 초기화
                StartCoroutine(CorAudioInit(fClipTime));
            }
            else
            {
            }
        }
        else
        {
            Debug.LogError("m_clipSkeletonAttack이 없습니다.");
        }
    }

    //오디오소스 초기화 함수
    IEnumerator CorAudioInit(float fClipTime)
    {
        //fClipTime이후 오디오 소스 초기화
        yield return new WaitForSeconds(fClipTime);
        m_HeadAudioSource.Stop();
        m_HeadAudioSource.loop = false;
        m_HeadAudioSource.clip = null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerBody"))
        {
           m_vecPlayerBody = other.transform.position; // 플레이어 몸통 좌표
           m_isInPlayer = true; //플레이어가 레이더 안에 있다(발견X)
        }
        else
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerBody"))
        {
            m_isInPlayer = false;
        }
    }

}
