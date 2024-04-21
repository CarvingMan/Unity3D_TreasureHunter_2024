using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestActive : MonoBehaviour
{
    Animator m_anmator = null;
    bool m_isOpen = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        
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
        }
        else
        {
            Debug.LogError("m_animator가 없습니다.");
        }
    }
}
