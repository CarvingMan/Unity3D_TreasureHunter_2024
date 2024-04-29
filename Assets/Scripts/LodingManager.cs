using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LodingManager : MonoBehaviour
{
    // �񵿱� �� ��ȯ�� �̿��Ͽ� �� ��ȯ //
    GameManager m_csGameManager = null;

    GameObject m_objPlayer = null;
    
    //�ε� ��
    [SerializeField]
    Image m_imgLodingBar = null;
    //�ε��� ����
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
            //�ε��� �ʺ� ����
            m_fLodingBarWidth = m_imgLodingBar.rectTransform.rect.width;
        }
        else
        {
            Debug.LogError("m_imgLodingBar�� �����ϴ�.");
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

    //�񵿱�� ���� �ε��ϸ� ���� ��Ȳ�� ���� �ε��� ������ ǥ��
    IEnumerator LodingScene()
    {
        // �񵿱� ScenLoad
        //LoadSceneAsync �޼ҵ忡�� AsyncOperation Ŭ������ ��ȯ
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("DungeonScene");
        // Scene�� load�Ǵ� ��� Ȱ��ȭ ��Ű�� ��� ���θ� �����Ѵٰ� �Ѵ�. �⺻������ false�� �ϰ� ����
        // true�� �ٲپ� �־� �ε� �߰� UI���� �۾��� �� �� �ִ�.
        asyncOperation.allowSceneActivation = false;
        // isDone�� ���� ���� �غ�Ǿ������� �˷��شٰ� �Ѵ�.
        while(asyncOperation.isDone == false)
        {
            //progress�� �� �ε��� ��������� 0~1�� �˷��شٰ� �Ѵ�.
            if(asyncOperation.progress < 0.9f)
            {
                //�� �������� ���������� ��ٸ��� UpDate�� ���� ȣ��.
                yield return new WaitForEndOfFrame();
                //progress��ŭ �ε��� �̹��� ǥ��
                SetLodingBar(asyncOperation.progress);
                //Debug.Log("�ε���" + asyncOperation.progress.ToString());
            }
            else
            {
                m_txtMessage.text = "Almost done . . .";
                //�Ʒ� WaitForSeconds�� �ʿ������ ���� ���Ӿ��� �׸� ������ �ʾ� ������ �ε��ȴ�.
                //���� �ÿ��� �ε����� �����ְ��� ��� ��ٸ���.(���� ���׺��� ������ ����)
                yield return new WaitForSeconds(2);
                SetLodingBar(1);
                yield return new WaitForSeconds(1);
                //progress�� 0.9 �ʰ��Ͻ� �ε��ٸ� �� ä��� allowSceneActivation�� ture ��Ų��
                asyncOperation.allowSceneActivation = true;
                yield break; //coroutine �ߴ�
            }
        }

        
    }

    //�ε��� ������ ���� �Լ�
    void SetLodingBar(float currentRate)
    {
        if(m_imgLodingBar != null)
        {
            // ���� ������ �ε��� �ʺ� ���Ͽ� ���� �ʺ��� ���Ѵ�.
            float fBarWidth = currentRate * m_fLodingBarWidth;

            Vector3 vecRectSize = m_imgLodingBar.rectTransform.sizeDelta;
            vecRectSize.x = fBarWidth;
            m_imgLodingBar.rectTransform.sizeDelta = vecRectSize;
        }
        else
        {
            Debug.LogError("m_imgLodingBar�� �����ϴ�.");
        }
    }
}
