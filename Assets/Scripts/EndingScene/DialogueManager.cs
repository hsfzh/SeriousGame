using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public EndingManager endingManager;

    public GameObject textPanel;
    public Text textDisplay;
    public float typingSpeed = 0.05f;

    private string[] sentences;
    private int index = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private bool isFinished = false;

    void Start()
    {
        sentences = new string[] {
            "사랑이 필요하다.",
            "작금의 시대엔 온갖 분열과 혐오로 가득하다.",
            "그래서 우리는 작전이 필요하다.",
            "지구를 가득 덮은 모든 미움들을 사랑으로 뒤바꿀 작전이.",
            "… 그런데 어떻게?",
            "일단 조금씩이라도 좋으니 사랑을 해보자.",
            "미운 놈에게 떡이라도 하나 더 줘보고,",
            "불안을 분노로 표출하는 이들에 숨겨진 외로움을 헤아려보고,",
            "색안경을 낀 이들의 눈을 다정하게 맞춰보고,",
            "말이 통하지 않는 이들에게도 한 번쯤 손을 내밀어보자.",
            "그렇게 사랑이 하나씩 모일수록 비로소 우린 진심으로 서로를 마주할 수 있다.",
            "사랑은 절대 지지 않기에!",
            "서로 사랑하며 살아가보자."
        };

        StartCoroutine(Type());
    }

    void Update()
    {
        if (isFinished) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (textDisplay.text == sentences[index])
            {
                NextSentence();
            }
            else if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                textDisplay.text = sentences[index];
                isTyping = false;
            }
        }
    }

    IEnumerator Type()
    {
        isTyping = true;
        textDisplay.text = "";

        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            typingCoroutine = StartCoroutine(Type());
        }
        else
        {
            textDisplay.text = "";
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        isFinished = true;
        
        textPanel.SetActive(false);
        endingManager.StartEndingSequence();
    }
}