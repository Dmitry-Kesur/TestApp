using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace DefaultNamespace
{
    public class DataOperationService
    {
        private readonly string _gameDataPath;
        private readonly BinaryFormatter _binaryFormatter;

        public DataOperationService()
        {
            _binaryFormatter = new BinaryFormatter();
            _gameDataPath = Application.persistentDataPath + "/gameData.save";
        }
        
        public GameData LoadGameData()
        {
            GameData gameData;
            FileStream fileStream;

            if (!File.Exists(_gameDataPath))
            {
                gameData = new GameData();
                fileStream = new FileStream(_gameDataPath, FileMode.Create);
                _binaryFormatter.Serialize(fileStream, gameData);
                
                fileStream.Close();
                return gameData;
            }
            
            fileStream = new FileStream(_gameDataPath, FileMode.Open);
            gameData = _binaryFormatter.Deserialize(fileStream) as GameData;
            
            fileStream.Close();
            return gameData;
        }

        public void SaveGameData(GameData gameData)
        {
            var fileStream = new FileStream(_gameDataPath, FileMode.Create);
            _binaryFormatter.Serialize(fileStream, gameData);
            fileStream.Close();
        }
    }
}