using UnityEngine;
using UnityEngine.UI; 
using System.Collections.Generic;

[System.Serializable] 
public struct MonsterInfo
{
    public string monsterName;       
    public bool isElite;             
    [TextArea] public string description; 
    public Sprite sprite;            
    public float scaleFactor;        
    public int unlockWave;           
}

public class MonsterDogamManager : MonoBehaviour
{
    [Header("UI Components")]
    public Image monsterImage;       
    public Text nameText;            
    public Text descText;            
    public Button leftBtn;           
    public Button rightBtn;          
    
    // 사용자가 도달한 최대 웨이브 수. 나중에 게임에서 연결해야됨
    public int currentMaxWave = 0;   
    
    [Header("Monster Data List")]
    public MonsterInfo[] monsterDataList; 

    private int currentIndex = 0;    

    void Start()
    {
        leftBtn.onClick.AddListener(OnClickLeft);
        rightBtn.onClick.AddListener(OnClickRight);

        UpdateUI();
    }

    void OnClickLeft()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateUI();
        }
    }

    void OnClickRight()
    {
        if (currentIndex < monsterDataList.Length - 1)
        {
            currentIndex++;
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        MonsterInfo data = monsterDataList[currentIndex];
        bool isUnlocked = currentMaxWave >= data.unlockWave;

        leftBtn.gameObject.SetActive(currentIndex > 0);
        rightBtn.gameObject.SetActive(currentIndex < monsterDataList.Length - 1);

        string gradeTag = data.isElite ? "<color=#f04f3a>[Elite]</color>" : "<color=#898b91>[Common]</color>";
        nameText.text = $"{gradeTag} {data.monsterName}";

        monsterImage.sprite = data.sprite;
        monsterImage.SetNativeSize();
        monsterImage.transform.localScale = Vector3.one * data.scaleFactor;

        if (isUnlocked)
        {
            monsterImage.color = Color.white; 
            descText.text = data.description;
        }
        else
        {
            monsterImage.color = Color.black; 
            descText.text = $"<b>{data.unlockWave}웨이브</b>에 도달하면 정보를 확인할 수 있습니다.";
        }
    }
}