using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager{
    
    
    
    
    public static void SetActive(GameObject item,bool active)
    {
        item.SetActive(active);
    }
    public static void AddEventToAButton(GameObject item,LuaFunction func)
    {
        
        item.GetComponentInChildren<Button>().onClick.AddListener(func.Call);
    }
}
