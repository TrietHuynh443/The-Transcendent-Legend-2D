using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileUtils
{
    public static string Read(string resouresPath)
    {
        return File.ReadAllText(resouresPath);
    }
}
