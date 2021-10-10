using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject containerPrefab;
    public GameObject worldButtonPrefab;
    public GameObject levelButtonPrefab;
    public GameObject HUDPrefab;
    public GameObject resultPrefab;
    public Font angrybirdsFont;
    
    private CanvasGroup[] displays;
    private GameObject[] worldButtons;
    private GameObject[] levelButtons;
    private GameObject hud;
    private GameObject results;
    private int currentDisplay;
    private int worldIndex;
    private int levelIndex;
    private GameManager manager;

    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        currentDisplay = 0;
        InitDisplays();        
    }
    void Update()
    {
        UpdateCurrent();
        UpdateDisplayed();
    }
    void  UpdateCurrent()
    {
        if (Input.GetKeyUp(KeyCode.Backspace) && currentDisplay > 0)
        {
            currentDisplay -= 1;
        }
        for (int i = 0; i < displays.Length; i++)
        {
            if (displays[i] != null)
            {
                if (i == currentDisplay)
                {
                    displays[i].alpha = 1;
                    displays[i].blocksRaycasts = true;
                }
                else
                {
                    displays[i].alpha = 0;
                    displays[i].blocksRaycasts = false;
                }
            }
        }
    }
    public GameManager Manager
    {
        get{ return manager;}
    }
    public int WorldIndex
    {
        set
        {
            //buda çalışmoıyor current world eşitleyemiyor pic
            worldIndex = value;
            manager.CurrWorld = manager.allWorlds[worldIndex];
            //bende bunun yerine aşagıdaki clicked fonksiyonuna ekleme yaptım
        }
        get
        {
            return worldIndex;
        }
    }
    public int LevelIndex
    {
        set
        {
            //bu siktimin settrları çalışmıyor managerın current levelini eşitleyemiyor 
            levelIndex = value;
            manager.CurrLevel = manager.CurrWorld.Levels[levelIndex];
        }
        get
        {
            return levelIndex;
        }
    }
    void UpdateDisplayed()
    {
        switch (currentDisplay)
        {
            case 0:
                UpdateWorldButtons();
                break;

            case 1:
                UpdateLevelButtons();
                break;
            
            case 2:
                if (manager.currentGame != null)
                {
                    if (manager.currentGame.gameOver)
                    {
                        Debug.Log("Going to results");
                        currentDisplay += 1;
                    }
                    else
                    {
                        hud.GetComponent<HUDManager>().UpdateHUD(manager.CurrLevel.CurrentScore,
                        manager.CurrLevel.Highscore, LevelIndex + 1, manager.currentGame.numAsteroids);
                    }
                }
                else if(manager.currentGame == null && !Game.isMenuOpn)
                {
                    manager.StartGame(LevelIndex, WorldIndex);    
                }
                break;
            case 3:
                if (manager.currentGame != null)
                {
                    results.GetComponent<Results>().UpdateResults(GetComponent<UIManager>());
                    Destroy(manager.currentGame.gameObject);
                    manager.currentGame = null;
                }
                break;
        }
    }
    void UpdateLevelButtons()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            Text levelText = levelButtons[i].transform.Find("LevelText").GetComponent<Text>();
            levelText.text = (i + 1).ToString();
            GameObject stars = levelButtons[i].transform.Find("Stars").gameObject;
            if (i == 0 || manager.allWorlds[worldIndex].Levels[i].Unlocked)
            {
                stars.SetActive(true);
                levelButtons[i].GetComponent<Button>().interactable = true;
                for (int j = 0; j < stars.transform.childCount; j++)
                {
                    stars.transform.GetChild(j).gameObject.SetActive(false);
                    if (manager.allWorlds[worldIndex].Levels[i].HighStarScore > j)
                    {
                        stars.transform.transform.GetChild(j).gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                stars.SetActive(false);
                levelButtons[i].GetComponent<Button>().interactable = false;
            }          
        }
    }
    void UpdateWorldButtons()
    {
        for (int i = 0; i < worldButtons.Length; i++)
        {
            Text titleText = worldButtons[i].transform.Find("TitleText").GetComponent<Text>();
            titleText.text = "WORLD" + (i + 1).ToString();
            Text infoText = worldButtons[i].transform.Find("InfoPanel").
            Find("InfoText").GetComponent<Text>();
            infoText.text = "Levels:" + manager.allWorlds[i].TotalDefeated + " / " + GameManager.numLevels;
            infoText.text += "\nStars:" + manager.allWorlds[i].TotalStars + " / 60";
            infoText.text += "\nScore:" + manager.allWorlds[i].TotalScore;
        }
    }
    void InitDisplays()
    {
        displays = new CanvasGroup[4];
        worldButtons = InitializeItems(worldButtonPrefab,"WORLDS",GameManager.numWorlds,
        180, 300, 10, GameManager.numWorlds);

        levelButtons = InitializeItems(levelButtonPrefab, "LEVELS", GameManager.numLevels, 75, 75, 10, 5);

        hud = InitUIElement(HUDPrefab, transform);
        displays[2] = hud.GetComponent<CanvasGroup>();
        
        results = InitUIElement(resultPrefab, transform);
        displays[3] = results.GetComponent<CanvasGroup>();

        UpdateFont();
    }
    void UpdateFont()
    {
        Text[] allText = FindObjectsOfType<Text>();
        foreach (Text text in allText)
        {
            if (text.font != angrybirdsFont)
            {
                text.font = angrybirdsFont;
            }
        }
    }
    GameObject[] InitializeItems(GameObject prefab, string title, int numItems,
    int itemWidth, int itemHight, int padding, int cols)
    {
        GameObject[] temp = new GameObject[numItems];
        int rows = numItems /cols;
        int leftinset = padding;
        int titleSpace = 50;
        int topinset = titleSpace;

        GameObject container = InitUIElement(containerPrefab, transform);
        container.GetComponentInChildren<Text>().text = title;
        RectTransform contRect = container.GetComponent<RectTransform>();
        contRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
        (itemWidth + padding) * cols + padding);
        contRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
        (itemHight + padding) * rows + titleSpace);
        container.AddComponent<CanvasGroup>();
        for (int i = 0; i < displays.Length; i++)
        {
            if (displays[i] == null)
            {
                displays[i] = container.GetComponent<CanvasGroup>();
                break;
            }
        }
        int colCount = 0;
        for (int i = 0; i < numItems; i++)
        {
            GameObject item = InitUIElement(prefab, container.transform);
            item.name = i.ToString();
            RectTransform itemRect = item.GetComponent<RectTransform>();
            itemRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, leftinset, itemWidth);
            itemRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, topinset, itemHight);
            leftinset += (itemWidth + padding);
            colCount++;
            if (colCount == cols)
            {
                colCount = 0;
                topinset += (padding + itemHight);
                leftinset = padding;
            }
            temp[i] = item;
        }
        return temp;
    }
    public GameObject InitUIElement(GameObject prefab, Transform parent)
    {
        GameObject temp = Instantiate(prefab) as GameObject;
        temp.transform.SetParent(parent);
        RectTransform tempRect = temp.GetComponent<RectTransform>();
        RectTransform prefabRect = prefab.GetComponent<RectTransform>();
        tempRect.localPosition = prefabRect.localPosition;
        tempRect.localScale = prefabRect.localScale;
        
        if (temp.GetComponent<Button>())
        {
            Button b = temp.GetComponent<Button>();
            b.onClick.AddListener(() => Clicked(b));
        }
        return temp;
    }
    public void Clicked(Button b)
    {
        // setterlar çalışmadığından buraya ekleme yaptımm currwolrd ile currlevelleri static yapıp buradan eriştim ve daha sonra 
        // onları burada değiştirdim ve çalıştılar artık yaklaşık 1 saat boynuca bunlar ile uğraştım ama sonunda çözdüm : ) 
        if (currentDisplay == 0)
        {
            currentDisplay += 1;
            worldIndex = int.Parse(b.name);
            GameManager.currWorld = manager.allWorlds[worldIndex];
            Game.isMenuOpn  = false;
            Debug.Log(Game.isMenuOpn);
        }
        else if(currentDisplay == 1)
        {
            currentDisplay += 1;
            levelIndex = int.Parse(b.name);
            GameManager.currLevel = manager.CurrWorld.Levels[levelIndex];
        }
        else if(currentDisplay == 3)
        {
            if (b.name == "BackButton")
            {
                currentDisplay = 1;
            }
            else if(b.name == "NextButton")
            {
                levelIndex += 1;
                currentDisplay = 2;
            }
            else if(b.name == "ResetButton")
            {
                currentDisplay = 2;
            }
        }
    }
}
