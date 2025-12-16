using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    [Header("UI Components")]
    public Image fadePanel;        // 검은색 패널
    public GameObject credits;       // 크레딧 텍스트

    [Header("Settings")]
    public string nextSceneName = "GameScene"; // 이동할 씬 이름
    public float fadeDuration = 3.0f;          // 페이드 걸리는 시간
    public float waitSeconds = 1.0f;
    public float creditSeconds = 3.0f;           // 크레딧 보여주는 시간

    void Start()
    {
        // 시작할 때 패널은 투명하게, 크레딧은 안 보이게 초기화
        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(true);
            fadePanel.color = new Color(0, 0, 0, 0);
        }
        if (credits != null) credits.SetActive(false);
    }

    // 외부(DialogueManager)에서 이 함수를 부르면 엔딩이 시작됨
    public void StartEndingSequence()
    {
        StartCoroutine(ProcessEnding());
    }

    IEnumerator ProcessEnding()
    {
        // 1. 페이드 아웃 (점점 어둡게)
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = time / fadeDuration;
            if (fadePanel != null) fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        if (fadePanel != null) fadePanel.color = new Color(0, 0, 0, 1);

        yield return new WaitForSeconds(waitSeconds);

        // 2. 크레딧 켜기
        if (credits != null) credits.SetActive(true);

        // 3. 대기
        yield return new WaitForSeconds(creditSeconds);

        // 4. 씬 이동
        SceneManager.LoadScene(nextSceneName);
    }
}