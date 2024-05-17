using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListener : MonoBehaviour
{
    //��ư�� onClick.AddListener()���� ���� �ʿ��� ��ư�� addlistener�� ��ũ��Ʈ�� �߰�
    //GameManager�� ���̵��� �ı����� �ʰ� ���������� �⺻������ ���ε��������� �����Ƿ�
    //�ش� ��ũ��Ʈ�� ���� ����ϴ� �����ɽ� �ҷ��´�.

    [SerializeField]
    Button m_btnTitle = null;
    [SerializeField]
    Button m_btnRetry = null;
    [SerializeField]
    Button m_btnStart = null;
    [SerializeField]
    Button m_btnContinue = null;
    [SerializeField] 
    Button m_btnExit = null;

    GameManager m_csGameManager = null;

    string m_strCurrentSceneName = null;


    // Start is called before the first frame update
    void Start()
    {
        m_csGameManager = FindObjectOfType<GameManager>();
        m_strCurrentSceneName = m_csGameManager.GetCurrentSceneName();
        
        // �ش� ��ũ��Ʈ�� ��ư�� �ִ� ������ ���� �����Ǿ������Ƿ� ���ε�� �ѹ��� addListener�� ���
        if (m_strCurrentSceneName == "TitleScene")
        {
         
            BtnStartListen();
            BtnExitListen();
        }
        else if(m_strCurrentSceneName == "DungeonScene")
        {
            BtnContinueListen();
            BtnTitleListen();
        }
        else if (m_strCurrentSceneName == "GameOver")
        {
            BtnRetryListen();
            BtnTitleListen();
        }
        else
        {

        }

    }


    //��ư�� AddListener �߰� �޼ҵ��//

    void BtnContinueListen()
    {
        if(m_btnContinue != null)
        {
            m_btnContinue.onClick.AddListener(m_csGameManager.PlayTime);
            //GameManager.cs���� timeScale�� �ٽ� 1�� ����
            m_btnContinue = null;
        }
        else
        {
            Debug.LogError("m_btnContinue�� �����ϴ�.");
        }
    }


    void BtnStartListen()
    {
        if (m_btnStart != null)
        {
            //���� ���۽� �ε������� �̵� ���� Deongeon��
            m_btnStart.onClick.AddListener(m_csGameManager.LoadLodingScene);
            m_btnStart = null;
        }
        else
        {
            Debug.LogError("m_btnStart�� �����ϴ�.");
        }
    }

    void BtnRetryListen()
    {
        if(m_btnRetry != null)
        {
            m_btnRetry.onClick.AddListener(m_csGameManager.LoadLodingScene);
            //Debug.Log("�����");
            m_btnRetry = null;
        }
        else
        {
            Debug.LogError("m_btnRetry�� �����ϴ�.");
        }

    }


    void BtnTitleListen()
    {
        if(m_btnTitle != null)
        {
            m_btnTitle.onClick.AddListener(m_csGameManager.LoadTitleScene);
            m_btnTitle = null;
        }
        else
        {
            Debug.LogError("m_btnTitle�� �����ϴ�.");
        }
    }

    void BtnExitListen()
    {
        if (m_btnExit != null)
        {
            m_btnExit.onClick.AddListener(m_csGameManager.AppExit);
            m_btnExit = null;
        }
        else
        {
            Debug.LogError("m_btnExit�� �����ϴ�.");
        }
    }

}
