using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LodingManager : MonoBehaviour
{
    // 비동기 씬 전환을 이용하여 씬 전환 //
    GameManager m_csGameManager = null;

    GameObject m_objPlayer = null;
    
    //로딩 바
    [SerializeField]
    Image m_imgLodingBar = null;
    //로딩바 넓이
    float m_fLodingBarWidth = 0.0f;

    Text m_txtMessage = null;

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
        if(m_objPlayer == null)
        {
            m_objPlayer = GameObject.Find("Player");
            m_objPlayer.GetComponent<Animator>().Play("loding_dance");
        }
        else
        {

        }

        if(m_imgLodingBar != null)
        {
            //로딩바 너비 대입
            m_fLodingBarWidth = m_imgLodingBar.rectTransform.rect.width;
        }
        else
        {
            Debug.LogError("m_imgLodingBar가 없습니다.");
        }

        if(m_txtMessage == null)
        {
            m_txtMessage = GameObject.Find("LodingMessage").GetComponent<Text>();
        }
        else
        {
        }
        SetLodingBar(0);
        LoadDeongeonScene();
    }


    void LoadDeongeonScene()
    {
        StartCoroutine(LodingScene());
    }

    //비동기로 씬을 로드하며 진행 상황에 따라 로딩바 게이지 표시
    IEnumerator LodingScene()
    {
        // 비동기 ScenLoad
        //LoadSceneAsync 메소드에서 AsyncOperation 클래스를 반환
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("DungeonScene");
        // Scene이 load되는 즉시 활성화 시키는 허용 여부를 결정한다고 한다. 기본적으로 false로 하고 이후
        // true로 바꾸어 주어 로딩 중간 UI같은 작업을 할 수 있다.
        asyncOperation.allowSceneActivation = false;
        // isDone은 씬이 전부 준비되었는지를 알려준다고 한다.
        while(asyncOperation.isDone == false)
        {
            //progress는 씬 로드의 진행비율을 0~1로 알려준다고 한다.
            if(asyncOperation.progress < 0.9f)
            {
                //한 프레임이 끝날때까지 기다린다 UpDate와 같은 호출.
                yield return new WaitForEndOfFrame();
                //progress만큼 로딩바 이미지 표시
                SetLodingBar(asyncOperation.progress);
                //Debug.Log("로딩중" + asyncOperation.progress.ToString());
            }
            else
            {
                m_txtMessage.text = "Almost done . . .";
                //아래 WaitForSeconds는 필요없지만 현재 게임씬이 그리 무겁지 않아 빠르게 로딩된다.
                //따라서 시연시 로딩씬을 보여주고자 잠시 기다린다.(이후 상항보고 제거할 예정)
                yield return new WaitForSeconds(2);
                SetLodingBar(1);
                yield return new WaitForSeconds(1);
                //progress가 0.9 초과일시 로딩바를 다 채우고 allowSceneActivation을 ture 시킨다
                asyncOperation.allowSceneActivation = true;
                yield break; //coroutine 중단
            }
        }

        
    }

    //로딩바 게이지 세팅 함수
    void SetLodingBar(float currentRate)
    {
        if(m_imgLodingBar != null)
        {
            // 현재 비율에 로딩바 너비를 곱하여 현재 너비값을 구한다.
            float fBarWidth = currentRate * m_fLodingBarWidth;

            Vector3 vecRectSize = m_imgLodingBar.rectTransform.sizeDelta;
            vecRectSize.x = fBarWidth;
            m_imgLodingBar.rectTransform.sizeDelta = vecRectSize;
        }
        else
        {
            Debug.LogError("m_imgLodingBar가 없습니다.");
        }
    }
}
