using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Text;

using MadLevelManager;

public class DatabaseXML : Singleton<DatabaseXML> {

    //Andrea is using ID 1 for internal testing
    public int PatientId = 1;
    public int DatasetId = 0;
    public TextAsset database_xml_file;
    //create an XML file to keep and read it
    public XmlDocument database_xml;
    //form to send thq ueries
    WWWForm xml_form;
    string xml_query_url;
    //xml file directory
    //for android Application.persistentDataPath
    string xml_file;
    string xml_location;
    //queue for the forms to insert
    Queue<DatabaseQuery> xml_forms_queue;
    bool insert_in_order;
    //urls
    //test
    /*public string therapy_daily_insert = "https://ageofalgo.com/LI/therapy_daily_insert.php";
    public string therapy_daily_update = "https://ageofalgo.com/LI/therapy_daily_update.php";
    public string therapy_session_insert = "https://ageofalgo.com/LI/therapy_session_insert.php";
    public string therapy_session_update = "https://ageofalgo.com/LI/therapy_session_update.php";
    public string therapy_challenge_insert = "https://ageofalgo.com/LI/therapy_challenge_insert.php";
    public string therapy_challenge_update = "https://ageofalgo.com/LI/therapy_challenge_update.php";
    public string therapy_block_insert = "https://ageofalgo.com/LI/therapy_block_insert.php"; 
    public string insert_patient_progress = "https://ageofalgo.com/LI/patient_game_update.php";
    public string select_patient_progress = "https://ageofalgo.com/LI/patient_game_select.php";
    */
    public string therapy_daily_insert = "http://italk.ucl.ac.uk/listenin_dev/therapy_daily_insert.php";
    public string therapy_daily_update = "http://italk.ucl.ac.uk/listenin_dev/therapy_daily_update.php";
    public string therapy_session_insert = "http://italk.ucl.ac.uk/listenin_dev/therapy_session_insert.php";
    public string therapy_session_update = "http://italk.ucl.ac.uk/listenin_dev/therapy_session_update.php";
    public string therapy_challenge_insert = "http://italk.ucl.ac.uk/listenin_dev/therapy_challenge_insert.php";
    public string therapy_challenge_update = "http://italk.ucl.ac.uk/listenin_dev/therapy_challenge_update.php";
    public string therapy_block_insert = "http://italk.ucl.ac.uk/listenin_dev/therapy_block_insert.php";
    public string insert_patient_progress = "http://italk.ucl.ac.uk/listenin_dev/patient_game_update.php";
    public string select_patient_progress = "http://italk.ucl.ac.uk/listenin_dev/patient_game_select.php";

    public string therapy_time_insert = "http://italk.ucl.ac.uk/listenin_dev/therapy_time_insert.php";
    public string game_time_insert = "http://italk.ucl.ac.uk/listenin_dev/game_time_insert.php";
    public string select_patient_datasetid = "http://italk.ucl.ac.uk/listenin_dev/patient_datasetid_select.php";
    public string therapy_history_insert = "http://italk.ucl.ac.uk/listenin_dev/therapy_history_insert.php";

    //timers
    float therapy_time = 0;
    float therapy_pinball_time = 0;
    float therapy_worldmap_time = 0;
    bool count_therapy_time = false;
    bool count_pinball_time = false;
    bool count_worlmap_time = false;
    bool isMenuPaused = false;
    public bool SetIsMenu { get { return isMenuPaused; } set { isMenuPaused = value; } }
    private int reasonToExit = 0;
    public int ReasonToExit { get { return reasonToExit; } set { reasonToExit = value; } }
    //iddle time
    float idle_time = 0;
    bool count_iddle_time = false;
    //insert daily threapy once
    bool therapy_daily_inserted = false;

