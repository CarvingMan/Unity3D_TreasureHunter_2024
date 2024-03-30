using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    /* GetComponet�� SerializedField�� ������Ʈ�� �������� ����� �� �� ����Ͽ���.
    ��ǻ� SerializedField ������� ���ϱ��ϳ� ���� ������Ʈ���� �ٲ�� ��찡 �ƴϸ�
    ���������� �ʴ´�. */

    GameObject m_objPlayer = null; //�÷��̾� ������Ʈ

    // ���¹̳� ���� ������� 
    [SerializeField]
    GameObject m_objStamina = null;  // setActive toggle��ų ���׹̳� ������Ʈ(canvas)
    [SerializeField]
    Image m_imgStaminaBackground = null; //���¹̳� ������
    [SerializeField]
    Image m_imgStaminaGauge = null; //���¹̳� ������
    float m_fStaminaMaxWith = 0; // ���׹̳� ������ �ִ����
    float m_fCurrentStaminaRate = 0; //���� ���¹̳� ����

    //Props�޼��� ���� �������
    GameObject m_objPropsMessage = null; //text UI ��ü�� setAcive toggle ������Ʈ
    Text m_txtPropsName = null; // Props�̸�
    Text m_txtPropsDescription = null; //Props����


    // Start is called before the first frame update
    void Start()
    {
        if(m_objPlayer == null)
        {
            m_objPlayer = GameObject.Find("Player");
        }
        else
        {
        }

        if(m_objStamina == null)
        {
            Debug.LogError("m_objStamina�� �����ϴ�.");
        }
        else
        {
        }

        if(m_imgStaminaBackground == null)
        {
            Debug.LogError("m_imgStaminaBackground�� �����ϴ�.");
        }
        else
        {
        }
        
        if(m_imgStaminaGauge == null)
        {
            Debug.LogError("m_imgStaminaGauge�� �����ϴ�.");
        }
        else
        {
        }
        //���¹̳� UI�� �ְ� �ʺ�� m_imgStaminaBackground�� �ʺ�� ����.
        m_fStaminaMaxWith = m_imgStaminaBackground.rectTransform.rect.width;

        if(m_objPropsMessage == null)
        {
            m_objPropsMessage = GameObject.Find("PropsMessage");
        }
        else
        {
        }

        if(m_txtPropsName == null)
        {
            m_txtPropsName = GameObject.Find("PropsName").GetComponent<Text>();
        }
        else
        {
        }

        if(m_txtPropsDescription == null)
        {
            m_txtPropsDescription = GameObject.Find("PropsDescription").GetComponent<Text>();
        }
        else
        {
        }

        //�⺻���� ������ �ʴ´�. ������ ���� ��Ȱ��ȭ �����ϰ�, �ڽĿ�����Ʈ�� Find�ϸ� ���̾��Ű â���� �������´�.
        m_objPropsMessage.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
        SetStaminaUI();
    }

    // ���׹̳� Gauge ����
    void SetStaminaUI()
    {
        //PlayerControl ��ũ��Ʈ���� ���� ���¹̳� ������ �����´�.
        m_fCurrentStaminaRate = m_objPlayer.GetComponent<PlayerControl>().GetCurrentStaminaRate();
        //Debug.Log(m_fCurrentStaminaRate);
        if(m_fCurrentStaminaRate == 1)
        {
            //���� ���� ���¹̳� �ܷ� ������ 1�̶�� Full�̹Ƿ� ���׹̳� UI�� ��Ȱ��ȭ ��Ų��.
            m_objStamina.SetActive(false);
        }
        else
        {
            //���� 1�� �ƴ϶��, ���¹̳ʰ� �ٰų� ȸ���ϰ� �����Ƿ� UI�� ǥ���Ѵ�.
            m_objStamina.SetActive(true);
            // ���� �����̳� �̹����� ���̴� �ְ� �ʺ񿡼� ���� ���¹̳� �ܷ� ������ �����ش�.
            float fCurrentWidth = m_fStaminaMaxWith * m_fCurrentStaminaRate;
            // ���� ���¹̳� gauge�� rectTransform�� ���� �����´�.
            Vector3 vecRectSize = m_imgStaminaGauge.rectTransform.sizeDelta;
            //�޾ƿ� ������� x�� ���� ���� �ʺ�� ������ �ٽ� ����
            vecRectSize.x = fCurrentWidth;
            m_imgStaminaGauge.rectTransform.sizeDelta = vecRectSize;
        }
    }

   //�������� �����ɽ� ������ �޼������� �Լ���//

    //PropsMessage ������Ʈ�� toggle�� �Լ�
    public void SetActivePropsMessage(bool isActive)
    {
        m_objPropsMessage.SetActive(isActive);
    }

    //PropsMessage�� text���� ������ �Լ�
    public void SetPropsMessage(string strName, string strDescription)
    {
        m_txtPropsName.text = strName;
        m_txtPropsDescription.text = strDescription;
    }
}
