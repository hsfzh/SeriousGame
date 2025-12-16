using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
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
            if (activePanel)
            {
                activePanel.SetActive(false);
                activePanel = null;
            }
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
    }
}
