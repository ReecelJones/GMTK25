using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public Button startTurnButton, resetButton;

    public LevelLoader levelLoader;
    public ResultHandler resultHandler;

    private void Start()
    {
        startTurnButton.onClick.AddListener(OnStartTurnClicked);
        resetButton.onClick.AddListener(OnResetClicked);

        levelLoader = FindFirstObjectByType<LevelLoader>();
        resultHandler = FindFirstObjectByType<ResultHandler>();
    }

    private void OnStartTurnClicked()
    {
        GameManager.instance.UpdateGameState(GameState.PlayerTurn);
    }
    public void OnResetClicked()
    {
        GameManager.instance.HandleResetLevel();
        resultHandler.resultScreen.SetActive(false);

    }
    public void NextLevel()
    {
        levelLoader.LoadNextLevel();
        resultHandler.resultScreen.SetActive(false);
    }
}
