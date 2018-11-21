using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Env : MonoBehaviour
{


    private void Start()
    {
        
        LuaUtil.ToLua("Env");
        LuaUtil.CallFunc("Env.InitEnv",gameObject);
        LuaUtil.CallFunc("Env.GenStage");
    }
}
