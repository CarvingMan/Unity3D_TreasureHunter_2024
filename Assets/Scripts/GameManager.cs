using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //�������� ������ �� ���� �� ���� ����

    Scene m_currentScene;

    private void Awake()
    {
        //���� scene�� �ִ� GameManager ���� ���� �����´�. 
        GameManager[] gameManagers = FindObjectsOfType<GameManager>();
        if (gameManagers.Length == 1)
        {
            //���� �迭�� ũ�Ⱑ 1���̸� ó������ GameManager�̹Ƿ� DontDestory�� ����ȯ�ÿ���
            //���̾��Ű â�� ������Ų��.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // �迭�� ũ�Ⱑ 1�̻��̶�� ����ȯ�Ǹ鼭 ���λ��� GameManager�̹Ƿ� �ߺ��� ������Ʈ ����
            Destroy(gameObject);
        }

        m_currentScene = SceneManager.GetActiveScene();
    }



    private void Update()
    {
        m_currentScene = SceneManager.GetActiveScene();
        // ���� Ŀ�� ����
        if (m_currentScene.name == "TitleScene" || m_currentScene.name == "GameOver")
        {
            Cursor.visible = true;
        }
        else if(m_currentScene.name == "DungeonScene")
        {
            if(Time.timeScale == 0) //���� �����϶����� Ŀ���� ���̰� ����
            {
                Cursor.visible = true;
            }
            else //timeScale�� 0�� �ƴ� �� Ŀ���� �Ⱥ��̰� ����
            {
                Cursor.visible = false;
            }
        }
        else
        {
            Cursor.visible = false;
        }
    }


    //time���� ����

    //timeScale = 1
    public void PlayTime()
    {
        if(Time.timeScale != 1)
        {
            Time.timeScale = 1; //timeScale�� 1�� ����
        }
    }

    //time scale = 1
    public void PauseTime()
    {
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0; //timeScale�� 0���� ����(FixedUpdate�� �ڵ����� ����)
                                //�ٸ� update�Լ��� �״�� ȣ��Ǳ⿡ ������ �ʿ��� update�� ������ ���Ǽ��� ���־�� �Ѵ�.
                                //deletime = (���� ������ �ð� - ���� ������ �ð�)*Time.timeScale
                                //update�� ������ ������� ȣ��Ǳ⿡ ������ ���� �ʰ�
                                //FixedUpdate�� Time.fixedDeltaTime���� ȣ���ϳ� Time.timeScale�� ������ �޴´�.
        }
    }


    //�� ���� ����//
    public string GetCurrentSceneName()
    {
        m_currentScene = SceneManager.GetActiveScene(); //���� Ȱ��ȭ�� Scene ������
        return m_currentScene.name;
    }

    //�⺻ 1���� �ε�
    //��ư�� �����ڸ� ����ȯ�ϴ� ���� �ƴ� ��� �ð��� �� -> ��ư �Ҹ��� �Բ� ��ȯ�ǵ���
    //���ڿ��� �޾� ��ȯ
    IEnumerator LoadScene(string strScene, float fTime=1)
    {
        yield return new WaitForSeconds(fTime);
        SceneManager.LoadScene(strScene);
    }

    //�ٸ� ��ũ��Ʈ���� ���� ����LoadScene�� ����ϰ� ���� �� ��� ��� pubic
    //Ÿ�Ӷ��� ��ũ��Ʈ���� Ÿ�Ӷ��� �ð��� ���� ���(�ð� �ʼ� �Է�)
    public void LoadSceneEndTime(string strScene, float fTime)
    {
        StartCoroutine(LoadScene(strScene, fTime));
    }

    //���� ���� TitleScene�� �ƴҽ� �̵�
    public void LoadTitleScene()
    {
        m_currentScene = SceneManager.GetActiveScene(); //���� Ȱ��ȭ�� Scene ������
        if (m_currentScene.name != "TitleScene")
        {
            if(m_currentScene.name == "DungeonScene")
            {
                //���������� �޴��� �������� timeScale�� 0�� �ǹǷ� title��ư�� ����
                //Title������ �Ѿ�� ���� timeScale�� 1�� �ٲپ� �־�� �Ѵ�.
                PlayTime(); 
            }
            StartCoroutine(LoadScene("TitleScene"));
        }
        else
        {

        }
    }

    //���� ���� LodingScene�� �ƴҶ� �̵�
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


    //���� ���� GameOver�� �ƴҽ� �̵�
    public void LoadGameOverScene()
    {
        m_currentScene = SceneManager.GetActiveScene(); //���� Ȱ��ȭ�� Scene ������
        if (m_currentScene.name != "GameOver")
        {
            //���ӿ��� ���� �̹� PlayerUIManager���� ���̵��ΰ� �Բ� �ð��� �α⿡ �ٷ� ��ȯ
            SceneManager.LoadScene("GameOver");
        }
        else
        {

        }
    }

    //���� ���� DungeonScene�� �ƴҽ� �̵�
    public void LoadDungeonScene()
    {
        m_currentScene = SceneManager.GetActiveScene(); //���� Ȱ��ȭ�� Scene ������
        if (m_currentScene.name != "DungeonScene")
        {
            StartCoroutine(LoadScene("DungeonScene"));
        }
        else
        {

        }
    }

    //���� ���� CutScene�� �ƴ� �� �̵�
    public void LoadCutScene()
    {
        m_currentScene = SceneManager.GetActiveScene(); //���� Ȱ��ȭ�� Scene ������
        if (m_currentScene.name != "CutScene")
        {
            StartCoroutine(LoadScene("CutScene", 3f));
        }
        else
        {

        }
    }

   

    //���ø����̼� ���� �Լ�
    public void AppExit()
    {
        // ����Ƽ�����Ϳ����� ���ᰡ �ȵǹǷ� ��ó���� ���ǹ��� ����Ͽ� �ܼ� ���(WEB������ ���ᰡ �ȵȴٰ� �Ѵ�.)
#if UNITY_EDITOR
       // Debug.Log("����");
#else // �ٸ� �÷��������� ����
        Application.Quit();
#endif
    }

}
