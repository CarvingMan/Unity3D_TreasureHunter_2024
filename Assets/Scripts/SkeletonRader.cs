using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonRader : MonoBehaviour
{
    /*
     * ArmedSkeleton�� PT_Male_Armor_Skeleton_01_head(�Ӹ�)�� �߰��� ��ũ��Ʈ
     * ���̷����� Head ��ǥ���� Trigger �ݶ��̴��� ���� �÷��̾ �����ϰ� ������ �Ǿ��ٸ� �÷��̾� ����������� 
        ����ĳ��Ʈ�� ��� �տ� ������ �ִٸ� �״��, �÷��̾��� Body�� ����ȴٸ� SkeletonControl�� Ÿ���� Attack���� �ٲپ��ش�.
        ���Ͱ��� ������� �÷��̾ ���󹰵ڿ� ��ũ�� ������ �ִٸ� ���̴��� �ɸ����ʴ´�.
     */

    float m_fAroundRotate = 0;
    // ȸ������ 1/-1�� ���
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

    //������ �ѷ����� �Լ�
    void LookAround()
    {
        float fAroundSpeed = 0.5f;
        transform.Rotate(fAroundSpeed * m_fAroundDirectionY,0, 0);
        m_fAroundRotate += fAroundSpeed * m_fAroundDirectionY;
        //80�� ���Ϸθ� �Ӹ��� ȸ����ų�� �ֵ��� ȸ��
        m_fAroundRotate = Mathf.Clamp(m_fAroundRotate, -80, 80);
        //���� 80�� �̻��� �Ǿ��ٸ�
        if(Mathf.Abs(m_fAroundRotate) >= 80)
        {
            m_fAroundDirectionY *= -1;//������ �ٲپ��ش�.
        }

    }
}
