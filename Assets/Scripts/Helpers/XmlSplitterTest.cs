using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XmlSplitterTest : MonoBehaviour {

    
    public void TestSplitting()
    {
        for (int i = 0; i < DatabaseXML.MaximumQueriesPerFile; i++)
        {
            Dictionary<string, string> dailyTherapy = new Dictionary<string, string>();

            int pid = DatabaseXML.Instance.PatientId;

            dailyTherapy.Add("patient", pid.ToString());
            dailyTherapy.Add("level_start", "level");
            dailyTherapy.Add("date", "aaaaaaaa");

            DatabaseXML.Instance.WriteDatabaseXML(dailyTherapy, DatabaseXML.Instance.therapy_session_update);
        }
        
    }

}
