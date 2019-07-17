using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

public class CSV_Maker : MonoBehaviour {

	[HideInInspector]
	public List<string[]> dataCollected = new List<string[]> ();
	[HideInInspector]
    public string path { get
        {
            string folder = GlobalVars.GetPathToLIROOutput(NetworkManager.UserId);
            string filename = String.Format("SART_{0}.csv", TherapyLIROManager.Instance.GetCurrentTherapyCycle());
            return Path.Combine(folder, filename);
        }
    }
   
    [HideInInspector]
    public string filenameSart
    {
        get
        {
            return String.Format("SART_{0}.csv", TherapyLIROManager.Instance.GetCurrentTherapyCycle());
        }
    }
	private bool created = false;

	void Awake(){
	}

	void Start(){
		dataCollected = CreateHeader ();
	}

	public List<string[]> CreateHeader(){
		List<string[]> rowData = new List<string[]> ();
		string[] temp = new string[8];

		temp [0] = "BlockType";
		temp [1] = "BlockNumber";
		temp [2] = "TrialNumber";
		temp [3] = "isGo?";
		temp [4] = "PresentationTime";
		temp [5] = "ReactionTime";
		temp [6] = "ReactionTime2";
		temp [7] = "Hit";

		rowData.Add (temp);
		return rowData;
	}

	public void Write(string type, int block, int trial, bool isGo, float presentationTime, float reactionTime, float reactionTime_2, bool hit){
		string[] rowDataTemp = new string[8];

		rowDataTemp [0] = type;
		rowDataTemp [1] = block.ToString ();
		rowDataTemp [2] = trial.ToString();
		rowDataTemp [3] = isGo.ToString ();
		rowDataTemp [4] = presentationTime.ToString ();
		rowDataTemp [5] = reactionTime.ToString ();
		rowDataTemp [6] = reactionTime_2.ToString ();
		rowDataTemp [7] = hit.ToString ();

		dataCollected.Add (rowDataTemp);
	}

	public void CreateCSVFile(string directory, List<string[]> data){
        try
        {
            string[][] output = new string[data.Count][];

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = data[i];
            }

            int lenght = output.GetLength(0);
            string delimeter = ",";

            StringBuilder sb = new StringBuilder();

            for (int index = 0; index < lenght; index++)
            {
                sb.AppendLine(string.Join(delimeter, output[index]));
            }

            using (StreamWriter outStream = System.IO.File.CreateText(directory))
            {
                outStream.WriteLine(sb);
                outStream.Close();
            }

            WWWForm form = new WWWForm();
            form.AddField("id_user", NetworkManager.UserId);
            form.AddField("file_name", filenameSart);
            form.AddField("content", sb.ToString());

            NetworkManager.SendDataServer(form, NetworkManager.ServerUrlDataInput, sb.ToString(), filenameSart);

            
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }

	}


}
