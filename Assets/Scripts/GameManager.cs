using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //전반적인 씬들의 씬 관리 및 게임 관리

    Scene m_currentScene;

    private void Awake()
    {
        //현재 scene에 있는 GameManager 들을 전부 가져온다. 
        GameManager[] gameManagers = FindObjectsOfType<GameManager>();
        if (gameManagers.Length == 1)
        {
            //만약 배열의 크기가 1나이면 처음생긴 GameManager이므로 DontDestory로 씬전환시에서
            //하이어라키 창에 유지시킨다.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 배열에 크기가 1이상이라면 씬전환되면서 새로생긴 GameManager이므로 중복된 오브젝트 삭제
            Destroy(gameObject);
        }

        m_currentScene = SceneManager.GetActiveScene();
    }



    private void Update()
    {
        m_currentScene = SceneManager.GetActiveScene();
        // 씬별 커서 설정
        if (m_currentScene.name == "TitleScene" || m_currentScene.name == "GameOver")
        {
            Cursor.visible = true;
        }
        else if(m_currentScene.name == "DungeonScene")
        {
            if(Time.timeScale == 0) //정지 상태일때에는 커서를 보이게 설정
            {
                Cursor.visible = true;
            }
            else //timeScale이 0이 아닐 시 커서를 안보이게 설정
            {
                Cursor.visible = false;
            }
        }
        else
        {
            Cursor.visible = false;
        }
    }


    //time관련 관리

    //timeScale = 1
    public void PlayTime()
    {
        if(Time.timeScale != 1)
        {
            Time.timeScale = 1; //timeScale을 1로 설정
        }
    }

    //time scale = 1
    public void PauseTime()
    {
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0; //timeScale을 0으로 설정(FixedUpdate는 자동으로 멈춤)
                                //다만 update함수는 그대로 호출되기에 멈춤이 필요한 update는 별개로 조건설정 해주어야 한다.
                                //deletime = (현재 프레임 시간 - 이전 프레임 시간)*Time.timeScale
                                //update는 프레임 기반으로 호출되기에 영향을 받지 않고
                                //FixedUpdate는 Time.fixedDeltaTime마다 호출하나 Time.timeScale의 영향을 받는다.
        }
    }


    //씬 관련 관리//
    public string GetCurrentSceneName()
    {
        m_currentScene = SceneManager.GetActiveScene(); //현재 활성화된 Scene 가져옴
        return m_currentScene.name;
    }

    //기본 1초후 로드
    //버튼을 누르자마 씬전환하는 것이 아닌 잠시 시간을 둠 -> 버튼 소리와 함께 전환되도록
    //문자열로 받아 전환
    IEnumerator LoadScene(string strScene, float fTime=1)
    {
        yield return new WaitForSeconds(fTime);
        SceneManager.LoadScene(strScene);
    }

    //다른 스크립트에서 직접 위의LoadScene을 사용하고 싶을 때 대신 사용 pubic
    //타임라인 스크립트에서 타임라인 시간에 따라 사용(시간 필수 입력)
    public void LoadSceneEndTime(string strScene, float fTime)
    {
        StartCoroutine(LoadScene(strScene, fTime));
    }

    //현재 씬이 TitleScene가 아닐시 이동
    public void LoadTitleScene()
    {
        m_currentScene = SceneManager.GetActiveScene(); //현재 활성화된 Scene 가져옴
        if (m_currentScene.name != "TitleScene")
        {
            if(m_currentScene.name == "DungeonScene")
            {
                //던전씬에서 메뉴를 눌렀을시 timeScale이 0이 되므로 title버튼을 눌러
                //Title씬으로 넘어가기 전에 timeScale을 1로 바꾸어 주어야 한다.
                PlayTime(); 
            }
            StartCoroutine(LoadScene("TitleScene"));
        }
        else
        {

        }
    }

    //현재 씬이 LodingScene이 아닐때 이동
    public void LoadLodingScene()
    {
        m_currentScene = SceneManager.GetActiveScene();
        if (m_currentScene.name != "LodingScene")
        {
            StartCoroutine(LoadScene("LodingScene"));
        }
        else
        {

        }
    }


    //현재 씬이 GameOver가 아닐시 이동
    public void LoadGameOverScene()
    {
        m_currentScene = SceneManager.GetActiveScene(); //현재 활성화된 Scene 가져옴
        if (m_currentScene.name != "GameOver")
        {
            //게임오버 씬은 이미 PlayerUIManager에서 페이드인과 함께 시간을 두기에 바로 전환
            SceneManager.LoadScene("GameOver");
        }
        else
        {

        }
    }

    //현재 씬이 DungeonScene가 아닐시 이동
    public void LoadDungeonScene()
    {
        m_currentScene = SceneManager.GetActiveScene(); //현재 활성화된 Scene 가져옴
        if (m_currentScene.name != "DungeonScene")
        {
            StartCoroutine(LoadScene("DungeonScene"));
        }
        else
        {

        }
    }

    //현재 씬이 CutScene이 아닐 시 이동
    public void LoadCutScene()
    {
        m_currentScene = SceneManager.GetActiveScene(); //현재 활성화된 Scene 가져옴
        if (m_currentScene.name != "CutScene")
        {
            StartCoroutine(LoadScene("CutScene", 3f));
        }
        else
        {

        }
    }

   

    //애플리케이션 종료 함수
    public void AppExit()
    {
        // 유니티에디터에서는 종료가 안되므로 전처리기 조건문을 사용하여 콘솔 출력(WEB에서도 종료가 안된다고 한다.)
#if UNITY_EDITOR
       // Debug.Log("종료");
#else // 다른 플랫폼에서는 종료
        Application.Quit();
#endif
    }

}
