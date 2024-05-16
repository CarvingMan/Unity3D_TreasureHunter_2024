using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestActive : MonoBehaviour
{
    //���� ���� ���� ��ũ��Ʈ DoorActive�� ������ ��������� Ȥ���� ���� �ٸ� ������ �߰��� �� �����Ƿ�
    //������ �ۼ�
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
                Debug.LogError("m_clipChestOpen�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("m_animator�� �����ϴ�.");
        }
    }
}
