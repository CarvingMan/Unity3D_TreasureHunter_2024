using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListener : MonoBehaviour
{
    //버튼의 onClick.AddListener()통해 씬별 필요한 버튼의 addlistener을 스크립트로 추가
    //GameManager은 씬이동시 파괴되지 않고 생성되지만 기본적으로 씬로드전까지는 없으므로
    //해당 스크립트와 같이 대기하다 생성될시 불러온다.

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
        
        // 해당 스크립트는 버튼이 있는 씬마다 개별 부착되어있으므로 씬로드시 한번만 addListener을 등록
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


    //버튼에 AddListener 추가 메소드들//

    void BtnContinueListen()
    {
        if(m_btnContinue != null)
        {
            m_btnContinue.onClick.AddListener(m_csGameManager.PlayTime);
            //GameManager.cs에서 timeScale을 다시 1로 설정
            m_btnContinue = null;
        }
        else
        {
            Debug.LogError("m_btnContinue가 없습니다.");
        }
    }


    void BtnStartListen()
    {
        if (m_btnStart != null)
        {
            //게임 시작시 로딩씬으로 이동 이후 Deongeon씬
            m_btnStart.onClick.AddListener(m_csGameManager.LoadLodingScene);
            m_btnStart = null;
        }
        else
        {
            Debug.LogError("m_btnStart가 없습니다.");
        }
    }

    void BtnRetryListen()
    {
        if(m_btnRetry != null)
        {
            m_btnRetry.onClick.AddListener(m_csGameManager.LoadLodingScene);
            //Debug.Log("재시작");
            m_btnRetry = null;
        }
        else
        {
            Debug.LogError("m_btnRetry가 없습니다.");
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
            Debug.LogError("m_btnTitle이 없습니다.");
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
            Debug.LogError("m_btnExit가 없습니다.");
        }
    }

}
