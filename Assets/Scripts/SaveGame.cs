using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public class SaveGame
{
    
    private int maxLevelBeated = 0;
    private long totalTimePlayed = 0, lastTimeSaved = 0;
    private readonly string saveDate;

    public SaveGame()
    {
        saveDate = DateTime.Now.ToString();
        lastTimeSaved = ((DateTimeOffset) DateTime.Now).ToUnixTimeSeconds();
    }

    public void levelPassed(int buildIndex)
    {
        maxLevelBeated = buildIndex > maxLevelBeated ? buildIndex : maxLevelBeated;
        totalTimePlayed += ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() - lastTimeSaved;
    }

}