using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonControl : MonoBehaviour
{
    //���� AI�� ���� �̵� ��ũ��Ʈ ���� ���̴� �ݰ�� ���� ��ũ��Ʈ�� ���� �Ӹ��� ����

    // ������ �̵� �� ���� Ÿ�Լ���
    public enum E_SKELETON_TYPE
    {
        None,
        Patrol,//���� ���
        Trace,//�߰ݸ��
        Return, // ó����ġ�� ���ư���
        Max,
    }


    public E_SKELETON_TYPE m_eSkeletonType = E_SKELETON_TYPE.None;
    //E_SKELETON_TYPE m_ePriviousSkeletonType = E_SKELETON_TYPE.None;

    Vector3 m_vecStartPosition = Vector3.zero;
    float m_fMoveSpeed = 0;
    Vector3 m_vecMoveDirection = Vector3.zero;

    //MosterRoom���� ����� ������ ���߰� �ǵ��� ���� �ϱ�����
    Vector3 m_vecGateEntra = Vector3.zero;
    Vector3 m_vecGateExit = Vector3.zero;

    //��������϶� �¿�� �����Ѵ�.
    float m_fPatrolMaxX = 4;
    float m_fPatrolMinX = -4;
    bool m_isPatrolMaxX = false;
    bool m_isPatrolMinX = false;

    //������ �ڷ� �� ��
    bool m_isTurnning = false;
    Vector3 m_vecTurnBack = Vector3.zero;

    // Trace ����Ͻ� navigation ���
    NavMeshAgent m_navmeshAgent = null;
    GameObject m_objPlayer = null;


    //�ִϸ��̼� ����
    Animator m_animator = null;
    bool m_isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        if (m_eSkeletonType == E_SKELETON_TYPE.None)
        {
            Debug.LogError("Skeleton�� ������ ���Ÿ���� �������ּ���");
        }

        m_vecStartPosition = transform.position;
        m_vecMoveDirection = Vector3.forward;
        m_vecTurnBack = transform.forward * -1;

        // m_vecGateEntra = GameObject.Find("MonsterRoomEntraGate").transform.position;
        //m_vecGateExit = GameObject.Find("MonsterRoomExitGate").transform.position;

        if(m_navmeshAgent == null)
        {
            m_navmeshAgent = GetComponent<NavMeshAgent>();
        }

        m_objPlayer = GameObject.Find("Player");

        if(m_animator == null)
        {
            m_animator = GetComponent<Animator>();
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_eSkeletonType == E_SKELETON_TYPE.Patrol)
        {
            PatrolMode();
        }
        else if(m_eSkeletonType == E_SKELETON_TYPE.Trace)
        {
            TraceMode();
        }
    }

    private void LateUpdate()
    {
        SetAnimator();
    }

    private void FixedUpdate()
    {
        transform.Translate(m_vecMoveDirection * m_fMoveSpeed);
    }

    //������� �Լ�
    void PatrolMode()
    {
        Vector3 vecCurrentPos = transform.position;

        if (m_isTurnning == false)
        {
            //�������϶����� �ȱ�
            m_fMoveSpeed = 0.01f; //ȸ������ �ƴ� ������ �ȴ´�.
            // ���� �Ÿ��� �����ϸ� ������ �ٲپ��ش�.
            if (vecCurrentPos.x >= m_fPatrolMaxX && m_isPatrolMaxX == false)
            {
                m_vecTurnBack = transform.forward * -1;
                m_isPatrolMaxX = true;
                m_isPatrolMinX = false;
                m_isTurnning = true;
            }
            else if (vecCurrentPos.x <= m_fPatrolMinX && m_isPatrolMinX == false)
            {
                m_vecTurnBack = transform.forward * -1;
                m_isPatrolMaxX = false;
                m_isPatrolMinX = true;
                m_isTurnning = true;
            }
            else
            {
            }

        }
        else if (m_isTurnning == true)
        {
            //ȸ���ÿ��� �����.
            m_fMoveSpeed = 0;
            int nTrunSpeed = 5;
            //��������ȸ���ϱ�����
            //PlayerControl�� CameraRotate ó�� ������� LookRotation�Լ��� �ش� ��ǥ�� �ٶ󺸴� ȸ������ �޾ƿ´�.
            Quaternion qTurnRotation = Quaternion.LookRotation(m_vecTurnBack);
            //Quaternion�� Lerp�Լ��� ���Ͽ� ���������ϸ� ȸ��
            transform.rotation = Quaternion.Lerp(transform.rotation, qTurnRotation, Time.deltaTime * nTrunSpeed);
            //Debug.Log(transform.rotation.ToString());
            // qTurnRotation �� ���� rotation�� ���Ϸ��� y�� ���̰� 5���϶�� ��ǥ �� �������� �ٲپ��ش�.
            // -> ������ 180�� ������ m_isTurnning�� flase�� �ٲپ� �ٽ� ������ �����̰� �Ѵ�.
            float fDiffRotate = Mathf.Abs(transform.rotation.eulerAngles.y - qTurnRotation.eulerAngles.y);
            if (fDiffRotate <= 5f)
            {
                transform.rotation = Quaternion.LookRotation(m_vecTurnBack);
                m_isTurnning = false;
            }
        }

        // ���̴��� �÷��̾� ����
        // �ڽĿ�����Ʈ head�� �޸� SkeletonRader�� Trigger���� �ִ� �÷��̾ ���� ���̸� ���
        // ������ �߰ݸ��� ����
        bool isLookPlayer = GetComponentInChildren<SkeletonRader>().IsDetectPlayer();
        if (isLookPlayer)
        {
            m_fMoveSpeed = 0;
            m_eSkeletonType = E_SKELETON_TYPE.Trace;
        }
        else
        {

        }

    }

    //�߰ݸ�� �Լ�
    void TraceMode()
    {
        if(m_navmeshAgent != null)
        {
            m_isRunning = true;
            Vector3 vecPlayerPos = m_objPlayer.transform.position;
            //m_navmeshAgent �������� �÷��̾�� ����
            m_navmeshAgent.SetDestination(vecPlayerPos);
            //Debug.Log(vecPlayerPos);
        }
        else
        {
            Debug.LogError("m_navmeshAgent�� �����ϴ�.");
        }
    }

    void SetAnimator()
    {
        if(m_animator != null)
        {
            m_animator.SetBool("isRun", m_isRunning);
        }
        else
        {
            Debug.LogError("m_animator�� �����ϴ�.");
        }
    }
}
