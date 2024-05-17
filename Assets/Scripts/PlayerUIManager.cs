using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro ���

public class PlayerUIManager : MonoBehaviour
{
    /*���������� �÷��̾��� UI�� �����ϴ� ��ũ��Ʈ*/

    /* GetComponet�� SerializedField�� ������Ʈ�� �������� ����� �� �� ����Ͽ���.
    ��ǻ� SerializedField ������� ���ϱ��ϳ� ���� ������Ʈ���� �ٲ�� ��찡 �ƴϸ�
    ���������� �ʴ´�. */

    GameObject m_objPlayer = null; //�÷��̾� ������Ʈ

    // ���¹̳� ���� ������� 
    [SerializeField]
    GameObject m_objStamina = null;  // setActive toggle��ų ���׹̳� ������Ʈ(canvas)
    [SerializeField]
    Image m_imgStaminaBackground = null; //���¹̳� ������
    [SerializeField]
    Image m_imgStaminaGauge = null; //���¹̳� ������
    float m_fStaminaMaxWith = 0; // ���׹̳� ������ �ִ����
    float m_fCurrentStaminaRate = 0; //���� ���¹̳� ����

    //�޴� ���� �������
    GameObject m_objMenu = null;

    //Props�޼��� ���� �������
    GameObject m_objPropsMessage = null; //text UI ��ü�� setAcive toggle ������Ʈ
    Text m_txtPropsName = null; // Props�̸�
    Text m_txtPropsDescription = null; //Props����

    //item ���� �������
    Image[] m_arrItems = null;
    GameObject m_objItems = null;
    ItemManager m_csItemManager = null;

    //���ӿ��� ���� �������
    TextMeshProUGUI m_txtGameOver = null;
    GameManager m_csGameManager = null; //����ȯ�� ���
   
    //����� ����
    AudioSource m_UIAudioSource = null;
    [SerializeField]
    AudioClip m_clipPickUp = null;
    [SerializeField]
    AudioClip m_clipGameOver = null;

    // Start is called before the first frame update
    void Start()
    {
        if(m_objPlayer == null)
        {
            m_objPlayer = GameObject.Find("Player");
        }
        else
        {
        }

        if(m_objMenu == null)
        {
            m_objMenu = GameObject.Find("Menu");
        }

        //���¹̳�����
        if (m_objStamina == null)
        {
            Debug.LogError("m_objStamina�� �����ϴ�.");
        }
        else
        {
        }

        if(m_imgStaminaBackground == null)
        {
            Debug.LogError("m_imgStaminaBackground�� �����ϴ�.");
        }
        else
        {
        }
        
        if(m_imgStaminaGauge == null)
        {
            Debug.LogError("m_imgStaminaGauge�� �����ϴ�.");
        }
        else
        {
        }
        //���¹̳� UI�� �ְ� �ʺ�� m_imgStaminaBackground�� �ʺ�� ����.
        m_fStaminaMaxWith = m_imgStaminaBackground.rectTransform.rect.width;

        //������ �޼��� ����
        if(m_objPropsMessage == null)
        {
            m_objPropsMessage = GameObject.Find("PropsMessage");
        }
        else
        {
        }

        if(m_txtPropsName == null)
        {
            m_txtPropsName = GameObject.Find("PropsName").GetComponent<Text>();
        }
        else
        {
        }

        if(m_txtPropsDescription == null)
        {
            m_txtPropsDescription = GameObject.Find("PropsDescription").GetComponent<Text>();
        }
        else
        {
        }

        //�⺻���� ������ �ʴ´�. ������ ���� ��Ȱ��ȭ �����ϰ�, �ڽĿ�����Ʈ�� Find�ϸ� ���̾��Ű â���� �������´�.
        m_objPropsMessage.SetActive(false); 

        //�κ��丮����
        if(m_objItems == null)
        {
            m_objItems = GameObject.Find("Items");
        }
        else 
        {
        }

        if(m_arrItems == null)
        {
            //Canvas�� Inventory/Items �� �̹������� �����´�.
            m_arrItems = m_objItems.GetComponentsInChildren<Image>();
        }
        
        if(m_csItemManager == null)
        {
            m_csItemManager = m_objPlayer.GetComponent<ItemManager>();
        }

        ClearInventory();// UI�ʱ�ȭ

        //GameOver����
        if(m_txtGameOver == null)
        {
            m_txtGameOver = GameObject.Find("GameOver").GetComponent<TextMeshProUGUI>();
        }
        else
        {
        }

        if(m_csGameManager == null)
        {
            m_csGameManager = FindObjectOfType<GameManager>();
        }
        else
        {
        }

        if(m_UIAudioSource == null)
        {
            m_UIAudioSource = GetComponent<AudioSource>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale != 0)
        {
            //���׹̳� ����
            SetStaminaUI();

            //timeScale�� 0�϶����� �޴��� ��Ȱ��ȭ ��Ų��.
            if(m_objMenu.activeSelf == true)
            {
                // Ȱ��ȭ �Ǿ��ִ� timeScale�� 1�̵ɶ� �� ���� ȣ��
                m_objMenu.SetActive(false);
            }
        }
        else //timeScale�� 0�� ��
        {
  
        }
        SetMenu();// �޴����� �Լ��� timeScale�� ���� ���� ȣ���Ѵ�.
    }

