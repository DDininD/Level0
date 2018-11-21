
using LuaInterface;
using UnityEngine;

public static class LuaUtil
{
     private static LuaFunction _luaFunc = null;
     private static LuaState _lua = null;
     
     public static void ToLua(string name)
     {
          new LuaResLoader();
          var path = Application.dataPath + "/Lua/" + name; 
          if(_lua==null)
          {
               _lua = new LuaState();
               _lua.Start();
               LuaBinder.Bind(_lua);
          }
          _lua.DoFile(path);
     }

     public static void CallFunc(string funcName,params object[] args)
     {
          _luaFunc = _lua.GetFunction(funcName);
          if (_luaFunc == null) return;
          _luaFunc.BeginPCall();
          if (args.Length != 0)
          foreach (var arg in args)
          {
               _luaFunc.Push(arg);                    
          }

          _luaFunc.PCall();
          _luaFunc.EndPCall();
     }
      
     
}
