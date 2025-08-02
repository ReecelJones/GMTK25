using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultHandler : MonoBehaviour
{
    private UIManager uiManager;

    public GameObject resultScreen;
    public Color winColor, loseColor;
    public TextMeshProUGUI resultTxt, buttonTxt;
    public Button resultNextButton;
    public bool didPlayerWin;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager = FindFirstObjectByType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (didPlayerWin)
        {
            resultNextButton.onClick.AddListener(uiManager.NextLevel);
            buttonTxt.text = "Next Level";
            resultScreen.GetComponent<Image>().color = winColor;

            resultTxt.text = "Victory";
        }
        else
        {
            resultNextButton.onClick.AddListener(uiManager.OnResetClicked);
            buttonTxt.text = "Retry";
            resultScreen.GetComponent<Image>().color = loseColor;


            resultTxt.text = "Defeat";
        }
    }

    public void DisplayEndScreen()
    {
        resultScreen.SetActive(true);
    }
}
