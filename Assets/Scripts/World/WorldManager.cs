using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] FallingObject obj;
    public ItemData coinSimple;
    public List<ItemData> items = new List<ItemData>();
    public float interval;
    private float curTime;

    [Header("Game Over")]
    [SerializeField] float gameOverTime = 30f;
    [SerializeField] GameObject gameOverPanel;
    private bool isGameOver = false;
    private float currentGameTime;

    [Header("Falling config")]
    [SerializeField] private int lowX = -5;
    [SerializeField] private int highX = 5;
    [SerializeField] private int maxStep = 2;
    private int currentDropPosX = 0;
    
    // Update is called once per frame

    private void Start()
    {
        currentGameTime = gameOverTime;
    }

    void Update()
    {

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
            if (items.Count > 0)
            {
                int rand = Random.Range(0, items.Count);
                
                SpawnObject(items[rand], currentDropPosX);
            }
            else
            {
                SpawnObject(coinSimple, currentDropPosX);
            }
        }
        else
        {
            curTime += Time.deltaTime;
        }

        if (currentGameTime <= 0)
        {
            if (isGameOver)
                GameOver();
        }
        else
        {
            currentGameTime -= Time.deltaTime;
        }
    }

    void SpawnObject(ItemData itemData, int xPos)
    {
        FallingObject spawnObject = Instantiate(obj);
        spawnObject.SetItemData(itemData);
        spawnObject.transform.position = new Vector3(xPos, 7, 0);
    }

    void GameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
    }

    public void ResetGame()
    {
        isGameOver = false;
        gameObject.SetActive(false);
        currentGameTime = gameOverTime;
    }
}
