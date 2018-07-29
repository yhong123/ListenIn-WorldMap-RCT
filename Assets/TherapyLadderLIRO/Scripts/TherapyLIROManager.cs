using UnityEngine;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using Newtonsoft.Json;

//public enum TherapyLadderStep { ACT1 = 0, OUT1 = 1, CORE1 =  2, SETA = 3, ACT2 = 4, OUT2 = 5, CORE2 = 6, SETB = 7};

public enum TherapyLadderStep { CORE = 0, ACT = 1};

public class TherapyLIROManager : Singleton<TherapyLIROManager> {

    #region CORE

    [SerializeField]
    private UserProfileManager m_UserProfileManager = new UserProfileManager();

    private int m_currSectionCounter;
    public int SectionCounter { get { return m_currSectionCounter; } set { m_currSectionCounter = value; } }
    private static int m_numberSelectedPerLexicalItem = 5;
    #endregion

    #region Delegates
    public delegate void OnUpdateProgress(int progressAmount);
    public OnUpdateProgress m_onUpdateProgress;
    public delegate void OnAdvancingTherapyLadder(TherapyLadderStep newStep);
    public OnAdvancingTherapyLadder m_onAdvancingTherapy;
    public delegate void OnFinishingCurrentSectionSetup(TherapyLadderStep newStep, int amount);
    public OnFinishingCurrentSectionSetup m_onFinishingSetupCurrentSection;
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
            m_UserProfileManager.LIROStep = (TherapyLadderStep)0;
            m_UserProfileManager.m_userProfile.m_currIDUser = 1;
            m_UserProfileManager.m_userProfile.m_TherapyLiroUserProfile.m_currentBlock = -1; //It is a shortcut for when initializing the game for the first time.
            m_UserProfileManager.m_userProfile.m_TherapyLiroUserProfile.m_totalBlocks = 0;
            m_UserProfileManager.m_userProfile.m_TherapyLiroUserProfile.m_cycleNumber = 0;
            m_UserProfileManager.m_userProfile.m_ACTLiroUserProfile.m_currentBlock = -1; //It is a shortcut for when initializing the game for the first time.
            m_UserProfileManager.m_userProfile.m_ACTLiroUserProfile.m_totalBlocks = 8;
            //Wait for the data to be saved
            yield return StartCoroutine(SaveCurrentUserProfile());
        }
        else
        {
            bool exceptionThrown = false;

            try
            {
                //Loading current profile

                m_UserProfileManager.LoadFromDisk(currFullPath);
                // If file exists load current profile, notify only if finding a mismatch
                //XElement root = XElement.Load(currFullPath);
                //m_UserProfileManager.m_userProfile.m_currIDUser = (int)root.Element("UserID");
                //if (m_UserProfileManager.m_userProfile.m_currIDUser != DatabaseXML.Instance.PatientId)
                //{
                //    Debug.LogWarning("ID mismatch between database xml and therapy LIRO manager");
                //}

                //string currStageEnum = (string)root.Element("LadderStep");
                //m_UserProfileManager.LIROStep = (TherapyLadderStep)Enum.Parse(typeof(TherapyLadderStep), currStageEnum);

                //Debug.Log("Current player section: " + m_UserProfileManager.LIROStep.ToString());

                //m_UserProfile.m_currentBlock = (int)root.Element("CurrentBlockNumber");
                //m_UserProfile.m_current_Total_Blocks = (int)root.Element("CurrentTotalBlockNumber");

            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                exceptionThrown = true;
                //Getting information from the last checkpoint -> PlayerPrefs
                m_UserProfileManager.GetFromPrefs();
            }

            if (exceptionThrown)
                yield return StartCoroutine(SaveCurrentUserProfile());

        }

        yield return null;
    }
    public IEnumerator SaveCurrentUserProfile()
    {
        try
        {
            m_UserProfileManager.SaveToPrefs();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
        yield return null;

        try
        {
            //AndreaLIRO: switching to json
            m_UserProfileManager.SaveToDisk();
            //string currUser = string.Format(GlobalVars.LiroProfileTemplate, DatabaseXML.Instance.PatientId);
            //string currFullPath = Path.Combine(GlobalVars.GetPathToLIROUserProfile(), currUser);

            //// save lsTrial to xml 
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml("<?xml version='1.0' encoding='utf-8'?>" +
            //    "<root>" +
            //    "</root>");

            //// Save the document to a file. White space is preserved (no white space).
            //string strXmlFile = System.IO.Path.Combine(currFullPath, currUser);

            //XmlElement xmlChild = doc.CreateElement("UserID");
            //xmlChild.InnerText = m_UserProfileManager.m_userProfile.m_currIDUser.ToString();
            //doc.DocumentElement.AppendChild(xmlChild);

            //xmlChild = doc.CreateElement("LadderStep");
            //xmlChild.InnerText = m_UserProfileManager.LIROStep.ToString();
            //doc.DocumentElement.AppendChild(xmlChild);

            //xmlChild = doc.CreateElement("CurrentBlockNumber");
            ////xmlChild.InnerText = m_UserProfileManager.m_currentBlock.ToString();
            //doc.DocumentElement.AppendChild(xmlChild);

            //xmlChild = doc.CreateElement("CurrentTotalBlockNumber");
            ////xmlChild.InnerText = m_UserProfile.m_current_Total_Blocks.ToString();
            //doc.DocumentElement.AppendChild(xmlChild);

            //doc.Save(currFullPath);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
        yield return null;


    }

    #region API
    public TherapyLadderStep GetCurrentLadderStep()
    {
        return m_UserProfileManager.LIROStep;
    }
    public int GetCurrentBlockNumber()
    {
        //AndreaLiro must be separated according to section
        //return m_UserProfile.m_currentBlock;
        return 0;
    }
    /// <summary>
    /// Checking the folder with the blocks if they match the current registered step of the algorithm.
    /// If not load from playerprefs and redo from the last saved checkpoint
    /// </summary>
    public void CheckCurrentSection()
    {
        string currFolder = GlobalVars.GetPathToLIROCurrentLadderSection();

        //Get a single item from the ladder section folder (any is fine since they should belonging to all of the same batch)
        string[] currFiles = Directory.GetFiles(currFolder);
        //If folder is empty create the splitted file

        if (currFiles != null && currFiles.Length != 0)
        {
            //Check consistency
            string full_filename = Path.GetFileName(currFiles[0]);
            string[] splittedElements = full_filename.Replace(".csv", string.Empty).Split(new char[] { '_' });

            //Checking the name
            if (splittedElements[0] == m_UserProfileManager.LIROStep.ToString())
            {
                Debug.Log("Being in " + m_UserProfileManager.LIROStep.ToString() + " section");
                //AndreaLIRO: must be separated in sections
                //if (m_UserProfile.m_currentBlock > m_UserProfile.m_current_Total_Blocks)
                //{
                //    Debug.Log("Therapy LIRO Manager detected end of current section");
                //    ListenIn.Logger.Instance.Log(String.Format("ENDING OF LIRO SECTION: {0}", m_UserProfile.LIROStep.ToString()), ListenIn.LoggerMessageType.Info);
                //    //Advancing the section
                //    AdvanceCurrentSection();
                //    StartCoroutine(CreateCurrentSection());
                //    return;
                //}
                //else
                //{
                //    PrepareCurrentSection();
                //}
            }
            else
            {
                //AndreaLIRO: must decide how to act in this case
                Debug.LogError("TherapyLIROManager: found a mismatch between file" + splittedElements[0] + " and current loaded user profile " + m_UserProfileManager.LIROStep.ToString());
            }
        }
        else
        {
            //AndreaLIRO: must be separated according to sections
            //if (m_UserProfile.m_current_Total_Blocks == -1)
            //{
            //    //Escape code for first initization - WE DO NOT HAVE TO ADVANCE
            //    StartCoroutine(CreateCurrentSection());
            //}
            //else if (m_UserProfile.m_currentBlock > m_UserProfile.m_current_Total_Blocks)
            //{
            //    Debug.Log("TherapyLiroManager: creating current file sections");
            //    AdvanceCurrentSection();
            //    StartCoroutine(CreateCurrentSection());
            //}
            //else
            //{
            //    Debug.LogError("TherapyLiroManager: No more file, but no condition for advancing...");
            //}

        }
    }
    /// <summary>
    /// Preparing visual for the current section
    /// </summary>
    public void PrepareCurrentSection()
    {
        switch (m_UserProfileManager.LIROStep)
        {
            case TherapyLadderStep.CORE:
                //AndreaLIRO: 
                //Activate button to go to the world map
                PrepareTherapyScreen();
                break;
            case TherapyLadderStep.ACT:
                PrepareACTScreen(100);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Called to increment the section before starting a new one
    /// </summary>
    public void AdvanceCurrentSection()
    {

        int currStep = (int)m_UserProfileManager.LIROStep;
        Array currValues = Enum.GetValues(typeof(TherapyLadderStep));
        int size = currValues.Length;

        currStep = (currStep + 1) % size;
        m_UserProfileManager.LIROStep = (TherapyLadderStep)currStep;
    }
    public void StartCoreTherapy(List<int> selectedBasket)
    {
        StartCoroutine(LoadTherapyFromBasketFiles(selectedBasket));
    }
    public void StartACT()
    {
        StartCoroutine(LoadACTFile());
    }

    /// <summary>
    /// Called at the end of the section cycle (core and ACT)
    /// </summary>
    /// <returns></returns>
    public IEnumerator AdvanceCurrentBlockInSection()
    {
        switch (m_UserProfileManager.LIROStep)
        {
            case TherapyLadderStep.CORE:
                yield return StartCoroutine(CleanCurrentBlock());
                AdvanceBlock();
                break;
            case TherapyLadderStep.ACT:
                yield return StartCoroutine(CleanCurrentBlock());
                AdvanceBlock();
                break;
            default:
                break;
        }
        //Saving profile
        yield return StartCoroutine(SaveCurrentUserProfile());
    }
    #endregion

    #region Internal Functions

    
    internal void PrepareTherapyScreen()
    {
        if (m_onFinishingSetupCurrentSection != null)
            m_onFinishingSetupCurrentSection(m_UserProfileManager.LIROStep, 100);
    }
    internal void PrepareBasketSelectionScreen()
    {
        //Do eventually additional things here!
        if (m_onAdvancingTherapy != null)
            m_onAdvancingTherapy(m_UserProfileManager.LIROStep);
    }
    internal void PrepareACTScreen(int initialAmount)
    {
        //Do eventually additional things here!
        if (m_onAdvancingTherapy != null)
            m_onAdvancingTherapy(m_UserProfileManager.LIROStep);
        if (m_onFinishingSetupCurrentSection != null)
            m_onFinishingSetupCurrentSection(m_UserProfileManager.LIROStep, initialAmount);
    }
    /// <summary>
    /// Called every time a new preparation for the ladder step must be completed
    /// </summary>
    /// <returns></returns>
    internal IEnumerator CreateCurrentSection()
    {
        //AndreaLIRO
        yield return new WaitForSeconds(2);
        switch (m_UserProfileManager.LIROStep)
        {
            case TherapyLadderStep.CORE:
                PrepareBasketSelectionScreen();
                break;
            case TherapyLadderStep.ACT:
                PrepareACTScreen(0);
                StartCoroutine(LoadACTFile());
                break;
            default:
                Debug.LogError("This has not been found...");
                break;
        }
        
        yield return null;
    }
    internal void ShuffleLines(ref List<string> lines)
    {
        List<string> shuffledLines = lines.OrderBy(x => Guid.NewGuid()).ToList();
        lines.Clear();
        lines.AddRange(shuffledLines);
    }
    internal IEnumerator LoadTherapyFromBasketFiles(List<int> baskets)
    {
        int currAmount = 2;

        string currBasketsPath;
        List<Challenge> curr_basket_list_read = new List<Challenge>();
        List<string> curr_basket_lexical_item_list = new List<string>();
        List<Challenge> curr_basket_list_write = new List<Challenge>();

        string coreFormat = String.Concat( m_UserProfileManager.LIROStep.ToString(), "_{0}");
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
            currAmount += 4;
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

            currAmount += 6;
            if (m_onUpdateProgress != null)
            {
                m_onUpdateProgress(currAmount);
            }
            yield return new WaitForEndOfFrame();

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

            currAmount += 11;
            if (m_onUpdateProgress != null)
            {
                m_onUpdateProgress(currAmount);
            }
            yield return new WaitForEndOfFrame();
        }

        //Updating UserProfile
        //AndreaLIRO: must be separated according to section
        //m_UserProfile.m_current_Total_Blocks = total_blocks - 1;
        //m_UserProfile.m_currentBlock = 1;

        yield return StartCoroutine(SaveCurrentUserProfile());

        currAmount = 100;
        if (m_onUpdateProgress != null)
        {
            m_onUpdateProgress(currAmount);
        }

    }
    internal IEnumerator LoadACTFile()
    {
        int currAmount = 3;
        if (m_onFinishingSetupCurrentSection != null)
        {
            m_onFinishingSetupCurrentSection(m_UserProfileManager.LIROStep, currAmount);
        }
        yield return new WaitForEndOfFrame();

        TextAsset ta = Resources.Load<TextAsset>(GlobalVars.LiroACT);

        List<string> lines = ta.text.Split( new char[] { '\n' }).ToList();

        currAmount = 16;
        if (m_onFinishingSetupCurrentSection != null)
        {
            m_onFinishingSetupCurrentSection(m_UserProfileManager.LIROStep, currAmount);
        }
        yield return new WaitForEndOfFrame();

        ShuffleLines(ref lines);

        currAmount = 27;
        if (m_onFinishingSetupCurrentSection != null)
        {
            m_onFinishingSetupCurrentSection(m_UserProfileManager.LIROStep, currAmount);
        }
        yield return new WaitForEndOfFrame();

        int currCount = 0;
        int total_blocks = 1;
        List<string> currBlockLines = new List<string>();
        string currFilename = String.Empty;
        for (int i = 0; i < lines.Count; i++)
        {
            currBlockLines.Add(lines[i].Replace("\r","").Trim());
            currCount++;
            if (currCount == GlobalVars.ActChallengeLength)
            {
                currFilename = String.Format("{0}_{1}", m_UserProfileManager.LIROStep, total_blocks);
                File.WriteAllLines(Path.Combine(GlobalVars.GetPathToLIROCurrentLadderSection(), currFilename), currBlockLines.ToArray());
                currBlockLines.Clear();
                total_blocks++;
            }

            currAmount += 5;
            if (m_onFinishingSetupCurrentSection != null)
            {
                m_onFinishingSetupCurrentSection(m_UserProfileManager.LIROStep, currAmount);
            }
            yield return new WaitForEndOfFrame();

        }

        if (currBlockLines.Count != 0)
        {
            currFilename = String.Format("{0}_{1}", m_UserProfileManager.LIROStep, total_blocks);
            File.WriteAllLines(Path.Combine(GlobalVars.GetPathToLIROCurrentLadderSection(), currFilename), currBlockLines.ToArray());
            currBlockLines.Clear();
            total_blocks++;
        }

        //AndreaLIRO: must be separated according to section
        //m_UserProfile.m_current_Total_Blocks = total_blocks - 1;
        //m_UserProfile.m_currentBlock = 1;

        currAmount += 3;
        if (m_onFinishingSetupCurrentSection != null)
        {
            m_onFinishingSetupCurrentSection(m_UserProfileManager.LIROStep, currAmount);
        }
        yield return new WaitForEndOfFrame();

        yield return StartCoroutine(SaveCurrentUserProfile());

        currAmount = 100;
        if (m_onFinishingSetupCurrentSection != null)
        {
            m_onFinishingSetupCurrentSection(m_UserProfileManager.LIROStep, currAmount);
        }
        yield return new WaitForEndOfFrame();
    }

    internal IEnumerator CleanCurrentBlock()
    {
        try
        {
            //AndreaLIRO: must be separated according to section
            //string filefullpath = Path.Combine(GlobalVars.GetPathToLIROCurrentLadderSection(), String.Format("{0}_{1}", m_UserProfile.LIROStep.ToString(), m_UserProfile.m_currentBlock));
            //File.Delete(filefullpath);
        }
        catch (Exception ex)
        {
            Debug.LogError("Could not delete curr block file: " + ex.Message);
        }
        yield return null;
    }
    internal void AdvanceBlock()
    {
        //AndreaLIRO: must be separated according to section
        //m_UserProfileManager.m_userProfile.m_currentBlock++;C:\Users\AndreaCastegnaro\Documents\Unity Projects\ListenIn-WorldMap-RCT\Assets\TherapyLadderLIRO\Scripts\ACTUiWorldMAp.cs
    }
    #endregion

}
