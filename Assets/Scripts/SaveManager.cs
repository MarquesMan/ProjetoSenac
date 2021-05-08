using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UIElements;

public class SaveManager : MonoBehaviour
{

    // Instancia do SaveManager
    private static SaveManager saveManager;

    private SaveGame currentSaveGame;

    // Obter a instancia
    public static SaveManager instance
    {
        get
        {
            if (!saveManager)
            {
                saveManager = FindObjectOfType(typeof(SaveManager)) as SaveManager;

                if (!saveManager)
                {
                    Debug.LogError("There needs to be one active SaveManger script on a GameObject in your scene.");
                }
                else
                {
                    saveManager.Init();
                }
            }

            return saveManager;
        }
    }

    private void Init()
    {
        Debug.Log("Save manager criado");
        currentSaveGame = new SaveGame();
    }

    private static SaveGame LoadGame(int saveSlot = 0)
    {
        if (File.Exists(Application.persistentDataPath + $"/savegame_{saveSlot}.save"))
        {
            Debug.Log("Loading Save Game");
            // Carregue do armazenamento
            // Cria o formatodor de binario
            BinaryFormatter bf = new BinaryFormatter();
            // Carrega o arquivo por meio de stream
            FileStream file = File.Open(Application.persistentDataPath + $"/savegame_{saveSlot}.save", FileMode.Open);
            // Deserializa o arquivo do armazenamento para SaveGame de novo
            SaveGame save = (SaveGame)bf.Deserialize(file);
            file.Close();

            return save;
        }
        else
        {
            // Crie um novo jogo
            Debug.Log("Creating New Game");
            return new SaveGame();
        }
    }

    public static void SaveGame(int saveSlot = 0)
    {
        Debug.Log("Saving the Game");
        // Salvar o arquivo no armazenamento
        // Cria o formatodor de binario

        BinaryFormatter bf = new BinaryFormatter();
        // Cria o arquivo por meio de stream
        FileStream file = File.Create(Application.persistentDataPath + $"/savegame_{saveSlot}.save");
        // Serializa o objeto SaveGame
        bf.Serialize(file, instance.currentSaveGame);
        // Fecha o arquivo depois de escrito
        file.Close();
    }

    public static void DeleteSaveGame(int saveSlot = 0)
    {
        if (CheckForSlot(saveSlot))
        {
            File.Delete(Application.persistentDataPath + $"/savegame_{saveSlot}.save");
        }
    }

    public static bool CheckForSlot(int saveSlot)
    {
        return File.Exists(Application.persistentDataPath + $"/savegame_{saveSlot}.save");
    }

    /*public static string LoadLevelScore(string levelName)
    {
        return instance.saveGameObject.LoadLevelScore(levelName);
    }

    public static void SaveLevelScore(string levelName, float levelScore)
    {
        instance.saveGameObject.SaveLevelScore(levelName, levelScore);
    }*/
}