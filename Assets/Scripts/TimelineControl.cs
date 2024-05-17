using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineControl : MonoBehaviour
{
    //Ÿ�Ӷ��� ������ ���� �ڵ����� ���� ������ �̵��ϰ� �ϱ�����
    //������ ��� Ÿ���� ����Ͽ� ���� Ÿ�Ը� �����Ͽ� ����Ҽ� �ְ� �ۼ�

    enum E_TIME_LINE
    {
        NONE,
        CUT = 26, //�ƾ� 27��
        ENDING = 20,//����ũ������ 20��
        MAX,
    }

    [SerializeField]
    E_TIME_LINE m_eTimeLine = E_TIME_LINE.NONE;

    GameManager m_csGameManager;

    // Start is called before the first frame update
    void Start()
    {

        if(m_csGameManager == null)
        {
            m_csGameManager = FindObjectOfType<GameManager>();
        }
        else
        {

        }

        if(m_eTimeLine == E_TIME_LINE.NONE || m_eTimeLine == E_TIME_LINE.MAX)
        {
            Debug.LogError("������ ��� Ÿ���� �����Ͻÿ�");
        }
        else
        {

        }
        //������ ����� ���� Ÿ�Ӷ����� �ð��� �����Ͽ����� float���� ����ȯ
        float fTimeLineTime = (float)m_eTimeLine;

        //switch case���� ����Ͽ� Ÿ�Կ� ���� �� �̵� 
        switch (m_eTimeLine)
        {
            // �ƾ��϶� 27�� �� ����������
            case E_TIME_LINE.CUT:
                m_csGameManager.LoadSceneEndTime("EndingScene", fTimeLineTime);
                break;
            // �������϶� 20�� �� Ÿ��Ʋ ������
            case E_TIME_LINE.ENDING:
                m_csGameManager.LoadSceneEndTime("TitleScene", fTimeLineTime);
                break;
            default:
                break;
        }

    }

}
