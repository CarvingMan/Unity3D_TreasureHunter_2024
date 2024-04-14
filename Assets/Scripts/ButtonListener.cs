using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListener : MonoBehaviour
{
    //GameManager은 씬이동시 파괴되지 않고 생성되지만 기본적으로 씬로드전까지는 없으므로
    //해당 스크립트와 같이 대기하다 생성될시 불러온다.

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
        //인게임 중에는 다시 시작버튼이 아닌 continue 버튼이다.
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
            //Debug.Log("재시작");
            m_btnRetry = null;
        }
        else
        {
            Debug.LogError("m_btnRetryListen이 없습니다.");
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
