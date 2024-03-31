using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    //플레이어의 아이템 오브젝트를 담을 리스트
    List<GameObject> m_listItems = new List<GameObject>();
    // 리스트의 최고 갯수는  UI panel의 이미지 갯수이므로 상수로 바뀌지 않게 한다.
    int m_nMaxLenght = 3; 

    // Start is called before the first frame update
    void Start()
    {
        
    }


    //현재 리스트에 아이템을 추가가 가능한지 파악하는 함수
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

    //플레이어가 넘겨준 아이템을 리스트에 담을 함수
    public void SetItem(GameObject objItem)
    {
        // 현재 리스트의 갯수가 꽉 차있지 않을때
        if(m_listItems.Count < m_nMaxLenght)
        {
            m_listItems.Add(objItem); //플레이어가 넘겨준 아이템을 추가
             //하이어라키 상의 아이템은 Activefalse로 변경하여 안보이게 한다.
            objItem.SetActive(false);
        }
    }

    //해당 아이템이 리스트에 있는지 확인(LockedDoor의 key가 있는지 확인)
    public bool CheckItem(GameObject objItem)
    {
        return m_listItems.Contains(objItem);
    }

    //아이템 사용시 리스트 삭제
    public void UseItem(GameObject objItem)
    {
        m_listItems.Remove(objItem);
        Destroy(objItem);
    }

    //UI에서 아이템 리스트를 받아오기위한 함수
    public List<GameObject> GetListItems()
    {
        return m_listItems;
    }
}
