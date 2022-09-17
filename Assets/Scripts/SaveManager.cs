using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    public int currentScore = 0;
    public string currentName = "";
    public string savedName = "";
    public TMP_InputField nameField;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 0)
        {
            nameField = GameObject.Find("InputField (TMP)").GetComponent<TMP_InputField>();
            nameField.text = currentName;
        }
    }

    private void Start()
    {
        LoadHighScore();
        nameField.text = currentName;
    }

    [System.Serializable]
    private class SaveData
    {
        public string highScoreName;
        public string currentName;
        public int score;
    }

    public void SetNameFromField()
    {
        currentName = nameField.text;
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.highScoreName = currentName;
        data.score = currentScore;
        data.currentName = currentName;
        savedName = currentName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            savedName = data.highScoreName;
            currentName = data.currentName;
            currentScore = data.score;
        }
    }
}
