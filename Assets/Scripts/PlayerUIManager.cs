using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용

public class PlayerUIManager : MonoBehaviour
{
    /*던전씬에서 플레이어의 UI를 관리하는 스크립트*/

    /* GetComponet와 SerializedField로 오브젝트를 가져오는 방법을 둘 다 사용하였다.
    사실상 SerializedField 같은경우 편하긴하나 값이 오브젝트마다 바뀌는 경우가 아니면
    권장하지는 않는다. */

    GameObject m_objPlayer = null; //플레이어 오브젝트

    // 스태미나 관련 멤버변수 
    [SerializeField]
    GameObject m_objStamina = null;  // setActive toggle시킬 스테미나 오브젝트(canvas)
    [SerializeField]
    Image m_imgStaminaBackground = null; //스태미나 프레임
    [SerializeField]
    Image m_imgStaminaGauge = null; //스태미나 게이지
    float m_fStaminaMaxWith = 0; // 스테미나 게이지 최대넓이
    float m_fCurrentStaminaRate = 0; //현재 스태미나 비율

    //메뉴 관련 멤버변수
    GameObject m_objMenu = null;

    //Props메세지 관련 멤버변수
    GameObject m_objPropsMessage = null; //text UI 자체를 setAcive toggle 오브젝트
    Text m_txtPropsName = null; // Props이름
    Text m_txtPropsDescription = null; //Props설명

    //item 관련 멤버변수
    Image[] m_arrItems = null;
    GameObject m_objItems = null;
    ItemManager m_csItemManager = null;

    //게임오버 관련 멤버변수
    TextMeshProUGUI m_txtGameOver = null;
    GameManager m_csGameManager = null; //씬전환시 사용
   
    //오디오 관련
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

        //스태미나관련
        if (m_objStamina == null)
        {
            Debug.LogError("m_objStamina가 없습니다.");
        }
        else
        {
        }

        if(m_imgStaminaBackground == null)
        {
            Debug.LogError("m_imgStaminaBackground가 없습니다.");
        }
        else
        {
        }
        
        if(m_imgStaminaGauge == null)
        {
            Debug.LogError("m_imgStaminaGauge가 없습니다.");
        }
        else
        {
        }
        //스태미나 UI의 최고 너비는 m_imgStaminaBackground의 너비와 같다.
        m_fStaminaMaxWith = m_imgStaminaBackground.rectTransform.rect.width;

        //아이템 메세지 관련
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

        //기본으로 보이지 않는다. 주의할 점은 비활성화 먼저하고, 자식오브젝트를 Find하면 하이어라키 창에서 못가져온다.
        m_objPropsMessage.SetActive(false); 

        //인벤토리관련
        if(m_objItems == null)
        {
            m_objItems = GameObject.Find("Items");
        }
        else 
        {
        }

        if(m_arrItems == null)
        {
            //Canvas의 Inventory/Items 의 이미지들을 가져온다.
            m_arrItems = m_objItems.GetComponentsInChildren<Image>();
        }
        
        if(m_csItemManager == null)
        {
            m_csItemManager = m_objPlayer.GetComponent<ItemManager>();
        }

        ClearInventory();// UI초기화

        //GameOver관련
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
            //스테미나 세팅
            SetStaminaUI();

