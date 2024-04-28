using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    //오래된 전구가 지지직? 점등하는 인터렉션 구현//

    Light m_light = null;
    //light의 강도 intensity값
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
        // 4초에 한번씩 점등 코루틴 호출
        if(m_fTime >= fCallTime)
        {
            m_fTime = 0;
            StartCoroutine(LightSparkle());
        }
    }

    IEnumerator LightSparkle()
    {
        GetComponent<AudioSource>().Play();
        //m_light의 강도을 0/2로 점등
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
