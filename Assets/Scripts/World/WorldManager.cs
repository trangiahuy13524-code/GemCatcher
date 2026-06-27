using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance { get; private set; }
    [Header("Combo")]
    [SerializeField] int currentCombo = 1;
    [SerializeField] TextMeshProUGUI comboText;
    
    public int CurrentCombo
    {
        get => currentCombo;
        set
        {
            currentCombo = Mathf.Max(1, value);

            if (comboText == null)
                return;

            if (currentCombo <= 1)
            {
                comboText.SetText("");
            }
            else
            {
                comboText.SetText("COMBO: X{0}", currentCombo);
            }
        }
    }

    [Header("Items")]
    [SerializeField] FallingObject obj;
    public ItemData coinSimple;
    public List<ItemData> fallingItems = new List<ItemData>();
    public float interval;
    private float curTime;
    public static HashSet<FallingObject> spawnedObjects = new HashSet<FallingObject>();
    [SerializeField] private int initialPoolSize = 30;
    private Queue<FallingObject> objectPool = new Queue<FallingObject>();

    [Header("Game Over")]
    [SerializeField] float gameOverTime = 30f;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TextMeshProUGUI pointCountTMP;
    [SerializeField] int countFrame = 120;
    [SerializeField] TextMeshProUGUI gameTimeText;
    private bool isGameOver = false;
    private float currentGameTime;

    [Header("Pause Menu")]
    [SerializeField] GameObject pausePanel;
    private bool isPaused = false;


    [Header("Falling config")]
    [SerializeField] private int lowX = -5;
    [SerializeField] private int highX = 5;
    [SerializeField] private int maxStep = 2;
    private int currentDropPosX = 0;
    
    // Update is called once per frame
    [Header("CoinsDisplay")]
    [SerializeField] Transform coinContent;
    [SerializeField] ItemIcon coinIconPrefab;
    Dictionary<ItemData, ItemIcon> itemIcons = new();

    public void AddItemToDisplay(ItemData item, int amount)
    {
        if (item == null || amount <= 0) return;

        // If already exists, increase count
        if (itemIcons.TryGetValue(item, out ItemIcon existingIcon))
        {
            existingIcon.AddAmount(amount);
        }
        else
        {
            // Create new icon
            ItemIcon icon = Instantiate(coinIconPrefab, coinContent);
            icon.Setup(item, amount);

            itemIcons.Add(item, icon);
        }
    }

    public void ClearDisplayItem()
    {
        foreach (var icon in itemIcons.Values)
        {
            if (icon != null)
            {
                Destroy(icon.gameObject);
            }
        }

        itemIcons.Clear();
    }
        

    private void Start()
    {
        Application.targetFrameRate = 60;
        Instance = this;
        currentGameTime = gameOverTime;
        for (int i = 0; i < initialPoolSize; i++)
        {
            FallingObject pooledObj = Instantiate(obj);
            pooledObj.gameObject.SetActive(false);

            objectPool.Enqueue(pooledObj);
        }
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }


        if (curTime >= interval)
        {
            curTime = 0;
            int randomNum = 0;
            if (currentDropPosX == lowX)
            {
                randomNum = Random.Range(0, maxStep + 1);
            }
            else if (currentDropPosX == highX)
            {
                randomNum = Random.Range(-maxStep, 1);
            }
            else
            {
                randomNum = Random.Range(-maxStep, maxStep + 1);
            }
            currentDropPosX += randomNum;
            currentDropPosX = Mathf.Clamp(currentDropPosX, lowX, highX);
            SpawnObject(GetRandomItem(), currentDropPosX);
        }
        else
        {
            curTime += Time.deltaTime;
        }

        if (currentGameTime <= 0)
        {
            if (!isGameOver)
                GameOver();
        }
        else
        {
            currentGameTime -= Time.deltaTime;
            gameTimeText.SetText(Mathf.FloorToInt(currentGameTime).ToString());
        }


    }

    void SpawnObject(ItemData itemData, int xPos)
    {
        FallingObject spawnObject = GetPooledObject();
        spawnObject.SetItemData(itemData);
        spawnObject.transform.position = new Vector3(xPos, 7, 0);
        spawnedObjects.Add(spawnObject);
    }
    private ItemData GetRandomItem()
    {
        // 70% coin
        if (Random.value < 0.7f || fallingItems.Count == 0)
            return coinSimple;

        // 30% weighted selection from fallingItems

        int totalWeight = 0;

        for (int i = 0; i < fallingItems.Count; i++)
        {
            totalWeight += 1 << (fallingItems.Count - 1 - i);
        }

        int roll = Random.Range(0, totalWeight);

        int currentWeight = 0;

        for (int i = 0; i < fallingItems.Count; i++)
        {
            currentWeight += 1 << (fallingItems.Count - 1 - i);

            if (roll < currentWeight)
                return fallingItems[i];
        }

        return fallingItems[0];
    }
    private FallingObject GetPooledObject()
    {
        if (objectPool.Count > 0)
        {
            FallingObject pooledObj = objectPool.Dequeue();
            pooledObj.gameObject.SetActive(true);
            return pooledObj;
        }

        // Optional: expand pool if needed
        FallingObject newObj = Instantiate(obj);
        return newObj;
    }
    public void ReturnToPool(FallingObject obj)
    {
        obj.gameObject.SetActive(false);
        objectPool.Enqueue(obj);
    }

    Coroutine countPointCoroutine;
    void GameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        countPointCoroutine = StartCoroutine(countPoint());
    }

    void TogglePause()
    {
        if (isGameOver) return;

        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }


    IEnumerator countPoint()
    {
        for (int i = 1; i < countFrame + 1; i++)
        {
            pointCountTMP.text = (PlayerData.instance.point * i / countFrame).ToString() + " Points";
            yield return null;
        }
    }

    public void ResetGame()
    {
        isGameOver = false;
        isPaused = false;
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        if (countPointCoroutine != null)
        {
            StopCoroutine(countPointCoroutine);
            countPointCoroutine = null;
        }
        currentGameTime = gameOverTime;
        foreach (FallingObject obj in spawnedObjects)
        {
            if (obj != null)
                ReturnToPool(obj);
        }
        spawnedObjects.Clear();
        PlayerData.instance.ResetPoint();
        ClearDisplayItem();
        CurrentCombo = 1;
        Time.timeScale = 1f;
    }

    public void LoadScene(int indedx)
    {
        if (countPointCoroutine != null)
        {
            StopCoroutine(countPointCoroutine);
            countPointCoroutine = null;
        } 
        
        SceneManager.LoadScene(indedx);
        Time.timeScale = 1f;
    }
}
