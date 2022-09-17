using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    public int currentScore = 0;
    public string currentName = "";
    public string savedName = "";
    public TMP_InputField nameField;
    public Button deleteButton;

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
            nameField.onEndEdit.AddListener(SetNameFromField);

            deleteButton = GameObject.Find("Delete Data Button").GetComponent<Button>();
            deleteButton.onClick.AddListener(DeleteSaveData);
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

    public void SetNameFromField(string name)
    {
        currentName = name;
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

    public void DeleteSaveData()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            File.Delete(path);

            savedName = "";
            currentName = "";
            currentScore = 0;

            nameField.text = "";
        }
    }
}
