namespace DefaultNamespace
{
    public class GameDataController
    {
        private readonly DataOperationService _dataOperationService;
        private readonly GameData _gameData;

        public GameDataController(DataOperationService dataOperationService)
        {
            _dataOperationService = dataOperationService;
            _gameData = _dataOperationService.LoadGameData();
        }

        public void SaveGameScore(int gameScore)
        {
            _gameData.gameScore = gameScore;
            _dataOperationService.SaveGameData(_gameData);
        }

        public int GetGameScore() => _gameData.gameScore;
    }
}