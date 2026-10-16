using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileSaveService : ISaveService
{
    private readonly string _filePath;

        [System.Serializable]
        private class SaveData
        {
            public List<int> topScores = new List<int>();
        }

        public FileSaveService()
        {
            _filePath = Path.Combine(Application.persistentDataPath, "top_scores.json");
        }

        public void SaveScore(int newScore)
        {
            List<int> scores = LoadTopScores();
            scores.Add(newScore);
            scores.Sort((a, b) => b.CompareTo(a));
            if (scores.Count > 3)
            {
                scores = scores.GetRange(0, 3);
            }

            SaveData data = new SaveData { topScores = scores };
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(_filePath, json);
        }

        public List<int> LoadTopScores()
        {
            if (!File.Exists(_filePath))
            {
                return new List<int> { 0, 0, 0 };
            }

            try
            {
                string json = File.ReadAllText(_filePath);
                SaveData data = JsonUtility.FromJson<SaveData>(json);
                while (data.topScores.Count < 3)
                {
                    data.topScores.Add(0);
                }

                return data.topScores;
            }
            catch
            {
                return new List<int> { 0, 0, 0 };
            }
        }
}
