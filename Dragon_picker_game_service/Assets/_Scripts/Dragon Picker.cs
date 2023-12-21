using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class DragonPicker : MonoBehaviour
{
    private void OnEnable() => YandexGame.GetDataEvent += GetYGSaves;
    private void OnDisable() => YandexGame.GetDataEvent -= GetYGSaves;

    [SerializeField] private GameObject energyShieldPrefab;
    [SerializeField] private int numEnegryShield = 3;
    [SerializeField] private float energyShieldBottomY = -6f;
    [SerializeField] private float energyShieldRadius = 1.5f;
    [SerializeField] private List<GameObject> shieldList;
    [SerializeField] private List<int> elementsInInventoryCount;
    [SerializeField] private List<GameObject> elementsList;
    [SerializeField] private int elementsCount;
    private int timeBetweenSpawnElements = 5;
    private float time = 0;
    private int score = 0;

    public TextMeshProUGUI scoreGT;
    public TextMeshProUGUI playerNameGT;

    void Start()
    {
        shieldList = new List<GameObject>();

        for(int i =1; i<= numEnegryShield; i++)
        {
            GameObject tShieldGo = Instantiate<GameObject>(energyShieldPrefab);
            tShieldGo.transform.position = new Vector3(0, energyShieldBottomY, 0);
            tShieldGo.transform.localScale = new Vector3(1 * i, 1 * i, 1 * i);
            shieldList.Add(tShieldGo);
        }

        scoreGT = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        scoreGT.text = "Score: 0";

        playerNameGT = GameObject.Find("Player Name").GetComponent<TextMeshProUGUI>();

        if (YandexGame.SDKEnabled) { GetYGSaves(); }
    }

    // Update is called once per frame
    void Update()
    {
        time = time + Time.deltaTime;
        if (time >= timeBetweenSpawnElements)
        {
            Debug.Log("Time passed");
            SpawnElement();
            time = 0;
        }
    }

    public void EggHitShield()
    {
        GameObject tShieldGo = shieldList[shieldList.Count - 1];
        shieldList.RemoveAt(shieldList.Count - 1);
        Destroy(tShieldGo);

        if(shieldList.Count <= 0)
        {
            YandexGame.RewVideoShow(0);
            YGSaveData("Береги щиты!");
            SceneManager.LoadScene("_0Scene");
        }
    }

    public void EggHitGround()
    {
        score += 1;
        scoreGT.text = $"Score: {score}";
    }

    public void SpawnElement()
    {
        Debug.Log("element must be spawned");
        var r = new System.Random();
        var element = r.Next(0,4);
        var coordinatesWidth = r.Next(-19, 19);
        var coordinatesHeight = r.Next(-9, 9);
        Instantiate(elementsList[element], new Vector3(coordinatesWidth, coordinatesHeight, 0), new Quaternion(0,0,0,0));
    }

    public void GetYGSaves()
    {
        playerNameGT.text = YandexGame.playerName;
    }

    public void YGSaveData(string? newAchivment)
    {
        YandexGame.savesData.bestScore = Math.Max(YandexGame.savesData.bestScore, score);
        YandexGame.NewLeaderboardScores("TopPlayerScore", score);

        if(newAchivment != null)
        {
            YandexGame.savesData.achivments = YandexGame.savesData.achivments
                .Append(newAchivment)
                .Distinct()
                .ToArray();
        }

        YandexGame.SaveProgress();
    }
}
