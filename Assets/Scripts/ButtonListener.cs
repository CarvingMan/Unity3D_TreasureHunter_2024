using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListener : MonoBehaviour
{
    //GameManager�� ���̵��� �ı����� �ʰ� ���������� �⺻������ ���ε��������� �����Ƿ�
    //�ش� ��ũ��Ʈ�� ���� ����ϴ� �����ɽ� �ҷ��´�.

    [SerializeField]
    Button m_btnTitle = null;
    [SerializeField]
    Button m_btnRetry = null;

    GameManager m_csGameManager = null;

    string m_strCurrentSceneName = null;

    // Start is called before the first frame update
    void Start()
    {
        m_csGameManager = FindObjectOfType<GameManager>();
        m_strCurrentSceneName = m_csGameManager.GetCurrentSceneName();
        //�ΰ��� �߿��� �ٽ� ���۹�ư�� �ƴ� continue ��ư�̴�.
        if (m_strCurrentSceneName == "GameOver")
        {
            BtnRetryListen();
            BtnTitleListen();
        }
        else
        {

        }

    }


    void BtnRetryListen()
    {
        if(m_btnRetry != null)
        {
            m_btnRetry.onClick.AddListener(m_csGameManager.LoadDongeonScene);
            //Debug.Log("�����");
            m_btnRetry = null;
        }
        else
        {
            Debug.LogError("m_btnRetryListen�� �����ϴ�.");
        }

    }


    void BtnTitleListen()
    {
        if(m_btnTitle != null)
        {
            //Debug.Log("dd");
        }
        else
        {

        }
    }

}
