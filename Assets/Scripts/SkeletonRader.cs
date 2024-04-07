using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonRader : MonoBehaviour
{
    /*
     * ArmedSkeleton의 PT_Male_Armor_Skeleton_01_head(머리)에 추가한 스크립트
     * 스켈레톤의 Head 좌표에서 Trigger 콜라이더를 통해 플레이어를 감지하고 감지가 되었다면 플레이어 몸통방향으로 
        레이캐스트를 쏘아 앞에 엄폐물이 있다면 그대로, 플레이어의 Body가 검출된다면 SkeletonControl의 타입을 Attack으로 바꾸어준다.
        위와같은 방법으로 플레이어가 엄폐물뒤에 웅크려 가려져 있다면 레이더에 걸리지않는다.
     */

    float m_fAroundRotate = 0;
    // 회전방향 1/-1로 토글
    float m_fAroundDirectionY = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_fAroundDirectionY = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
       
    }

    private void LateUpdate()
    {
        LookAround();
    }

    //주위를 둘러보는 함수
    void LookAround()
    {
        float fAroundSpeed = 0.5f;
        transform.Rotate(fAroundSpeed * m_fAroundDirectionY,0, 0);
        m_fAroundRotate += fAroundSpeed * m_fAroundDirectionY;
        //80도 이하로만 머리를 회전시킬수 있도록 회전
        m_fAroundRotate = Mathf.Clamp(m_fAroundRotate, -80, 80);
        //만약 80도 이상이 되었다면
        if(Mathf.Abs(m_fAroundRotate) >= 80)
        {
            m_fAroundDirectionY *= -1;//방향을 바꾸어준다.
        }

    }
}
