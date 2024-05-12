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

    //audio ����
    //���̷��� PT_Spine�� Head�� �޷��ִ� ����� �ҽ� 
    AudioSource m_HeadAudioSource = null;
    [SerializeField]
    AudioClip m_clipSkeletonRoar = null;
    [SerializeField]
    AudioClip m_clipSkeletonAttack = null;

    private void Start()
    {
        if(m_HeadAudioSource == null)
        {
            m_HeadAudioSource=GetComponent<AudioSource>();
        }
        else
        {

        }
    }

    public bool IsDetectPlayer()
    {
        if (m_isInPlayer)
        {
            RaycastHit hit;
            float fDistance = 6f;
            //����Ʈ ���� �Ͽ� ������ ���̾ �ش��ϴ� ��Ʈ�� 1���ϰ� ��Ʈ or�����ڸ� ���� ���̾��ũ ����
            //�̸� ���� �ش� ���̾ ����� ������Ʈ�� ����ĳ��Ʈ�� �����.(���󹰰� �÷��̾� ����)
            //->���󹰿��� navigation���̾�� ����: NavMesh Surface���� navigation ���̾� ����ϱ� ����
            int nHitLayer = (1 << LayerMask.NameToLayer("PlayerBody")) | (1 << LayerMask.NameToLayer("Navigation"));
            
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
                    //�����Ҹ� ���
                    SetAudioRoar();
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

    //IsDetectPlayer()���� �÷��̾� �� ���������� ���̷����� �����Ҹ�(Roar) ���
    void SetAudioRoar()
    {
        //clip�� m_clipSkeletonRoar�� �ƴҽ� ����� ����
        if(m_HeadAudioSource.clip != m_clipSkeletonRoar)
        {
            //����� Ŭ�� ����/ ����false/ Play
            m_HeadAudioSource.clip = m_clipSkeletonRoar;
            m_HeadAudioSource.loop = false;
            m_HeadAudioSource.Play();
            //����� Ŭ�� �ð�
            float fClipTime = m_clipSkeletonRoar.length;
            //Ŭ���ð� ���� �ʱ�ȭ
            StartCoroutine(CorAudioInit(fClipTime));
        }
        else
        {
        }
    }

    //���ݼҸ� Ŭ�� ��� // ȣ���� SkeletonControl���� �ϱ⿡ pubic
    public void SetAttackAudio()
    {
        if (m_clipSkeletonAttack != null)
        {
            if (m_HeadAudioSource.clip != m_clipSkeletonAttack)
            {
                m_HeadAudioSource.clip = m_clipSkeletonAttack;
                m_HeadAudioSource.loop = false;
                m_HeadAudioSource.Play();
                //����� Ŭ�� �ð�
                float fClipTime = m_clipSkeletonAttack.length;
                //Ŭ���ð� ���� �ʱ�ȭ
                StartCoroutine(CorAudioInit(fClipTime));
            }
            else
            {
            }
        }
        else
        {
            Debug.LogError("m_clipSkeletonAttack�� �����ϴ�.");
        }
    }

    //������ҽ� �ʱ�ȭ �Լ�
    IEnumerator CorAudioInit(float fClipTime)
    {
        //fClipTime���� ����� �ҽ� �ʱ�ȭ
        yield return new WaitForSeconds(fClipTime);
        m_HeadAudioSource.Stop();
        m_HeadAudioSource.loop = false;
        m_HeadAudioSource.clip = null;
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
