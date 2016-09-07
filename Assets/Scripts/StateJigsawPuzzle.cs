using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

//integrating madlevelmanager
using MadLevelManager;

public class StateJigsawPuzzle : State
{
    #region singleton
    private static readonly StateJigsawPuzzle instance = new StateJigsawPuzzle();
    public static StateJigsawPuzzle Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    #region members
    private GameObject jigsawObject = null;
    private ChapterSelectMono m_chapterSelectMono;
    private ChapterIconMono m_chapterIconMono;
    public int currLevelPinball = -1;

    private GameObject jigsawUnlocked;
    private List<GameObject> lerpers = new List<GameObject>();
    private bool activateJigsawAnimation;
    private bool animating = false;
    private bool rewardAnimation = false;
    private bool returnToSelectScreen = false;
    private bool startTimer = false;
    private float initialTime;
    private float secToWait = 3.5f;
    private ParticleSystem endSystemEffect;

    private int m_patientId = -1;

    public Dictionary<string, Chapter> Chapters;

    //Every jigsawpuzzle will be just one level
    private Chapter currChapter;    
    public Chapter CurrChapter
    {
        get { return currChapter; }
        set { currChapter = value; }
    }


    // This initialization is when the game starts, for loading in memory the different levels
    private bool m_Init = false;
    #endregion


    private void InitializeChapters(){
        Chapters = new Dictionary<string, Chapter>();
        Chapters.Add("Space Travel", (new Chapter("Space", 2, "Background_002", "Postcards/Postcard_Space", Color.black, false, "")));
        Chapters.Add("Cheeky Jungle", (new Chapter("Cheeky Jungle", 3, "Background_008", "Postcards/Postcard_CheekyJungle", Color.red, false, "")));
        Chapters.Add("Candy Madness", (new Chapter("Candy Island", 1, "Background_009", "Postcards/Postcard_Candyland", new Color32(255, 0, 162, 255), false, "")));
        Chapters.Add("Cookie Monster", (new Chapter("Cookie Monsters", 4, "Background_011", "Postcards/Postcard_CookieMonsters", new Color32(255, 221, 0, 255), false, "")));
        Chapters.Add("Tricky Lights", (new Chapter("Tricky Lights", 5, "Background_014", "Postcards/Postcard_TrickyLights", Color.black, false, "")));
        Chapters.Add("Tibet", (new Chapter("Tibet", 6, "Background_015", "Postcards/Postcard_Tibet", Color.white, false, "")));
        Chapters.Add("Halloween", (new Chapter("Halloween", 7, "Background_003", "Postcards/Postcard_Halloween", new Color(0.686275f, 0.933333f, 0.933333f), false, "")));
        Chapters.Add("Fun Fair", (new Chapter("Fun Fair", 8, "Background_016", "Postcards/Postcard_FunFair", Color.white, false, "")));
        Chapters.Add("Far Islands", (new Chapter("Fancy Island", 9, "Background_004", "Postcards/Postcard_FancyIsland", Color.yellow, false, "")));
        Chapters.Add("Great Canyon", (new Chapter("Grand Canyon", 10, "Background_005", "Postcards/Postcard_GrandCanyon", new Color(0.498039f, 1f, 0.831373f), false, "")));
        Chapters.Add("Underworld", (new Chapter("Underwater", 11, "Background_006", "Postcards/Postcard_UnderWater", Color.cyan, false, "")));
        Chapters.Add("Artic View", (new Chapter("Green Land", 12, "Background_007", "Postcards/Postcard_Greenland", Color.green, false, "")));
        Chapters.Add("Wonder Land", (new Chapter("Wonderland", 13, "Background_010", "Postcards/Postcard_Wonderland", new Color32(36, 145, 0, 255), false, "")));

        Chapters.Add("Fuji Mountain", (new Chapter("Fuji", 14, "Background_012", "Postcards/Postcard_Fuji", new Color32(255, 0, 221, 255), false, "")));
        Chapters.Add("Moscow", (new Chapter("Moscow", 15, "Background_013", "Postcards/Postcard_Moscow", new Color32(175, 0, 0, 255), false, "")));

        Chapters.Add("Dodgeball", (new Chapter("Basketball", 16, "Background_017", "Postcards/Postcard_Basketball", Color.blue, false, "")));
        Chapters.Add("Great Coliseum", (new Chapter("Colosseum", 17, "Background_018", "Postcards/Postcard_Colosseum", Color.magenta, false, "")));
        Chapters.Add("Egypt", (new Chapter("Pyramids", 18, "Background_019", "Postcards/Postcard_Egypt", Color.yellow, false, "")));
		Chapters.Add("Fearless Ski", (new Chapter("Fearless Ski", 19, "Background_021", "Postcards/Postcard_SkiAlps", Color.blue, false, "")));
        Chapters.Add("London", (new Chapter("London", 20, "Background_001", "Postcards/Postcard_London", new Color(0f, 0f, 0.803922f), false, "")));
        
    }

