using DefaultNamespace;
using UI;
using UnityEngine;

public class GameApplication : MonoBehaviour
{
    [SerializeField] private InterfaceView interfaceView;
    private DataOperationService _dataOperationService;
    private GameHandler _gameHandler;

    private void Awake()
    {
        InitApplication();
    }

    private void InitApplication()
    {
        Api.InitializeApi();

        _dataOperationService = new DataOperationService();
        _gameHandler = new GameHandler(_dataOperationService);

        interfaceView.Init(_gameHandler);

        _gameHandler.Init();
    }
}