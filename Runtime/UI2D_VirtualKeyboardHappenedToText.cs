using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI2D_VirtualKeyboardHappenedToText : MonoBehaviour
{

    public string m_format = "{0} {1} {2}";

    public UnityEvent<string> m_onRelayed;
    public void PushIn(VirtualKeyboardHappened happened)
    {

        string t = string.Format(m_format, happened.m_key, happened.m_isDown, happened.m_dateTimeNowUtcTicks);
        m_onRelayed.Invoke(t);
    }
}
