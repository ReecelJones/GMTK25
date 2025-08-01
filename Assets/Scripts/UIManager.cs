using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public Button startLoopButton;

    private void Start()
    {
        startLoopButton.onClick.AddListener(OnStartLoopClicked);
    }

    private void OnStartLoopClicked()
    {
        GameManager.instance.StartPlayerTurn(); // Custom method to trigger turn
    }
}
