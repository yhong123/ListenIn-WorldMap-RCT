using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using System.Xml;
using System.Xml.Linq;
using System.Collections;


//using Assets.Seperate_Core_Work.Scripts.TherapyItemRecSys;

//namespace Assets.Seperate_Core_Work.Scripts
//{
class CBlock
{
    // list of trials / challenges
    List<CTrial> m_lsTrial = new List<CTrial>();

    // list of responses
    List<CResponse> m_lsResponse = new List<CResponse>();
}

class CUserTherapy : Singleton<CUserTherapy>
{
    /*#region singleton
    private static readonly CUserTherapy instance = new CUserTherapy();
    public static CUserTherapy Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion*/

    CRecommender m_recommender = new CRecommender();
    //CDataset m_dataset = new CDataset();
    List<CChallengeItem> m_lsChallengeItem;
    List<CChallengeItemFeatures> m_lsChallengeItemFeatures = new List<CChallengeItemFeatures>();

    // list of blocks;
    List<CBlock> m_lsBlock = new List<CBlock>();

    // list of trials / challenges
    List<CTrial> m_lsTrial = new List<CTrial>();

    // list of responses
    List<CResponse> m_lsResponse = new List<CResponse>();

    // index of the current displayed trial/challenge, zero-based
    int m_intCurIdx = -1;

    // end of game if user has achieved 60% (based on user's first response) correct of current level
    int m_intTotalCorrect = 0;

    // level start time, this is to keep track how long does the patient take to complete a level 
    DateTime m_dtCurLevelStartTime;

    //----------------------------------------------------------------------------------------------------
    // CUserTherapy
    //----------------------------------------------------------------------------------------------------
    public CUserTherapy()
    {
            
    }

    //----------------------------------------------------------------------------------------------------
    // LoadUserProfile 
    //----------------------------------------------------------------------------------------------------
    public void LoadDataset_UserProfile()
    {
        /*m_recommender.init(Application.persistentDataPath + "/", DatabaseXML.Instance.PatientId.ToString());
        m_lsChallengeItem = m_recommender.getChallengeItemList();
        m_lsChallengeItemFeatures = m_recommender.getChallengeItemFeaturesList();*/

        StartCoroutine("Coroutine_LoadDataset_UserProfile");

        // show total therapy time on main screen
        StateChallenge.Instance.SetTotalTherapyTime(getTotalTherapyTimeMin());
        StateChallenge.Instance.SetTodayTherapyTime(getTodayTherapyTimeMin());
    }

    //----------------------------------------------------------------------------------------------------
    // Coroutine_LoadDataset_UserProfile
    //----------------------------------------------------------------------------------------------------
    IEnumerator Coroutine_LoadDataset_UserProfile()
    {
        m_recommender.init(Application.persistentDataPath /*+ "/"*/, DatabaseXML.Instance.PatientId.ToString(), DatabaseXML.Instance.DatasetId);
        m_lsChallengeItem = m_recommender.getChallengeItemList();
        m_lsChallengeItemFeatures = m_recommender.getChallengeItemFeaturesList();

        yield return null;        
        Debug.Log("CUserTHerapy: Coroutine_LoadDataset_UserProfile()");
    }

    //----------------------------------------------------------------------------------------------------
    // LoadUserProfile - when switching users, dataset remains same, just need to reload user profiles
    //----------------------------------------------------------------------------------------------------
    /*public void LoadUserProfile()
    {
        m_recommender.loadUserProfile(Application.persistentDataPath + "/", DatabaseXML.Instance.PatientId.ToString());
        
        // show total therapy time on main screen
        StateChallenge.Instance.SetTherapyTime(getTotalTherapyTimeMin());
    }*/

    //----------------------------------------------------------------------------------------------------
    // GetCurNoiseLevel
    //----------------------------------------------------------------------------------------------------
    public int GetCurNoiseLevel()
    {
        return m_recommender.getCurNoiseLevel();
    }