    public void InitializeDatabase()
    {
        Debug.Log(Application.persistentDataPath);
        xml_file = Application.persistentDataPath + @"/ListenIn/Database/database.xml";
        xml_location = Application.persistentDataPath + @"/ListenIn/Database/";

        //create the document
        database_xml = new XmlDocument();

        //check if directory doesn't exit
        if (!Directory.Exists(Application.persistentDataPath + @"/ListenIn/"))
        {
            //if it doesn't, create it
            Directory.CreateDirectory(xml_location);
            Directory.CreateDirectory(Application.persistentDataPath + @"/ListenIn/Database/backup");
            //create an xml from the local sample one
            database_xml.LoadXml(database_xml_file.text);
            //and save it only once
            database_xml.Save(xml_file);
        }
        //load the xml from the path
        database_xml.LoadXml(File.ReadAllText(xml_file));

        //Debug.Log(Application.persistentDataPath);

        //get id patient from the xml
        PatientId = int.Parse(GetPatient());
        DatasetId = int.Parse(GetDatasetId());

        //if internet read the xml
        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            //read the xml
            ReadDatabaseXML();
            //and get the patient progress
            //StartCoroutine(get_patient_progress());
        }
    }

    protected override void Awake()
    {
        //Getting the xml template from the resources
        database_xml_file = Resources.Load("database") as TextAsset;        
    }

    protected void Update()
    {
        //get fingers on screen android only 
        int fingerCount = 0;

#if UNITY_ANDROID
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                fingerCount++;

        }
#endif

#if UNITY_EDITOR
        fingerCount = Input.GetMouseButtonDown(0) ? 1 : 0;
