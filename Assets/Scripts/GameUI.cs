using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public TextMeshProUGUI victoryPanelTimerLabel;
    public TextMeshProUGUI hudTimerLabel;

    private float time;
    private bool isPaused;

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ShowVictoryPanel()
    {
        victoryPanel.SetActive(true);
        Time.timeScale = 0;

        isPaused = true;
        UpdateTimerLabel(victoryPanelTimerLabel);
    }

    void Update()
    {
        if (isPaused)
        {
            return;
        }

        time += Time.deltaTime;

        UpdateTimerLabel(hudTimerLabel);
    }

    private void UpdateTimerLabel(TextMeshProUGUI label)
    {
        var minutes = time / 60;
        var seconds = time % 60;
        var fraction = (time * 100) % 100;

        label.text = $"{minutes:00} : {seconds:00} : {fraction:000}";
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
