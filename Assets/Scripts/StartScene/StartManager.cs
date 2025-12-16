using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button monsterButton;
    [SerializeField] private GameObject monsterPanel;
    private GameObject activePanel;

    void Start()
    {
        monsterPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }
    }

    public void ClosePanel()
    {
        if (activePanel)
        {
            activePanel.SetActive(false);
            activePanel = null;

            startButton.interactable = true;
            monsterButton.interactable = true;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void ActivatePanel(GameObject panel)
    {
        if (activePanel)
            return;
        activePanel = panel;
        panel.SetActive(true);

        startButton.interactable = false;
        monsterButton.interactable = false;
    }
}
