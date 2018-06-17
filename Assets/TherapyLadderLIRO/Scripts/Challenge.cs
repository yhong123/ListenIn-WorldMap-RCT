using System.Collections;
using System.Collections.Generic;

public class Challenge {
    public long ChallengeID;
    public string LexicalItem;
    public int Untrained;
    public string FileAudioID;
    public long CorrectImageID;
    public List<long> Foils = new List<long>();

    public Challenge()
    {

    }

    public Challenge(long chID, string lexicalItem, int untrained, string faID, long ciID, long f1, long f2, long f3, long f4, long f5)
    {
        ChallengeID = chID;
        LexicalItem = lexicalItem;
        Untrained = untrained; 
        FileAudioID = faID;
        CorrectImageID = ciID;
        Foils.Add(ciID);
        Foils.Add(f1);
        Foils.Add(f2);
        Foils.Add(f3);
        Foils.Add(f4);
        Foils.Add(f5);
    }

}
