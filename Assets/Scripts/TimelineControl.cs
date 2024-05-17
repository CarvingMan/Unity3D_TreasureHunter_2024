using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineControl : MonoBehaviour
{
    //타임라인 종류에 따라 자동으로 다음 씬으로 이동하게 하기위해
    //열거형 상수 타입을 사용하여 쉽게 타입만 설정하여 사용할수 있게 작성

    enum E_TIME_LINE
    {
        NONE,
        CUT = 26, //컷씬 27초
        ENDING = 20,//엔딩크레딧씬 20초
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
            Debug.LogError("열거형 상수 타입을 선택하시오");
        }
        else
        {

        }
        //열거형 상수로 현재 타임라인의 시간을 정의하였으니 float으로 형변환
        float fTimeLineTime = (float)m_eTimeLine;

        //switch case문을 사용하여 타입에 따라 씬 이동 
        switch (m_eTimeLine)
        {
            // 컷씬일때 27초 후 엔딩씬으로
            case E_TIME_LINE.CUT:
                m_csGameManager.LoadSceneEndTime("EndingScene", fTimeLineTime);
                break;
            // 엔딩씬일때 20초 후 타이틀 씬으로
            case E_TIME_LINE.ENDING:
                m_csGameManager.LoadSceneEndTime("TitleScene", fTimeLineTime);
                break;
            default:
                break;
        }

    }

}