	private void ChapterInitialization(){
        //Curr Chapter initialization
        if (Chapters.ContainsKey(MadLevel.arguments))
        {
            Chapters.TryGetValue(MadLevel.arguments, out currChapter);
            currChapter.UnLocked = true;

            m_chapterIconMono.OnPressedButton += PressButton;
            currChapter.Mono = m_chapterIconMono;
            currLevelPinball = currChapter.LevelNumber;
            //Setting Images

            currChapter.Mono.m_nextLvl_Image = Resources.Load<Sprite>("PinballBackgrounds/" + currChapter.nextLvlPictureName);
            currChapter.Mono.m_nextLvl_Image_small = Resources.Load<Sprite>("PinballBackgrounds/" + currChapter.nextLvlPictureName + "_small");
            currChapter.Mono.Completed_Image.sprite = Resources.Load<Sprite>("PinballBackgrounds/" + currChapter.nextLvlPictureName);

            //Setting Name
            currChapter.Mono.Title.text = currChapter.Name;
            {
                currChapter.Mono.Title.GetComponent<Text>().color = currChapter.TitleColor;
            }

            currChapter.Mono.JigsawPieceRoot.SetActive(true);
            currChapter.Mono.Completed_Image.gameObject.SetActive(false);

            for (int j = 0; j < currChapter.JigsawPeicesUnlocked.Length; j++)
            {
                currChapter.Mono.JigsawPieces[j].transform.GetChild(0).GetComponent<Image>().sprite = currChapter.Mono.m_nextLvl_Image;
                Color temp = currChapter.Mono.JigsawPieces[j].transform.GetChild(0).GetComponent<Image>().color;
                temp.a = 0.4f;
                {
                    if (currChapter.JigsawPeicesUnlocked[j] == 1.0f && !currChapter.JigsawPiecesToUnlock[j])
                    {
                        temp.a = 1.0f;
                    }
                }
                currChapter.Mono.JigsawPieces[j].transform.GetChild(0).GetComponent<Image>().color = temp;
            }

            currChapter.Mono.PlayButton.GetComponent<PlayButtonAnimator>().ActivePlay = true;

            m_chapterSelectMono.DemoButton.SetActive(MadLevel.arguments.Equals("Great Canyon"));

        }
        else
        {
            Debug.LogError("Level " + MadLevel.arguments + " not found!");
        }
    }

    //private void LoadSavedJigsaw()
    //{
    //    try
    //    {
    //        GameStateSaver.Instance.Load();
    //    }
    //    catch (System.Exception ex)
    //    {
    //        Debug.LogError(ex.Message);
    //        GameStateSaver.Instance.ResetListenIn();
    //    }
    //}

    /// <summary>
    /// This is called once and for after the game is loaded
    /// </summary>
    public void OnGameLoadedInitialization()
    {
        InitializeChapters();
        //LoadSavedJigsaw();
        m_patientId = DatabaseXML.Instance.PatientId;
        m_Init = true;
    }

	// Use this for initialization
	public override void Init () {

        if(jigsawObject == null)
        {
            jigsawObject = GameObject.Find("JigsawPuzzle");
        }

        jigsawObject.SetActive(true);
        m_chapterSelectMono = jigsawObject.GetComponent<ChapterSelectMono>();
        m_chapterIconMono = m_chapterSelectMono.GetComponentInChildren<ChapterIconMono>();

        //Resetting animation variables
        activateJigsawAnimation = false;
        animating = false;
        rewardAnimation = false;
        endSystemEffect = null;

        if (!activateReward)
        {
            ChapterInitialization();
            SetCurrentBadge();
            SetCurrentLevelDifficulty();
        }
        else
        {
            activateReward = false;
            jigsawUnlocked = GameObject.Find("JiggsawEarned(Clone)");
            if (jigsawUnlocked != null && jigsawUnlocked.transform.childCount != 0)
            {
                activateJigsawAnimation = true;
            }
            else
            {
                returnToSelectScreen = true;
            }
            currChapter.Mono.PlayButton.SetActive(false);
        }    

    }

    // Update is called once per frame
    public override void Update() {

        if (activateJigsawAnimation)
        {
            activateJigsawAnimation = false;
            animating = true;
            StartAnimation();
        }
        else if (animating && CheckLerperAnimationEnded())
        {
            FinishJigsawTransition();
            CleanLerpers();
            animating = false;
        }
        else if (endSystemEffect != null && rewardAnimation && !endSystemEffect.isPlaying)
        {
            animating = false;
            EndRewardAnimation();
            UnlockBadge();
            CleanJigsawPiecesValue();
            rewardAnimation = false;
            returnToSelectScreen = true;
        }
        else if (returnToSelectScreen)
        {
            returnToSelectScreen = false;
            startTimer = true;
            initialTime = Time.time;
            m_chapterSelectMono.SavePinballGame();
        }
        else if (startTimer)
        {
            if( (Time.time - initialTime) > secToWait)
            {
                startTimer = false;
                MadLevel.LoadLevelByName("World Map Select");
            }
        }
    }   

