using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton
{
    static Singleton instance;
    public static Singleton GetInstance()
    {
        return instance;
    }

    bool m_isInputA = false;

    public bool GetInputA()
    {
        return m_isInputA;
    }

    public void SetInputA(bool isInputA)
    {
        m_isInputA = isInputA;
    }
}
