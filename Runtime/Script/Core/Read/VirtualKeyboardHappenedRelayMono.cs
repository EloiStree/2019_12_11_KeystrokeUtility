using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VirtualKeyboardHappenedRelayMono : MonoBehaviour
{

    public VirtualKeyboardHappened m_lastRelayed;
    public UnityEvent<VirtualKeyboardHappened> m_onRelayed;
    public void PushIn(VirtualKeyboardHappened happened)
    {
        m_lastRelayed = happened;   
        m_onRelayed.Invoke(happened);
    }

}