    public override void Exit()
    {
        m_chapterIconMono.OnPressedButton -= PressButton;
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GameLoop")
        {   
            if(jigsawObject != null)       
                jigsawObject.SetActive(false);
        }

        DatabaseXML.Instance.SetTimerState(DatabaseXML.TimerType.WorldMap, false);
    }

    #region properties
    private bool activateReward;

    public bool ActivateReward
    {
        get { return activateReward; }
        set { activateReward = value; }
    }

    #endregion

    #region methods

    private void CleanJigsawPiecesValue()
    {
        for (int j = 0; j < currChapter.JigsawPeicesUnlocked.Length; j++)
        {
            currChapter.JigsawPiecesToUnlock[j] = false;
            currChapter.JigsawPeicesUnlocked[j] = 0.0f;
        }
    }

    private void SetCurrentLevelDifficulty()
    {
        StatePinball.Instance.currDifficulty = ListenIn.LevelDifficulty.Easy;
        if (MadLevelProfile.GetLevelBoolean(MadLevel.currentLevelName, "jigsaw_2") || MadLevelProfile.GetLevelBoolean(MadLevel.currentLevelName, "jigsaw_3"))
        {
            StatePinball.Instance.currDifficulty = ListenIn.LevelDifficulty.Hard;
        }
        else if (MadLevelProfile.GetLevelBoolean(MadLevel.currentLevelName, "jigsaw_1"))
        {
            StatePinball.Instance.currDifficulty = ListenIn.LevelDifficulty.Medium;
        }
    }

    private void SetCurrentBadge()
    {        

        if (MadLevelProfile.GetLevelBoolean(MadLevel.currentLevelName, "jigsaw_3"))
        {
            Color col = currChapter.Mono.GoldReward.color;
            col.a = 1.0f;
            currChapter.Mono.GoldReward.color = col;
        }

        if (MadLevelProfile.GetLevelBoolean(MadLevel.currentLevelName, "jigsaw_2"))
        {
            Color col = currChapter.Mono.SilverReward.color;
            col.a = 1.0f;
            currChapter.Mono.SilverReward.color = col;
        }

        if (MadLevelProfile.GetLevelBoolean(MadLevel.currentLevelName, "jigsaw_1"))
        {
            Color col = currChapter.Mono.BronzeReward.color;
            col.a = 1.0f;
            currChapter.Mono.BronzeReward.color = col;            
        }
    }

    private void UnlockBadge()
    {
        // set level boolean sets level property of type boolean
        if (!MadLevelProfile.IsCompleted(MadLevel.currentLevelName))
        {
            MadLevelProfile.SetCompleted(MadLevel.currentLevelName, true);
        }

        if (!MadLevelProfile.GetLevelBoolean(MadLevel.currentLevelName, "jigsaw_1"))
        {
            MadLevelProfile.SetLevelBoolean(MadLevel.currentLevelName, "jigsaw_1", true);
            return;
        }

        if (!MadLevelProfile.GetLevelBoolean(MadLevel.currentLevelName, "jigsaw_2"))
        {
            MadLevelProfile.SetLevelBoolean(MadLevel.currentLevelName, "jigsaw_2", true);
            return;
        }

        if (!MadLevelProfile.GetLevelBoolean(MadLevel.currentLevelName, "jigsaw_3"))
        {
            MadLevelProfile.SetLevelBoolean(MadLevel.currentLevelName, "jigsaw_3", true);
            return;
        }
    }

    //this is used in combination with the background image animator and EndRewardAnimation function
    public void SwapBackgroundImage()
    {
        CleanLerpers();
        currChapter.Mono.JigsawPieceRoot.SetActive(false);
        currChapter.Mono.Completed_Image.gameObject.SetActive(true);
    }

    public void SetCapToJigsawProgression(int pieceToUnlock, float threshold)
    {
        currChapter.JigsawPeicesUnlocked[pieceToUnlock] = Mathf.Min(currChapter.JigsawPeicesUnlocked[pieceToUnlock] + (1.0f / threshold), 1.0f);
    }

    public void RecordPieceToUnlock(int pieceToUnlock)
    {
        currChapter.JigsawPiecesToUnlock[pieceToUnlock] = true;
    }

