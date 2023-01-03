using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;
using UnityEngine.UI;

public class DiffucultyLevelScript : MonoBehaviour
{
    public static int DifficultyLevel = 1;

    private void Awake()
    {
        DifficultyLevel = 1;
        GetSavedReferences();
    }

    public void SaveCurrentProgress()
    {
        var writer = QuickSaveWriter.Create("DifficultyManager");

        writer.Write("Difficulty", DifficultyLevel);
        writer.Commit();
    }

    public void GetSavedReferences()
    {
        var reader = QuickSaveReader.Create("DifficultyManager");

        if (reader.Exists("Difficulty"))
        {
            DifficultyLevel = reader.Read<int>("Difficulty");
        }
    }

    public void SetDifficultyLevel(int num)
    {
        DifficultyLevel = num;
        SaveCurrentProgress();
    }

}
