using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;
using UnityEngine.UI;

public class DiffucultyLevelScript : MonoBehaviour
{
    public static int DifficultyLevel = 1;
    public static bool IsRandomItemSpawning = false;

    private void Awake()
    {
        DifficultyLevel = 1;
        GetSavedReferences();
    }

    public void SaveCurrentProgress()
    {
        var writer = QuickSaveWriter.Create("DifficultyManager");

        writer.Write("Difficulty", DifficultyLevel);
        writer.Write("RandomItemSpawning", IsRandomItemSpawning);
        writer.Commit();
    }

    public void GetSavedReferences()
    {
        var reader = QuickSaveReader.Create("DifficultyManager");

        if (reader.Exists("Difficulty"))
        {
            DifficultyLevel = reader.Read<int>("Difficulty");
        }

        IsRandomItemSpawning = reader.Read<bool>("RandomItemSpawning");
    }

    public void SetDifficultyLevel(int num)
    {
        DifficultyLevel = num;
        SaveCurrentProgress();
    }

    public void SetRandomItemSpawning(bool set)
    {
        IsRandomItemSpawning = set;
    }

}
