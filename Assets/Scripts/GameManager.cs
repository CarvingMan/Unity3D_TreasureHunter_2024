using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void Update()
    {
        m_currentScene = SceneManager.GetActiveScene(); //���� Ȱ��ȭ�� Scene ������
    }


    public string GetCurrentSceneName()
    {
        m_currentScene = SceneManager.GetActiveScene(); //���� Ȱ��ȭ�� Scene ������
        return m_currentScene.name;
    }


    //���� ���� GameOver�� �ƴҽ� �̵�
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

    //���� ���� DongeonScene�� �ƴҽ� �̵�
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

    //���� ���� CutScene�� �ƴ� �� �̵�
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

    //�ڷ�ƾ ����Ͽ� 3�� ���� �ƾ� �ε�
    IEnumerator CorCutScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("CutScene");
    }
}
