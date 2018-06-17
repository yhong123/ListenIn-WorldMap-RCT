using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CoreItemReader : ICsvReader<Challenge>
{
    private int currStep;
    int ICsvReader<Challenge>.CurrentStep
    {
        get
        {
            return currStep;
        }
    }
    public IEnumerable<Challenge> ParseCsv(string path)
    {
        List<Challenge> currList = new List<Challenge>();
        
        //Read a single file and extract the list of challenges
        string currBlock = File.ReadAllText(path);
        string currFileName = Path.GetFileName(path);

        //Extracting number of file so to track the progress of the player
        TherapyLadderStep currStep = TherapyLIROManager.Instance.GetCurrentLadderStep();
        string currNumber = Regex.Replace(currFileName, String.Concat(currStep.ToString(), "_"), String.Empty);
        int stepNumber;

        if (int.TryParse(currNumber, out stepNumber))
        {
            TherapyLIROManager.Instance.SectionCounter = stepNumber;
        }

        string[] lines = currBlock.Split(new char[] { '\n' });

        foreach (string item in lines)
        {
            if (item != String.Empty)
            {
                Challenge currChallenge = new Challenge();
                string[] sections = item.Replace("\r", String.Empty).Trim().Split(new char[] { ',' });
                currChallenge.LexicalItem = sections[1];
                int untrained = 0;
                int.TryParse(sections[2], out untrained);
                currChallenge.Untrained = untrained;
                currChallenge.FileAudioID = sections[3];
                long id = long.MaxValue;
                //Recovering ChallengeID
                long.TryParse(sections[0], out id);
                currChallenge.ChallengeID = id;
                long.TryParse(sections[4], out id);
                currChallenge.CorrectImageID = id;
                currChallenge.Foils.Add(id);
                long.TryParse(sections[5], out id);
                currChallenge.Foils.Add(id);
                long.TryParse(sections[6], out id);
                currChallenge.Foils.Add(id);
                long.TryParse(sections[7], out id);
                currChallenge.Foils.Add(id);
                long.TryParse(sections[8], out id);
                currChallenge.Foils.Add(id);
                long.TryParse(sections[9], out id);
                currChallenge.Foils.Add(id);
                currList.Add(currChallenge);
            }
        }

        return currList;

    }
}