    void SetMenu()
    {
        // �޴�panel ����
        if (m_objMenu != null)
        {
            //�޴��� ��Ȱ��ȭ �����϶��� esc��ư�� �Է¹޴´�.
            if (m_objMenu.activeSelf == false)
            {
                bool isMenuKeyDown = Input.GetKeyDown(KeyCode.Escape);
                if (isMenuKeyDown)
                {
                    m_csGameManager.PauseTime();
                    m_objMenu.SetActive(true);//esc������ �޴� Ȱ��ȭ -> ��Ȱ��ȭ�� update���� �ϰ� �ִ�. 
                }
            }
            else // �޴��� Ȱ��ȭ �����϶�
            {

            }
        }
        else
        {
            Debug.LogError("m_objMenu�� �����ϴ�.");
        }
    }
 

    // ���׹̳� Gauge ����
    void SetStaminaUI()
    {
        //PlayerControl ��ũ��Ʈ���� ���� ���¹̳� ������ �����´�.
        m_fCurrentStaminaRate = m_objPlayer.GetComponent<PlayerControl>().GetCurrentStaminaRate();
        //Debug.Log(m_fCurrentStaminaRate);
        if(m_fCurrentStaminaRate == 1)
        {
            //���� ���� ���¹̳� �ܷ� ������ 1�̶�� Full�̹Ƿ� ���׹̳� UI�� ��Ȱ��ȭ ��Ų��.
            m_objStamina.SetActive(false);
        }
        else
        {
            //���� 1�� �ƴ϶��, ���¹̳ʰ� �ٰų� ȸ���ϰ� �����Ƿ� UI�� ǥ���Ѵ�.
            m_objStamina.SetActive(true);
            // ���� �����̳� �̹����� ���̴� �ְ� �ʺ񿡼� ���� ���¹̳� �ܷ� ������ �����ش�.
            float fCurrentWidth = m_fStaminaMaxWith * m_fCurrentStaminaRate;
            // ���� ���¹̳� gauge�� rectTransform�� ���� �����´�.
            Vector3 vecRectSize = m_imgStaminaGauge.rectTransform.sizeDelta;
            //�޾ƿ� ������� x�� ���� ���� �ʺ�� ������ �ٽ� ����
            vecRectSize.x = fCurrentWidth;
            m_imgStaminaGauge.rectTransform.sizeDelta = vecRectSize;
        }
    }

   //�������� �����ɽ� ������ �޼������� �Լ���//

    //PropsMessage ������Ʈ�� toggle�� �Լ�
    public void SetActivePropsMessage(bool isActive)
    {
        m_objPropsMessage.SetActive(isActive);
    }

    //PropsMessage�� text���� ������ �Լ�
    public void SetPropsMessage(string strName, string strDescription)
    {
        m_txtPropsName.text = strName;
        m_txtPropsDescription.text = strDescription;
    }

    //������ ���� ����ǰų� ���۵ɶ� �ʱ�ȭ
    //�̹����� ������ ������ �־� �迭�� ������ ����Ʈ ó�� ������ ����
    //�� ������ ������� �ʱ����� �ʱ�ȭ �� �ٽ� ����Ʈ(ItemManager) ��Ҵ�� ä���.
    public void ClearInventory()
    {
        foreach(Image i in m_arrItems)
        {
            //��������Ʈ�� null�� �ٲپ��ش�.
            i.GetComponent<Image>().sprite = null;
            //������ 0���� �ٲپ� �⺻������ �Ⱥ��̰� �Ѵ�.
            Color color = i.GetComponent<Image>().color;
            color.a = 0;
            i.GetComponent<Image>().color = color;
        }
    }