            //timeScale이 0일때에는 메뉴를 비활성화 시킨다.
            if(m_objMenu.activeSelf == true)
            {
                // 활성화 되어있다 timeScale이 1이될때 한 번만 호출
                m_objMenu.SetActive(false);
            }
        }
        else //timeScale이 0일 때
        {
  
        }
        SetMenu();// 메뉴관련 함수는 timeScale에 영향 없이 호출한다.
    }

    void SetMenu()
    {
        // 메뉴panel 세팅
        if (m_objMenu != null)
        {
            //메뉴가 비활성화 상태일때만 esc버튼을 입력받는다.
            if (m_objMenu.activeSelf == false)
            {
                bool isMenuKeyDown = Input.GetKeyDown(KeyCode.Escape);
                if (isMenuKeyDown)
                {
                    m_csGameManager.PauseTime();
                    m_objMenu.SetActive(true);//esc누를시 메뉴 활성화 -> 비활성화는 update에서 하고 있다. 
                }
            }
            else // 메뉴가 활성화 상태일때
            {

            }
        }
        else
        {
            Debug.LogError("m_objMenu가 없습니다.");
        }
    }
 

    // 스테미나 Gauge 관리
    void SetStaminaUI()
    {
        //PlayerControl 스크립트에서 현재 스태미너 비율을 가져온다.
        m_fCurrentStaminaRate = m_objPlayer.GetComponent<PlayerControl>().GetCurrentStaminaRate();
        //Debug.Log(m_fCurrentStaminaRate);
        if(m_fCurrentStaminaRate == 1)
        {
            //만약 현재 스태미너 잔량 비율이 1이라면 Full이므로 스테미너 UI을 비활성화 시킨다.
            m_objStamina.SetActive(false);
        }
        else
        {
            //만약 1이 아니라면, 스태미너가 줄거나 회복하고 있으므로 UI를 표시한다.
            m_objStamina.SetActive(true);
            // 현재 스테이너 이미지의 길이는 최고 너비에서 현재 스태미나 잔량 비율을 곱해준다.
            float fCurrentWidth = m_fStaminaMaxWith * m_fCurrentStaminaRate;
            // 현재 스태미나 gauge의 rectTransform의 값을 가져온다.
            Vector3 vecRectSize = m_imgStaminaGauge.rectTransform.sizeDelta;
            //받아온 사이즈에서 x축 값을 현재 너비로 변경후 다시 대입
            vecRectSize.x = fCurrentWidth;
            m_imgStaminaGauge.rectTransform.sizeDelta = vecRectSize;
        }
    }

   //아이템이 감지될시 보여줄 메세지관련 함수들//

    //PropsMessage 오브젝트를 toggle할 함수
    public void SetActivePropsMessage(bool isActive)
    {
        m_objPropsMessage.SetActive(isActive);
    }

    //PropsMessage의 text들을 설정할 함수
    public void SetPropsMessage(string strName, string strDescription)
    {
        m_txtPropsName.text = strName;
        m_txtPropsDescription.text = strDescription;
    }

    //아이템 값이 변경되거나 시작될때 초기화
    //이미지는 갯수가 정해져 있어 배열로 쓰지만 리스트 처럼 아이템 사용시
    //빈 공간을 사용하지 않기위해 초기화 후 다시 리스트(ItemManager) 요소대로 채운다.
    public void ClearInventory()
    {
        foreach(Image i in m_arrItems)
        {
            //스프라이트를 null로 바꾸어준다.
            i.GetComponent<Image>().sprite = null;
            //투명도를 0으로 바꾸어 기본적으로 안보이게 한다.
            Color color = i.GetComponent<Image>().color;
            color.a = 0;
            i.GetComponent<Image>().color = color;
        }
    }

    //인벤토리의 이미지들을 표시할 함수
    public void SetInventory()
    {
        List<GameObject> objListItems = m_csItemManager.GetListItems();
        if(objListItems.Count > 0) // 배열에 요소가 있을 때에만 적용한다. 없을시에는 변경 X
        {
            //스프라이트를 아이템스프라이트로 설정하고 투명도를 1로 설정
            for (int i = 0; i < objListItems.Count; i++)
            {
                //스프라이트 파일명이 해당 오브젝트의 이름과 같다.
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

    //코루틴을 통하여 페이드 인 효과
    IEnumerator FadeOutGameOver()
    {
        yield return new WaitForSeconds(1f);
        // 게임오버 오디오 클립 재생
        SetGameoverClip();

        float fRaise = 0;
        Color color = m_txtGameOver.color;
        if(color.a == 0) //현재 투명도가 0일때에만 실행 -> 중복 코루틴 호출 Fade In 방지
        {
            while (fRaise < 1)// 증가값이 투명도 최대치 1이하일때
            {
                fRaise += 0.025f; //투명도값 증가

                color.a = fRaise;
                m_txtGameOver.color = color; //fRaise값으로 투명도값 변경 -> fade in
                yield return new WaitForSeconds(0.1f); //0.1초 마다 실행
            }
            yield return new WaitForSeconds(2f); // 딜레이 후 씬 전환
            m_csGameManager.LoadGameOverScene();
        }
    }
    // 게임오버 오디오 클립 재생
    void SetGameoverClip()
    {
        //메인 카메라 AudioSource 설정 GameOver클립도 (BGM)이기때문
        AudioSource CameraAudioSource = Camera.main.GetComponent<AudioSource>();
        if (CameraAudioSource == null)
        {
            Debug.LogError("메인카메라에 오디오 소스가 없습니다.");
        }
        else
        {
            if (m_clipGameOver != null)
            {
                //현재 오디오 클립이 해당로직에서 필요한 클립이 아닐시 한번만 실행
                if (CameraAudioSource.clip != m_clipGameOver)
                {
                    CameraAudioSource.Stop();//재생 멈춤
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

    // 아이템 얻는 오디오 재생 함수 PlayerControl.cs에서 호출ㄴ
    public void SetPickUpAudio()
    {
        if(m_UIAudioSource != null)
        {
            if(m_clipPickUp != null)
            {   //현재 오디오 클립이 해당로직에서 필요한 클립이 아닐시 한번만 실행
                if (m_UIAudioSource.clip != m_clipPickUp)
                {
                    float fClipLenght = m_clipPickUp.length;//오디오 클립길이
                    m_UIAudioSource.clip = m_clipPickUp; //클립설정
                    m_UIAudioSource.loop = false; //루프false
                    m_UIAudioSource.Play(); //재생
                    StartCoroutine(CorInitAudioSource(fClipLenght));
                }
            }
            else
            {
                Debug.LogError("m_clipPickUp이 없습니다.");
            }
        }
        else
        {
            Debug.LogError("m_UIAudioSource가 없습니다.");
        }

    }

    //클립 속도 만큼 기다린 후 AudioSource초기화 하는 코루틴
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
            Debug.LogError("m_UIAudioSource가 없습니다.");
        }
    }
}
