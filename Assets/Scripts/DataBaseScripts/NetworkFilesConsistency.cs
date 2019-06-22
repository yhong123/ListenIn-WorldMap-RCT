using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NetworkFilesConsistency : MonoBehaviour
{
    public static NetworkFilesConsistency Instance = null;
    private int filesIteration;
    private string userID;
    private bool isRemoteEmpty;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }

    public void InitFileConsistencyCheck(string userID)
    {
        this.userID = userID;
        filesIteration = 1;
        string fileLocation = string.Concat(NetworkManager.FilePathCSV, NetworkManager.FileNameSeparator, userID, NetworkManager.FileNameSeparator, NetworkManager.GetFileName(NetworkManager.FileType.InsertSequenceData));

        //check local files
        if (File.Exists(fileLocation))
        {
            if (NetworkManager.HasInternet)
            {
                Debug.Log("<color=green>LOCAL FILES EXIST - CHECKING CONSISTENCY</color>");
                CheckLocalFiles();
            }
            else
            {
                Debug.Log("<color=green>LOCAL FILES EXIST - <b>NO INTERNET</b> - STARTING GAME</color>");
                return;
            }
        }
        else if (!File.Exists(fileLocation))
        {
            if (NetworkManager.HasInternet)
            {
                Debug.Log("<color=green>LOCAL FILES DON'T EXIST - CHECKING FILES FROM SERVER</color>");
                CheckRemoteFiles();
            }
            else
            {
                Debug.Log("<color=green>LOCAL FILES DON'T EXIST - <b>NO INTERNET</b></color>");
                return;
            }
        }
    }

    private void CheckLocalFiles()
    {
        foreach (NetworkManager.FileTypeHolder nameOfFile in NetworkManager.ListOfFileNamesStatic)
        {
            //LOADNG SYNCING
            string content = string.Concat(NetworkManager.FilePathCSV, NetworkManager.FileNameSeparator, userID, NetworkManager.FileNameSeparator, nameOfFile.FileName);

            if (File.Exists(content))
            {
                content = File.ReadAllText(content);
            }
            else
            {
                content = "";
            }

            byte[] bytes = new byte[content.Length * sizeof(char)];
            System.Buffer.BlockCopy(content.ToCharArray(), 0, bytes, 0, bytes.Length);
            uint checksum = NetworkManager.crc32(content);

            WWWForm form = new WWWForm();
            form.AddField("file_name", nameOfFile.FileName);
            form.AddField("user_id", userID);
            form.AddField("content", content);
            form.AddField("checksum", checksum.ToString());

            Action<string> callBack = new Action<string>(CheckCSVFilesCallBack);

            NetworkManager.SendDataServer(
                form,
                NetworkManager.ServerUrlFileConsistencyCheck,
                content,
                nameOfFile.FileName,
                callBack
            );
        }
    }

    public void CheckCSVFilesCallBack(string callback)
    {
        if (callback != "ok" && callback != "local")
        {
            NetworkManager.AppendServerDataCSV(callback);
        }

        if (filesIteration == NetworkManager.ListOfFileNamesStatic.Count)
        {
            Debug.Log("<color=green>LOCAL FILES CHECKED</color> - <b>READY TO START THE GAME</b>");
            return;
        }

        filesIteration++;
    }

    private void CheckRemoteFiles()
    {
        foreach (NetworkManager.FileTypeHolder nameOfFile in NetworkManager.ListOfFileNamesStatic)
        {
            //LOADNG SYNCING
            string content = string.Concat(nameOfFile.FileName, userID);

            byte[] bytes = new byte[content.Length * sizeof(char)];
            System.Buffer.BlockCopy(content.ToCharArray(), 0, bytes, 0, bytes.Length);
            uint checksum = NetworkManager.crc32(content);

            WWWForm form = new WWWForm();
            form.AddField("file_name", nameOfFile.FileName);
            form.AddField("id_user", userID);
            form.AddField("content", content);
            form.AddField("checksum", checksum.ToString());

            Action<string> callBack = new Action<string>(CheckRemoteFilesCallBack);

            NetworkManager.SendDataServer(
                form,
                NetworkManager.ServerUrlFileCheck,
                content,
                nameOfFile.FileName,
                callBack
            );
        }
    }

    public void CheckRemoteFilesCallBack(string callback)
    {
        if (callback != "empty")
        {
            NetworkManager.AppendServerDataCSV(callback);
            isRemoteEmpty = false;
        }

        if (filesIteration == NetworkManager.ListOfFileNamesStatic.Count)
        {
            //all files has been checked - PROCEED
            if (isRemoteEmpty)
            {
                Debug.Log("<color=green>NO FILES IN SERVER</color> - <b>NEW USER</b>");
                return;
            }
            else
            {
                Debug.Log("<color=green>FILES DOWNLOADED</color> - <b>READY TO START THE GAME</b>");
                return;
            }
        }

        filesIteration++;
    }
}
