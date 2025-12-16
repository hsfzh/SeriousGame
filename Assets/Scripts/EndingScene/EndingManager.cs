using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DialogueStep
{
    [TextArea] public string sentence; // 대사
    public EndingEnemy targetEnemy; // 찾아갈 적
}

public class EndingManager : MonoBehaviour
{
    [Header("UI & Object Refs")]
    public Text textDisplay;
    public Transform playerTransform;
    public QuestPointer questPointer;
    public Image fadePanel;
    public GameObject credits;

    [Header("Settings")]
    public float typingSpeed = 0.05f;
    public float interactionDistance = 2.0f;
    public string nextSceneName = "TitleScene";

    public DialogueStep[] dialogues; 

    private int index = 0;
    private bool isTyping = false;
    private bool isWaitingForInteraction = false;
    private bool isTerminated = false;
    private EndingEnemy currentTarget;

    void Start()
    {
        fadePanel.color = new Color(0,0,0,0);
        credits.SetActive(false);
        questPointer.HidePointer();

        StartCoroutine(Type());
    }

    void Update()
    {
        if (isWaitingForInteraction && currentTarget != null)
        {
            float distance = Vector2.Distance(playerTransform.position, currentTarget.transform.position);

            if (distance <= interactionDistance && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
            {
                CompleteInteraction();
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                textDisplay.text = dialogues[index].sentence;
                isTyping = false;
                
                CheckIfInteractionNeeded();
            }
            else
            {
                NextSentence();
            }
        }
    }

    void CheckIfInteractionNeeded()
    {
        if (dialogues[index].targetEnemy != null)
        {
            isWaitingForInteraction = true;
            currentTarget = dialogues[index].targetEnemy;

            currentTarget.StartHighlight();
            
            questPointer.SetTarget(currentTarget.transform);
        }
    }

    void CompleteInteraction()
    {
        currentTarget.StopHighlight();
        
        questPointer.HidePointer();

        isWaitingForInteraction = false;
        currentTarget = null;

        NextSentence();
    }

    IEnumerator Type()
    {
        isTyping = true;
        textDisplay.text = "";
        
        string sentence = dialogues[index].sentence;

        foreach (char letter in sentence.ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        
        CheckIfInteractionNeeded();
    }

    void NextSentence()
    {
        if (isTerminated) return;

        if (index < dialogues.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            StartCoroutine(EndingSequence());
        }
    }

    IEnumerator EndingSequence()
    {
        isTerminated = true;

        float duration = 3f;
        float time = 0f;

        fadePanel.gameObject.SetActive(true);
        while (time < duration)
        {
            time += Time.deltaTime;
            fadePanel.color = new Color(0, 0, 0, time / duration);
            yield return null;
        }

        fadePanel.color = Color.black;
        yield return new WaitForSeconds(1f);

        credits.SetActive(true);

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(nextSceneName);
    }
}