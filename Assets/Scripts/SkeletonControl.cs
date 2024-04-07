using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonControl : MonoBehaviour
{
    //몬스터 AI에 따른 이동 스크립트 감지 레이더 반경과 관련 스크립트는 몬스터 머리에 부착

    // 몬스터의 이동 및 상태 타입설정
    public enum E_SKELETON_TYPE
    {
        None,
        Patrol,//순찰 모드
        Attack,//공격모드
        Return, // 처음위치로 돌아가기
        Max,
    }


    public E_SKELETON_TYPE m_eSkeletonType = E_SKELETON_TYPE.None;
    //E_SKELETON_TYPE m_ePriviousSkeletonType = E_SKELETON_TYPE.None;

    Vector3 m_vecStartPosition = Vector3.zero;
    float m_fMoveSpeed = 0;
    Vector3 m_vecMoveDirection = Vector3.zero;

    //MosterRoom에서 벗어날시 공격을 멈추고 되돌아 가게 하기위해
    Vector3 m_vecGateEntra = Vector3.zero;
    Vector3 m_vecGateExit = Vector3.zero;

    //순찰모드일때 좌우로 순찰한다.
    float m_fPatrolMaxX = 4;
    float m_fPatrolMinX = -4;
    bool m_isPatrolMaxX = false;
    bool m_isPatrolMinX = false;

    //정찰시 뒤로 돌 때
    bool m_isTurnning = false;
    Vector3 m_vecTurnBack = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        if (m_eSkeletonType == E_SKELETON_TYPE.None)
        {
            Debug.LogError("Skeleton의 열거형 상수타입을 지정해주세요");
        }

        m_vecStartPosition = transform.position;
        m_vecMoveDirection = Vector3.forward;
        m_vecTurnBack = transform.forward * -1;

        // m_vecGateEntra = GameObject.Find("MonsterRoomEntraGate").transform.position;
        //m_vecGateExit = GameObject.Find("MonsterRoomExitGate").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_eSkeletonType == E_SKELETON_TYPE.Patrol)
        {
            PatrolMode();
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(m_vecMoveDirection * m_fMoveSpeed);
    }

    //순찰모드 함수
    void PatrolMode()
    {
        Vector3 vecCurrentPos = transform.position;

        if (m_isTurnning == false)
        {
            //순찰중일때에는 걷기
            m_fMoveSpeed = 0.01f; //회전중이 아닐 때에만 걷는다.
            // 순찰 거리에 도달하면 방향을 바꾸어준다.
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
            //회전시에는 멈춘다.
            m_fMoveSpeed = 0;
            int nTrunSpeed = 5;
            //선형보간회전하기위해
            //PlayerControl의 CameraRotate 처럼 사원수의 LookRotation함수로 해당 좌표를 바라보는 회전값을 받아온다.
            Quaternion qTurnRotation = Quaternion.LookRotation(m_vecTurnBack);
            //Quaternion의 Lerp함수를 통하여 선형보간하며 회전
            transform.rotation = Quaternion.Lerp(transform.rotation, qTurnRotation, Time.deltaTime * nTrunSpeed);
            //Debug.Log(transform.rotation.ToString());
            // qTurnRotation 과 현재 rotation의 오일러각 y축 차이가 5이하라면 목표 뒤 방향으로 바꾸어준다.
            // -> 완전히 180로 돌리고 m_isTurnning을 flase로 바꾸어 다시 앞으로 움직이게 한다.
            float fDiffRotate = Mathf.Abs(transform.rotation.eulerAngles.y - qTurnRotation.eulerAngles.y);
            if (fDiffRotate <= 5f)
            {
                transform.rotation = Quaternion.LookRotation(m_vecTurnBack);
                m_isTurnning = false;
            }
        }

    }

}
