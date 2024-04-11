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

    //�÷��̾ ���� �þ߿� �ִ��� Ȯ��
    bool m_isInPlayer = false; 

    //�÷��̾� ������ Vector
    Vector3 m_vecPlayerBody = Vector3.zero;

  

    public bool IsDetectPlayer()
    {
        if (m_isInPlayer)
        {
            RaycastHit hit;
            float fDistance = 5.0f;
            //����Ʈ ���� �Ͽ� ������ ���̾ �ش��ϴ� ��Ʈ�� 1���ϰ� ��Ʈ or�����ڸ� ���� ���̾��ũ ����
            //�̸� ���� �ش� ���̾ ����� ������Ʈ�� ����ĳ��Ʈ�� �����.(���󹰰� �÷��̾� ����)
            int nHitLayer = (1 << LayerMask.NameToLayer("PlayerBody")) | (1 << LayerMask.NameToLayer("Obstacle"));
            
            // �÷��̾�body position(����)���� transform.position(����)�� �� �� normalized�� ���Ͽ�
            // ũ�Ⱑ 1�� ���� ���͸� ���Ͽ� �ش�������� ���̸� ���.
            Vector3 vecDirectionToPlayer = (m_vecPlayerBody - transform.position).normalized;
            bool isCast =  Physics.Raycast(transform.position, vecDirectionToPlayer, out hit, fDistance, nHitLayer);
            //Debug.DrawRay(transform.position, vecDirectionToPlayer * 5, Color.red);
            if (isCast)
            {
                //���� ���� �ڿ� ������ Obstacle �±װ� ���� �������̴�.
                if (hit.collider.CompareTag("PlayerBody"))
                {
                    //Debug.Log("�÷��̾� ����");
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false; //���� �������̴� �ȿ� �÷��̾ ������ ����ĳ��Ʈ�� ���� �ʴ´�.
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerBody"))
        {
           m_vecPlayerBody = other.transform.position; // �÷��̾� ���� ��ǥ
           m_isInPlayer = true; //�÷��̾ ���̴� �ȿ� �ִ�(�߰�X)
        }
        else
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerBody"))
        {
            m_isInPlayer = false;
        }
    }

}
