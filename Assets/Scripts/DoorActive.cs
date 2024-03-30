using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActive : MonoBehaviour
{
    [SerializeField]
    bool m_isLocked = false; //�ش繮�� ����ִ��� Serialized�� ����(�ʼ�)
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
    public void SetActiveAnimator(bool Active)
    {
        m_animator.SetBool("Activated", Active); //�Ű������� �޾� ���� ����� �� �ְ� ���
    }
    
    public GameObject GetKey() { 
        return m_objKey;
    }

    public void OpenDoor()
    {
        m_isLocked = false;
    }
}
