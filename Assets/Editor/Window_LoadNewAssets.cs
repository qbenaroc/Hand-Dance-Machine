using UnityEngine;
using UnityEditor;
using Unity.EditorCoroutines.Editor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Diagnostics;
using System.Security.Policy;
using UnityEngine.Networking;

public class HDM_LoadNewAssets : EditorWindow
{
    public string data;
    string serverString = "Z:\\GAMING\\MICROBE GAMES\\HDM_SOURCES\\01_FROM_BLENDER";
    string sntString = "";
    string projString = System.IO.Directory.GetCurrentDirectory() + "\\Assets\\Blender";

    [MenuItem("HDM/Load New Assets")]
    public static void LoadNewAssets()
    {
        EditorWindow.GetWindow(typeof(HDM_LoadNewAssets));
    }

    void OnGUI()
    {
        GUILayout.Label(" ");
        // TITLE DISPLAY SERVER ASSETS
        GUILayout.Label("IMPORT SETTINGS", EditorStyles.boldLabel);
        // Local server text field
        EditorGUILayout.TextField("Local server path", serverString);

        if (GUILayout.Button("Select Assets Folder"))
        {
            string path = EditorUtility.OpenFolderPanel("Select Blender Assets Folder", "", "");
            if (path != "") {
                UnityEngine.Debug.Log("Selected path : " + path);
                serverString = path;
            }
        }

        GUILayout.Label(" ");

        // SNT Server text field
        sntString = EditorGUILayout.TextField("Share'nTalk id folder", sntString);
        
        // AFFICHAGE LOCAL FILES 
        GUILayout.Label(" ");
        GUILayout.Label(" ");
        GUILayout.Label("FILES FOUND ON LAN", EditorStyles.boldLabel);
        GUILayout.Label(" ");

        // LIST FILES IN LOCAL DIR
        DirectoryInfo dir = new DirectoryInfo(serverString);
        if (dir.Exists)
        {
            FileInfo[] info = dir.GetFiles("*.blend");
            foreach (FileInfo f in info)
            {
                DateTime dt = File.GetLastWriteTime(f.ToString());
                string FileToDisplay = f.ToString() + " / " + dt;

                //Display File Name & Date
                GUILayout.Label(FileToDisplay, EditorStyles.miniLabel);
            }
        }
        // AFFICHAGE SNT FILES 
        GUILayout.Label(" ");
        GUILayout.Label(" ");
        
        GUILayout.Label("FILES FOUND ON SNT", EditorStyles.boldLabel);
        
        // START COROUTINE TO GET JSON DATA FROM SERVER
        string url = "https://www.sharentalk.com/src/folderslist_json?idfolder=" + sntString;
        
        // CALL ROUTINE TO GET DATA FROM SNT
        if (sntString!="")
            EditorCoroutineUtility.StartCoroutine(getData(url), this);
        
        // DISPLAY FILES LIST FROM JSON TEXT
        if (data!="" && sntString!="")
            processJsonData(data);

        // TITLE DISPLAY LOCAL ASSETS
        GUILayout.Label(" ");
        GUILayout.Label("ASSETS BLENDER IN PROJECT", EditorStyles.boldLabel);
        // LIST FILES IN PROJ
        dir = new DirectoryInfo(projString);
        if (dir.Exists)
        { 
            FileInfo[] info = dir.GetFiles("*.blend");
            foreach (FileInfo f in info)
            {
                DateTime dt = File.GetLastWriteTime(f.ToString());
                string FileToDisplay = f.ToString() + " / " + dt;

                //Display File Name & Date
                GUILayout.Label(FileToDisplay, EditorStyles.miniLabel);
            }
        }

        GUILayout.Label(" ");

        GUIStyle style_green = new GUIStyle(GUI.skin.button);
        style_green.normal.textColor = Color.blue;
        style_green.hover.textColor = Color.blue;

        // BUTTON UPDATE FILES PRESSED
        if (GUILayout.Button("UPDATE ASSETS", style_green))
        {
            UnityEngine.Debug.Log("Running update");

            // SYNC FILES FROM LOCAL SERVER
            dir = new DirectoryInfo(serverString);
            if (dir.Exists)
            {
                FileInfo[] info = dir.GetFiles("*.blend");
                foreach (FileInfo f in info)
                {
                    string projFileName = System.IO.Path.GetFileName(f.ToString());

                    // IF FILE ALREADY EXISTS LOCALLY, COMPARING DATE
                    if (File.Exists(projString + "\\" + projFileName))
                    {
                        UnityEngine.Debug.Log("File exists");

                        // Get Server File's Date
                        DateTime sfdt = File.GetLastWriteTime(f.ToString());

                        // Get proj file date                    
                        DateTime pfdt = File.GetLastWriteTime(projString + "\\" + projFileName);

                        // Comparing date
                        int result = DateTime.Compare(sfdt, pfdt);

                        // IF OLDER
                        if (result < 0)
                            UnityEngine.Debug.Log("File : " + projString + "\\" + projFileName + " is more recent than the server version");
                        // SAME
                        else if (result == 0)
                            UnityEngine.Debug.Log("File : " + projString + "\\" + projFileName + " is up to date");
                        // EARLIER
                        else
                        {
                            // CREATING ARCHIVES FOLDER IF NOT
                            if (!Directory.Exists(projString + "\\ARCHIVES"))
                                Directory.CreateDirectory(projString + "\\ARCHIVES");

                            // BACKUP PREVIOUS FILE en _sav DateTime File
                            File.Move(projString + "\\" + projFileName, projString + "\\ARCHIVES\\" + projFileName + "_sav" + pfdt.ToString("dd-MM-yyyy--hh-mm-ss"));
                            UnityEngine.Debug.Log("Previous File : " + projString + "\\ARCHIVES\\" + projFileName + " has been backuped.");

                            // DELETE PROJ FILE 
                            File.Delete(projString + "\\" + projFileName);

                            // COPY FILE FROM SERVER
                            File.Copy(f.ToString(), projString + "\\" + projFileName);
                            UnityEngine.Debug.Log("File : " + projString + "\\" + projFileName + " has been updated successfully!");

                            // Refresh Database
                            AssetDatabase.Refresh();
                        }
                    }

                    // COPY FILE DIRECTLY
                    else
                    {
                        UnityEngine.Debug.Log("File : " + projString + "\\" + projFileName + " does not exists");
                        File.Copy(f.ToString(), projString + "\\" + projFileName);
                        UnityEngine.Debug.Log(projString + "\\" + projFileName + " has been updated successfully!");
                    }
                }
            }

            // SYNC FILES FROM SNT
            if (data != "" && sntString != "")
                downloadFilesFromSNT(data);

            // Refresh Database
            AssetDatabase.Refresh();
        }

        GUILayout.Label(" ");

        GUIStyle style_red = new GUIStyle(GUI.skin.button);
        style_red.normal.textColor = Color.red;
        style_red.hover.textColor = Color.red;

        // BUTTON DELETE ARCHIVES PRESSED
        if (GUILayout.Button("DELETE ARCHIVES", style_red))
        {
            if (Directory.Exists(projString + "\\ARCHIVES"))
                Directory.Delete(projString + "\\ARCHIVES", true);
            
            // Refresh Database
            AssetDatabase.Refresh();
        }
    }

