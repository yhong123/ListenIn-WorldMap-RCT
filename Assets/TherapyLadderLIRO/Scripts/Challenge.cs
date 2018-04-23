using System.Collections;

public class Challenge {
    public int ChallengeID;
    public string FileAudioID;
    public int CorrectImageID;
    public int Foil1;
    public int Foil2;
    public int Foil3;
    public int Foil4;
    public int Foil5;

    public Challenge(int chID, string faID, int ciID, int f1, int f2, int f3, int f4, int f5)
    {
        ChallengeID = chID;
        FileAudioID = faID;
        CorrectImageID = ciID;
        Foil1 = f1;
        Foil2 = f2;
        Foil3 = f3;
        Foil4 = f4;
        Foil5 = f5;
    }

}
