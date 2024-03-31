using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    //�÷��̾��� ������ ������Ʈ�� ���� ����Ʈ
    List<GameObject> m_listItems = new List<GameObject>();
    // ����Ʈ�� �ְ� ������  UI panel�� �̹��� �����̹Ƿ� ����� �ٲ��� �ʰ� �Ѵ�.
    int m_nMaxLenght = 3; 

    // Start is called before the first frame update
    void Start()
    {
        
    }


    //���� ����Ʈ�� �������� �߰��� �������� �ľ��ϴ� �Լ�
    public bool GetIsAddItem()
    {
        if(m_listItems.Count < m_nMaxLenght)
        {
            return true;
        }
        else
        {
            return false;
        }
 
    }

    //�÷��̾ �Ѱ��� �������� ����Ʈ�� ���� �Լ�
    public void SetItem(GameObject objItem)
    {
        // ���� ����Ʈ�� ������ �� ������ ������
        if(m_listItems.Count < m_nMaxLenght)
        {
            m_listItems.Add(objItem); //�÷��̾ �Ѱ��� �������� �߰�
             //���̾��Ű ���� �������� Activefalse�� �����Ͽ� �Ⱥ��̰� �Ѵ�.
            objItem.SetActive(false);
        }
    }

    //�ش� �������� ����Ʈ�� �ִ��� Ȯ��(LockedDoor�� key�� �ִ��� Ȯ��)
    public bool CheckItem(GameObject objItem)
    {
        return m_listItems.Contains(objItem);
    }

    //������ ���� ����Ʈ ����
    public void UseItem(GameObject objItem)
    {
        m_listItems.Remove(objItem);
        Destroy(objItem);
    }

    //UI���� ������ ����Ʈ�� �޾ƿ������� �Լ�
    public List<GameObject> GetListItems()
    {
        return m_listItems;
    }
}
