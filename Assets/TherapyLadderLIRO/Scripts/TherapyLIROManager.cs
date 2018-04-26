using UnityEngine;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Xml;
using System.Xml.Linq;

//public enum TherapyLadderStep { ACT1 = 0, OUT1 = 1, CORE1 =  2, SETA = 3, ACT2 = 4, OUT2 = 5, CORE2 = 6, SETB = 7};

public enum TherapyLadderStep { CORE1 = 0, SETA = 1};

public class TherapyLIROManager : Singleton<TherapyLIROManager> {

    private TherapyLadderStep m_LIROTherapyStep;
    private int currIDUser;

    protected override void Awake()
    {
        //m_LIROTherapySteps.Add(TherapyLadderStep.CORE);
        //m_LIROTherapySteps.Add(TherapyLadderStep.ACT);
    }

    void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            Debug.Log("First level was loaded");
            //Check for PlayerPrefs to see if there is a data for the current step
        }

    }

    /// <summary>
    /// This function is called at initialiation to load the last saved information for the current user on the therapy ladder
    /// </summary>
    public IEnumerator LoadCurrentUserProfile()
    {
        string currUser = string.Format(GlobalVars.LiroProfileTemplate, DatabaseXML.Instance.PatientId);
        string currFullPath = Path.Combine(GlobalVars.GetPathToLIROUserProfile(), currUser);

        FileInfo info = new FileInfo(currFullPath);

        if (!info.Exists || info.Length == 0)
        {
            Debug.Log("LIRO User Profile not found, creating a new one");
            //Setting a new LIRO user profile
            m_LIROTherapyStep = (TherapyLadderStep)0;
            currIDUser = 1;
            yield return StartCoroutine(SaveCurrentUserProfile());
        }
        else
        {
            bool exceptionThrown = false;

            try
            {
                XElement root = XElement.Load(currFullPath);
                currIDUser = (int)root.Element("UserID");
                if (currIDUser != DatabaseXML.Instance.PatientId)
                {
                    Debug.LogWarning("ID mismatch between database xml and therapy LIRO manager");
                }

                string currStageEnum = (string)root.Element("LadderStep");
                m_LIROTherapyStep = (TherapyLadderStep)Enum.Parse(typeof(TherapyLadderStep), currStageEnum);

            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                exceptionThrown = true;
                m_LIROTherapyStep = (TherapyLadderStep)0;
                currIDUser = 1;
            }

            if (exceptionThrown)
                yield return StartCoroutine(SaveCurrentUserProfile());

        }

        //AndreaLIRO: to eliminate
        CheckCurrentSection();

        yield return null;
    }
    public IEnumerator SaveCurrentUserProfile()
    {

        string currUser = string.Format(GlobalVars.LiroProfileTemplate, DatabaseXML.Instance.PatientId);
        string currFullPath = Path.Combine(GlobalVars.GetPathToLIROUserProfile(), currUser);

        // save lsTrial to xml 
        XmlDocument doc = new XmlDocument();
        doc.LoadXml("<?xml version='1.0' encoding='utf-8'?>" +
            "<root>" +
            "</root>");

        // Save the document to a file. White space is preserved (no white space).
        string strXmlFile = System.IO.Path.Combine(currFullPath, currUser);

        XmlElement xmlChild = doc.CreateElement("UserID");
        xmlChild.InnerText = currIDUser.ToString();
        doc.DocumentElement.AppendChild(xmlChild);

        xmlChild = doc.CreateElement("LadderStep");
        xmlChild.InnerText = m_LIROTherapyStep.ToString();
        doc.DocumentElement.AppendChild(xmlChild);

        doc.Save(currFullPath);

        yield return null;
    }

    #region API
    public void CheckCurrentSection()
    {
        //Andrea: add the following 
        string currFolder = GlobalVars.GetPathToLIROCurrentLadderSection();
        //If folder is empty create the splitted file
        string[] currFiles = Directory.GetFiles(currFolder);
        if (currFiles != null && currFiles.Length != 0)
        {
            //Proceed
        }
        else
        {
            Debug.Log("TherapyLiroManager: creating current file sections");
            StartCoroutine(CreateCurentSection());
        }
    }

    public void AdvanceCurrentSection()
    {
        int currStep = (int)m_LIROTherapyStep;
        Array currValues = Enum.GetValues(typeof(TherapyLadderStep));
        int size = currValues.Length;

        currStep = (currStep + 1) % size;
    }
    #endregion

    #region internalFunctions
    internal IEnumerator CreateCurentSection()
    {
        if (m_LIROTherapyStep == TherapyLadderStep.CORE1)
        {
            //Load csv file
            TextAsset coreItemText = Resources.Load<TextAsset>(GlobalVars.LiroCoreItems);
            string[] itemLines = coreItemText.text.Split(new char[] { '\n' });

            string coreFormat = "Core_{0}";
            string currFileName;
            int currFileIdx = 0;
            int challengeCounter = 0;

            List<string> currLines = new List<string>();

            for (int i = 0; i < itemLines.Length; i++)
            {
                currLines.Add(itemLines[i].Replace("\r",String.Empty).Trim());
                challengeCounter++;
                if (challengeCounter == GlobalVars.ChallengeLength)
                {
                    //SaveFile and create a new one
                    currFileName = String.Format(coreFormat, currFileIdx);
                    File.WriteAllLines(Path.Combine(GlobalVars.GetPathToLIROCurrentLadderSection(), currFileName), currLines.ToArray());
                    currLines.Clear();
                    challengeCounter = 0;
                    currFileIdx++;
                }
                
            }
            //Save the remaining in the last file
            if (currLines.Count != 0)
            {
                currFileName = String.Format(coreFormat, currFileIdx);
                File.WriteAllLines(Path.Combine(GlobalVars.GetPathToLIROCurrentLadderSection(), currFileName), currLines.ToArray());
                currLines.Clear();
            }               
        }
        yield return null;
    }

    internal void ShuffleLines(ref string[] lines)
    {
        //Andrea to be implemented
    }
    #endregion

}
