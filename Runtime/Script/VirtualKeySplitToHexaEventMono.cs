using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VirtualKeySplitToHexaEventMono : MonoBehaviour
{



    [Header("Last Push")]
    public string m_lastKeyAsString;
    public WindowsInput.Native.VirtualKeyCode m_lastKeyAsEnum;
    public int m_lastKeyIndex;
    public string m_lastKeyHexaDecimal;

    [Header("Event")]
    public UnityEvent<string> m_onVirtualKeyAsString;
    public UnityEvent<WindowsInput.Native.VirtualKeyCode> m_onVirtualKeyCodeAsEnum;
    public UnityEvent<int> m_onVirtualKeyCodeIndex;
    public UnityEvent<string> m_onVirtualKeyCodeHexaDecimal;

    public void PushIn(VirtualKeyboardHappened keyPushed) { 
    
        m_lastKeyAsString = keyPushed.m_key.ToString();
        m_lastKeyAsEnum = keyPushed.m_key;
        m_lastKeyIndex = (int)keyPushed.m_key;
        m_lastKeyHexaDecimal = ((int)keyPushed.m_key).ToString("X");


        m_onVirtualKeyAsString.Invoke(keyPushed.m_key.ToString());
        m_onVirtualKeyCodeAsEnum.Invoke(keyPushed.m_key);
        m_onVirtualKeyCodeIndex.Invoke((int)keyPushed.m_key);
        m_onVirtualKeyCodeHexaDecimal.Invoke(((int)keyPushed.m_key).ToString("X"));
    }

 }
