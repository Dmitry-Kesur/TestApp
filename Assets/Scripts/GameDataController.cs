using System;
using System.Threading.Tasks;

namespace DefaultNamespace
{
    public class GameDataController
    {
        private readonly DataOperationService _dataOperationService;
        private readonly GameScoreModel _gameScoreModel;
        private readonly GameData _gameData;

        public GameDataController(DataOperationService dataOperationService)
        {
            _dataOperationService = dataOperationService;
            _gameData = _dataOperationService.LoadGameData();

            _gameScoreModel = new GameScoreModel(_gameData.gameScore)
            {
                OnChangeScoreAction = UpdateGameData
            };
        }

        public async Task InitScoreMultiplier()
        {
            var result = await Api.Get<MultiplierData>("https://mocki.io/v1/9d718cd5-3e36-48c2-9d46-4f043eecb646");
            if (result == null)
            {
                throw new Exception("[Api] request return null");
            }

            _gameScoreModel.SetScoreMultiplier(result.multiplier);
        }

        public void SetScore(int scoreAmount)
        {
            _gameScoreModel.SetScore(scoreAmount);
        }

        public GameScoreModel GetGameScoreModel() => _gameScoreModel;

        private void UpdateGameData()
        {
            _gameData.gameScore = _gameScoreModel.gameScore;

            _dataOperationService.SaveGameData(_gameData);
        }
    }
}