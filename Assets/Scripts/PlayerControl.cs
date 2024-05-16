using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //�ڽİ���: �÷��̾� > ScreenRotation >MainCamera   

    //�̵� ���� �������//
    Vector3 m_vecMoveDirection = Vector3.zero;
    float m_fMoveSeed = 0.03f;
    float m_fRunAcceleration = 1; //�⺻ 1, �޸��� �����ҽ� 3 
    bool m_isUpKey = false;
    bool m_isDownKey = false;
    bool m_isRightKey = false;
    bool m_isLeftKey = false;
    bool m_isRunKey = false; // Shift Key �Է�
    bool m_isRuning = false; // ���� �ٰ��ִ� �������� Ȯ��
    bool m_isCrouchKey = false; //��ũ���� Ű

    float m_fMaxStamina = 5; //�ִ� ���׹̳�
    float m_fCurrentStamina = 0; //���� ���¹̳�

    //������(�� ���� ����) ��ȣ�ۿ� �������
    bool m_isActiveKeyDown = false; // ��ȣ�ۿ� Ű E
    // Getkey �ٿ� �� �� ������ ture�̹Ƿ� ȣ�� �����ӿ� ����
    // OnTriggerStay���� ������ ���� �� �ִ�. ���� �Ʒ� ������ ���� ���� �� ������ ���� �� ����Ѵ�.
    bool m_isActivating = false;
    //���� ȸ�� ���� �������//
    Vector3 m_vecScreenRotation = Vector3.zero;
    Vector3 m_vecCameraRotation = Vector3.zero;
    [SerializeField]
    Camera m_camera = null;
    [SerializeField]
    // ī�޶�� �Ʒ� ������Ʈ �ڽ��̱⿡ ȸ���� ī�޶� ȸ���Ѵ�.
    //�ٸ� �÷��̾�� �⺻����, �����Ͻ� �ش������Ʈ ȸ�������� ȸ��
    GameObject m_objScreenRotation = null;
    float m_fMouseX = 0;//���콺 axis�� ���� ����
    float m_fMouseY = 0;
    float m_fMouseSpeed = 10;

    //������ٵ� ���� �������
    Rigidbody m_rigidBody = null;

    //����ĳ��Ʈ�� ���� ����� �����ɶ�
    bool m_isCheckStairs = false;


    //�ִϸ��̼� ���� �������//
    Animator m_animator = null;

    //PlayerUIManager ��ũ��Ʈ�� ������������ ������� -> ���� public �޼ҵ� ����ϱ�����
    PlayerUIManager m_csPlayerUIManager = null;

    //GameManager ��ũ��Ʈ
    GameManager m_csGameManager = null;
    //Player ��/�����
    bool m_isAlive = true;

    //���� ȹ��
    bool m_isFoundTresure = false;


    //���� ����
    //�÷��̾��� ����� �ҽ�
    AudioSource m_PlayerAudioSource = null;
    //����� �ҽ�Ŭ���� ����(�����) Ŭ����
    [SerializeField]
    AudioClip m_clipPlayerWalk = null;
    [SerializeField]
    AudioClip m_clipPlayerRun = null;
    [SerializeField]
    AudioClip m_clipPlayerDie = null;
    [SerializeField]
    AudioClip m_clipDance = null;

    // Start is called before the first frame update
    void Start()
    {
        if (m_objScreenRotation != null)
        {
            //�⺻������ rotation�� ������� �Ǿ��־� ��������̴�
            //���� ���� ���ϰ� �����ϱ����� ���Ϸ� ������ ��ȯ�Ͽ� ����Ѵ�
            m_vecScreenRotation = m_objScreenRotation.transform.rotation.eulerAngles;
        }
        else
        {
            Debug.LogError("m_objScreenRotation�� �����ϴ�.");
            m_objScreenRotation = GameObject.Find("ScreenRotation");
            m_vecScreenRotation = m_objScreenRotation.transform.localRotation.eulerAngles;
        }

        if (m_camera != null)
        {
            m_vecCameraRotation = m_camera.transform.localRotation.eulerAngles;
        }
        else
        {
            Debug.LogError("m_camera�� �����ϴ�.");
            m_camera = Camera.main;
            m_vecCameraRotation = m_camera.transform.localRotation.eulerAngles;
        }

        if (m_animator == null)
        {
            //�ִϸ����� ������Ʈ��������
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
            //�ش��ϴ� Ÿ���� ������Ʈ�� ã�´�.
            m_csPlayerUIManager = FindObjectOfType<PlayerUIManager>();
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

        if (m_PlayerAudioSource == null)
        {
            m_PlayerAudioSource = GetComponent<AudioSource>();
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        // �÷��̾ ��������ÿ��� ó�� && ������ ã����
        if (m_isAlive && !m_isFoundTresure)
        {
            InputProcess();
            CheckStairs();
            MoveProcess();
        }
        else
        {

        }
        SetStamina();
        SetAnimator();
    }

    //������ �������� ���� ������ ȣ��ȴ�. �������� �������� ����̽�������� ���� �ӵ����� �Ѵ�.
    private void FixedUpdate()
    {
        //����*���ǵ�*���ӵ�(�⺻1�̶� ����X �޸���� 3���)
        transform.Translate(m_vecMoveDirection * m_fMoveSeed * m_fRunAcceleration);
    }
    //��� Update ������ ������ �ǽ��Ѵ�. ī�޶� �������� �����̵��� �Ѵ�.
    private void LateUpdate()
    {
        //�����
        SetAudio();
        //ī�޶� ȸ��
        CameraRotate();
    }

    // Ű �Է� ó�� �Լ�
    void InputProcess()
    {
        // �⺻ �̵� Ű �Է�
        m_isUpKey = Input.GetKey(KeyCode.W);
        m_isDownKey = Input.GetKey(KeyCode.S);
        m_isLeftKey = Input.GetKey(KeyCode.A);
        m_isRightKey = Input.GetKey(KeyCode.D);
        // �޸��� Ű
        m_isRunKey = Input.GetKey(KeyCode.LeftShift);
        //��ũ���� Ű -> ctrl Ű�̾����� ����Ƽ ����� ctrl + s(��) �ϸ� �����ϱ�� ������ �ӽ�(���� ctrl)�� ���氡��
        m_isCrouchKey = Input.GetKey(KeyCode.Space);

        //������ ��ȣ�ۿ� Ű (������ ������)
        m_isActiveKeyDown = Input.GetKeyDown(KeyCode.E);
        if (m_isActiveKeyDown)
        {
            m_isActivating = true; //Ȱ��ȭ ��
        }
        else
        {
            // false�� OnTriggerStay ������ ������ ���
        }

        //ī�޶�(����) ����
        m_fMouseX = Input.GetAxis("Mouse X");
        m_fMouseY = Input.GetAxis("Mouse Y") * -1; //���� ȸ���� ���� ���� -���̴�. ���� axis�� ������ �����ش�.
    }

    // �̵� ó�� �Լ�
    void MoveProcess()
    {
        if (m_isUpKey)
        {
            //z�� ���� 1(����)
            m_vecMoveDirection.z = 1;

            // �޸���� �Ϲ������� ���� �������� �޸��� ����
            // ���̵峪 �ڷ� �����ٱ�� ���������� �޸���� �ָ��ϴ�
            // ��ũ���� ���������� �޸��� ���Ѵ�.
            if (m_isRunKey && m_fCurrentStamina > 0 && !m_isCrouchKey) //�޸��� Ű�� ������ ���罺�¹̳��� 0���� Ŭ��
            {
                //�޸���� ���ӵ� 3��
                m_fRunAcceleration = 3;
                m_isRuning = true;
            }
            else
            {
                //�޸��� ���� �ÿ��� ���ӵ� �⺻ 1
                m_fRunAcceleration = 1;
                m_isRuning = false;
            }
        }
        else if (m_isDownKey)
        {
            //z�� ���� -1 (��)
            m_vecMoveDirection.z = -1;
        }
        else
        {
            //z�� ���� 0 (�� �� �̵�X)
            m_vecMoveDirection.z = 0;
            //�޸��� ���� �ÿ��� ���ӵ� �⺻ 1
            m_fRunAcceleration = 1;
            m_isRuning = false;
        }


        if (m_isRightKey)
        {
            //x�� ���� 1(������)
            m_vecMoveDirection.x = 1;
        }
        else if (m_isLeftKey)
        {
            //x�� ���� -1(����)
            m_vecMoveDirection.x = -1;
        }
        else
        {
            //x�� ���� 0(�¿� �̵�X)
            m_vecMoveDirection.x = 0;
        }

        //�߳��� ����ĳ��Ʈ�� ���� ����� ������ �� 
        if (m_isCheckStairs && m_vecMoveDirection != Vector3.zero) 
        {
            // ���� ��� �տ� �׳� ���������� ���Ʒ��� �յնߴ� ���� �����ϱ�����
            // ���̷� ����� ������ �ǰ� �÷��̾ �����̰� ������ 
          
            m_vecMoveDirection.y = 1; //y�� �������ε� �����δ�.
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
            //���� �޸��� ������ ���¹̳� ����
            if(m_fCurrentStamina > 0)
            {
                m_fCurrentStamina -= Time.deltaTime*1.5f; // ���¹̳� ����: ���Ҵ� ȸ������ ������
            }
        }
        else
        {
            //���� �޸��� ���� ������ ���¹̳� ȸ��
            if(m_fCurrentStamina < m_fMaxStamina)
            {
                m_fCurrentStamina += Time.deltaTime; //
            }
        }
        // m_fCurrentStamina�� ��/�� �ϸ鼭 0 Ȥ�� m_fMaxStamina�� ����� �ʵ��� ����
        m_fCurrentStamina = Mathf.Clamp(m_fCurrentStamina, 0, m_fMaxStamina);
    }
    // ���� ���� ���¹̳� ������ ������ �Լ� -> PlayerUIManager���� ���
    public float GetCurrentStaminaRate()
    {
        return m_fCurrentStamina/m_fMaxStamina; // ���� ���¹̳� ���� ����
    }

    void SetAnimator()
    {
        if (m_animator != null)
        {
            //�ִϸ����� �Ķ���� ����
            m_animator.SetBool("isCrouch", m_isCrouchKey);
            m_animator.SetBool("isFoward", m_isUpKey);
            m_animator.SetBool("isBack", m_isDownKey);
            m_animator.SetBool("isRight", m_isRightKey);
            m_animator.SetBool("isLeft", m_isLeftKey);
            m_animator.SetBool("isRun", m_isRuning); //�޸��⸸ Ű �Է��� �ƴ� m_isRunning�̴�.
            if (m_isFoundTresure)
            {
                m_animator.Play("dancing"); //���� ã���� ���
            }
        }
        else
        {
            Debug.LogError("m_animator�� �����ϴ�.");
            m_animator = GetComponent<Animator>();
        }
    }

    //ī�޶�(����) ȸ�� 
    void CameraRotate()
    {
        if (m_camera != null)
        {
            if (m_objScreenRotation != null)
            {
                //���콺�� x�� ��ŭ rotation�� �����ش�. + ȸ������ �¿�ȸ���� y���̴�.
                m_vecScreenRotation.y = m_vecScreenRotation.y + m_fMouseX * m_fMouseSpeed;
                //���Ϸ� ���� �ٽ� ������� �����ؼ� rotation�� �־��ش�.
                m_objScreenRotation.transform.rotation = Quaternion.Euler(m_vecScreenRotation);
                //�÷��̾ �����Ͻ� m_objScreenRotation�� ȸ�������� �ٲپ��ش�.
                // ȭ�� ȸ���� ���� �÷��̾� ȸ�� ���� ����


                // �÷��̾ ������ �� ��ũ�� ȸ�������� ������ ȸ��
                float fPlayerRotateSpeed = 10f;
                Vector3 targetDirection = m_objScreenRotation.transform.forward; //m_objScreenRotation�� ��������� target
                if (m_vecMoveDirection != Vector3.zero)
                {
                    // �÷��̾��� ȸ���� �ε巴�� ����
                    //tagetDirection�� �ٶ󺸴� ȸ�� ����
                    //Quaternion.LookRotation�� �־��� ���� ���͸� �ٶ󺸴� ȸ���� �����ϴ� Unity�� �Լ���� �Ѵ�.
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    //Quaternion.Lerp �Լ��� �� ���� ȸ�� ���̸� ���� �����ϴ� �Լ��̴�.
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * fPlayerRotateSpeed);

                }
                else
                {

                }
            }
            else
            {
                Debug.LogError("m_objScreenRotation�� �����ϴ�.");
                m_objScreenRotation = GameObject.Find("ScreenRotation");
                m_vecScreenRotation = m_objScreenRotation.transform.localRotation.eulerAngles;
            }


            //���� ȸ�� ī�޶� �������� �ؾ��Ѵ�. -> �÷��̾�� �������ֱ⶧��//
            //���콺�� y�� ��ŭ m_camera�� �����ش�.
            m_vecCameraRotation.x = m_vecCameraRotation.x + m_fMouseY * m_fMouseSpeed;
            // ���� ���콺�� Clamp�� �ּ� �ִ밪�� �����Ͽ� 360���� ���� ���ϰ� �Ѵ�.
            m_vecCameraRotation.x = Mathf.Clamp(m_vecCameraRotation.x, -25, 5);
            //���Ϸ� ���� �ٽ� ������� �����ؼ� rotation�� �־��ش�.
            m_camera.transform.localRotation = Quaternion.Euler(m_vecCameraRotation);
        }
        else
        {
            Debug.LogError("m_camera�� ã�� �� �����ϴ�.");
            m_camera = GetComponent<Camera>();
            m_vecCameraRotation = m_camera.transform.localRotation.eulerAngles;
        }
    }

    //����ĳ��Ʈ�� �߳����� Ž���Ͽ� ����̸� ���� �ö󰡰� �ϴ� �޼ҵ�//
    void CheckStairs()
    {
        RaycastHit hitFront; // ���̿� ���� �ݶ��̴��� ������ �ޱ����� ����
        RaycastHit hitBack;
        RaycastHit hitRight;
        RaycastHit hitLeft;
        //y����ǥ�� �⺻���� �ش� ������Ʈ �߳��� ���� �Ǿ��ִ�.
        Vector3 vecOrigin = transform.position; //Ray�� origin vector
        float fMaxDistance = 0.5f; // �����ɽ�Ʈ ���� -> ������ �չ߰� �߽����� z�� ���̰� �����Ƿ� �����Ӱ� ���ش�.

        //��� ���̾ ���� �� �ֵ��� ����
        int nLayerMask = 1 << LayerMask.NameToLayer("Stairs");
        //���� ��ǥ(�߹ٴ�)���� ���� �������� 0.5��ŭ ���̸� ���.
        bool isCastFront = Physics.Raycast(vecOrigin, transform.forward, out hitFront, fMaxDistance, nLayerMask);
        // �� ����
        bool isCastBack = Physics.Raycast(vecOrigin, transform.forward * -1, out hitBack, fMaxDistance, nLayerMask);
        bool isCastRight = Physics.Raycast(vecOrigin, transform.right, out hitRight, fMaxDistance, nLayerMask); 
        bool isCastLeft = Physics.Raycast(vecOrigin,transform.right * -1, out hitLeft, fMaxDistance, nLayerMask);
       // Debug.DrawRay(vecOrigin, transform.forward * 0.5f, Color.red);
       // Debug.DrawRay(vecOrigin, transform.forward * -0.5f, Color.red);

        if (m_rigidBody != null)
        {
            if (isCastFront || isCastBack || isCastRight || isCastLeft)
            {
                //��ܿ� �¾����� �߷��� ��Ȱ��ȭ �ϰ� m_vecMoveDirection.y�� 1�� ����
                //(Addforce�� �ö󰡸� ƨ�ܳ����µ� ���ڿ�������.) Ȥ�� PlayerController�� ����ϸ� ���ϴٰ� �ϴµ�
                //                                              �߷°���� ���� �ؾ��Ѵ�.
                m_rigidBody.useGravity = false;
                m_rigidBody.velocity = Vector3.zero;
                //m_rigidBody.isKinematic = true; Ű��ƽ�� �����ϸ� ���� �����Ͻ� ���� �ʾƼ� ����

                m_isCheckStairs = true; // ���� �̵�ó���� MoveProcess���� �Ѵ�.
            }
            else
            {
                //���̰� �ƹ��͵� ���� �ʾ��� �ÿ��� false
                m_isCheckStairs = false;
                m_rigidBody.useGravity = true;
            }
        }
        else
        {
            Debug.LogError("m_rigidBody�� �����ϴ�.");
            m_rigidBody = GetComponent<Rigidbody>();
        }
    }

    //����� ����
    void SetAudio()
    {
        //�÷��̾ ���� ����ִٸ�
        if (m_isAlive)
        {
            if(m_vecMoveDirection != Vector3.zero)
            {
                //�̵����Ͻ�
                if (m_isRuning) //�޸��� ���� ��
                {
                    if(m_clipPlayerRun != null)
                    {
                        //���� ����� Ŭ���� �ش�������� �ʿ��� Ŭ���� �ƴҽ� �ѹ��� ����
                        // �ش������� ������ ������� ������ ���� ��� Play�ϱ⿡ ������ �����.
                        if (m_PlayerAudioSource.clip != m_clipPlayerRun)
                        {
                            m_PlayerAudioSource.clip = m_clipPlayerRun;
                            m_PlayerAudioSource.loop = true;
                            m_PlayerAudioSource.Play();
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        Debug.LogError("m_clipPlayerRun�� �����ϴ�.");
                    }
                }
                else //�Ȱ� ���� ��
                {
                    if(m_clipPlayerWalk != null)
                    {
                         if (m_PlayerAudioSource.clip != m_clipPlayerWalk)
                    {
                        //Debug.Log("walk");
                        m_PlayerAudioSource.Stop();
                        m_PlayerAudioSource.clip = m_clipPlayerWalk;
                        m_PlayerAudioSource.loop = true;
                        m_PlayerAudioSource.Play();
                    }
                    else
                    {
                    }
                    }
                    else
                    {
                        Debug.LogError("m_clipPlayerWalk�� �����ϴ�.");
                    }
                }
            }
            else
            {
                if(m_PlayerAudioSource.clip != null)
                {
                    //�̵����� �ƴҽ� ����� ���߰� �ʱ�ȭ
                    m_PlayerAudioSource.Stop();
                    //���߱⸸ �Ͽ��� �ٽ� ����� �ʿ�� Ŭ���� �Ҵ������� Ȥ�� �� ��Ȳ�� �ʱ�ȭ �Ѵ�. 
                    m_PlayerAudioSource.clip = null;
                    m_PlayerAudioSource.loop = false;
                }
                else
                {

                }
            }
        }
        else //�÷��̾� ����� �����
        {
            if (m_clipPlayerDie != null)
            {
                //���� ����� Ŭ���� �ش�������� �ʿ��� Ŭ���� �ƴҽ� �ѹ��� ����
                if (m_PlayerAudioSource.clip != m_clipPlayerDie)
                {
                    m_PlayerAudioSource.clip = m_clipPlayerDie;
                    //��� ����� loop�� false�� �Ѵ�.
                    m_PlayerAudioSource.loop = false;
                    m_PlayerAudioSource.Play();
                }
            }
            else
            {
                Debug.LogError("m_clipPlayerDie�� �����ϴ�.");
            }
        }

        //���� ������ ã�� �� ����� ���� ����ī�޶��� AudioSource�� �����Ѵ�.
        if (m_isFoundTresure)
        {
            if(m_clipDance != null)
            {
                //���� ī�޶� AudioSource ���� DanceŬ���� (BGM)�̱⶧��
                AudioSource CameraAudioSource = Camera.main.GetComponent<AudioSource>();
                //���� ����� Ŭ���� �ش�������� �ʿ��� Ŭ���� �ƴҽ� �ѹ��� ����
                if(CameraAudioSource.clip != m_clipDance)
                {
                    CameraAudioSource.Stop();//��� ����
                    CameraAudioSource.clip = m_clipDance;
                    CameraAudioSource.loop = false; 
                    CameraAudioSource.Play();
                }
                else
                {
                }
                // ������ ã���� �ƾ����� �̵��ϹǷ�, �ٽ� ���� BGM���� �ٲٴ� ������ �켱 �����Ͽ���.
            }
            else
            {
                Debug.LogError("m_clipDance�� �����ϴ�.");
            }
        }
        else
        {

        }
    }

    //�÷��̾� die
    void PlayerDie()
    {
        //isAlive false
        m_isAlive = false;
        //vecMoveDirection�� zero�� �Ͽ� �������� ����� ���� �ӵ������� �з����� �ʰ� �ϱ�����
        m_vecMoveDirection = Vector3.zero;
        m_animator.Play("die");
        // ���� ��ü �ݶ��̴��� ����Ͽ� �ִϸ��̼� ����� �ٴڿ� ����� �ְ� �Ѵ�.
        //���� root �ٸ��� �ݶ��̴��� �����־� ������ٵ�� ���� �ٴڶմ� ������ ����.
        GetComponentInChildren<BoxCollider>().enabled = false;
        m_csPlayerUIManager.SetGameOver();
    }

    //�÷��̾��� Rader Colider�� Ʈ���� ����(��,����,����)
    private void OnTriggerEnter(Collider other)
    {
        m_isActivating = false; //EŰ�� �̸� ������ ������ �� �����Ƿ� false�� �ʱ�ȭ ���ش�.
        string strPropsName = null;
        string strPropsDescription = null;
        if (other.CompareTag("Door"))
        {
            strPropsName = other.tag;
            bool isLocked = other.GetComponentInParent<DoorActive>().GetIsLocked();
            if (isLocked) //Door(Locked)�� �����ȴٸ�
            {
                //��乮�� ���踦 �����´�.
                GameObject objKey = other.GetComponentInParent<DoorActive>().GetKey();
                //�ش� ���谡 �����۸���Ʈ�� �ִ��� Ȯ��
                bool isCheckKey = GetComponent<ItemManager>().CheckItem(objKey);
                if (isCheckKey) // ���踦 ������ �ִٸ�
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
                //Door(�Ϲ�)�� �����ȴٸ�
                //������ Door�� �ֻ��� ������Ʈ�� ��ũ��Ʈ�� �ֱ⿡ InParent ���
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

            //TextUI����
            m_csPlayerUIManager.SetPropsMessage(strPropsName, strPropsDescription);
            m_csPlayerUIManager.SetActivePropsMessage(true);
        }
        else if (other.CompareTag("Item")) //�������϶�
        {
            strPropsName = other.name.ToString();
            //���� �κ��丮 â�� �߰��������� Ȯ��
            bool isAddItem = GetComponent<ItemManager>().GetIsAddItem();
            if (isAddItem)
            {
                strPropsDescription = "Press E to get";
            }
            else
            {
                strPropsDescription = "Inventory is full";
            }
            //TextUI����
            m_csPlayerUIManager.SetPropsMessage(strPropsName, strPropsDescription);
            m_csPlayerUIManager.SetActivePropsMessage(true);
        }
        else if (other.CompareTag("Chest"))
        { //�������� �϶�
            strPropsName = other.tag;
            //���ڰ� �����ִ��� Ȯ��
            bool isOpen = other.GetComponent<ChestActive>().GetIsOpen();
            //���� �κ��丮 â�� �߰��������� Ȯ��
            bool isAddItem = GetComponent<ItemManager>().GetIsAddItem();
            if (!isOpen)
            {
                //���ڰ� �����ִٸ�
                strPropsDescription = "Press E to open";
            }
            else
            {
                //���ڰ� �����ִٸ�
                if (isAddItem)
                {
                    strPropsDescription = "Press E to get";
                }
                else
                {
                    strPropsDescription = "Inventory is full";
                }
            }
           
            //TextUI����
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
            bool isLocked = other.GetComponentInParent<DoorActive>().GetIsLocked();
            // Ʈ���ſ� ������ �ݶ��̴� ������Ʈ �±װ� Door(�Ϲ�)�̶��
            if (m_isActivating) //��ȣ�ۿ� Ű�� �������� ture�� �ȴ�.
            {
                if (isLocked) //���������
                {
                    //��乮�� ���踦 �����´�.
                    GameObject objKey = other.GetComponentInParent<DoorActive>().GetKey();
                    //�ش� ���谡 �����۸���Ʈ�� �ִ��� Ȯ��
                    bool isCheckKey = GetComponent<ItemManager>().CheckItem(objKey);
                    if (isCheckKey)
                    {
                        //�����ۻ��
                        GetComponent<ItemManager>().UseItem(objKey);
                        //�������
                        other.GetComponentInParent<DoorActive>().UnLockDoor();
                        m_csPlayerUIManager.ClearInventory();
                        m_csPlayerUIManager.SetInventory();
                        //Door�� ���� �ִϸ��̼� �Ķ���� ���¸� �����´�.
                        //������ Door�� �ֻ��� ������Ʈ�� ��ũ��Ʈ�� �ֱ⿡ InParent ���
                        bool isAtive = other.GetComponentInParent<DoorActive>().GetIsActive();
                        //Door �ִϸſ����� �Ķ���͸� ����Ѵ�. (��������)->(����)
                        other.GetComponentInParent<DoorActive>().SetActiveAnimator(!isAtive);
                    }
                    else
                    {

                    }
                }
                else
                {
                    //Door�� ���� �ִϸ��̼� �Ķ���� ���¸� �����´�.
                    //������ Door�� �ֻ��� ������Ʈ�� ��ũ��Ʈ�� �ֱ⿡ InParent ���
                    bool isAtive = other.GetComponentInParent<DoorActive>().GetIsActive();
                    //Door �ִϸſ����� �Ķ���͸� ����Ѵ�. (��������)->(����)
                    other.GetComponentInParent<DoorActive>().SetActiveAnimator(!isAtive);
                    m_csPlayerUIManager.SetActivePropsMessage(false);
                }
            }
            else
            {
                m_isActivating = false; //Ȱ��ȭ ����(�������)
            }
            m_isActivating = false; //Ȱ��ȭ ����
        }
        else if (other.CompareTag("Item"))
        {
            //���� �κ��丮 â�� �߰��������� Ȯ��
            bool isAddItem = GetComponent<ItemManager>().GetIsAddItem();
            if (isAddItem && m_isActivating)
            {
                //Debug.Log("Ȯ��");
                GetComponent<ItemManager>().SetItem(other.gameObject);
                m_csPlayerUIManager.ClearInventory(); //�κ��丮 �ʱ�ȭ
                m_csPlayerUIManager.SetInventory();// ������Ʈ�� �κ��丮 ����
                m_csPlayerUIManager.SetPickUpAudio();// ����� ���
                m_csPlayerUIManager.SetActivePropsMessage(false);
            }
            else
            {

            }
            m_isActivating = false;
        }
        else if (other.CompareTag("Chest"))
        {
            bool isOpen = other.GetComponent<ChestActive>().GetIsOpen();
            if(isOpen && m_isActivating)
            {
                //���ڰ� �����ְ� ��ȣ�ۿ� Ű�� ��������
                //���� �κ��丮 â�� �߰��������� Ȯ��
                bool isAddItem = GetComponent<ItemManager>().GetIsAddItem();
                if (isAddItem)
                {
                    //Debug.Log("Ȯ��");
                    GetComponent<ItemManager>().SetItem(other.gameObject);
                    m_csPlayerUIManager.ClearInventory();
                    m_csPlayerUIManager.SetInventory();
                    m_csPlayerUIManager.SetPickUpAudio();// ����� ���
                    m_csPlayerUIManager.SetActivePropsMessage(false);
                    //���� ��� ȹ��� ���� �߰� �ƾ� ����
                    m_isFoundTresure = true;
                    m_csGameManager.LoadCutScene();
                }
                else
                {

                }
                m_isActivating = false;
            }
            else if(!isOpen && m_isActivating)
            {
                //���ڰ� �������� ������ ��ȣ�ۿ�Ű�� ������ ���ڸ� ����.
                other.GetComponent<ChestActive>().OpenChest();
                m_csPlayerUIManager.SetPropsMessage(other.tag, "Press E to get");
              //  m_csPlayerUIManager.SetActivePropsMessage(true);
                m_isActivating = false; // ������ �ٽ� Ȱ��ȭ Ű�� ��Ȱ��ȭ �ؾ��Ѵ�
                                        // ���ڸ��� ������ ȹ�����
            }
        }
        else
        {
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //���� Ʈ���ſ��� ��� �� UI SetActive�� false(�������� ��� �������� �����ش�.)
        m_csPlayerUIManager.SetActivePropsMessage(false); 
    }

    //�÷��̾�� �浹
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Monster"))
        {
            PlayerDie();
        }
        else
        {

        }
    }
}
