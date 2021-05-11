using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveGame
{
    
    public int maxLevelBeated = 1;
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

    public string FormatText() 
    {
        
        StringBuilder detailsText = new StringBuilder();
        detailsText.AppendLine($"Criado: {saveDate.Split(' ')[0]}"); // Criado: XX/XX/XXXX                        
        detailsText.AppendLine($"Tempo: {totalTimePlayed/3600}:{totalTimePlayed /60}:{totalTimePlayed % 60}h"); // Tempo: XXXXh
        detailsText.Append($"Concluído: {maxLevelBeated * 0.0f / (SceneManager.sceneCountInBuildSettings - 1)*100} %"); // Concluído: XX %
        return detailsText.ToString();
    }

    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dtDateTime;
    }

}