    //�κ��丮�� �̹������� ǥ���� �Լ�
    public void SetInventory()
    {
        List<GameObject> objListItems = m_csItemManager.GetListItems();
        if(objListItems.Count > 0) // �迭�� ��Ұ� ���� ������ �����Ѵ�. �����ÿ��� ���� X
        {
            //��������Ʈ�� �����۽�������Ʈ�� �����ϰ� ������ 1�� ����
            for (int i = 0; i < objListItems.Count; i++)
            {
                //��������Ʈ ���ϸ��� �ش� ������Ʈ�� �̸��� ����.
                Sprite spriteItem = Resources.Load<Sprite>("Items/"+objListItems[i].name.ToString());
                m_arrItems[i].GetComponent<Image>().sprite = spriteItem;
                Color color = m_arrItems[i].GetComponent<Image>().color;
                color.a = 1;
                m_arrItems[i].GetComponent <Image>().color = color;
            }
        }
        else 
        {
            
        }
    }

    public void SetGameOver()
    {
        StartCoroutine(FadeOutGameOver());
    }

    //�ڷ�ƾ�� ���Ͽ� ���̵� �� ȿ��
    IEnumerator FadeOutGameOver()
    {
        yield return new WaitForSeconds(1f);
        // ���ӿ��� ����� Ŭ�� ���
        SetGameoverClip();

        float fRaise = 0;
        Color color = m_txtGameOver.color;
        if(color.a == 0) //���� ������ 0�϶����� ���� -> �ߺ� �ڷ�ƾ ȣ�� Fade In ����
        {
            while (fRaise < 1)// �������� ���� �ִ�ġ 1�����϶�
            {
                fRaise += 0.025f; //������ ����

                color.a = fRaise;
                m_txtGameOver.color = color; //fRaise������ ������ ���� -> fade in
                yield return new WaitForSeconds(0.1f); //0.1�� ���� ����
            }
            yield return new WaitForSeconds(2f); // ������ �� �� ��ȯ
            m_csGameManager.LoadGameOverScene();
        }
    }
    // ���ӿ��� ����� Ŭ�� ���
    void SetGameoverClip()
    {
        //���� ī�޶� AudioSource ���� GameOverŬ���� (BGM)�̱⶧��
        AudioSource CameraAudioSource = Camera.main.GetComponent<AudioSource>();
        if (CameraAudioSource == null)
        {
            Debug.LogError("����ī�޶� ����� �ҽ��� �����ϴ�.");
        }
        else
        {
            if (m_clipGameOver != null)
            {
                //���� ����� Ŭ���� �ش�������� �ʿ��� Ŭ���� �ƴҽ� �ѹ��� ����
                if (CameraAudioSource.clip != m_clipGameOver)
                {
                    CameraAudioSource.Stop();//��� ����
                    CameraAudioSource.clip = m_clipGameOver;
                    CameraAudioSource.loop = false;
                    CameraAudioSource.Play();
                }
            }
            else
            {

            }
        }
    }

    // ������ ��� ����� ��� �Լ� PlayerControl.cs���� ȣ�⤤
    public void SetPickUpAudio()
    {
        if(m_UIAudioSource != null)
        {
            if(m_clipPickUp != null)
            {   //���� ����� Ŭ���� �ش�������� �ʿ��� Ŭ���� �ƴҽ� �ѹ��� ����
                if (m_UIAudioSource.clip != m_clipPickUp)
                {
                    float fClipLenght = m_clipPickUp.length;//����� Ŭ������
                    m_UIAudioSource.clip = m_clipPickUp; //Ŭ������
                    m_UIAudioSource.loop = false; //����false
                    m_UIAudioSource.Play(); //���
                    StartCoroutine(CorInitAudioSource(fClipLenght));
                }
            }
            else
            {
                Debug.LogError("m_clipPickUp�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("m_UIAudioSource�� �����ϴ�.");
        }

    }

    //Ŭ�� �ӵ� ��ŭ ��ٸ� �� AudioSource�ʱ�ȭ �ϴ� �ڷ�ƾ
    IEnumerator CorInitAudioSource(float fClipLength)
    {
        yield return new WaitForSeconds(fClipLength);
        if(m_UIAudioSource != null)
        {
            m_UIAudioSource.Stop();
            m_UIAudioSource.clip = null;
            m_UIAudioSource.loop = false;
        }
        else
        {
            Debug.LogError("m_UIAudioSource�� �����ϴ�.");
        }
    }
}
