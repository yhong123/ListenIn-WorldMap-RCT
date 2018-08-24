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

    /// <summary>
    /// This delegate is used to detect that a new section is currently being created
    /// </summary>
    public delegate void OnSwitchingSection(UserProfileManager currUserSection, int amount);
    public OnSwitchingSection m_OnSwitchingSection;

    /// <summary>
    /// This delegate is used to update the current section, no chage detected
    /// </summary>
    public delegate void OnUpdatingCurrentSection(UserProfileManager currUserSection, int amount);
    public OnUpdatingCurrentSection m_OnUpdatingCurrentSection;

    /// <summary>
    /// This delegate is used to trigger special animation when ending a particular section
    /// </summary>
    public delegate void OnEndingSection(UserProfileManager currUserSection);
    public OnEndingSection m_OnEndingSection;

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
            m_UserProfileManager.m_userProfile.isFirstInit = true;
            m_UserProfileManager.m_userProfile.m_currCycle = 0;
            m_UserProfileManager.m_userProfile.m_currIDUser = 1;
            m_UserProfileManager.m_userProfile.m_TherapyLiroUserProfile.m_currentBlock = -1; //It is a shortcut for when initializing the game for the first time.
            m_UserProfileManager.m_userProfile.m_TherapyLiroUserProfile.m_totalBlocks =0;
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
                Debug.Log("Current player section: " + m_UserProfileManager.LIROStep.ToString());

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
        switch (m_UserProfileManager.LIROStep)
        {
            case TherapyLadderStep.CORE:
                return m_UserProfileManager.m_userProfile.m_TherapyLiroUserProfile.m_currentBlock;
                break;
            case TherapyLadderStep.ACT:
                return m_UserProfileManager.m_userProfile.m_ACTLiroUserProfile.m_currentBlock;
                break;
            default:
                break;
        }
        return -1;
    }
    public int GetCurrentTherapyCycle() {
        switch (m_UserProfileManager.LIROStep)
        {
            case TherapyLadderStep.CORE:
            case TherapyLadderStep.ACT:
                return m_UserProfileManager.m_userProfile.m_TherapyLiroUserProfile.m_cycleNumber;
                break;            
            default:
                break;
        }
        return -1;
    }
    /// <summary>
    /// Checking the folder with the blocks if they match the current registered step of the algorithm.
    /// If not load from playerprefs and redo from the last saved checkpoint
    /// </summary>
    public void CheckCurrentSection()
    {
        //AndreaLIRO: simplify this version. We switch between the sections and call separate methods

        //We check for first initialization only as this is treated specially
        if (m_UserProfileManager.m_userProfile.isFirstInit)
        {
            //AndreaLIRO:
            //First initalization, should start practice
            Debug.Log("Detected first initialization. Need to start tutorial section");
            return;
        }

        //AdnreaLIRO: this will be removed, it s here just to check consistency with the game.
        //************************************************************************************************************************
        string currFolder = GlobalVars.GetPathToLIROCurrentLadderSection();

        //Get a single item from the ladder section folder (any is fine since they should belonging to all of the same batch)
        string[] currFiles = Directory.GetFiles(currFolder);
        //If folder is empty create the splitted file

        //If we find any files
        if (currFiles != null && currFiles.Length != 0)
        {
            //We check consistency by looking at the filename in the section folder
            string full_filename = Path.GetFileName(currFiles[0]);
            string[] splittedElements = full_filename.Replace(".csv", string.Empty).Split(new char[] { '_' });

            //Checking the name is the same
            if (splittedElements[0] != m_UserProfileManager.LIROStep.ToString())
            {
                //AndreaLIRO: must decide how to act in this case
                Debug.LogError("TherapyLIROManager: found a mismatch between file" + splittedElements[0] + " and current loaded user profile " + m_UserProfileManager.LIROStep.ToString());
            }
            else
            {
                Debug.Log("Being in " + m_UserProfileManager.LIROStep.ToString() + " section, but found file in section folder with name: " + splittedElements[0]);
            }
        }
        //************************************************************************************************************************

        bool advance = false;
        switch (m_UserProfileManager.LIROStep)
        {
            case TherapyLadderStep.CORE:
                advance = CheckTherapyCoreEscapeSection();
                break;
            case TherapyLadderStep.ACT:
                advance = CheckACTEscapeSection();
                break;
            default:
                break;
        }

        if (advance)
        {
            //We first do any rewarding animation
            if (m_OnEndingSection != null)
            {
                m_OnEndingSection(m_UserProfileManager);
            }
            return;
        }
        else
        {
            PrepareCurrentSection();
        }
    }

    /// <summary>
    /// Escaping condition for therapy core section
    /// </summary>
    /// <returns></returns>
    private bool CheckTherapyCoreEscapeSection()
    {
        return (m_UserProfileManager.m_userProfile.m_TherapyLiroUserProfile.m_currentBlock > m_UserProfileManager.m_userProfile.m_TherapyLiroUserProfile.m_totalBlocks);        
    }
    /// <summary>
    /// Escaping condition for ACT section
    /// </summary>
    /// <returns></returns>
    private bool CheckACTEscapeSection()
    {
        return (m_UserProfileManager.m_userProfile.m_ACTLiroUserProfile.m_currentBlock > m_UserProfileManager.m_userProfile.m_ACTLiroUserProfile.m_totalBlocks);
    }

    /// <summary>
    /// Tis function is called from the UI to notify the manager to change the section
    /// </summary>
    public void GoToNextSection()
    {
        AdvanceCurrentSection();
        StartCoroutine(CreateCurrentSection());
    }
    public void PrepareCurrentSection()
    {
        switch (m_UserProfileManager.LIROStep)
        {
            case TherapyLadderStep.CORE:
                PrepareTherapyScreen();
                break;
            case TherapyLadderStep.ACT:
                PrepareACTScreen();
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

    //UPDATING
    //*******************************************************
    internal void PrepareTherapyScreen()
    {
        if (m_OnUpdatingCurrentSection != null)
            m_OnUpdatingCurrentSection(m_UserProfileManager, 100);
    }
    internal void PrepareACTScreen()
    {
        //Do eventually additional things here!
        if (m_OnUpdatingCurrentSection != null)
            m_OnUpdatingCurrentSection(m_UserProfileManager, 100);
    }
    //*******************************************************

    //SWITCHING
    //*******************************************************
    /// <summary>
    /// Called once after the UI notify that the ending animation for current section is being done
    /// </summary>
    /// <returns></returns>
    internal IEnumerator CreateCurrentSection()
    {
        //AndreaLIRO
        switch (m_UserProfileManager.LIROStep)
        {
            case TherapyLadderStep.CORE:
                PrepareBasketSelectionScreen();
                break;
            case TherapyLadderStep.ACT:
                LoadingACTScreen(0);
                yield return StartCoroutine(LoadACTFile());
                break;
            default:
                Debug.LogError("This has not been found...");
                break;
        }

        yield return null;
    }
    /// <summary>
    /// Called once when switching to the therapy
    /// </summary>
    internal void PrepareBasketSelectionScreen()
    {
        if (m_OnSwitchingSection != null)
            m_OnSwitchingSection(m_UserProfileManager,0);
    }
    /// <summary>
    /// Called many time when switching to the ACT
    /// </summary>
    /// <param name="amount"></param>
    internal void LoadingACTScreen(int amount)
    {
        if (m_OnSwitchingSection != null)
            m_OnSwitchingSection(m_UserProfileManager, amount);
    }
    /// <summary>
    /// Loading ACT file and generate splitted files
    /// </summary>
    /// <returns></returns>
    internal IEnumerator LoadACTFile()
    {
        yield return new WaitForEndOfFrame();

        int currAmount = 3;
        if (m_OnSwitchingSection != null)
        {
            m_OnSwitchingSection(m_UserProfileManager, currAmount);
        }
        yield return new WaitForEndOfFrame();

        TextAsset ta = Resources.Load<TextAsset>(GlobalVars.LiroACT);

        List<string> lines = ta.text.Split(new char[] { '\n' }).ToList();

        currAmount = 16;
        if (m_OnSwitchingSection != null)
        {
            m_OnSwitchingSection(m_UserProfileManager, currAmount);
        }
        yield return new WaitForEndOfFrame();

        ShuffleLines(ref lines);

        currAmount = 27;
        if (m_OnSwitchingSection != null)
        {
            m_OnSwitchingSection(m_UserProfileManager, currAmount);
        }
        yield return new WaitForEndOfFrame();

        int currCount = 0;
        int total_blocks = 1;
        List<string> currBlockLines = new List<string>();
        string currFilename = String.Empty;
        for (int i = 0; i < lines.Count; i++)
        {
            currBlockLines.Add(lines[i].Replace("\r", "").Trim());
            currCount++;
            if (currCount == GlobalVars.ActChallengeLength)
            {
                currFilename = String.Format("{0}_{1}", m_UserProfileManager.LIROStep, total_blocks);
                File.WriteAllLines(Path.Combine(GlobalVars.GetPathToLIROCurrentLadderSection(), currFilename), currBlockLines.ToArray());
                currBlockLines.Clear();
                currCount = 0;
                total_blocks++;
            }

            currAmount = 30 + 30 * (i / lines.Count);
            if (m_OnSwitchingSection != null)
            {
                m_OnSwitchingSection(m_UserProfileManager, currAmount);
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

        //Updating UserProfile in the ACT section
        m_UserProfileManager.m_userProfile.m_ACTLiroUserProfile.m_totalBlocks = total_blocks - 1; //Should be always 8
        m_UserProfileManager.m_userProfile.m_ACTLiroUserProfile.m_currentBlock = 1;

        currAmount += 15;
        if (m_OnSwitchingSection != null)
        {
            m_OnSwitchingSection(m_UserProfileManager, currAmount);
        }
        yield return new WaitForEndOfFrame();

        yield return StartCoroutine(SaveCurrentUserProfile());

        currAmount = 100;
        if (m_OnSwitchingSection != null)
        {
            m_OnSwitchingSection(m_UserProfileManager, currAmount);
        }
        yield return new WaitForEndOfFrame();
    }
    //*******************************************************
    /// <summary>
    /// Called every time a new preparation for the ladder step must be completed
    /// </summary>
    /// <returns></returns>

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

        //Updating UserProfile in the therapy section
        m_UserProfileManager.m_userProfile.m_TherapyLiroUserProfile.m_totalBlocks = total_blocks - 1;
        m_UserProfileManager.m_userProfile.m_TherapyLiroUserProfile.m_currentBlock = 1;
        m_UserProfileManager.m_userProfile.m_TherapyLiroUserProfile.m_cycleNumber++;

        yield return StartCoroutine(SaveCurrentUserProfile());

        currAmount = 100;
        if (m_onUpdateProgress != null)
        {
            m_onUpdateProgress(currAmount);
        }

    }
    internal IEnumerator CleanCurrentBlock()
    {
        try
        {
            string fullPathFile = String.Empty;
            int currBlock = -1;
            switch (m_UserProfileManager.LIROStep)
            {
                case TherapyLadderStep.CORE:
                    currBlock = m_UserProfileManager.m_userProfile.m_TherapyLiroUserProfile.m_currentBlock;
                    break;
                case TherapyLadderStep.ACT:
                    currBlock = m_UserProfileManager.m_userProfile.m_ACTLiroUserProfile.m_currentBlock;
                    break;
                default:
                    break;
            }

            if (currBlock == -1)
            {
                Debug.Log("Wrong registered block to delete: section is " + m_UserProfileManager.LIROStep.ToString());
            }

            fullPathFile = Path.Combine(GlobalVars.GetPathToLIROCurrentLadderSection(), String.Format("{0}_{1}", m_UserProfileManager.LIROStep.ToString(), currBlock));
            File.Delete(fullPathFile);
        }
        catch (Exception ex)
        {
            Debug.LogError("Could not delete curr block file: " + ex.Message);
        }
        yield return null;
    }
    internal void AdvanceBlock()
    {
        switch (m_UserProfileManager.LIROStep)
        {
            case TherapyLadderStep.CORE:
                m_UserProfileManager.m_userProfile.m_TherapyLiroUserProfile.m_currentBlock++;
                break;
            case TherapyLadderStep.ACT:
                m_UserProfileManager.m_userProfile.m_ACTLiroUserProfile.m_currentBlock++;
                break;
            default:
                break;
        }
    }
    #endregion

}
