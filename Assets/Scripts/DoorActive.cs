using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActive : MonoBehaviour
{
    [SerializeField]
    bool m_isLocked = false; //해당문이 잠겨있는지 Serialized로 설정(필수)
    [SerializeField]
    GameObject m_objKey = null;

    Animator m_animator = null;

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
    public void SetActiveAnimator(bool Active)
    {
        m_animator.SetBool("Activated", Active); //매개변수로 받아 이후 토글할 수 있게 사용
    }
    
    public GameObject GetKey() { 
        return m_objKey;
    }

    public void OpenDoor()
    {
        m_isLocked = false;
    }
}
