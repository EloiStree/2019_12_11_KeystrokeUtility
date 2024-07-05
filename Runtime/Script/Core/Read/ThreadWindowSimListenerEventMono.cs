//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;

//public class ThreadWindowSimListenerEventMono : MonoBehaviour
//{

//    public ThreadWindowSimListenerMono m_source;
//    public UnityEvent<KeyboardTouch, bool> m_onKeyPressChanged;
//    public Queue<KeyboardTouchFrame> m_queue = new Queue<KeyboardTouchFrame>();

//    public KeyboardTouch m_lastReceivedKey;
//    public bool m_lastReceivedKeyState;
//    public void OnEnable()
//    {
//        if (m_source != null)
//        {
//            m_source.m_onKeyboardPressChangedThreaded += PushInQueue; 
//        }
//    }
//    private void PushInQueue(KeyboardTouch touch, bool arg2)
//    {
//        m_queue.Enqueue(new KeyboardTouchFrame() { m_isDown = arg2, m_key = touch });
//    }
//    public void Update()
//    {
//        while (m_queue.Count > 0)
//        {
//            KeyboardTouchFrame frame = m_queue.Dequeue();
//            m_lastReceivedKey = frame.m_key;
//            m_lastReceivedKeyState = frame.m_isDown;
//            m_onKeyPressChanged.Invoke(frame.m_key, frame.m_isDown);
//        }
//    }

//    [System.Serializable]
//    public struct KeyboardTouchFrame
//    {
//        public bool m_isDown;
//        public KeyboardTouch m_key;
//    }
    
//}
