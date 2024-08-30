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

        m_touchKeyJoin = string.Join(", " ,m_threadListener.GetTouchActiveAsCopy());
        m_windowKeyJoin = string.Join(", ", m_threadListener.GetWindowKeyAsCopy());

    
        m_onTouchKey.Invoke(m_touchKeyJoin);
        m_onWindowKey.Invoke(m_windowKeyJoin);


        
    }
}
