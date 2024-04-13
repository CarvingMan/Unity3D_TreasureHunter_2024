using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro ���

public class PlayerUIManager : MonoBehaviour
{
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
        //���¹̳�����
        if(m_objStamina == null)
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
    }

    // Update is called once per frame
    void Update()
    {
        SetStaminaUI();
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
                Sprite spriteItem = Resources.Load<Sprite>("Keys/"+objListItems[i].name.ToString());
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

    IEnumerator FadeOutGameOver()
    {
        yield return new WaitForSeconds(1f);
        float fRaise = 0;
        Color color = m_txtGameOver.color;
        if(color.a == 0) //���� ������ 0�϶����� ���� -> �ߺ� Fade In ����
        {
            while (fRaise < 1)// �������� ���� �ִ�ġ 1�����϶�
            {
                fRaise += 0.1f; //������ ����

                color.a = fRaise;
                m_txtGameOver.color = color; //fRaise������ ������ ���� -> fade in
                yield return new WaitForSeconds(0.1f); //0.1�� ���� ����
            }
            yield return new WaitForSeconds(2f); // ������ �� �� ��ȯ
            m_csGameManager.LoadGameOverScene();
        }
    }
}
