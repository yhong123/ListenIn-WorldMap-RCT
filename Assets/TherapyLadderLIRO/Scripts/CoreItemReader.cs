using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

public class CoreItemReader : ICsvReader<Challenge>
{
    public IEnumerable<Challenge> ParseCsv(string path)
    {
        List<Challenge> currList = new List<Challenge>();
        
        //Read a single file and extract the list of challenges
        string currBlock = File.ReadAllText(path);

        string[] lines = currBlock.Split(new char[] { '\n' });

        foreach (string item in lines)
        {
            if (item != String.Empty)
            {
                Challenge currChallenge = new Challenge();
                string[] sections = item.Replace("\r", String.Empty).Trim().Split(new char[] { ',' });
                currChallenge.FileAudioID = sections[1];
                long id = long.MaxValue;
                long.TryParse(sections[2], out id);
                currChallenge.CorrectImageID = id;
                currChallenge.Foils.Add(id);
                long.TryParse(sections[3], out id);
                currChallenge.Foils.Add(id);
                long.TryParse(sections[4], out id);
                currChallenge.Foils.Add(id);
                long.TryParse(sections[5], out id);
                currChallenge.Foils.Add(id);
                long.TryParse(sections[6], out id);
                currChallenge.Foils.Add(id);
                long.TryParse(sections[7], out id);
                currChallenge.Foils.Add(id);
                currList.Add(currChallenge);
            }

        }

        return currList;

    }
}
