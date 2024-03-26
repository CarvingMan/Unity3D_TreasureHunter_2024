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
    float m_fMouseSpeed = 8;

    //������ٵ� ���� �������
    Rigidbody m_rigidBody = null;

    //����ĳ��Ʈ�� ���� ����� �����ɶ�
    bool m_isCheckStairs = false;


    //�ִϸ��̼� ���� �������//
    Animator m_animator = null;



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
    }

    // Update is called once per frame
    void Update()
    {
        InputProcess();
        CheckStairs();
        MoveProcess();
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
            if (m_isRunKey)
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

    void SetAnimator()
    {
        if (m_animator != null)
        {
            //�ִϸ����� �Ķ���� ����
            m_animator.SetBool("isFoward", m_isUpKey);
            m_animator.SetBool("isBack", m_isDownKey);
            m_animator.SetBool("isRight", m_isRightKey);
            m_animator.SetBool("isLeft", m_isLeftKey);
            m_animator.SetBool("isRun", m_isRuning); //�޸��⸸ Ű �Է��� �ƴ� m_isRunning�̴�.
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
                float fPlayerRotateSpeed = 1; //���ص� �����ؼ� ���� 1.2~ ó�� �׽�Ʈ 
                Vector3 targetDirection = m_objScreenRotation.transform.forward; //m_objScreenRotation�� ��������� target
                if (m_vecMoveDirection != Vector3.zero)
                {
                    // �÷��̾��� ȸ���� �ε巴�� ����
                    //tagetDirection�� �ٶ󺸴� ȸ�� ����
                    //Quaternion.LookRotation�� �־��� ���� ���͸� �ٶ󺸴� ȸ���� �����ϴ� Unity�� �Լ���� �Ѵ�.
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    //Quaternion.Lerp �Լ��� �� ���� ȸ�� ���̸� ���� �����ϴ� �Լ��̴�.
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.2f * fPlayerRotateSpeed);

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
        RaycastHit hit; // ���̿� ���� �ݶ��̴��� ������ �ޱ����� ����
        //y����ǥ�� �⺻���� �ش� ������Ʈ �߳��� ���� �Ǿ��ִ�.
        Vector3 vecOrigin = transform.position; //Ray�� origin vector
        float fMaxDistance = 0.5f; // �����ɽ�Ʈ ���� -> ������ �չ߰� �߽����� z�� ���̰� �����Ƿ� �����Ӱ� ���ش�.

        //���� ��ǥ(�߹ٴ�)���� ���� �������� 0.5��ŭ ���̸� ���.
        bool isCast = Physics.Raycast(vecOrigin, transform.forward, out hit, fMaxDistance);
        Debug.DrawRay(vecOrigin, transform.forward * 0.5f, Color.red);

        if (m_rigidBody != null)
        {
            if (isCast)
            {
                if (hit.collider.CompareTag("Stairs"))
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
                    m_isCheckStairs = false;
                    m_rigidBody.useGravity = true;

                }

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
}
