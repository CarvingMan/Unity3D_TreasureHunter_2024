using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActive : MonoBehaviour
{
    // 여러 종류의 문의 잠김/열림을 설정하고 문열림 닫힘 관련 스크립트

    [SerializeField]
    bool m_isLocked = false; //해당문이 잠겨있는지 Serialized로 설정(필수)
    [SerializeField]
    GameObject m_objKey = null;

    Animator m_animator = null;

    //오디오 관련
    AudioSource m_DoorAudioSource = null;
    // 문열림 오디오 클립은 serializeField로 하여 문 종류별로 다른 클립 삽입
    [SerializeField]
    AudioClip m_clipDoorOpen = null;
    [SerializeField]
    AudioClip m_clipDoorClose = null;

    // Start is called before the first frame update
    void Start()
    {
        if(m_animator == null)
        {
            m_animator = GetComponent<Animator>();
        }
        else
        {

        }

        if(m_DoorAudioSource == null)
        {
            m_DoorAudioSource = GetComponent<AudioSource>();
        }
    }
    
    //문 속성 참소 및 변경/ Getta Setta
    public bool GetIsLocked() { return m_isLocked; }
    public void SetIsLocked(bool isLocked) { m_isLocked = isLocked; }

    // 현재 애니메이터의 파라미터 Active 상태를 가져오는 함수
    // PlyerControl 스크립트의 Trigger에서 호출.
    public bool GetIsActive()
    {
        return m_animator.GetBool("Activated");
    }
    // 현재 문의 애니메이션을 활성화할 함수
    // PlyerControl 스크립트의 Trigger에서 호출.
    public void SetActiveAnimator(bool isActive)
    {
        m_animator.SetBool("Activated", isActive); //매개변수로 받아 이후 토글할 수 있게 사용
        //애니메이션에 맞추어 오디오 재생
        SetDoorAudio(isActive);
    }
    
    // 열쇠를 가져오는 함수
    public GameObject GetKey() { 
        return m_objKey;
    }

    // 잠김해제
    public void UnLockDoor()
    {
        m_isLocked = false;
    }

    // 오디오 재생
    void SetDoorAudio(bool isActive)
    {
        //isActive가 true면 문열림이다.
        if (isActive == true)
        {
            if (m_clipDoorOpen != null)
            {
                //현재 오디오 클립이 해당로직에서 필요한 클립이 아닐시 한번만 실행
                if (m_DoorAudioSource.clip != m_clipDoorOpen)
                {
                    m_DoorAudioSource.clip = m_clipDoorOpen;
                    m_DoorAudioSource.loop = false;
                    m_DoorAudioSource.Play();
                }
            }
            else
            {
                Debug.LogError("m_clipDoorOpen이 없습니다.");
            }
        }
        else //문이 닫힐때
        {
            if (m_clipDoorClose != null)
            {
                //현재 오디오 클립이 해당로직에서 필요한 클립이 아닐시 한번만 실행
                if (m_DoorAudioSource.clip != m_clipDoorClose)
                {
                    m_DoorAudioSource.clip = m_clipDoorClose;
                    m_DoorAudioSource.loop = false;
                    m_DoorAudioSource.Play();
                }
            }
            else
            {
                Debug.LogError("m_clipDoorClose이 없습니다.");
            }
        }
    }
}
