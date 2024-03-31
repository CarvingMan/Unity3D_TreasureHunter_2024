using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //자식관계: 플레이어 > ScreenRotation >MainCamera   

    //이동 관련 멤버변수//
    Vector3 m_vecMoveDirection = Vector3.zero;
    float m_fMoveSeed = 0.03f;
    float m_fRunAcceleration = 1; //기본 1, 달리기 시작할시 3 
    bool m_isUpKey = false;
    bool m_isDownKey = false;
    bool m_isRightKey = false;
    bool m_isLeftKey = false;
    bool m_isRunKey = false; // Shift Key 입력
    bool m_isRuning = false; // 현재 뛰고있는 상태인지 확인
    bool m_isCrouchKey = false; //웅크리기 키

    float m_fMaxStamina = 5; //최대 스테미나
    float m_fCurrentStamina = 0; //현재 스태미나

    //아이템(문 열쇠 보물) 상호작용 멤버변수
    bool m_isActiveKeyDown = false; // 상호작용 키 E
    // Getkey 다운 시 일 순간만 ture이므로 호출 프레임에 따라
    // OnTriggerStay에서 감지를 못할 수 있다. 따라서 아래 변수를 통해 제어 후 로직이 끝날 시 토글한다.
    bool m_isActivating = false;
    //시점 회전 관련 멤버변수//
    Vector3 m_vecScreenRotation = Vector3.zero;
    Vector3 m_vecCameraRotation = Vector3.zero;
    [SerializeField]
    Camera m_camera = null;
    [SerializeField]
    // 카메라는 아래 오브젝트 자식이기에 회전시 카메라도 회전한다.
    //다만 플레이어는 기본고정, 움직일시 해당오브젝트 회전값으로 회전
    GameObject m_objScreenRotation = null;
    float m_fMouseX = 0;//마우스 axis값 받을 변수
    float m_fMouseY = 0;
    float m_fMouseSpeed = 10;

    //리지드바디 관련 멤버변수
    Rigidbody m_rigidBody = null;

    //레이캐스트를 통해 계단이 감지될때
    bool m_isCheckStairs = false;


    //애니메이션 관련 멤버변수//
    Animator m_animator = null;

    //PlayerUIManager 스크립트를 가져오기위한 멤버변수 -> 내장 public 메소드 사용하기위함
    PlayerUIManager m_csPlayerUIManager;

    // Start is called before the first frame update
    void Start()
    {
        if (m_objScreenRotation != null)
        {
            //기본적으로 rotation은 사원수로 되어있어 행렬형태이다
            //따라서 값을 편하게 변경하기위해 오일러 각으로 변환하여 사용한다
            m_vecScreenRotation = m_objScreenRotation.transform.rotation.eulerAngles;
        }
        else
        {
            Debug.LogError("m_objScreenRotation이 없습니다.");
            m_objScreenRotation = GameObject.Find("ScreenRotation");
            m_vecScreenRotation = m_objScreenRotation.transform.localRotation.eulerAngles;
        }

        if (m_camera != null)
        {
            m_vecCameraRotation = m_camera.transform.localRotation.eulerAngles;
        }
        else
        {
            Debug.LogError("m_camera가 없습니다.");
            m_camera = Camera.main;
            m_vecCameraRotation = m_camera.transform.localRotation.eulerAngles;
        }

        if (m_animator == null)
        {
            //애니메이터 컴포넌트가져오기
            m_animator = GetComponent<Animator>();
        }
        else
        {
        }

        if (m_rigidBody == null)
        {
            m_rigidBody = GetComponent<Rigidbody>();
        }
        else
        {

        }
        m_fCurrentStamina = m_fMaxStamina;

        if (m_csPlayerUIManager == null)
        {
            //해당하는 타입의 오브젝트를 찾는다.
            m_csPlayerUIManager = FindObjectOfType<PlayerUIManager>();
        }
        else
        {
        }

    }

    // Update is called once per frame
    void Update()
    {
        InputProcess();
        CheckStairs();
        MoveProcess();
        SetStamina();
        SetAnimator();
    }

    //설정된 프레임이 지날 때마다 호출된다. 물리적인 움직임은 디바이스상관없이 같은 속도여야 한다.
    private void FixedUpdate()
    {
        //방향*스피드*가속도(기본1이라 영향X 달리기시 3배속)
        transform.Translate(m_vecMoveDirection * m_fMoveSeed * m_fRunAcceleration);
    }
    //모든 Update 로직이 끝나고 실시한다. 카메라가 마지막에 움직이도록 한다.
    private void LateUpdate()
    {
        CameraRotate();
    }

    // 키 입력 처리 함수
    void InputProcess()
    {
        // 기본 이동 키 입력
        m_isUpKey = Input.GetKey(KeyCode.W);
        m_isDownKey = Input.GetKey(KeyCode.S);
        m_isLeftKey = Input.GetKey(KeyCode.A);
        m_isRightKey = Input.GetKey(KeyCode.D);
        // 달리기 키
        m_isRunKey = Input.GetKey(KeyCode.LeftShift);
        //웅크리기 키 -> ctrl 키이었으나 유니티 실행시 ctrl + s(뒤) 하면 저장하기와 겹쳐져 임시(추후 ctrl)로 변경가능
        m_isCrouchKey = Input.GetKey(KeyCode.Space);

        //아이템 상호작용 키 (눌렀을 때에만)
        m_isActiveKeyDown = Input.GetKeyDown(KeyCode.E);
        if (m_isActiveKeyDown)
        {
            m_isActivating = true; //활성화 중
        }
        else
        {
            // false는 OnTriggerStay 로직이 끝나고 토글
        }

        //카메라(시점) 관련
        m_fMouseX = Input.GetAxis("Mouse X");
        m_fMouseY = Input.GetAxis("Mouse Y") * -1; //상하 회전은 위로 볼때 -값이다. 따라서 axis에 음수를 곱해준다.
    }

    // 이동 처리 함수
    void MoveProcess()
    {
        if (m_isUpKey)
        {
            //z축 방향 1(정면)
            m_vecMoveDirection.z = 1;

            // 달리기는 일반적으로 정면 방향으로 달릴때 가능
            // 사이드나 뒤로 빨리뛰기는 현실적으로 달리기라 애매하다
            // 웅크리고 있을때에는 달리지 못한다.
            if (m_isRunKey && m_fCurrentStamina > 0 && !m_isCrouchKey) //달리기 키를 누르고 현재스태미나가 0보다 클때
            {
                //달리기시 가속도 3배
                m_fRunAcceleration = 3;
                m_isRuning = true;
            }
            else
            {
                //달리지 않을 시에는 가속도 기본 1
                m_fRunAcceleration = 1;
                m_isRuning = false;
            }
        }
        else if (m_isDownKey)
        {
            //z축 방향 -1 (뒤)
            m_vecMoveDirection.z = -1;
        }
        else
        {
            //z축 방향 0 (앞 뒤 이동X)
            m_vecMoveDirection.z = 0;
            //달리지 않을 시에는 가속도 기본 1
            m_fRunAcceleration = 1;
            m_isRuning = false;
        }


        if (m_isRightKey)
        {
            //x축 방향 1(오른쪽)
            m_vecMoveDirection.x = 1;
        }
        else if (m_isLeftKey)
        {
            //x축 방향 -1(왼쪽)
            m_vecMoveDirection.x = -1;
        }
        else
        {
            //x축 방향 0(좌우 이동X)
            m_vecMoveDirection.x = 0;
        }

        //발끝의 레이캐스트를 통해 계단이 감지될 때 
        if (m_isCheckStairs && m_vecMoveDirection != Vector3.zero) 
        {
            // 위는 계단 앞에 그냥 멈춰있을때 위아래로 둥둥뜨는 것을 방지하기위함
            // 레이로 계단이 감지가 되고 플레이어가 움직이고 있을때 
          
            m_vecMoveDirection.y = 1; //y축 방향으로도 움직인다.
        }
        else
        {
            m_vecMoveDirection.y = 0;
        }
    }

    void SetStamina()
    {
        if (m_isRuning)
        {
            //현재 달리고 있을때 스태미나 감소
            if(m_fCurrentStamina > 0)
            {
                m_fCurrentStamina -= Time.deltaTime*1.5f; // 스태미나 감소: 감소는 회복보다 빠르다
            }
        }
        else
        {
            //현재 달리고 있지 않을때 스태미나 회복
            if(m_fCurrentStamina < m_fMaxStamina)
            {
                m_fCurrentStamina += Time.deltaTime; //
            }
        }
        // m_fCurrentStamina가 증/감 하면서 0 혹은 m_fMaxStamina를 벗어나지 않도록 고정
        m_fCurrentStamina = Mathf.Clamp(m_fCurrentStamina, 0, m_fMaxStamina);
    }
    // 현재 남은 스태미나 비율을 전달할 함수 -> PlayerUIManager에서 사용
    public float GetCurrentStaminaRate()
    {
        return m_fCurrentStamina/m_fMaxStamina; // 남은 스태미나 비율 전달
    }

    void SetAnimator()
    {
        if (m_animator != null)
        {
            //애니메이터 파라미터 설정
            m_animator.SetBool("isCrouch", m_isCrouchKey);
            m_animator.SetBool("isFoward", m_isUpKey);
            m_animator.SetBool("isBack", m_isDownKey);
            m_animator.SetBool("isRight", m_isRightKey);
            m_animator.SetBool("isLeft", m_isLeftKey);
            m_animator.SetBool("isRun", m_isRuning); //달리기만 키 입력이 아닌 m_isRunning이다.
        }
        else
        {
            Debug.LogError("m_animator가 없습니다.");
            m_animator = GetComponent<Animator>();
        }
    }

    //카메라(시점) 회전 
    void CameraRotate()
    {
        if (m_camera != null)
        {
            if (m_objScreenRotation != null)
            {
                //마우스의 x축 만큼 rotation에 더해준다. + 회전축의 좌우회전은 y축이다.
                m_vecScreenRotation.y = m_vecScreenRotation.y + m_fMouseX * m_fMouseSpeed;
                //오일러 값을 다시 사원수로 변경해서 rotation에 넣어준다.
                m_objScreenRotation.transform.rotation = Quaternion.Euler(m_vecScreenRotation);
                //플레이어가 움직일시 m_objScreenRotation의 회전값으로 바꾸어준다.
                // 화면 회전에 따라 플레이어 회전 방향 설정


                // 플레이어가 움직일 때 스크린 회전값으로 점진적 회전
                float fPlayerRotateSpeed = 10f;
                Vector3 targetDirection = m_objScreenRotation.transform.forward; //m_objScreenRotation의 정면방향을 target
                if (m_vecMoveDirection != Vector3.zero)
                {
                    // 플레이어의 회전을 부드럽게 보간
                    //tagetDirection을 바라보는 회전 생성
                    //Quaternion.LookRotation은 주어진 방향 벡터를 바라보는 회전을 생성하는 Unity의 함수라고 한다.
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    //Quaternion.Lerp 함수는 두 개의 회전 사이를 선형 보간하는 함수이다.
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * fPlayerRotateSpeed);

                }
                else
                {

                }
            }
            else
            {
                Debug.LogError("m_objScreenRotation이 없습니다.");
                m_objScreenRotation = GameObject.Find("ScreenRotation");
                m_vecScreenRotation = m_objScreenRotation.transform.localRotation.eulerAngles;
            }


            //상하 회전 카메라 기준으로 해야한다. -> 플레이어와 떨어져있기때문//
            //마우스의 y축 만큼 m_camera에 더해준다.
            m_vecCameraRotation.x = m_vecCameraRotation.x + m_fMouseY * m_fMouseSpeed;
            // 상하 마우스는 Clamp로 최소 최대값을 고정하여 360도로 돌지 못하게 한다.
            m_vecCameraRotation.x = Mathf.Clamp(m_vecCameraRotation.x, -25, 5);
            //오일러 값을 다시 사원수로 변경해서 rotation에 넣어준다.
            m_camera.transform.localRotation = Quaternion.Euler(m_vecCameraRotation);
        }
        else
        {
            Debug.LogError("m_camera를 찾을 수 없습니다.");
            m_camera = GetComponent<Camera>();
            m_vecCameraRotation = m_camera.transform.localRotation.eulerAngles;
        }
    }

    //레이캐스트로 발끝에서 탐지하여 계단이면 위로 올라가게 하는 메소드//
    void CheckStairs()
    {
        RaycastHit hitFront; // 레이에 맞은 콜라이더의 정보를 받기위한 변수
        RaycastHit hitBack;
        RaycastHit hitRight;
        RaycastHit hitLeft;
        //y축좌표는 기본으로 해당 오브젝트 발끝에 설정 되어있다.
        Vector3 vecOrigin = transform.position; //Ray의 origin vector
        float fMaxDistance = 0.5f; // 레이케스트 길이 -> 걸을때 앞발과 중심점과 z축 차이가 있으므로 여유롭게 쏴준다.

        //계단 레이어만 맞출 수 있도록 설정
        int nLayerMask = 1 << LayerMask.NameToLayer("Stairs");
        //현재 좌표(발바닥)에서 앞쪽 방향으로 0.5만큼 레이를 쏜다.
        bool isCastFront = Physics.Raycast(vecOrigin, transform.forward, out hitFront, fMaxDistance, nLayerMask);
        // 뒤 방향
        bool isCastBack = Physics.Raycast(vecOrigin, transform.forward * -1, out hitBack, fMaxDistance, nLayerMask);
        bool isCastRight = Physics.Raycast(vecOrigin, transform.right, out hitRight, fMaxDistance, nLayerMask); 
        bool isCastLeft = Physics.Raycast(vecOrigin,transform.right * -1, out hitLeft, fMaxDistance, nLayerMask);
       // Debug.DrawRay(vecOrigin, transform.forward * 0.5f, Color.red);
       // Debug.DrawRay(vecOrigin, transform.forward * -0.5f, Color.red);

        if (m_rigidBody != null)
        {
            if (isCastFront || isCastBack || isCastRight || isCastLeft)
            {
                //계단에 맞았을시 중력을 비활성화 하고 m_vecMoveDirection.y를 1로 설정
                //(Addforce로 올라가면 튕겨나가는등 부자연스럽다.) 혹은 PlayerController를 사용하면 편하다고 하는데
                //                                              중력계산을 따로 해야한다.
                m_rigidBody.useGravity = false;
                m_rigidBody.velocity = Vector3.zero;
                //m_rigidBody.isKinematic = true; 키네틱은 설정하면 직접 움직일시 좋지 않아서 제외

                m_isCheckStairs = true; // 위로 이동처리는 MoveProcess에서 한다.
            }
            else
            {
                //레이가 아무것도 맞지 않았을 시에도 false
                m_isCheckStairs = false;
                m_rigidBody.useGravity = true;
            }
        }
        else
        {
            Debug.LogError("m_rigidBody가 없습니다.");
            m_rigidBody = GetComponent<Rigidbody>();
        }
    }

    //플레이어의 Rader Colider의 트리거 감지(문,열쇠,보물)
    private void OnTriggerEnter(Collider other)
    {
        m_isActivating = false; //E키를 미리 누르고 접근할 수 있으므로 false로 초기화 해준다.
        string strPropsName = null;
        string strPropsDescription = null;
        if (other.CompareTag("Door"))
        {   
            strPropsName = other.tag;
            bool isLocked = other.GetComponentInParent<DoorActive>().GetIsLocked();
            if (isLocked) //Door(Locked)가 감지된다면
            {
                //잠긴문의 열쇠를 가져온다.
                GameObject objKey = other.GetComponentInParent<DoorActive>().GetKey();
                //해당 열쇠가 아이템리스트에 있는지 확인
                bool isCheckKey = GetComponent<ItemManager>().CheckItem(objKey);
                if (isCheckKey) // 열쇠를 가지고 있다면
                {
                    strPropsDescription = "Press E to unlock";
                }
                else
                {
                    strPropsDescription = "It's locked.";
                }
            }
            else
            {
                //Door(일반)가 감지된다면
                //감지된 Door의 최상위 오브젝트에 스크립트가 있기에 InParent 사용
                bool isActive = other.GetComponentInParent<DoorActive>().GetIsActive();
                if (!isActive)
                {
                    strPropsDescription = "Press E to open";
                }
                else
                {
                    strPropsDescription = "Press E to close";
                }
            }

            //TextUI설정
            m_csPlayerUIManager.SetPropsMessage(strPropsName, strPropsDescription);
            m_csPlayerUIManager.SetActivePropsMessage(true);
        }
        else if (other.CompareTag("Item")) //아이템일때
        {
            strPropsName = other.name.ToString();
            //현재 인벤토리 창에 추가가능한지 확인
            bool isAddItem = GetComponent<ItemManager>().GetIsAddItem();
            if (isAddItem)
            {
                strPropsDescription = "Press E to get";
            }
            else
            {
                strPropsDescription = "Inventory is full";
            }
            //TextUI설정
            m_csPlayerUIManager.SetPropsMessage(strPropsName, strPropsDescription);
            m_csPlayerUIManager.SetActivePropsMessage(true);
        }
        else
        {
        }
    }


    private void OnTriggerStay(Collider other)
    {
       
        if (other.CompareTag("Door"))
        {
            m_csPlayerUIManager.SetActivePropsMessage(true);
            bool isLocked = other.GetComponentInParent<DoorActive>().GetIsLocked();
            // 트리거에 접촉한 콜라이더 오브젝트 태그가 Door(일반)이라면
            if (m_isActivating) //상호작용 키를 눌렀을때 ture가 된다.
            {
                if (isLocked) //잠겨있을시
                {
                    //잠긴문의 열쇠를 가져온다.
                    GameObject objKey = other.GetComponentInParent<DoorActive>().GetKey();
                    //해당 열쇠가 아이템리스트에 있는지 확인
                    bool isCheckKey = GetComponent<ItemManager>().CheckItem(objKey);
                    if (isCheckKey)
                    {
                        //아이템사용
                        GetComponent<ItemManager>().UseItem(objKey);
                        //잠금해제
                        other.GetComponentInParent<DoorActive>().UnLockDoor();
                        m_csPlayerUIManager.ClearInventory();
                        m_csPlayerUIManager.SetInventory();
                        //Door의 현재 애니메이션 파라미터 상태를 가져온다.
                        //감지된 Door의 최상위 오브젝트에 스크립트가 있기에 InParent 사용
                        bool isAtive = other.GetComponentInParent<DoorActive>().GetIsActive();
                        //Door 애니매에션의 파라미터를 토글한다. (닫혀있음)->(열림)
                        other.GetComponentInParent<DoorActive>().SetActiveAnimator(!isAtive);
                    }
                    else
                    {

                    }
                }
                else
                {
                    //Door의 현재 애니메이션 파라미터 상태를 가져온다.
                    //감지된 Door의 최상위 오브젝트에 스크립트가 있기에 InParent 사용
                    bool isAtive = other.GetComponentInParent<DoorActive>().GetIsActive();
                    //Door 애니매에션의 파라미터를 토글한다. (닫혀있음)->(열림)
                    other.GetComponentInParent<DoorActive>().SetActiveAnimator(!isAtive);
                }
            }
            else
            {
                m_isActivating = false; //활성화 종료(오류대비)
            }
            m_isActivating = false; //활성화 종료
        }
        else if (other.CompareTag("Item"))
        {
            m_csPlayerUIManager.SetActivePropsMessage(true);
            //현재 인벤토리 창에 추가가능한지 확인
            bool isAddItem = GetComponent<ItemManager>().GetIsAddItem();
            if (isAddItem && m_isActivating)
            {
                //Debug.Log("확인");
                GetComponent<ItemManager>().SetItem(other.gameObject);
                m_csPlayerUIManager.ClearInventory();
                m_csPlayerUIManager.SetInventory();
            }
            else
            {

            }
            m_isActivating = false;
        }
        else
        {
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //감지 트리거에서 벗어날 시 UI SetActive를 false(아이템이 닿아 있을때만 보여준다.)
        m_csPlayerUIManager.SetActivePropsMessage(false); 
    }
}
