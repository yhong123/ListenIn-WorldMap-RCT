using System.Collections;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;


public class ACTItemReader : ICsvReader<ACTChallenge>
{
    private int currStep;
    int ICsvReader<ACTChallenge>.CurrentStep
    {
        get
        {
            return currStep;
        }
    }
    public IEnumerable<ACTChallenge> ParseCsv(string path)
    {
        List<ACTChallenge> currList = new List<ACTChallenge>();

        //Read a single file and extract the list of challenges
        string currBlock = File.ReadAllText(path);

        string[] lines = currBlock.Split(new char[] { '\n' });

        foreach (string item in lines)
        {
            if (item != String.Empty)
            {
                ACTChallenge currChallenge = new ACTChallenge();
                string[] sections = item.Replace("\r", String.Empty).Trim().Split(new char[] { ',' });
                currChallenge.LexicalItem = sections[1];
                currChallenge.FileAudioID_F = sections[8];
                currChallenge.FileAudioID_M = sections[9];
                long id = long.MaxValue;
                //Recovering ChallengeID
                long.TryParse(sections[0], out id);
                currChallenge.ChallengeID = id;
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

public class ACTItemWriter : ICsvWriter<ChallengeResponseACT>
{
    public void WriteCsv(string path, string filename, IEnumerable<ChallengeResponseACT> listToWrite)
    {
        string fullPath = Path.Combine(path, filename);
        List <string> listString = new List<string>();

        foreach (var item in listToWrite)
        {
            listString.Add(String.Join(",", new string[] {

                  item.m_challengeID.ToString(), 
                  item.m_timeStamp.ToString("dd/MM/yyyy"),
                  item.m_timeStamp.ToString("HH:mm:ss"),
                  item.m_number.ToString(),
                  item.m_block.ToString(),
                  item.m_cycle.ToString(),
                  item.m_accuracy.ToString(),
                  item.m_reactionTime.ToString(),
                  item.m_repeat.ToString(),
                  item.m_pictureID.ToString()
    }));


        }

        try
        {
            File.WriteAllLines(fullPath, listString.ToArray());
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            throw;
        }
    }
}

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

        //AndreaLIRO: don t put this here, have who call this when preparing the block, register the information 
        //Extracting number of file so to track the progress of the player
        //TherapyLadderStep currStep = TherapyLIROManager.Instance.GetCurrentLadderStep();
        //string currNumber = Regex.Replace(currFileName, String.Concat(currStep.ToString(), "_"), String.Empty);
        //int stepNumber;

        //if (int.TryParse(currNumber, out stepNumber))
        //{
        //    TherapyLIROManager.Instance.SectionCounter = stepNumber;
        //}

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