    IEnumerator getData(string url)
    {
        UnityEngine.Debug.Log("Processing Data from : " + url);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.certificateHandler = new BypassCertificate();
            webRequest.SetRequestHeader("User-Agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36");
            webRequest.SetRequestHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                UnityEngine.Debug.Log("Error URL");
            }
            else
            {
                data = webRequest.downloadHandler.text ;
            }
        }
    }

    IEnumerator DownloadFile(string idFile)
    {
        string pathFile;

        // GET PATH FROM ENCRYPTED ID
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://www.sharentalk.com/getFilePath?id=" + idFile))
        {
            webRequest.certificateHandler = new BypassCertificate();
            webRequest.SetRequestHeader("User-Agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36");
            webRequest.SetRequestHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                UnityEngine.Debug.Log("Error URL GetFilePath");
            }
            else
            {
                pathFile = webRequest.downloadHandler.text;

                // DOWNLOAD WITH CLEAN PATH
                var uwr = new UnityWebRequest("https://www.sharentalk.com/" + pathFile, UnityWebRequest.kHttpVerbGET);
                uwr.certificateHandler = new BypassCertificate();
                uwr.SetRequestHeader("User-Agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36");
                uwr.SetRequestHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");

                string FileName = Path.GetFileName(pathFile);
                string path = Path.Combine(projString, FileName);
                uwr.downloadHandler = new DownloadHandlerFile(path);
                yield return uwr.SendWebRequest();
                if (uwr.isNetworkError || uwr.isHttpError)
                    UnityEngine.Debug.LogError("ERROR DOWNLOADING : " + uwr.error);
                else
                    UnityEngine.Debug.Log("File successfully downloaded and saved to " + path);
            }
        }
    }

    private void processJsonData(string _url)
    {
        JSONDataClass jsnData = JsonUtility.FromJson<JSONDataClass>(_url);
    
        foreach (filesList x in jsnData.files)
        {
            //Display File Name & Date
            GUILayout.Label(x.video_file + " / " + x.creationdate, EditorStyles.miniLabel);
        }
    }

    private void downloadFilesFromSNT(string _url)
    {
        JSONDataClass jsnData = JsonUtility.FromJson<JSONDataClass>(_url);
    
        foreach (filesList x in jsnData.files)
        {
            //RUN COROUTINE TO DOWNLOAD FILE
            EditorCoroutineUtility.StartCoroutine(DownloadFile(x.idfile), this); 
        }
    }

    public class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            //Simply return true no matter what
            return true;
        }
    }
}
