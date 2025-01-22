using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Text gasText;
    public GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void UpdateGasUI(float gas)
    {
        gasText.text = "" + Mathf.Max(0, gas).ToString("F0");
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void HideGameOverPanel()
    {
        gameOverPanel.SetActive(false);
    }

    public void OnGameRestartButton()
    {
        GameManager.Instance.StartGame();
        HideGameOverPanel();
    }
}