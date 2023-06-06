using System;
using System.Threading.Tasks;

namespace DefaultNamespace
{
    public class GameDataService
    {
        private readonly DataOperation _dataOperation;
        private readonly GameScoreModel _gameScoreModel;
        private readonly GameData _gameData;

        public GameDataService()
        {
            _dataOperation = new DataOperation();
            _gameData = _dataOperation.LoadGameData();

            _gameScoreModel = new GameScoreModel(_gameData.gameScore)
            {
                OnChangeScoreAction = UpdateGameData
            };
        }

        public async Task Init()
        {
            var result = await ApiService.Get<MultiplierData>("https://mocki.io/v1/9d718cd5-3e36-48c2-9d46-4f043eecb646");
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

            _dataOperation.SaveGameData(_gameData);
        }
    }
}