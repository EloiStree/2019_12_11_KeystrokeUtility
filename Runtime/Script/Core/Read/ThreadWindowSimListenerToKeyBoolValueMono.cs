using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using WindowsInput.Native;

public class ThreadWindowSimListenerToKeyBoolValueMono : MonoBehaviour
{
    public ThreadWindowSimListenerMono m_source;

    static void DetectChanges(VirtualKeyCode[] oldArray, VirtualKeyCode[] newArray, 
        out List<VirtualKeyCode> newValues, out List<VirtualKeyCode> lostValues)
    {
        HashSet<VirtualKeyCode> oldSet = new HashSet<VirtualKeyCode>(oldArray);
        HashSet<VirtualKeyCode> newSet = new HashSet<VirtualKeyCode>(newArray);

        // Values present in the new array but not in the old array are "new" values
        newValues = newSet.Except(oldSet).ToList();

        // Values present in the old array but not in the new array are "lost" values
        lostValues = oldSet.Except(newSet).ToList();

    }

    public string m_startValue="Keyboard";
    public string m_startSplitter="_";
    public VirtualKeyCode[] oldArray;
    public VirtualKeyCode[] newArray;
    public List<VirtualKeyCode> newValues;
    public List<VirtualKeyCode> lostValues;
    public UnityEvent<string, bool> m_onChangedKeyValue;
    void Update()
    {
        oldArray = newArray;
        newArray = m_source.GetWindowKey().ToArray();
        DetectChanges(oldArray, newArray, out newValues, out lostValues);
        foreach (var item in newValues)
        {
            PushChanged(item, true);
        }
        foreach (var item in lostValues)
        {
            PushChanged(item, false);
        }
    }

    private void PushChanged(VirtualKeyCode item, bool isTrue)
    {
        m_onChangedKeyValue.Invoke($"{m_startValue}{m_startSplitter}{item.ToString()}", isTrue);
    }
}
