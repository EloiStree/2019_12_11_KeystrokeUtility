using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DebugThreadWindowSimListenerMono:MonoBehaviour
{

    public ThreadWindowSimListenerMono m_threadListener;
    public string m_touchKeyJoin;
    public string m_windowKeyJoin;

    public UnityEvent<string> m_onTouchKey;
    public UnityEvent<string> m_onWindowKey;



    void Update()
    {
        string touch = string.Join(", ", m_threadListener.GetTouchActiveAsCopy());
        if (touch != m_touchKeyJoin) { 
            m_touchKeyJoin = touch;
            m_onTouchKey.Invoke(touch);
        }
        string window = string.Join(", ", m_threadListener.GetWindowKeyAsCopy());
        if (window != m_windowKeyJoin)
        {
            m_windowKeyJoin = window;
            m_onWindowKey.Invoke(window);
        }
    }
}
