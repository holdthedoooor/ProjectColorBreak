using UnityEngine;
using UnityEditor;
using System.Collections;

public class EditorTest : Editor
{
    [MenuItem( "DevMenu/저장소 리셋" )]
    static public void ResetStorage()
    {
        PlayerPrefs.DeleteAll();
    }

}