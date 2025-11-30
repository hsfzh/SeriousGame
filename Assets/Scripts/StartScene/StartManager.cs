using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    [SerializeField] private GameObject monsterPanel;
    [SerializeField] private GameObject skillPanel;
    private GameObject activePanel;
    // Start is called before the first frame update
    void Start()
    {
        monsterPanel.SetActive(false);
        skillPanel.SetActive(false);
    }
    // Update is called once per frame
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
