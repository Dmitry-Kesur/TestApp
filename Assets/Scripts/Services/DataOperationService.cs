using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace DefaultNamespace
{
    public class DataOperationService
    {
        private readonly string _gameDataPath;

        public DataOperationService()
        {
            _gameDataPath = Application.persistentDataPath + "/gameData.json";
        }

        public GameData LoadGameData()
        {
            GameData gameData;

            if (File.Exists(_gameDataPath))
            {
                string dataJson = File.ReadAllText(_gameDataPath);
                gameData = JsonConvert.DeserializeObject<GameData>(dataJson);
            }
            else
            {
                gameData = new GameData();
            }

            return gameData;
        }

        public void SaveGameData(GameData gameData)
        {
            string dataJson = JsonConvert.SerializeObject(gameData);
            File.WriteAllText(_gameDataPath, dataJson);
        }
    }
}