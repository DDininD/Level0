using UnityEditor;
using UnityEngine;

public class Main : MonoBehaviour
{
    public GameObject Win;
    public GameObject Lose;
    public GameObject GroundType1;
    public GameObject GroundType2;
    public GameObject GroundType3;
    public GameObject Flag;
    public GameObject BarMask;
    public int MapLength = 10;
    
    void Awake () {
        LuaUtil.ToLua("Main");
        LuaUtil.CallFunc("Init",Win,Lose,GroundType1,GroundType2,GroundType3,Flag,MapLength,BarMask);
        
    }
}
