using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class JSONDataClass
{
    public List<filesList> files;
}

[Serializable]
public class filesList
{
    public string video_file;
    public string creationdate;
    public string idfile;
}