#endif

        if (fingerCount > 0)
        {
            //if finger, reset the timer
            count_iddle_time = false;
            idle_time = 0;
        }
        else
        {
            //if no finger, then run the counter
            count_iddle_time = true;
        }

        //block timer
        if (count_therapy_time)
        {
            therapy_time += Time.deltaTime;
        }

        if (count_pinball_time)
        {
            therapy_pinball_time += Time.deltaTime;
        }

        if (count_worlmap_time)
        {
            therapy_worldmap_time += Time.deltaTime;
        }

        //iddle timer
        if (count_iddle_time)
        {
            idle_time += Time.unscaledDeltaTime;
            //Debug.Log((int)therapy_block_time);
        }

        //TODO: unify the menu system
        #region TimeoutGame
        if (isMenuPaused && idle_time > 60 * 10)
        {
            Debug.Log("Forcing Quitting ListenIn");
            reasonToExit = 99;
            Application.Quit();

            //If we are running in the editor
#if UNITY_EDITOR
            //Stop playing the scene
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        else if (!isMenuPaused && idle_time > 60 * 5)
        {
            OpenPauseMenu();
        }

        #endregion
    }

    private void OpenPauseMenu()
    {
        //Works only on the WorldMapScene
        GameObject menuUI = GameObject.FindGameObjectWithTag("MenuUI");
        if (menuUI != null)
        {
            LevelSelectManager lsm = menuUI.GetComponent<LevelSelectManager>();
            if (lsm != null)
            {
                lsm.OpenPauseMenu();
                Debug.Log("Forcing menu after idle timeout - case WorldMap");
                return;
            }
        }

        GameObject jigsawPuzzle = GameObject.FindGameObjectWithTag("JigsawPuzzle");
        if (jigsawPuzzle != null)
        {
            ChapterSelectMono csm = jigsawPuzzle.GetComponent<ChapterSelectMono>();
            if (csm != null)
            {
                csm.OpenMenu();
                Debug.Log("Forcing menu after idle timeout - case Jigsaw puzzle before therapy or Pinball");
                return;
            }
        }

        GameObject challengeTherapy = GameObject.FindGameObjectWithTag("Challenge");
        if (challengeTherapy != null)
        {
            MenuManager mm = challengeTherapy.GetComponentInChildren<MenuManager>();
            if (mm != null)
            {
                mm.OpenMenu();
                Debug.Log("Forcing menu after idle timeout - case Therapy Challenge");
                return;
            }
        }

    }

    //return lenght
    public int QueriesOnTheXML()
    {
        XmlNodeList _list = database_xml.SelectNodes("/database/queries/query");
        //if there's none, do nothing
        return _list.Count;
    }
    
    //function which reads the xml in order
    public void ReadDatabaseXML()
    {
        //current time
        string current_date = System.DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss");
        //Debug.Log(current_date);
        //select all the query tags
        XmlNodeList _list = database_xml.SelectNodes("/database/queries/query");
        //if there's none, do nothing
        if (_list.Count != 0)
        {
            //queue to insert the forms
            xml_forms_queue = new Queue<DatabaseQuery>();
            //go through each one of them
            foreach (XmlNode _node in _list)
            {
                xml_form = new WWWForm();
                //get the url of the query to be send
                xml_query_url = _node.Attributes[0].Value;
                Debug.Log(_node.Attributes[0].Value);
                //go through all the XML variables nodes inside the current query
                foreach (XmlNode _node_variables in _node.ChildNodes)
                {
                    //create the fields for the WWWForm
                    xml_form.AddField(_node_variables.Attributes[0].Value, _node_variables.Attributes[1].Value);
                    //StartCoroutine(send_xml_query());
                    Debug.Log(_node_variables.Attributes[0].Value + " - " + _node_variables.Attributes[1].Value);
                }
                //queue the forms
                xml_forms_queue.Enqueue(new DatabaseQuery(xml_query_url, xml_form));
                Debug.Log("ADDED TO THE QUEUE");
            }
            //go through the queue and insert them in order
            StartCoroutine(send_xml_query());

            //when finishes, save it as the current date in another folder
            string xml_backup = Application.persistentDataPath + @"/ListenIn/Database/backup/" + current_date + ".xml";
            File.Copy(xml_file, xml_backup);
            //empty the inserter xml
            XmlElement elmRoot = (XmlElement)database_xml.SelectSingleNode("/database/queries");
            //remove all
            elmRoot.RemoveAll();
            //and save
            database_xml.Save(xml_file);

            //StartCoroutine("UploadHistory2");

        }
    }

    IEnumerator send_xml_query()
    {
        //first in, first out logic
        foreach (DatabaseQuery form in xml_forms_queue)
        {
            insert_in_order = false;
            WWW xml_www = new WWW(form.query_url, form.query_form);
            while (!insert_in_order)
            {
                yield return xml_www;
                if (xml_www.isDone) insert_in_order = true;
            }
           
        }
        Debug.Log("ENDO OF COURITINE - ALL INSERTED TO THE DB");
    }

    public void uploadHistoryXml()
    {
        StartCoroutine("UploadHistory2");
    }

    IEnumerator UploadHistory2()
    {
        /*map.LoadXml("<?xml version='1.0' encoding='utf-8'?>" +
                "<root>" +
                "</root>");*/

        string strProfile = "";
        string strTherapyBlocks = "";
        string strTherapyBlocksAll = "";
        string strLiExposure = "";
        //string strLiComplexity = "";
        //string strCiComplexity = "";
        //string strCifComplexity = "";
        string strCifHistory = "";
        
        XmlDocument doc1 = new XmlDocument();
        string strXmlFile1 = Application.persistentDataPath + "/" + "user_" + PatientId + "_profile.xml";
        if (System.IO.File.Exists(strXmlFile1))
        {
            doc1.Load(strXmlFile1);
            strProfile = doc1.OuterXml;
        }

        XmlDocument doc2 = new XmlDocument();
        string strXmlFile2 = Application.persistentDataPath + "/" + "user_" + PatientId + "_therapyblocks.xml";
        if (System.IO.File.Exists(strXmlFile2))
        {
            doc2.Load(strXmlFile2);
            strTherapyBlocks = doc2.OuterXml;
        }

        XmlDocument doc3 = new XmlDocument();
        string strXmlFile3 = Application.persistentDataPath + "/" + "user_" + PatientId + "_therapyblocks_all.xml";
        if (System.IO.File.Exists(strXmlFile3))
        {
            doc3.Load(strXmlFile3);
            strTherapyBlocksAll = doc3.OuterXml;
        }

        string strCsvFile4 = Application.persistentDataPath + "/" + "user_" + PatientId + "_lexicalitem_history_exposure.csv";
        if (System.IO.File.Exists(strCsvFile4))
            strLiExposure = System.IO.File.ReadAllText(strCsvFile4);            
        
        /*string strCsvFile5 = Application.persistentDataPath + "/" + "user_" + PatientId + "_lexicalitem_history_complexity.csv";
        if (System.IO.File.Exists(strCsvFile5))
            strLiComplexity = System.IO.File.ReadAllText(strCsvFile5);
        
        string strCsvFile6 = Application.persistentDataPath + "/" + "user_" + PatientId + "_challengeitem_history_complexity.csv";
        if (System.IO.File.Exists(strCsvFile6))
            strCiComplexity = System.IO.File.ReadAllText(strCsvFile6);
        
        string strCsvFile7 = Application.persistentDataPath + "/" + "user_" + PatientId + "_challengeitemfeatures_history_complexity.csv";
        if (System.IO.File.Exists(strCsvFile7))
            strCifComplexity = System.IO.File.ReadAllText(strCsvFile7);*/
        
        XmlDocument doc8 = new XmlDocument();
        string strXmlFile8 = Application.persistentDataPath + "/" + "user_" + PatientId + "_challengeitemfeatures_history.xml";
        if (System.IO.File.Exists(strXmlFile8))
        {
            doc8.Load(strXmlFile8);
            strCifHistory = doc8.OuterXml;
        }        

        // create WWWForm
        WWWForm form = new WWWForm();
        form.AddField("patientid", PatientId.ToString());
        form.AddField("profile", strProfile);
        form.AddField("tb", strTherapyBlocks);
        form.AddField("cifHistory", strCifHistory);
        form.AddField("tbAll", strTherapyBlocksAll);
        form.AddField("liExposure", strLiExposure);
        //form.AddField("liComplexity", strLiComplexity);
        //form.AddField("ciComplexity", strCiComplexity);
        //form.AddField("cifComplexity", strCifComplexity);        
        //form.AddBinaryData("file", levelData, fileName, "text/xml");

        //change the url to the url of the php file
        WWW w = new WWW(therapy_history_insert, form);
        
        yield return w;        
        if (w.error != null)
        {
            print(w.error);
            Debug.Log("UploadHistory - error - " + w.error);
        }
        else
        {
            Debug.Log("UploadHistory - done");
        }
    }    

    //function which inserts queries to the xml file
    public void WriteDatabaseXML(Dictionary<string,string> www_form, string url_www_form)
    {
        //insert daily once
        if (url_www_form != therapy_daily_insert)
        {
            //variable node for the xml
            XmlElement variable_node;

            //get the root element
            XmlElement root_tag = (XmlElement)database_xml.SelectSingleNode("/database/queries");

            //create the query
            XmlElement query_node = database_xml.CreateElement("query");
            query_node.SetAttribute("url", url_www_form);

            //run through the dictornary values (the form to send variables names and values)
            foreach (KeyValuePair<string, string> www_form_values in www_form)
            {
                variable_node = database_xml.CreateElement("variable");
                variable_node.SetAttribute("name", www_form_values.Key);
                variable_node.SetAttribute("value", www_form_values.Value);
                //parenting
                query_node.AppendChild(variable_node);
            }

            //append to root node
            root_tag.AppendChild(query_node);
            //save doc 
            database_xml.Save(xml_file);
        }
        else if(!therapy_daily_inserted)
        {
            //one therapy only
            therapy_daily_inserted = true;
            //variable node for the xml
            XmlElement variable_node;

            //get the root element
            XmlElement root_tag = (XmlElement)database_xml.SelectSingleNode("/database/queries");

            //create the query
            XmlElement query_node = database_xml.CreateElement("query");
            query_node.SetAttribute("url", url_www_form);

            //run through the dictornary values (the form to send variables names and values)
            foreach (KeyValuePair<string, string> www_form_values in www_form)
            {
                variable_node = database_xml.CreateElement("variable");
                variable_node.SetAttribute("name", www_form_values.Key);
                variable_node.SetAttribute("value", www_form_values.Value);
                //parenting
                query_node.AppendChild(variable_node);
            }

            //append to root node
            root_tag.AppendChild(query_node);
            //save doc 
            database_xml.Save(xml_file);
        } 
    }

    void OnApplicationQuit()
    {
        //if the patient played the game, then update the daily therapy - last level played, if not then don't update
        if(QueriesOnTheXML() != 0)
        {
            Dictionary<string, string> query_therapyDailyUpdate = new Dictionary<string, string>();
            query_therapyDailyUpdate.Add("patient", PatientId.ToString());
            query_therapyDailyUpdate.Add("level_end", StateJigsawPuzzle.Instance.currLevelPinball.ToString());
            //query_therapyDailyUpdate.Add("total_therapy_time", CUserTherapy.Instance.getTotalTherapyTimeMin().ToString());

            WriteDatabaseXML(query_therapyDailyUpdate, therapy_daily_update);

            //add patient progress to the xml when app close
            Dictionary<string, string> query_patient_game_progress = new Dictionary<string, string>();
            query_patient_game_progress.Add("patient", PatientId.ToString());
            query_patient_game_progress.Add("progress", MadLevelProfile.SaveProfileToString());

            WriteDatabaseXML(query_patient_game_progress, insert_patient_progress);
        }
        

        Dictionary<string, string> sessionUpdate = new Dictionary<string, string>();

        int patient = PatientId;

        sessionUpdate.Add("patient", patient.ToString());
        sessionUpdate.Add("therapy_time", therapy_time.ToString());
        sessionUpdate.Add("therapy_game_pinball", therapy_pinball_time.ToString());
        sessionUpdate.Add("therapy_game_world", therapy_worldmap_time.ToString());
        sessionUpdate.Add("exit_reason", reasonToExit.ToString());

        WriteDatabaseXML(sessionUpdate, therapy_session_update);


    }

    //return lenght
    public void SetNewPatient(string patient_id, string dataset_text = "")
    {
        //Save current game data
        StartCoroutine(update_patient_progress(PatientId));

        //get the patient element
        XmlElement patient_element = (XmlElement)database_xml.SelectSingleNode("/database/patient");
        patient_element.SetAttribute("id", patient_id);
        //save doc 
        database_xml.Save(xml_file);
        //set new patient
        PatientId = int.Parse(patient_id);

        //set new dataset
        DatasetId = 0;
        // retrieve dataset id of this patient from database
        StartCoroutine(get_patient_datasetid());        

        StartCoroutine(get_patient_progress());

        //CUserTherapy.Instance.LoadUserProfile();
        CUserTherapy.Instance.LoadDataset_UserProfile();
    }

    IEnumerator get_patient_datasetid()
    {
        //create the the form to send it 
        WWWForm patient_datasetid_form = new WWWForm();
        patient_datasetid_form.AddField("patient", PatientId);

        //sql_query
        WWW sql_patient_datasetid = new WWW(select_patient_datasetid, patient_datasetid_form);
        yield return sql_patient_datasetid;

        string strPatient_datasetid = sql_patient_datasetid.text;

        Debug.Log("Patient dataset id = " + strPatient_datasetid);

        if (strPatient_datasetid == null || strPatient_datasetid == string.Empty)
        {
            DatasetId = 0;
        }
        else
        {
            if (strPatient_datasetid.Equals("A"))
                DatasetId = 0;
            else if (strPatient_datasetid.Equals("B"))
                DatasetId = 1;

            XmlElement patient_element = (XmlElement)database_xml.SelectSingleNode("/database/patient");
            patient_element.SetAttribute("datasetid", DatasetId.ToString());
            database_xml.Save(xml_file);
        }        
    }

    //return lenght
    public string GetPatient()
    {
        //get the patient element
        XmlElement patient_element = (XmlElement)database_xml.SelectSingleNode("/database/patient");
        return patient_element.GetAttribute("id");
    }

    public string GetDatasetId()
    {
        //get the patient element
        XmlElement patient_element = (XmlElement)database_xml.SelectSingleNode("/database/patient");
        return patient_element.GetAttribute("datasetid");
    }

    //block timer
    public void SetTherapyBlockTimer()
    {
        //count_therapy_block_time = !count_therapy_block_time;
        //if (!count_therapy_block_time) therapy_block_time = 1;
    }

    IEnumerator get_patient_progress()
    {
        //create the the form to send it 
        WWWForm patient_progress_form = new WWWForm();
        patient_progress_form.AddField("patient", PatientId);

        //sql_query = new WWW(insert_patient_url, raw_data, headers);
        WWW sql_patient_progress_login = new WWW(select_patient_progress, patient_progress_form);
        yield return sql_patient_progress_login;

        string patient_game_progress = sql_patient_progress_login.text;

        Debug.Log(patient_game_progress);

        if (patient_game_progress == null || patient_game_progress == string.Empty)
        {
            MadLevelProfile.Reset();            
        }
        else
        {
            MadLevelProfile.LoadProfileFromString(patient_game_progress);
        }

        ResetTimer(TimerType.WorldMap);
        ResetTimer(TimerType.Therapy);
        ResetTimer(TimerType.Pinball);

        GameStateSaver.Instance.ResetListenIn();

        MadLevel.ReloadCurrent();

    }

    IEnumerator update_patient_progress(int currId)
    {
        //create the the form to send it 
        WWWForm patient_progress_update_form = new WWWForm();
        patient_progress_update_form.AddField("patient", currId);

        string savedState = MadLevelProfile.SaveProfileToString();

        patient_progress_update_form.AddField("progress", savedState);

        Debug.Log(savedState);

        //sql_query = new WWW(insert_patient_url, raw_data, headers);
        WWW sql_patient_progress_update = new WWW(insert_patient_progress, patient_progress_update_form);
        yield return sql_patient_progress_update;
    }
