using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    //������ ������ ������? �����ϴ� ���ͷ��� ����//

    Light m_light = null;
    //light�� ���� intensity��
    float m_fMaxIntensity = 2;
    float m_fMinIntensity = 0;
    float m_fTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(m_light == null)
        {
            m_light = GetComponent<Light>();
        }
        StartCoroutine(LightSparkle());
    }

    private void Update()
    {
        m_fTime += Time.deltaTime;
        float fCallTime = 4;
        // 4�ʿ� �ѹ��� ���� �ڷ�ƾ ȣ��
        if(m_fTime >= fCallTime)
        {
            m_fTime = 0;
            StartCoroutine(LightSparkle());
        }
    }

    IEnumerator LightSparkle()
    {
        GetComponent<AudioSource>().Play();
        //m_light�� ������ 0/2�� ����
        m_light.intensity = m_fMinIntensity;
        yield return new WaitForSeconds(0.1f);
        m_light.intensity = m_fMaxIntensity;
        yield return new WaitForSeconds(0.1f);
        m_light.intensity = m_fMinIntensity;
        yield return new WaitForSeconds(0.1f);
        m_light.intensity = m_fMaxIntensity;
        yield return new WaitForSeconds(0.5f);
        m_light.intensity = m_fMinIntensity;
        yield return new WaitForSeconds(0.1f);
        m_light.intensity = m_fMaxIntensity;
        yield return new WaitForSeconds(0.8f);
        m_light.intensity = m_fMinIntensity;
        yield return new WaitForSeconds(0.1f);
        m_light.intensity = m_fMaxIntensity;
    }
}
