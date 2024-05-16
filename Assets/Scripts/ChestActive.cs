using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestActive : MonoBehaviour
{
    //상자 열림 관련 스크립트 DoorActive랑 로직은 비슷하지만 혹여나 이후 다른 로직이 추가될 수 있으므로
    //별개로 작성
    Animator m_anmator = null;
    bool m_isOpen = false;

    AudioSource m_ChestAudioSource = null;
    [SerializeField]
    AudioClip m_clipChestOpen = null;

    // Start is called before the first frame update
    void Start()
    {
        if(m_anmator == null)
        {
            m_anmator = GetComponent<Animator>();
        }
        else
        {

        }

        if(m_ChestAudioSource == null)
        {
            m_ChestAudioSource=GetComponent<AudioSource>();
        }
        else
        {

        }
    }

    public bool GetIsOpen()
    {
        return m_isOpen;
    }

    public void OpenChest()
    {
        if(m_anmator != null)
        {
            m_anmator.SetBool("Activated", true);
            m_isOpen = true;
            if (m_clipChestOpen != null)
            {
                if(m_ChestAudioSource.clip != m_ChestAudioSource)
                {
                    m_ChestAudioSource.clip = m_clipChestOpen;
                    m_ChestAudioSource.loop = false;
                    m_ChestAudioSource.Play();
                }
            }
            else
            {
                Debug.LogError("m_clipChestOpen이 없습니다.");
            }
        }
        else
        {
            Debug.LogError("m_animator가 없습니다.");
        }
    }
}