    //----------------------------------------------------------------------------------------------------
    // LoadTrials
    //----------------------------------------------------------------------------------------------------
    public void LoadTrials()
    {
        //m_intCurLevel = intLevel;        

        m_lsTrial.Clear();
        m_lsResponse.Clear();

        List<int> lsIdx = m_recommender.getNextBlock();

        // for debugging
        /*lsIdx.Clear();
        lsIdx.Add(608); lsIdx.Add(1624); lsIdx.Add(1444); lsIdx.Add(1636); lsIdx.Add(588);
        lsIdx.Add(1408); lsIdx.Add(504); lsIdx.Add(2204); lsIdx.Add(1364); lsIdx.Add(1416);
        lsIdx.Add(1596); lsIdx.Add(172); lsIdx.Add(1412); lsIdx.Add(1420); lsIdx.Add(16);*/

        Debug.Log("lsIdx.Count = " + lsIdx.Count);
        if (lsIdx.Count < CConstants.g_intItemNumPerBlock)
        {
            return;
            //m_recommender.resetUserProfile();
            //lsIdx = m_recommender.getNextBlock();
        }
        for (var i = 0; i < lsIdx.Count; ++i)
        {
            CChallengeItemFeatures features = m_lsChallengeItemFeatures[lsIdx[i]];
            CChallengeItem challengeItem = m_lsChallengeItem[features.m_intChallengeItemIdx];

            CTrial trial = new CTrial();
            trial.m_intChallengeItemFeaturesIdx = lsIdx[i];
            trial.m_intChallengeItemIdx = features.m_intChallengeItemIdx;

            int intDistractorNum = convertDistractorNum(features.m_dComplexity_DistractorNum);

            List<int> lsPictureChoiceIdx = pickDistractors(intDistractorNum, challengeItem.m_lsPictureChoice);
            for (var j = 0; j < lsPictureChoiceIdx.Count; j++)
            {
                int intChoiceIdx = lsPictureChoiceIdx[j];
                CStimulus picChoice = new CStimulus();
                picChoice.intOriginalIdx = intChoiceIdx;
                picChoice.m_strName = challengeItem.m_lsPictureChoice[intChoiceIdx].m_strName;
                picChoice.m_strType = challengeItem.m_lsPictureChoice[intChoiceIdx].m_strType;
                picChoice.m_strPType = challengeItem.m_lsPictureChoice[intChoiceIdx].m_strPType;
                picChoice.m_strImage = challengeItem.m_lsPictureChoice[intChoiceIdx].m_strImageFile;
                trial.m_lsStimulus.Add(picChoice);
            }

            /*--------------------------------------------------------------------------------------
            // 2 foils
            if (intDistractorNum == 2)
            {
                // pick target & 1st foil = phonological
                for (var j = 0; j < 2; j++)
                {
                    CStimulus picChoice = new CStimulus();
                    picChoice.intOriginalIdx = j;
                    picChoice.m_strName = challengeItem.m_lsPictureChoice[j].m_strName;
                    picChoice.m_strType = challengeItem.m_lsPictureChoice[j].m_strType;
                    picChoice.m_strPType = challengeItem.m_lsPictureChoice[j].m_strPType;
                    picChoice.m_strImage = challengeItem.m_lsPictureChoice[j].m_strImageFile;
                    trial.m_lsStimulus.Add(picChoice);
                }

                // 2nd foil = semantic / foil 2 / 
                for (var j = 0; j < challengeItem.m_lsPictureChoice.Count; ++j)
                {
                    string strType = challengeItem.m_lsPictureChoice[j].m_strType.ToLower();
                    if ( (strType.Equals("s1")) || (strType.Equals("s2")) || (strType.Equals("s3")) || (strType.Equals("un")) || (strType.Equals("foil 2")) )
                    {
                        int intIdx1 = trial.m_lsStimulus.FindIndex(a => a.m_strType.Equals(strType));  // check if this has already been chosen
                        if (intIdx1 <= -1)
                        {
                            CStimulus picChoice2 = new CStimulus();
                            picChoice2.intOriginalIdx = j;
                            picChoice2.m_strName = challengeItem.m_lsPictureChoice[j].m_strName;
                            picChoice2.m_strType = challengeItem.m_lsPictureChoice[j].m_strType;
                            picChoice2.m_strPType = challengeItem.m_lsPictureChoice[j].m_strPType;
                            picChoice2.m_strImage = challengeItem.m_lsPictureChoice[j].m_strImageFile;
                            trial.m_lsStimulus.Add(picChoice2);
                            break;
                        }
                    }
                }
            }
            // 3 foils
            else if (intDistractorNum == 3)
            {
                // pick target & 1st & 2nd foil = phonological
                List<string> lsType = new List<string>();
                for (var j = 0; j < 3; ++j)
                {
                    CStimulus picChoice = new CStimulus();
                    picChoice.intOriginalIdx = j;
                    picChoice.m_strName = challengeItem.m_lsPictureChoice[j].m_strName;
                    picChoice.m_strType = challengeItem.m_lsPictureChoice[j].m_strType;
                    picChoice.m_strPType = challengeItem.m_lsPictureChoice[j].m_strPType;
                    picChoice.m_strImage = challengeItem.m_lsPictureChoice[j].m_strImageFile;
                    trial.m_lsStimulus.Add(picChoice);
                }

                // 3rd foil 
                for (var j = 0; j < challengeItem.m_lsPictureChoice.Count; ++j)
                {
                    string strType = challengeItem.m_lsPictureChoice[j].m_strType.ToLower();
                    if ((strType.Equals("s1")) || (strType.Equals("s2")) || (strType.Equals("s3")) || (strType.Equals("un")) || (strType.Equals("foil 2")) || (strType.Equals("foil 3")) || (strType.Equals("other")) )
                    {
                        int intIdx1 = trial.m_lsStimulus.FindIndex(a => a.m_strType.Equals(strType));  // check if this has already been chosen
                        if (intIdx1 <= -1)
                        {
                            CStimulus picChoice2 = new CStimulus();
                            picChoice2.intOriginalIdx = j;
                            picChoice2.m_strName = challengeItem.m_lsPictureChoice[j].m_strName;
                            picChoice2.m_strType = challengeItem.m_lsPictureChoice[j].m_strType;
                            picChoice2.m_strPType = challengeItem.m_lsPictureChoice[j].m_strPType;
                            picChoice2.m_strImage = challengeItem.m_lsPictureChoice[j].m_strImageFile;
                            trial.m_lsStimulus.Add(picChoice2);
                            break;
                        }
                    }
                }
            }
            else
            {
                for (var j = 0; j < intDistractorNum + 1; ++j)
                {
                    CStimulus picChoice = new CStimulus();
                    picChoice.intOriginalIdx = j;
                    picChoice.m_strName = challengeItem.m_lsPictureChoice[j].m_strName;
                    picChoice.m_strType = challengeItem.m_lsPictureChoice[j].m_strType;
                    picChoice.m_strPType = challengeItem.m_lsPictureChoice[j].m_strPType;
                    picChoice.m_strImage = challengeItem.m_lsPictureChoice[j].m_strImageFile;
                    trial.m_lsStimulus.Add(picChoice);
                    //Debug.Log("image = " + picChoice.m_strImage + ", ");
                }
            }
            ------------------------------------------------------------------------------*/

            string str = "";
            for (var j = 0; j < trial.m_lsStimulus.Count; ++j)            
                str = str + trial.m_lsStimulus[j].m_strType + ", ";
            Debug.Log("foil num = " + intDistractorNum + ", foil type = " + str);

            /*------------------------------------------------------------------
            // 2 foils - 1P, 1S; 3 foils - 2P, 1S; 4 foils - 2P, 2S; 5 foils - 1P, 1S; 
            if ( (intDistractorNum == 2) || (intDistractorNum == 3) )
            {
                // pick target & P1
                for (var j = 0; j < intDistractorNum; ++j)
                {
                    CStimulus picChoice = new CStimulus();
                    picChoice.m_strName = challengeItem.m_lsPictureChoice[j].m_strName;
                    picChoice.m_strType = challengeItem.m_lsPictureChoice[j].m_strType;
                    picChoice.m_strImage = challengeItem.m_lsPictureChoice[j].m_strImageFile;
                    trial.m_lsStimulus.Add(picChoice);
                }
                // pick S1                
                int intIdx1 = challengeItem.m_lsPictureChoice.FindIndex(a => a.m_strType.Equals("S1"));
                if ((intIdx1 <= -1) || (intIdx1 < intDistractorNum)) // some items has only 1 P1, and therefore S1 might have already been selected
                {
                    intIdx1 = challengeItem.m_lsPictureChoice.FindIndex(a => a.m_strType.Equals("S2"));
                    if ((intIdx1 <= -1) || (intIdx1 < intDistractorNum)) // some items has only 1 P1, and therefore S2 might have already been selected
                        intIdx1 = intDistractorNum;
                }
                CStimulus picChoice1 = new CStimulus();
                picChoice1.m_strName = challengeItem.m_lsPictureChoice[intIdx1].m_strName;
                picChoice1.m_strType = challengeItem.m_lsPictureChoice[intIdx1].m_strType;
                picChoice1.m_strImage = challengeItem.m_lsPictureChoice[intIdx1].m_strImageFile;
                trial.m_lsStimulus.Add(picChoice1);
            }
            else
            {
                for (var j = 0; j < intDistractorNum + 1; ++j)
                {
                    CStimulus picChoice = new CStimulus();
                    picChoice.m_strName = challengeItem.m_lsPictureChoice[j].m_strName;
                    picChoice.m_strType = challengeItem.m_lsPictureChoice[j].m_strType;
                    picChoice.m_strImage = challengeItem.m_lsPictureChoice[j].m_strImageFile;
                    trial.m_lsStimulus.Add(picChoice);
                    //Debug.Log("image = " + picChoice.m_strImage + ", ");
                }
            }
            -------------------------------------------------------------*/

            trial.m_intTargetIdx = challengeItem.m_intTargetIdx;

            // remove .wav
            // pick the first audio file
            for (var j = 0; j < challengeItem.m_lsAudioFile.Count; j++)
            {
                trial.m_strTargetAudio = challengeItem.m_lsAudioFile[j];
                int intIdx2 = challengeItem.m_lsAudioFile[j].IndexOf(".");
                if (intIdx2 >= 0)
                    trial.m_strTargetAudio = challengeItem.m_lsAudioFile[j].Substring(0, intIdx2);

                string strAudio = "Audio/phase1/" + trial.m_strTargetAudio;
                AudioClip clip = Resources.Load(strAudio) as AudioClip;
                if (clip != null) break;
            }
            // randomly pick an audio file
            /*System.Random rnd = new System.Random();
            int intIdx = rnd.Next(0, challengeItem.m_lsAudioFile.Count);
            trial.m_strTargetAudio = challengeItem.m_lsAudioFile[intIdx];
            int intIdx2 = challengeItem.m_lsAudioFile[intIdx].IndexOf(".");
            if (intIdx2 >= 0)
                trial.m_strTargetAudio = challengeItem.m_lsAudioFile[intIdx].Substring(0, intIdx2);*/

            m_lsTrial.Add(trial);
            //Debug.Log("target audio = " + trial.m_strTargetAudio); 
        }

        // shuffle stimuli in each trial
        for (var i = 0; i < m_lsTrial.Count; ++i)
            m_lsTrial[i].RandomizeStimuli();        

        //tell the score master how many questions we're asking
        ScoreMaster.SetQuestionAmount(m_lsTrial.Count);

        // add empty responses
        for (var i = 0; i < m_lsTrial.Count; ++i)
        {
            CResponse res = new CResponse();
            m_lsResponse.Add(res);
        }

        // reset current idx
        m_intCurIdx = -1;
        m_intTotalCorrect = 0;
    }

