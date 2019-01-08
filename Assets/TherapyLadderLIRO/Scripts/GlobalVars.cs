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

    public static string LiroProfileTemplate = "user_{0}_profile_LIRO.json";
    public static string LiroCoreItems = @"Doc/2018-04-core-challenge-list";
    //Remember this are always referenced with 
    public static string LiroACT = @"Doc/ACT_LIRO/ACT_LIRO";
    public static string LiroACT_A = @"Doc/ACT_LIRO/ACT_A";
    public static string LiroACT_B = @"Doc/ACT_LIRO/ACT_B";
    public static string LiroGeneratedACT = @"ListenIn/LIRO/ACT/GEN_ACT";

    public static string GetPathToLIROOutput()
    { 
        return Path.Combine(GetPath(), @"ListenIn/LIRO/Output");
    }

    public static string GetPathToLIROBaskets()
    {
        return @"DocLiro";
    }
    public static string GetPathToLIROUserProfile()
    {
        return Path.Combine(GetPath(), @"ListenIn/LIRO");
    }
    public static string GetPathToLIROCurrentLadderSection()
    {
        return Path.Combine(GetPath(), @"ListenIn/LIRO/Section");
    }
    public static string GetPathToLIROACTFolder()
    {
        return Path.Combine(GetPath(), @"ListenIn/LIRO/ACT");
    }
    public static string GetPathToLIROACTGenerated()
    {
        return Path.Combine(GetPath(), LiroGeneratedACT);
    }
}
