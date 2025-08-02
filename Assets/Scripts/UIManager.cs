using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public Button startTurnButton, resetButton;

    private void Start()
    {
        startTurnButton.onClick.AddListener(OnStartTurnClicked);
        resetButton.onClick.AddListener(OnResetClicked);
    }

    private void OnStartTurnClicked()
    {
        GameManager.instance.StartPlayerTurn();
    }
    private void OnResetClicked()
    {
        GameManager.instance.HandleResetLevel();
    }
}