    //----------------------------------------------------------------------------------------------------
    // pickDistractors
    //----------------------------------------------------------------------------------------------------
    private List<int> pickDistractors(int intDistractorNum, List<CPictureChoice> lsPictureChoice)
    {
        List<int> lsIdx = new List<int>();

        List<int> lsPIdx = new List<int>();  // list of P1, P2, P3, foil 1
        List<int> lsSIdx = new List<int>();  // list of S1, S2, S3, foil 2, foil 3
        List<int> lsUnIdx = new List<int>();  // list of un, other 

        for (var i = 0; i < lsPictureChoice.Count; i++)
        {
            string strType = lsPictureChoice[i].m_strType.ToLower();
            strType.Trim();
            if ((strType.Equals("p1")) || (strType.Equals("p2")) || (strType.Equals("p3")) || (strType.Equals("foil 1")))
                lsPIdx.Add(i);
            else if ((strType.Equals("s1")) || (strType.Equals("s2")) || (strType.Equals("s3")) || (strType.Equals("foil 2")) || (strType.Equals("foil 3")))
                lsSIdx.Add(i);
            else if ((strType.Equals("un")) || (strType.Equals("other")) )
                lsUnIdx.Add(i);
        }        

        // randomize list
        for (int i = 0; i < lsPIdx.Count; i++)
        {
            int temp = lsPIdx[i];
            int intRandomIndex = UnityEngine.Random.Range(i, lsPIdx.Count);
            lsPIdx[i] = lsPIdx[intRandomIndex];
            lsPIdx[intRandomIndex] = temp;
        }
        for (int i = 0; i < lsSIdx.Count; i++)
        {
            int temp = lsSIdx[i];
            int intRandomIndex = UnityEngine.Random.Range(i, lsSIdx.Count);
            lsSIdx[i] = lsSIdx[intRandomIndex];
            lsSIdx[intRandomIndex] = temp;
        }
        for (int i = 0; i < lsUnIdx.Count; i++)
        {
            int temp = lsUnIdx[i];
            int intRandomIndex = UnityEngine.Random.Range(i, lsUnIdx.Count);
            lsUnIdx[i] = lsUnIdx[intRandomIndex];
            lsUnIdx[intRandomIndex] = temp;
        }

        // add target first
        lsIdx.Add(0);        

        // 2 foils
        if (intDistractorNum == 2)
        {
            // pick 1st foil = phonological & 2nd foil = semantic
            int intCtr = 0;
            for (int i = 0; i < lsPIdx.Count; i++)
            {
                lsIdx.Add(lsPIdx[i]);
                intCtr++;
                if (intCtr >= 1) break;
            }
            for (int i = 0; i < lsSIdx.Count; i++)
            {
                lsIdx.Add(lsSIdx[i]);
                intCtr++;
                if (intCtr >= 2) break;
            }
            if (intCtr < 2)
            {
                for (int i = 0; i < lsUnIdx.Count; i++)
                {
                    lsIdx.Add(lsUnIdx[i]);
                    intCtr++;
                    if (intCtr >= 2) break;
                }
            }
        }
        // 3 foils
        else if (intDistractorNum == 3)
        {
            // pick 1st foils = phonological & 2rd foil = semantic, 3rd foil = randomly phonological/semantic
            int intCtr = 0;
            int intLastPIdx = 0;
            for (int i = 0; i < lsPIdx.Count; i++)
            {
                lsIdx.Add(lsPIdx[i]);
                intLastPIdx = i;
                intCtr++;
                if (intCtr >= 1) break;
            }
            int intLastSIdx = 0;
            for (int i = 0; i < lsSIdx.Count; i++)
            {
                lsIdx.Add(lsSIdx[i]);
                intLastSIdx = i;
                intCtr++;
                if (intCtr >= 2) break;
            }
            // 3rd foil selected randomly from phonological / semantic
            System.Random rnd = new System.Random();
            int intType = rnd.Next(0, 2); // random 0, 1
            if (intType == 0)  // pick from phonological
            {
                for (int i = intLastPIdx+1; i < lsPIdx.Count; i++)
                {
                    lsIdx.Add(lsPIdx[i]);                    
                    intCtr++;
                    if (intCtr >= 3) break;
                }
            }
            else  // pick from semantic
            {
                for (int i = intLastSIdx+1; i < lsSIdx.Count; i++)
                {
                    lsIdx.Add(lsSIdx[i]);                    
                    intCtr++;
                    if (intCtr >= 3) break;
                }
            }
            if (intCtr < 3)  // if still not enough then pick from unrelated
            {
                for (int i = 0; i < lsUnIdx.Count; i++)
                {
                    lsIdx.Add(lsUnIdx[i]);
                    intCtr++;
                    if (intCtr >= 3) break;
                }
            }
        }
        // 4 foils
        else if (intDistractorNum == 4)
        {
            // pick 1st & 2nd foils = phonological & 3rd & 4th foils = semantic
            int intCtr = 0;
            for (int i = 0; i < lsPIdx.Count; i++)
            {
                lsIdx.Add(lsPIdx[i]);
                intCtr++;
                if (intCtr >= 2) break;
            }
            for (int i = 0; i < lsSIdx.Count; i++)
            {
                lsIdx.Add(lsSIdx[i]);
                intCtr++;
                if (intCtr >= 4) break;
            }
            if (intCtr < 4)
            {
                for (int i = 0; i < lsUnIdx.Count; i++)
                {
                    lsIdx.Add(lsUnIdx[i]);
                    intCtr++;
                    if (intCtr >= 4) break;
                }
            }
        }
        // 5 foils
        else
        {
            for (var j = 1; j < intDistractorNum + 1; ++j)            
                lsIdx.Add(j);    
        }       

        return lsIdx;
    }

