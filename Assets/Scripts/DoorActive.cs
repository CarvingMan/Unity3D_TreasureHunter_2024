using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActive : MonoBehaviour
{
    // ���� ������ ���� ���/������ �����ϰ� ������ ���� ���� ��ũ��Ʈ

    [SerializeField]
    bool m_isLocked = false; //�ش繮�� ����ִ��� Serialized�� ����(�ʼ�)
    [SerializeField]
    GameObject m_objKey = null;

    Animator m_animator = null;

    //����� ����
    AudioSource m_DoorAudioSource = null;
    // ������ ����� Ŭ���� serializeField�� �Ͽ� �� �������� �ٸ� Ŭ�� ����
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
    
    //�� �Ӽ� ���� �� ����/ Getta Setta
    public bool GetIsLocked() { return m_isLocked; }
    public void SetIsLocked(bool isLocked) { m_isLocked = isLocked; }

    // ���� �ִϸ������� �Ķ���� Active ���¸� �������� �Լ�
    // PlyerControl ��ũ��Ʈ�� Trigger���� ȣ��.
    public bool GetIsActive()
    {
        return m_animator.GetBool("Activated");
    }
    // ���� ���� �ִϸ��̼��� Ȱ��ȭ�� �Լ�
    // PlyerControl ��ũ��Ʈ�� Trigger���� ȣ��.
    public void SetActiveAnimator(bool isActive)
    {
        m_animator.SetBool("Activated", isActive); //�Ű������� �޾� ���� ����� �� �ְ� ���
        //�ִϸ��̼ǿ� ���߾� ����� ���
        SetDoorAudio(isActive);
    }
    
    // ���踦 �������� �Լ�
    public GameObject GetKey() { 
        return m_objKey;
    }

    // �������
    public void UnLockDoor()
    {
        m_isLocked = false;
    }

    // ����� ���
    void SetDoorAudio(bool isActive)
    {
        //isActive�� true�� �������̴�.
        if (isActive == true)
        {
            if (m_clipDoorOpen != null)
            {
                //���� ����� Ŭ���� �ش�������� �ʿ��� Ŭ���� �ƴҽ� �ѹ��� ����
                if (m_DoorAudioSource.clip != m_clipDoorOpen)
                {
                    m_DoorAudioSource.clip = m_clipDoorOpen;
                    m_DoorAudioSource.loop = false;
                    m_DoorAudioSource.Play();
                }
            }
            else
            {
                Debug.LogError("m_clipDoorOpen�� �����ϴ�.");
            }
        }
        else //���� ������
        {
            if (m_clipDoorClose != null)
            {
                //���� ����� Ŭ���� �ش�������� �ʿ��� Ŭ���� �ƴҽ� �ѹ��� ����
                if (m_DoorAudioSource.clip != m_clipDoorClose)
                {
                    m_DoorAudioSource.clip = m_clipDoorClose;
                    m_DoorAudioSource.loop = false;
                    m_DoorAudioSource.Play();
                }
            }
            else
            {
                Debug.LogError("m_clipDoorClose�� �����ϴ�.");
            }
        }
    }
}
