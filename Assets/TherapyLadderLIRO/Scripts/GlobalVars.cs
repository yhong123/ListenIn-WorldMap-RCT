using UnityEngine;
using System.IO;
using System;
using System.Collections;

public static class GlobalVars {

    public static int ChallengeLength = 20;
    public static int ActChallengeLength = 30;
    public static string GetPath()
    {
#if UNITY_EDITOR
        return Application.persistentDataPath;
#elif UNITY_ANDROID
return Application.persistentDataPath;// +fileName;
#else
return Application.dataPath;// +"/"+ fileName;
#endif
    }

    public static string LiroProfileTemplate = "user_{0}_profile_LIRO";
    public static string LiroCoreItems = @"Doc/2018-04-core-challenge-list";

    public static string GetPathToLIROOutput()
    {
        return Path.Combine(GetPath(), @"ListenIn/LIRO/LIRO_Output");
    }

    public static string GetPathToLIROBaskets()
    {
        return Path.Combine(GetPath(), @"ListenIn/LIRO/Baskets");
    }
    public static string GetPathToLIROUserProfile()
    {
        return Path.Combine(GetPath(), @"ListenIn/LIRO");
    }
    public static string GetPathToLIROCurrentLadderSection()
    {
        return Path.Combine(GetPath(), @"ListenIn/LIRO/Section");
    }

}