    //----------------------------------------------------------------------------------------------------
    // convertDistractorNum
    //----------------------------------------------------------------------------------------------------
    private int convertDistractorNum(double dComplexityDistractorNum)
    {
        int intNum = 0;
        if (dComplexityDistractorNum == 0.1)
            intNum = 2;
        else if (dComplexityDistractorNum == 0.4)
            intNum = 3;
        else if (dComplexityDistractorNum == 0.7)
            intNum = 4;
        else if (dComplexityDistractorNum == 1)
            intNum = 5;
        return intNum;
    }

    //----------------------------------------------------------------------------------------------------
    // UpdateResponseList
    //----------------------------------------------------------------------------------------------------
    public void UpdateResponseList(CResponse response)
    {
        if (m_intCurIdx < 0)
            return; // no reponse to update 

        if (response.m_intScore > 0)
            m_intTotalCorrect++;

        m_lsResponse[m_intCurIdx].m_lsSelectedStimulusIdx = new List<int>(response.m_lsSelectedStimulusIdx);
        m_lsResponse[m_intCurIdx].m_fRTSec = response.m_fRTSec;
        m_lsResponse[m_intCurIdx].m_intReplayBtnCtr = response.m_intReplayBtnCtr;
        m_lsResponse[m_intCurIdx].m_intScore = response.m_intScore;
        m_lsResponse[m_intCurIdx].m_intCoinNum = response.m_intCoinNum;

        // insert db entry
        Dictionary<string, string> challenge_insert = new Dictionary<string, string>();

        challenge_insert.Add("patient", DatabaseXML.Instance.PatientId.ToString());
        challenge_insert.Add("date", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //challenge_insert.Add("category", "1");
        challenge_insert.Add("cif_idx", m_lsTrial[m_intCurIdx].m_intChallengeItemFeaturesIdx.ToString());

        string strStimOriIdx = "";        
        string strStimType = "";
        for (int i = 0; i < m_lsTrial[m_intCurIdx].m_lsStimulus.Count; i++)
        {
            strStimOriIdx = strStimOriIdx + m_lsTrial[m_intCurIdx].m_lsStimulus[i].intOriginalIdx + ",";
            strStimType = strStimType + m_lsTrial[m_intCurIdx].m_lsStimulus[i].m_strType + ",";
        }
        challenge_insert.Add("stim_ori_idx", strStimOriIdx);
        challenge_insert.Add("stim_type", strStimType);

        string strSelectedStimIdx = "";
        for (int i = 0; i < m_lsResponse[m_intCurIdx].m_lsSelectedStimulusIdx.Count; i++)
            strSelectedStimIdx = strSelectedStimIdx + m_lsResponse[m_intCurIdx].m_lsSelectedStimulusIdx[i] + ",";
        challenge_insert.Add("selected_stim_idx", strSelectedStimIdx);

        challenge_insert.Add("rt_sec", m_lsResponse[m_intCurIdx].m_fRTSec.ToString());
        challenge_insert.Add("accuracy", response.m_intScore.ToString());

        /*string foils = String.Empty;
        for (int i = 0; i < response.m_lsSelectedStimulusIdx.Count; i++)
        {
            foils += " ";
            foils += response.m_lsSelectedStimulusIdx[i].ToString();
        }
        challenge_insert.Add("foil_number", foils);
        challenge_insert.Add("foil_type", "1");
        challenge_insert.Add("image_list", "99");
        challenge_insert.Add("accuracy", response.m_intScore.ToString());*/

        DatabaseXML.Instance.WriteDatabaseXML(challenge_insert, DatabaseXML.Instance.therapy_challenge_insert);        

    }

    //----------------------------------------------------------------------------------------------------
    // IsEndOfLevel
    //----------------------------------------------------------------------------------------------------
    public bool IsEndOfLevel(bool bIsAdminMode)
    {
        m_intCurIdx++;
        if (m_intCurIdx >= m_lsTrial.Count)
        {
            if (!bIsAdminMode)
            {
                try
                { 
                    // save data
                    List<int> lsResponse = new List<int>();
                    for (int i = 0; i < m_lsResponse.Count; i++)
                    {
                        lsResponse.Add(m_lsResponse[i].m_intScore);                     
                    }
                    m_recommender.updateUserHistory(lsResponse);
                    //m_recommender.updateUserHistory(m_lsTrial, m_lsResponse);

                    // save total therapy time to db
                    SaveTherapyTimeToDB();

                    //AddCompletedLevel();
                    //UpdateStatisticDay();
                    SaveReactionTime();
                    SaveTrials();
                    //SaveUserProfile();

                    // save total therapy time to db
                    //SaveTherapyTimeToDB();
                }
                catch (System.Exception ex)
                {
                    //ListenIn.Logger.Instance.Log("CUserTherapy-IsEndOfLevel-" + ex.Message, ListenIn.LoggerMessageType.Info);
                    Debug.Log("CUserTherapy-IsEndOfLevel-" + ex.Message);
                }
            }
            return true;
        }
        return false;
    }

    //----------------------------------------------------------------------------------------------------
    // SaveTherapyTimeToDB
    //----------------------------------------------------------------------------------------------------
    private void SaveTherapyTimeToDB()
    {
        try
        { 
            // insert db entry
            Dictionary<string, string> time_insert = new Dictionary<string, string>();

            time_insert.Add("patientid", DatabaseXML.Instance.PatientId.ToString());
            time_insert.Add("date", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            time_insert.Add("totaltime", getTotalTherapyTimeMin().ToString());

            Debug.Log("CUserTherapy-SaveTherapyTimeToDB-DatabaseXML.Instance.WriteDatabaseXML start");

            DatabaseXML.Instance.WriteDatabaseXML(time_insert, DatabaseXML.Instance.therapy_time_insert);

            Debug.Log("CUserTherapy-SaveTherapyTimeToDB-DatabaseXML.Instance.WriteDatabaseXML end");
            
            //Andrea: 30/10 moved this on the uploadmanager

            //if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            //{
            //    //read the xml
            //    //DatabaseXML.Instance.ReadDatabaseXML();
            //    DatabaseXML.Instance.uploadHistoryXml();
            //}
        }
        catch (System.Exception ex)
        {
            //ListenIn.Logger.Instance.Log("CUserTherapy-SaveTherapyTimeToDB-" + ex.Message, ListenIn.LoggerMessageType.Info);
            Debug.Log("CUserTherapy-SaveTherapyTimeToDB-" + ex.Message);
        }
    }

    //----------------------------------------------------------------------------------------------------
    // SaveTrials
    //----------------------------------------------------------------------------------------------------
    private void SaveTrials()
    {
        try
        {            
            // Save the document to a file. White space is preserved (no white space).
            //string strXmlFile = Application.persistentDataPath + "/" + "user_" + DatabaseXML.Instance.PatientId.ToString() + "_therapyblocks_all.xml";
            string strXmlFile = System.IO.Path.Combine(Application.persistentDataPath, "user_" + DatabaseXML.Instance.PatientId.ToString() + "_therapyblocks_all.xml");

            string strXmlFileNew = strXmlFile + ".new";
            string strXmlFileOld = strXmlFile + ".old";

            Debug.Log("CUserTherapy-SaveTrials- load XmlDocument");

            XmlDocument doc = new XmlDocument();
            if (!System.IO.File.Exists(strXmlFile))
            {
                doc.LoadXml("<?xml version='1.0' encoding='utf-8'?>" +
                "<root>" +
                "</root>");
            }
            else
            {
                System.IO.FileInfo info = new System.IO.FileInfo(strXmlFile);
                if (info.Length == 0)
                {
                    // the file has corrupted file so recover from the backup folder
                    System.DateTime backup_date = System.DateTime.Now;
                    bool bFound = false;
                    int intCtr = 0;
                    string xml_backup_TherapyBlocksAll = "";
                    while (!bFound && intCtr < 10)
                    {
                        //backup_date = backup_date.AddDays(-1);
                        string strDate = backup_date.ToString("yyyy-MM-dd");
                        xml_backup_TherapyBlocksAll = Application.persistentDataPath + @"/ListenIn/Therapy/" + "user_" + DatabaseXML.Instance.PatientId.ToString() + "_therapyblocks_all-" + strDate + ".xml";

                        if (System.IO.File.Exists(xml_backup_TherapyBlocksAll))
                        {
                            System.IO.FileInfo info1 = new System.IO.FileInfo(xml_backup_TherapyBlocksAll);
                            if (info1.Length > 0)
                            {
                                bFound = true;
                                break;
                            }
                        }
                        backup_date = backup_date.AddDays(-1);
                    }  // end while

                    if (bFound)
                    {
                        System.IO.File.Copy(xml_backup_TherapyBlocksAll, strXmlFile, true);
                        doc.Load(strXmlFile);
                        Debug.Log(" ***** CUserTherapy-SaveTrials: FIX CORRUPTED THERAPY FILE ***** ");
                    }
                    else
                    {
                        doc.LoadXml("<?xml version='1.0' encoding='utf-8'?>" +
                        "<root>" +
                        "</root>");
                    }
                }
                else
                    doc.Load(strXmlFile);
            }

            /*
            <block idx="0">
                <trial idx="0" ciIdx="12" cifIdx="22" >
                    <stim idx="0" type="target" ptype="target" />
                    <stim idx="1" type="P1" ptype="assoc" />
                </trial>
                <res idx="0" score="" rt="" replay="">
                    <selected>0</selected>
                    <selected>2</selected>
                </res>                
            </block> 
            */

            Debug.Log("CUserTherapy-SaveTrials- append nodes");

            XmlElement xmlNode = doc.CreateElement("block");
            XmlAttribute attr = doc.CreateAttribute("idx");
            attr.Value = m_recommender.getLastTherapyBlockIdx().ToString();
            xmlNode.SetAttributeNode(attr);

            for (int i = 0; i < m_lsTrial.Count; i++)
            {                
                // add trials
                XmlElement xmlChild2 = doc.CreateElement("trial");
                XmlAttribute attr2 = doc.CreateAttribute("idx");
                attr2.Value = i.ToString();
                xmlChild2.SetAttributeNode(attr2);

                attr2 = doc.CreateAttribute("ciIdx");
                attr2.Value = m_lsTrial[i].m_intChallengeItemIdx.ToString();
                xmlChild2.SetAttributeNode(attr2);

                attr2 = doc.CreateAttribute("cifIdx");
                attr2.Value = m_lsTrial[i].m_intChallengeItemFeaturesIdx.ToString();
                xmlChild2.SetAttributeNode(attr2);

                for (var j = 0; j < m_lsTrial[i].m_lsStimulus.Count; j++)
                {
                    XmlElement xmlChild3 = doc.CreateElement("stim");
                    attr2 = doc.CreateAttribute("idx");
                    attr2.Value = j.ToString();
                    xmlChild3.SetAttributeNode(attr2);

                    attr2 = doc.CreateAttribute("oriIdx");
                    attr2.Value = m_lsTrial[i].m_lsStimulus[j].intOriginalIdx.ToString();
                    xmlChild3.SetAttributeNode(attr2);

                    attr2 = doc.CreateAttribute("t");
                    attr2.Value = m_lsTrial[i].m_lsStimulus[j].m_strType;
                    xmlChild3.SetAttributeNode(attr2);

                    attr2 = doc.CreateAttribute("pt");
                    attr2.Value = m_lsTrial[i].m_lsStimulus[j].m_strPType;
                    xmlChild3.SetAttributeNode(attr2);

                    xmlChild2.AppendChild(xmlChild3);
                }
                xmlNode.AppendChild(xmlChild2);           
            }

            for (int i = 0; i < m_lsResponse.Count; i++)
            {
                // add trials
                XmlElement xmlChild2 = doc.CreateElement("res");
                XmlAttribute attr2 = doc.CreateAttribute("idx");
                attr2.Value = i.ToString();
                xmlChild2.SetAttributeNode(attr2);

                attr2 = doc.CreateAttribute("score");
                attr2.Value = m_lsResponse[i].m_intScore.ToString();
                xmlChild2.SetAttributeNode(attr2);

                attr2 = doc.CreateAttribute("rt");
                attr2.Value = m_lsResponse[i].m_fRTSec.ToString();
                xmlChild2.SetAttributeNode(attr2);

                attr2 = doc.CreateAttribute("replay");
                attr2.Value = m_lsResponse[i].m_intReplayBtnCtr.ToString();
                xmlChild2.SetAttributeNode(attr2);

                for (var j = 0; j < m_lsResponse[i].m_lsSelectedStimulusIdx.Count; j++)
                {
                    XmlElement xmlChild3 = doc.CreateElement("stim");
                    xmlChild3.InnerText = m_lsResponse[i].m_lsSelectedStimulusIdx[j].ToString();            
                    xmlChild2.AppendChild(xmlChild3);
                }
                xmlNode.AppendChild(xmlChild2);
            }

            doc.DocumentElement.AppendChild(xmlNode);

            //doc.PreserveWhitespace = true;
            //doc.Save(strXmlFile);
            try
            {
                // Write to file.txt.new
                // Move file.txt to file.txt.old
                // Move file.txt.new to file.txt
                // Delete file.txt.old
                //doc.PreserveWhitespace = true;

                Debug.Log("CUserTherapy-SaveTrials- save XmlDocument");

                doc.Save(strXmlFileNew);
                if (System.IO.File.Exists(strXmlFile))
                    System.IO.File.Move(strXmlFile, strXmlFileOld);
                System.IO.File.Move(strXmlFileNew, strXmlFile);

                string strXmlFile_ = System.IO.Path.Combine(Application.persistentDataPath, "user_" + DatabaseXML.Instance.PatientId.ToString() + "_therapyblocks_all_.xml");
                System.IO.File.Copy(strXmlFile, strXmlFile_, true);

                // backup
                string strDate = System.DateTime.Now.ToString("yyyy-MM-dd");                
                string xml_backup = Application.persistentDataPath + @"/ListenIn/Therapy/" + "user_" + DatabaseXML.Instance.PatientId.ToString() + "_therapyblocks_all-" + strDate + ".xml";
                if (System.IO.File.Exists(strXmlFileOld))
                {
                    System.IO.File.Copy(strXmlFileOld, xml_backup, true);
                    System.IO.File.Delete(strXmlFileOld);
                }
            }
            catch (System.Xml.XmlException ex)
            {
                //ListenIn.Logger.Instance.Log("CUserTherapy-SaveTrials-" + ex.Message, ListenIn.LoggerMessageType.Info);
                Debug.Log("CUserTherapy-SaveTrials-" + ex.Message);
            }
            catch (Exception e)
            {
                //Console.WriteLine("The process failed: {0}", e.ToString());
                //ListenIn.Logger.Instance.Log("CUserTherapy-SaveTrials-" + e.ToString(), ListenIn.LoggerMessageType.Info);
                Debug.Log("CUserTherapy-SaveTrials-" + e.ToString());
            }
        }
        catch (System.Exception ex)
        {
            ListenIn.Logger.Instance.Log("CUserTherapy-SaveTrials-" + ex.Message, ListenIn.LoggerMessageType.Info);
            Debug.Log("CUserTherapy-SaveTrials-" + ex.Message);
        }
    }

    //----------------------------------------------------------------------------------------------------
    // SaveReactionTime
    //----------------------------------------------------------------------------------------------------
    private void SaveReactionTime()
    {
        try
        {
            // Save the document to a file. White space is preserved (no white space).
            //string strXmlFile = Application.persistentDataPath + "/" + "user_" + DatabaseXML.Instance.PatientId.ToString() + "_therapyblocks_all.xml";
            string strXmlFile = System.IO.Path.Combine(Application.persistentDataPath, "user_" + DatabaseXML.Instance.PatientId.ToString() + "_therapyblocks_rt.xml");

            string strXmlFileNew = strXmlFile + ".new";
            string strXmlFileOld = strXmlFile + ".old";

            Debug.Log("CUserTherapy-SaveReactionTime - load XmlDocument");

            XmlDocument doc = new XmlDocument();
            if (!System.IO.File.Exists(strXmlFile))
            {
                doc.LoadXml("<?xml version='1.0' encoding='utf-8'?>" +
                "<root>" +
                "</root>");
            }
            else
            {
                System.IO.FileInfo info = new System.IO.FileInfo(strXmlFile);
                if (info.Length == 0)
                {
                    // the file has corrupted file so recover from the backup folder
                    System.DateTime backup_date = System.DateTime.Now;
                    bool bFound = false;
                    int intCtr = 0;
                    string xml_backup_TherapyBlocksAll_Rt = "";
                    while (!bFound && intCtr < 10)
                    {
                        //backup_date = backup_date.AddDays(-1);
                        string strDate = backup_date.ToString("yyyy-MM-dd");
                        xml_backup_TherapyBlocksAll_Rt = Application.persistentDataPath + @"/ListenIn/Therapy/" + "user_" + DatabaseXML.Instance.PatientId.ToString() + "_therapyblocks_rt-" + strDate + ".xml";

                        if (System.IO.File.Exists(xml_backup_TherapyBlocksAll_Rt))
                        {
                            System.IO.FileInfo info1 = new System.IO.FileInfo(xml_backup_TherapyBlocksAll_Rt);                            
                            if (info1.Length > 0) 
                            {
                                bFound = true;
                                break;
                            }
                        }
                        backup_date = backup_date.AddDays(-1);
                    }  // end while

                    if (bFound)
                    {
                        System.IO.File.Copy(xml_backup_TherapyBlocksAll_Rt, strXmlFile, true);
                        doc.Load(strXmlFile);
                        Debug.Log(" ***** CUserTherapy-SaveReactionTime: FIX CORRUPTED THERAPY FILE ***** ");
                    }
                    else
                    {
                        doc.LoadXml("<?xml version='1.0' encoding='utf-8'?>" +
                        "<root>" +
                        "</root>");
                    }
                }
                else
                    doc.Load(strXmlFile);
            }

            /*
            <block idx="0">                
                <res idx="0" rt="" />    
            </block> 
            */

            Debug.Log("CUserTherapy-SaveReactionTime- append nodes");

            XmlElement xmlNode = doc.CreateElement("block");
            XmlAttribute attr = doc.CreateAttribute("idx");
            attr.Value = m_recommender.getLastTherapyBlockIdx().ToString();
            xmlNode.SetAttributeNode(attr);            

            for (int i = 0; i < m_lsResponse.Count; i++)
            {
                // add trials
                XmlElement xmlChild2 = doc.CreateElement("res");
                XmlAttribute attr2 = doc.CreateAttribute("i");
                attr2.Value = i.ToString();
                xmlChild2.SetAttributeNode(attr2);                

                attr2 = doc.CreateAttribute("rt");
                attr2.Value = m_lsResponse[i].m_fRTSec.ToString();
                xmlChild2.SetAttributeNode(attr2);
                               
                xmlNode.AppendChild(xmlChild2);
            }

            doc.DocumentElement.AppendChild(xmlNode);

            //doc.PreserveWhitespace = true;
            //doc.Save(strXmlFile);
            try
            {
                // Write to file.txt.new
                // Move file.txt to file.txt.old
                // Move file.txt.new to file.txt
                // Delete file.txt.old
                //doc.PreserveWhitespace = true;

                Debug.Log("CUserTherapy-SaveReactionTime- save XmlDocument");

                doc.Save(strXmlFileNew);
                if (System.IO.File.Exists(strXmlFile))
                    System.IO.File.Move(strXmlFile, strXmlFileOld);
                System.IO.File.Move(strXmlFileNew, strXmlFile);

                string strXmlFile_ = System.IO.Path.Combine(Application.persistentDataPath, "user_" + DatabaseXML.Instance.PatientId.ToString() + "_therapyblocks_rt_.xml");
                System.IO.File.Copy(strXmlFile, strXmlFile_, true);

                // backup
                string strDate = System.DateTime.Now.ToString("yyyy-MM-dd");
                string xml_backup = Application.persistentDataPath + @"/ListenIn/Therapy/" + "user_" + DatabaseXML.Instance.PatientId.ToString() + "_therapyblocks_rt-" + strDate + ".xml";
                if (System.IO.File.Exists(strXmlFileOld))
                {
                    System.IO.File.Copy(strXmlFileOld, xml_backup, true);
                    System.IO.File.Delete(strXmlFileOld);
                }
            }
            catch (System.Xml.XmlException ex)
            {
                Debug.Log("CUserTherapy-SaveReactionTime-" + ex.Message);
            }
            catch (Exception e)
            {
                Debug.Log("CUserTherapy-SaveReactionTime-" + e.ToString());
            }
        }
        catch (System.Exception ex)
        {
            ListenIn.Logger.Instance.Log("CUserTherapy-SaveReactionTime-" + ex.Message, ListenIn.LoggerMessageType.Info);
            Debug.Log("CUserTherapy-SaveReactionTime-" + ex.Message);
        }
    }

    //----------------------------------------------------------------------------------------------------
    // GetNextTrial
    //----------------------------------------------------------------------------------------------------
    public CTrial GetNextTrial()
    {
        if (m_intCurIdx >= m_lsTrial.Count)
            return null;

        if (m_intCurIdx == 0)
            m_dtCurLevelStartTime = DateTime.Now;

        return m_lsTrial[m_intCurIdx];
    }

    //----------------------------------------------------------------------------------------------------
    // getTotalTherapyTime
    //----------------------------------------------------------------------------------------------------
    public double getTotalTherapyTimeMin()
    {
        return m_recommender.getTotalTherapyTimeMin();
    }

    //----------------------------------------------------------------------------------------------------
    // getTodayTherapyTime
    //----------------------------------------------------------------------------------------------------
    public double getTodayTherapyTimeMin()
    {
        return m_recommender.getTodayTherapyTimeMin();
    }

}
//}
