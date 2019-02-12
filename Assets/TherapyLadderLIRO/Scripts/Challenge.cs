using System.Collections;
using System.Collections.Generic;

public class ACTChallenge
{
    public long ChallengeID;
    public string LexicalItem;
    public string FileAudioID_F;
    public string FileAudioID_M;
    public long CorrectImageID;
    public List<long> Foils = new List<long>();

    public ACTChallenge()
    {

    }

    public ACTChallenge(long chID, string lexicalItem, int untrained, string faID_F, string faID_M,long ciID, long f1, long f2, long f3, long f4, long f5)
    {
        ChallengeID = chID;
        LexicalItem = lexicalItem;
        FileAudioID_F = faID_F;
        FileAudioID_M = faID_M;
        CorrectImageID = ciID;
        Foils.Add(ciID);
        Foils.Add(f1);
        Foils.Add(f2);
        Foils.Add(f3);
        Foils.Add(f4);
        Foils.Add(f5);
    }
}

public class Challenge {
    public long ChallengeID;
    public int Difficulty;
    public string LexicalItem;
    public List<string> FileAudioIDs = new List<string>();
    public long CorrectImageID;
    public List<long> Foils = new List<long>();

    public Challenge()
    {

    }

    public Challenge(long chID, string lexicalItem, int difficulty, string faID1, string faID2, string faID3, string faID4, string faID5, long ciID, long f1, long f2, long f3, long f4, long f5)
    {
        ChallengeID = chID;
        LexicalItem = lexicalItem;
        Difficulty = difficulty;
        FileAudioIDs.Add(faID1);
        FileAudioIDs.Add(faID2);
        FileAudioIDs.Add(faID3);
        FileAudioIDs.Add(faID4);
        FileAudioIDs.Add(faID5);
        CorrectImageID = ciID;
        Foils.Add(ciID);
        Foils.Add(f1);
        Foils.Add(f2);
        Foils.Add(f3);
        Foils.Add(f4);
        Foils.Add(f5);
    }

}
