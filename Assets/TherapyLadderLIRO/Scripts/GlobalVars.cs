using UnityEngine;
using System.IO;
using System;
using System.Collections;

public static class GlobalVars {

    public static string GetPath()
    {
#if UNITY_EDITOR
        return Application.dataPath;
#elif UNITY_ANDROID
return Application.persistentDataPath;// +fileName;
#else
return Application.dataPath;// +"/"+ fileName;
#endif
    }

    public static string LiroProfileTemplate = "user_{0}_profile_LIRO";
    public static string GetPathToLIROUserProfile()
    {
        return Path.Combine(GetPath(), @"ListenIn/LIRO");
    }

}
