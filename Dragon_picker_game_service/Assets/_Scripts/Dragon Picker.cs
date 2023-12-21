using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public enum ElementsEnum
{
    Fire,
    Earth,
    Wind
}

public class DragonPicker : MonoBehaviour
{
    [Serializable]
    class ElementsListItem
    {
        public GameObject elementObj;
        public int count = 0;
    }

    private void OnEnable() => YandexGame.GetDataEvent += GetYGSaves;
    private void OnDisable() => YandexGame.GetDataEvent -= GetYGSaves;

    
    [SerializeField] private int numEnegryShield = 3;
    [SerializeField] private float energyShieldBottomY = -6f;
    [SerializeField] private float energyShieldRadius = 1.5f;
    [SerializeField] private GameObject energyShieldPrefab;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private List<GameObject> shieldList;
    [SerializeField] private List<ElementsListItem> elementsList;

    private int timeBetweenSpawnElements = 3;
    private float time = 0;
    private int score = 0;
    private ElementsEnum?[] elementsHand = new ElementsEnum?[2];

    private TextMeshProUGUI scoreGT;
    private TextMeshProUGUI playerNameGT;
    private TextMeshProUGUI elementsGT;
    private TextMeshProUGUI elementsHandGT;

    private ElementsEnum tagToElementrsEnum(string tag)
    {
        if (tag.Contains("Fire")) { return ElementsEnum.Fire; }
        else if (tag.Contains("Earth")) { return ElementsEnum.Earth; }
        else if (tag.Contains("Wind")) { return ElementsEnum.Wind; }
        else
        {
            throw new Exception(tag);
        }
    }

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
        elementsGT = GameObject.Find("Elements").GetComponent<TextMeshProUGUI>();
        elementsHandGT = GameObject.Find("ElementsHand").GetComponent<TextMeshProUGUI>();

        if (YandexGame.SDKEnabled) { GetYGSaves(); }
    }

    void Update()
    {
        time = time + Time.deltaTime;
        if (time >= timeBetweenSpawnElements)
        {
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

    public void ElementHit(string tag)
    {
        foreach (var element in elementsList)
        {
            if (element.elementObj.tag == tag)
            {
                element.count += 1;
                elementsGT.text = string.Join("\n", elementsList
                    .Select((e) => $"{tagToElementrsEnum(e.elementObj.tag)} - {e.count}")
                    .ToArray()
                );
                break;
            }
        }
    }

    public void SpawnElement()
    {
        var r = new System.Random();
        var element = r.Next(0,3);
        var coordinatesWidth = r.Next(-19, 19);
        var coordinatesHeight = r.Next(-9, 9);
        Instantiate(elementsList[element].elementObj, new Vector3(coordinatesWidth, coordinatesHeight, 0), new Quaternion(0,0,0,0));
    }

    public void AddElementToHand(ElementsEnum element)
    {
        for (var i = 0; i < elementsHand.Length; i++) 
        { 
            if (elementsHand[i] == null)
            {
                elementsHand[i] = element;
                elementsHandGT.text = string.Join(" ", elementsHand);
                break;
            }
        }
    }

    public void ExcSpell()
    {
        if (elementsHand[0] == ElementsEnum.Earth && elementsHand[1] == null) {  }
        else if (elementsHand[0] == ElementsEnum.Fire && elementsHand[1] == null) { }
        else if (elementsHand[0] == ElementsEnum.Wind && elementsHand[1] == null) { }
        else if (elementsHand[0] == ElementsEnum.Fire && elementsHand[1] == ElementsEnum.Wind) { }
        else if (elementsHand[0] == ElementsEnum.Wind && elementsHand[1] == ElementsEnum.Fire) { }
        else if (elementsHand[0] == ElementsEnum.Fire && elementsHand[1] == ElementsEnum.Earth) { }
        else if (elementsHand[0] == ElementsEnum.Earth && elementsHand[1] == ElementsEnum.Wind) { }
        else if (elementsHand[0] == ElementsEnum.Wind && elementsHand[1] == ElementsEnum.Earth) { }
        else if (elementsHand[0] == ElementsEnum.Earth && elementsHand[1] == ElementsEnum.Fire) { }

        elementsHand = new ElementsEnum?[elementsHand.Length];
        elementsHandGT.text = "";
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
