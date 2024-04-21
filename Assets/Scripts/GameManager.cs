using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void Update()
    {
        m_currentScene = SceneManager.GetActiveScene(); //현재 활성화된 Scene 가져옴
    }


    public string GetCurrentSceneName()
    {
        m_currentScene = SceneManager.GetActiveScene(); //현재 활성화된 Scene 가져옴
        return m_currentScene.name;
    }


    //현재 씬이 GameOver가 아닐시 이동
    public void LoadGameOverScene()
    {
        if(m_currentScene.name != "GameOver")
        {
            SceneManager.LoadScene("GameOver");
        }
        else
        {

        }
    }

    //현재 씬이 DongeonScene가 아닐시 이동
    public void LoadDongeonScene()
    {
        if (m_currentScene.name != "DongeonScene")
        {
            SceneManager.LoadScene("DongeonScene");
        }
        else
        {

        }
    }

    //현재 씬이 CutScene이 아닐 시 이동
    public void LoadCutScene()
    {
        if(m_currentScene.name != "CutScene")
        {
            StartCoroutine(CorCutScene());
        }
        else
        {

        }
    }

    //코루틴 사용하여 3초 이후 컷씬 로드
    IEnumerator CorCutScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("CutScene");
    }
}
