using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using WindowsInput.Native;
using WindowsInput;
using System.Linq;
using System;
using System.Threading.Tasks;
using UnityEngine.Events;
using JetBrains.Annotations;

public class ThreadWindowToVirtualkeyEventMono : MonoBehaviour
{

    public Queue<VirtualKeyboardHappened> m_queue = new Queue<VirtualKeyboardHappened>();

    public Action<VirtualKeyboardHappened> m_onKeyChangedSideThread;
    public UnityEvent<VirtualKeyboardHappened> m_onKeyPressChangedUnityThread;

    public VirtualKeyboardHappened m_lastReceivedKey;

    public void Update()
    {
        while (m_queue.Count > 0)
        {
            VirtualKeyboardHappened frame = m_queue.Dequeue();
            m_onKeyPressChangedUnityThread.Invoke(frame);
        }
    }

    private void NotifyKeyChanged(VirtualKeyboardHappened happened)
    {
        m_onKeyChangedSideThread?.Invoke(happened);
        m_queue.Enqueue(happened);
        m_lastReceivedKey= happened;
    }
    KillSwitch m_currentThreadKillSwitch;

    public int m_msBetweenReading = 5;
    public System.Threading.ThreadPriority m_threadPriority;
    // Start is called before the first frame update
    void OnEnable()
    {
        if(m_currentThreadKillSwitch != null) {
            m_currentThreadKillSwitch.Kill();
        }
        m_currentThreadKillSwitch = new KillSwitch();
        m_currentThread = new VirtualInputThread();
        m_currentThread.m_onKeyboardChanged += NotifyKeyChanged;

        Thread t = new Thread(ThreadStart => m_currentThread.Listen(m_currentThreadKillSwitch, m_msBetweenReading));
        t.Priority = m_threadPriority;
        t.Start();

    }
    public VirtualInputThread m_currentThread;
    

    // Update is called once per frame
    void OnDisable()
    {
        m_currentThreadKillSwitch.Kill();
    }
    private void OnDestroy()
    {
        m_currentThreadKillSwitch.Kill();
    }

    [System.Serializable]
    public class KillSwitch
    {
        public bool m_kill = false;

        public void Kill()
        {
            m_kill = true;
        }
    }

    [System.Serializable]
    public class VirtualInputThread {

       public Dictionary<VirtualKeyCode, bool> m_winKey = new Dictionary<VirtualKeyCode, bool>();
       public List<VirtualKeyCode> m_current = new List<VirtualKeyCode>();
       public List<VirtualKeyCode> m_previous = new List<VirtualKeyCode>();
       public List<VirtualKeyCode> m_added = new List<VirtualKeyCode>();
       public List<VirtualKeyCode> m_removed = new List<VirtualKeyCode>();

        public Action<VirtualKeyboardHappened> m_onKeyboardChanged;

        public void Listen(KillSwitch killSwitch, int delayBetweenKeyMs) {
            m_winKey= new Dictionary<VirtualKeyCode, bool>(); 

            List<VirtualKeyCode> vkList = KeystrokeUtility.GetEnumList<VirtualKeyCode>();
            foreach (VirtualKeyCode vk in vkList) {
                if (!m_winKey.ContainsKey(vk))
                      m_winKey.Add(vk, false);
            }
            InputSimulator winKey = new InputSimulator();

            while (!killSwitch.m_kill)
            {
                foreach (VirtualKeyCode vk in vkList)
                {
                    if (winKey.InputDeviceState.IsKeyDown(vk))
                    {
                        m_winKey[vk] = true;
                    }
                    if (winKey.InputDeviceState.IsKeyUp(vk))
                    {
                        m_winKey[vk] = false;
                    }
                }
                m_previous = m_current.ToList() ;
                m_current = m_winKey.Keys.Where(k => m_winKey[k]).ToList();
                m_added = m_current.Except(m_previous).ToList();
                m_removed = m_previous.Except(m_current).ToList();

                if (m_onKeyboardChanged != null)
                { 
                    foreach(VirtualKeyCode vk in m_added) {
                        m_onKeyboardChanged.Invoke(new VirtualKeyboardHappened() { 
                            m_dateTimeNowUtcTicks = DateTime.UtcNow.Ticks,
                            m_isDown = true,
                            m_key = vk });
                    }
                    foreach(VirtualKeyCode vk in m_removed) {
                        m_onKeyboardChanged.Invoke(new VirtualKeyboardHappened()
                        {
                            m_dateTimeNowUtcTicks = DateTime.UtcNow.Ticks,
                            m_isDown = false,
                            m_key = vk
                        });
                    }
                }
                
                Thread.Sleep(delayBetweenKeyMs);
            }
        
        }
    }
}

[System.Serializable]
public struct VirtualKeyboardHappened {

    public long m_dateTimeNowUtcTicks;
    public VirtualKeyCode m_key;
    public bool m_isDown;
}