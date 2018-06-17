using UnityEngine;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

//public enum TherapyLadderStep { ACT1 = 0, OUT1 = 1, CORE1 =  2, SETA = 3, ACT2 = 4, OUT2 = 5, CORE2 = 6, SETB = 7};

public enum TherapyLadderStep { CORE = 0, ACT = 1};

public class TherapyLIROManager : Singleton<TherapyLIROManager> {



    #region CORE

    [SerializeField]
    private UserProfile m_UserProfile = new UserProfile();

    private int m_currSectionCounter;
    public int SectionCounter { get { return m_currSectionCounter; } set { m_currSectionCounter = value; } }
    private static int m_numberSelectedPerLexicalItem = 5;
    #endregion

    #region Delegates
    public delegate void OnUpdateProgress(int progressAmount);
    public OnUpdateProgress m_onUpdateProgress;
    #endregion

    #region Unity
    protected override void Awake()
    {
    }
    #endregion

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
            m_UserProfile.LIROStep = (TherapyLadderStep)0;
            m_UserProfile.m_currIDUser = 1;
            yield return StartCoroutine(SaveCurrentUserProfile());
        }
        else
        {
            bool exceptionThrown = false;

            try
            {
                XElement root = XElement.Load(currFullPath);
                m_UserProfile.m_currIDUser = (int)root.Element("UserID");
                if (m_UserProfile.m_currIDUser != DatabaseXML.Instance.PatientId)
                {
                    Debug.LogWarning("ID mismatch between database xml and therapy LIRO manager");
                }

                string currStageEnum = (string)root.Element("LadderStep");
                m_UserProfile.LIROStep = (TherapyLadderStep)Enum.Parse(typeof(TherapyLadderStep), currStageEnum);

            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                exceptionThrown = true;
                m_UserProfile.LIROStep = (TherapyLadderStep)0;
                m_UserProfile.m_currIDUser = 1;
            }

            if (exceptionThrown)
                yield return StartCoroutine(SaveCurrentUserProfile());

        }

        //AndreaLIRO: to eliminate and put in a meaningful part.
        //Maybe when loading the world map allocate some time for creating the current section
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
        xmlChild.InnerText =  m_UserProfile.m_currIDUser.ToString();
        doc.DocumentElement.AppendChild(xmlChild);

        xmlChild = doc.CreateElement("LadderStep");
        xmlChild.InnerText = m_UserProfile.LIROStep.ToString();
        doc.DocumentElement.AppendChild(xmlChild);

        doc.Save(currFullPath);

        yield return null;
    }

    #region API
    public TherapyLadderStep GetCurrentLadderStep()
    {
        return m_UserProfile.LIROStep;
    }
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
            StartCoroutine(CreateCurrentSection());
        }
    }
    public void AdvanceCurrentSection()
    {
        int currStep = (int)m_UserProfile.LIROStep;
        Array currValues = Enum.GetValues(typeof(TherapyLadderStep));
        int size = currValues.Length;

        currStep = (currStep + 1) % size;
    }
    public void StartCoreTherapy(List<int> selectedBasket)
    {
        StartCoroutine(LoadBasketFiles(selectedBasket));
    }
    #endregion

    #region Internal Functions
    internal IEnumerator CreateCurrentSection()
    {
        if (m_UserProfile.LIROStep == TherapyLadderStep.CORE)
        {
            //Load csv file
            TextAsset coreItemText = Resources.Load<TextAsset>(GlobalVars.LiroCoreItems);
            string[] itemLines = coreItemText.text.Split(new char[] { '\n' });

            string coreFormat = String.Concat(m_UserProfile.LIROStep.ToString(), "_{0}");
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
                    currFileName = String.Format(coreFormat, currFileIdx + 1);
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
    internal IEnumerator LoadBasketFiles(List<int> baskets)
    {
        int currAmount = 2;

        string currBasketsPath;
        List<Challenge> curr_basket_list_read = new List<Challenge>();
        List<string> curr_basket_lexical_item_list = new List<string>();
        List<Challenge> curr_basket_list_write = new List<Challenge>();

        string coreFormat = String.Concat( m_UserProfile.LIROStep.ToString(), "_{0}");
        string currFilename;
        int total_blocks = 1;

        for (int i = 0; i < baskets.Count; i++)
        {
            curr_basket_list_read.Clear();
            curr_basket_lexical_item_list.Clear();
            curr_basket_list_write.Clear();
            
            yield return new WaitForEndOfFrame();

            CoreItemReader cir = new CoreItemReader();
            string basketName = String.Format("Basket_{0}.csv", baskets[i]);
            currBasketsPath = Path.Combine(GlobalVars.GetPathToLIROBaskets(), basketName);

            //Loading all the basket challenges excluding the untrained items
            curr_basket_list_read = cir.ParseCsv(currBasketsPath).Where(x => x.Untrained == 0).ToList();
            currAmount += 0;
            if (m_onUpdateProgress != null)
            {
                m_onUpdateProgress(currAmount);
            }
            yield return new WaitForEndOfFrame();

            //select distinct lexical item for this batch
            curr_basket_lexical_item_list = curr_basket_list_read.Select(x => x.LexicalItem).Distinct().ToList();

            //For each distinct lexical item choose a set number of random challenges
            foreach (string lexical_item in curr_basket_lexical_item_list)
            {
                curr_basket_list_write.AddRange(
                    curr_basket_list_read.Where(x => x.LexicalItem == lexical_item).OrderBy(y => RandomGenerator.GetRandom()).Take(m_numberSelectedPerLexicalItem).ToList()
                    );
            }

            currAmount += 4;
            if (m_onUpdateProgress != null)
            {
                m_onUpdateProgress(currAmount);
            }

            List<string> currLines = new List<string>();
            int challengeCounter = 0;
            //Selected all the items for this basket, allocating them to blocks
            for (int j = 0; j < curr_basket_list_write.Count; j++)
            {
                currLines.Add(String.Join(",", new string[] { curr_basket_list_write[j].ChallengeID.ToString(), curr_basket_list_write[j].LexicalItem.ToString(), curr_basket_list_write[j].Untrained.ToString(), curr_basket_list_write[j].FileAudioID.ToString(), curr_basket_list_write[j].CorrectImageID.ToString(), curr_basket_list_write[j].Foils[1].ToString(), curr_basket_list_write[j].Foils[2].ToString(), curr_basket_list_write[j].Foils[3].ToString(), curr_basket_list_write[j].Foils[4].ToString(), curr_basket_list_write[j].Foils[5].ToString() }));
                challengeCounter++;
                if (challengeCounter == GlobalVars.ChallengeLength)
                {
                    currFilename = String.Format(coreFormat, total_blocks); ; 
                    File.WriteAllLines(Path.Combine(GlobalVars.GetPathToLIROCurrentLadderSection(), currFilename), currLines.ToArray());
                    currLines.Clear();
                    total_blocks++;
                }
            }
            //Save the remaining in the last file
            if (currLines.Count != 0)
            {
                currFilename = String.Format(coreFormat, total_blocks); ;
                File.WriteAllLines(Path.Combine(GlobalVars.GetPathToLIROCurrentLadderSection(), currFilename), currLines.ToArray());
                currLines.Clear();
                total_blocks++;
            }

            currAmount += 16;
            if (m_onUpdateProgress != null)
            {
                m_onUpdateProgress(currAmount);
            }

        }

        currAmount = 100;
        if (m_onUpdateProgress != null)
        {
            m_onUpdateProgress(currAmount);
        }

    }
    #endregion

}