    private void FinishJigsawTransition()
    {
        for (int j = 0; j < currChapter.JigsawPiecesToUnlock.Length; j++)
        {
            if (currChapter.JigsawPiecesToUnlock[j])
            {
                currChapter.JigsawPiecesToUnlock[j] = false;
                currChapter.JigsawPeicesUnlocked[j] = 1.0f;
                currChapter.Mono.JigsawPieces[j].transform.GetChild(0).GetComponent<Image>().sprite = currChapter.Mono.m_nextLvl_Image;
                Color tempColor = currChapter.Mono.JigsawPieces[j].transform.GetChild(0).GetComponent<Image>().color;
                tempColor.a = 1.0f;
                currChapter.Mono.JigsawPieces[j].transform.GetChild(0).GetComponent<Image>().color = tempColor;
            }
        }

        UnityEngine.GameObject.DestroyImmediate(jigsawUnlocked);

        if (currChapter.AllPiecesUnlocked())
        {
            currChapter.Mono.PlayButton.SetActive(false);
            m_chapterSelectMono.EndPuzzleEffect.SetActive(true);
            m_chapterSelectMono.EndPuzzleEffect.GetComponent<Animator>().SetTrigger("StartTrophyAnim");

            GameObject.Find("Main Camera").GetComponent<SoundManager>().Play("TrophyWin");

            endSystemEffect = GameObject.FindGameObjectWithTag("ParticleSystemEnd").GetComponent<ParticleSystem>();

            rewardAnimation = true;

        }
        else
        {
            jigsawObject = null;
            returnToSelectScreen = true;
            //MadLevel.LoadLevelByName("World Map Select");
        }
    }

    private bool CheckLerperAnimationEnded()
    {
        if (lerpers.Count == 0)
            return false;

        foreach (GameObject item in lerpers)
        {
            if (!item.GetComponent<TransformLerper>().animationEnded)
            {
                return false;
            }
        }
        return true;
    }

    private void StartAnimation()
    {
        for (int i = 0; i < jigsawUnlocked.transform.childCount; i++)
        {
            GameObject jigCanvas = jigsawUnlocked.transform.GetChild(i).gameObject;

            GameObject lerp = new GameObject("Lerper");
            TransformLerper tr = lerp.AddComponent<TransformLerper>();
            tr.Init();
            tr.usingRectTransform = true;
            tr.isLocal = true;
            tr.slerpingIsActive = true;
            tr.singleStepDuration = 0.5f;
            tr.ObjectToLerp = jigCanvas.GetComponentInChildren<Canvas>().gameObject.transform.GetChild(0).gameObject;

            RectTransform jig = jigCanvas.GetComponentInChildren<Canvas>().gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
            tr.AddPosition(jig.localPosition);
            tr.AddScaler(jig.localScale);
            Vector3 finalPos = currChapter.Mono.JigsawPieces[jigCanvas.GetComponentInChildren<JigsawInfo>().Index].GetComponent<RectTransform>().localPosition;
            tr.AddPosition(finalPos);
            tr.AddScaler(new Vector3(1.0f, 1.0f));

            tr.StartAnimation();

            lerpers.Add(lerp);
        }
    }

    private void EndRewardAnimation()
    {
        currChapter.Completed = true;
        m_chapterSelectMono.EndPuzzleEffect.SetActive(false);
    }

    private IEnumerator BackToSelectScreen()
    {
        yield return new WaitForSeconds(15);
        Debug.Log("Going back to select screen");
        MadLevel.LoadLevelByName("World Map Select");
    }

    private void CleanLerpers()
    {
        foreach (var lerp in lerpers)
        {
            GameObject.Destroy(lerp);
        }
        lerpers.Clear();
    }
    #endregion methods

    #region events_holder
    public void PressButton(int ID)
    {
        //To put it back should not need it since once back on the scene it will just be disabled
        //if (animating)
        //{
        //    return;
        //}

        Dictionary<string, string> dailyTherapy = new Dictionary<string, string>();

        int patient = DatabaseXML.Instance.PatientId;
        DateTime now = System.DateTime.Now;
        
        dailyTherapy.Add("patient", patient.ToString());
        dailyTherapy.Add("level_start", currLevelPinball.ToString());        
        dailyTherapy.Add("date", now.ToString("yyyy-MM-dd HH:mm:ss"));

        DatabaseXML.Instance.WriteDatabaseXML(dailyTherapy,DatabaseXML.Instance.therapy_daily_insert);

        dailyTherapy.Clear();

        dailyTherapy.Add("patient", patient.ToString());
        dailyTherapy.Add("date", now.ToString("yyyy-MM-dd HH:mm:ss"));

        DatabaseXML.Instance.WriteDatabaseXML(dailyTherapy, DatabaseXML.Instance.therapy_session_insert);

        StatePinball.Instance.ID = currLevelPinball;//m_Chapters[ID].LevelNumber;
        GameController.Instance.ChangeState(GameController.States.StateChallenge);

    }
    #endregion events_holder


}
