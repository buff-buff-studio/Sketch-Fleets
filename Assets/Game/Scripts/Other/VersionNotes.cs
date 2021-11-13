using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SketchFleets
{
    [CreateAssetMenu(menuName = "Sketch Fleets/Update Notes")]
    public class VersionNotes : ScriptableObject
    {
        public VersionNews[] versionNotes;
    }
}

[System.Serializable]
public struct VersionNews
{
    public enum versionTypes
    {
        Gold,
        RC,
        Beta,
        Alpha
    }
    
    [Header("Version Name")]
    public string versionNumber;
    public versionTypes versionType;

    [Header("Version Notes"),Space(15f)]
    public VersionNewsText[] versionNotesLanguage;
}

[System.Serializable]
public struct VersionNewsText
{
    [TextArea(0,1)]
    public string versionNotesLanguage;
    [TextArea(0,1)]
    [Tooltip("If the version does not contain a name, leave it empty.")]
    public string versionName;
    [TextArea(1,100)]
    public string versionAdded;
    [TextArea(1,100)]
    public string versionChanged;
    [TextArea(1,100)]
    public string versionRemoved;
    [TextArea(1,100)]
    public string versionBugFix;
}