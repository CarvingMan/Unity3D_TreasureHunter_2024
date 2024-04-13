using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Scene m_currentScene;

    // Start is called before the first frame update
    void Start()
    {
        //���� scene�� �ִ� GameManager ���� ���� �����´�. 
        GameManager[] gameManagers = FindObjectsOfType<GameManager>();
        if(gameManagers.Length == 1)
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

    private void Update()
    {
        m_currentScene = SceneManager.GetActiveScene(); //���� Ȱ��ȭ�� Scene ������
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

}
