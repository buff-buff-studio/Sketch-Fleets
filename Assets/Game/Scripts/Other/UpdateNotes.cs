using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SketchFleets
{
    public class UpdateNotes : MonoBehaviour
    {
        [SerializeField] private VersionNotes versionNotes;
        [SerializeField] private TMP_InputField inputField;

        private void Start()
        {
            inputField.text = "";
            int i = 1;
            foreach (var version in versionNotes.versionNotes)
            {
                inputField.text += $"Version: {version.versionNumber} {version.versionType.ToString()}:\n" +
                                   $"Added:\n" +
                                   $"{version.versionNotesLanguage[0].versionAdded}\n \n" +
                                   $"Changed:\n" +
                                   $"{version.versionNotesLanguage[0].versionChanged}\n \n" +
                                   $"Removed:\n" +
                                   $"{version.versionNotesLanguage[0].versionRemoved}\n \n" +
                                   $"Bugs Fixed:\n" +
                                   $"{version.versionNotesLanguage[0].versionBugFix}";
                if (i < versionNotes.versionNotes.Length)
                {
                    inputField.text += "\n \n \n";
                }
                i++;
            }
        }
    }
}