#region TimerUpdates
    public enum TimerType { Idle, WorldMap, Therapy, Pinball }


    void OnGUI()
    {
        // to hide timer 
        /*float offset = 30;
        //YEAN: these are the timers 
        string iddle_time_string = ((int)idle_time).ToString();
        iddle_time_string = GUI.TextField(new Rect(10, 10 + offset, 200, 20), "idle time: " + iddle_time_string + "(s)", 25);

        string block_time_string = ((int)therapy_worldmap_time).ToString();
        block_time_string = GUI.TextField(new Rect(10, 30 + offset, 200, 20), "WorldMap time: " + block_time_string + "(s)", 25);

        string block_therapy_string = ((int)therapy_time).ToString();
        block_therapy_string = GUI.TextField(new Rect(10, 50 + offset, 200, 20), "Therapy time: " + block_therapy_string + "(s)", 25);

        string block_game_string = ((int)therapy_pinball_time).ToString();
        block_game_string = GUI.TextField(new Rect(10, 70 + offset, 200, 20), "Pinball time: " + block_game_string + "(s)", 25);
        */
    }

    public void SetTimerState(TimerType tymerType, bool state)
    {
        switch (tymerType)
        {
            case TimerType.Idle:
                count_iddle_time = state;
                break;
            case TimerType.WorldMap:
                count_worlmap_time = state;
                break;
            case TimerType.Therapy:
                count_therapy_time = state;
                break;
            case TimerType.Pinball:
                count_pinball_time = state;
                break;
            default:
                break;
        }
    }

    public void ResetTimer(TimerType tymerType)
    {
        switch (tymerType)
        {
            case TimerType.Idle:
                idle_time = 0.0f;
                break;
            case TimerType.WorldMap:
                therapy_time = 0.0f;
                break;
            case TimerType.Therapy:
                therapy_worldmap_time = 0.0f;
                break;
            case TimerType.Pinball:
                therapy_pinball_time = 0.0f;
                break;
            default:
                break;
        }
    }
#endregion
}
