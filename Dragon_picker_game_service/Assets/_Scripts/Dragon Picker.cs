using OpenCover.Framework.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;
using static DragonPicker;

public enum ElementsEnum
{
    Fire,
    Earth,
    Wind
}

public class DragonPicker : MonoBehaviour
{
    [Serializable]
   public class ElementsListItem
    {
        public GameObject elementObj;
        public int count = 0;

        public ElementsListItem(GameObject elementObj, int count)
        {
            this.elementObj = elementObj;
            this.count = count;
        }
    }

    private void OnEnable() => YandexGame.GetDataEvent += GetYGSaves;
    private void OnDisable() => YandexGame.GetDataEvent -= GetYGSaves;

    public List<GameObject> elementsList;

    [SerializeField] private int numEnegryShield = 3;
    [SerializeField] private float energyShieldBottomY = -6f;
    [SerializeField] private float energyShieldRadius = 1.5f;
    [SerializeField] private int elementsCopacity = 5;
    [SerializeField] private float damageMultiplier = 1;
    [SerializeField] private GameObject energyShieldPrefab;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private List<GameObject> shieldList;

    private int timeBetweenSpawnElements = 3;
    private float time = 0;
    private int score = 0;
    private bool isDamageBlocked = false;
    private bool isParryState = false;
    private Dictionary<ElementsEnum, ElementsListItem> elementsDict;
    private ElementsEnum?[] elementsHand = new ElementsEnum?[2];
    private NewBehaviourScript enemyDragon;

    private TextMeshProUGUI scoreGT;
    private TextMeshProUGUI playerNameGT;
    private TextMeshProUGUI elementsGT;
    private TextMeshProUGUI elementsHandGT;

    private ElementsEnum TagToElementrsEnum(string tag)
    {
        if (tag.Contains("Fire")) { return ElementsEnum.Fire; }
        else if (tag.Contains("Earth")) { return ElementsEnum.Earth; }
        else if (tag.Contains("Wind")) { return ElementsEnum.Wind; }
        else
        {
            throw new Exception(tag);
        }
    }

    private void ToggleBlock()
    {
        isDamageBlocked = !isDamageBlocked;
    }

    private void RemoveAura()
    {
        damageMultiplier = 1;
    }

    private void RemoveParryState() 
    {
        isParryState = false;
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

        elementsDict = elementsList.ToDictionary(e => TagToElementrsEnum(e.tag), e => new ElementsListItem(e, 0));

        enemyDragon = GameObject.Find("Enemy").GetComponent<NewBehaviourScript>();

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
        /*if (elementsList[0].count > 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                AddElementToHand(ElementsEnum.Earth);
        }
        if (elementsList[1].count > 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
                AddElementToHand(ElementsEnum.Fire);
        }
        if (elementsList[2].count > 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
                AddElementToHand(ElementsEnum.Wind);
        }
        if (elementsHand[0] is not null)
            if (Input.GetKeyDown(KeyCode.G))
            {
                ExcSpell();              
            }*/
    }

    public void EggHitShield()
    {
        if (isDamageBlocked) return;
        else if (isParryState)
        {
            enemyDragon.GetDamage((float)(3 * (28 - Math.Pow(shieldList.Count, 3))), enemyDragon.element, false);
        }

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
        var newElement = TagToElementrsEnum(tag);
        elementsDict[newElement].count = Math.Clamp(elementsDict[newElement].count + 1, 0, elementsCopacity);
        elementsGT.text = string.Join("\n", elementsDict
            .Select((e) => $"{TagToElementrsEnum(e.Value.elementObj.tag)} - {e.Value.count}")
            .ToArray()
        );
    }

    public void SpawnElement()
    {
        var r = new System.Random();
        var element = r.Next(0,3);
        var coordinatesWidth = r.Next(-19, 19);
        var coordinatesHeight = r.Next(-5, 9);
        Instantiate(elementsList[element], new Vector3(coordinatesWidth, coordinatesHeight, -2), new Quaternion(0,0,0,0));
    }

    public void AddElementToHand(ElementsEnum element)
    {
        if (elementsDict[element].count > 0 )
        {
            elementsDict[element].count -= 1;
            elementsGT.text = string.Join("\n", elementsDict
               .Select((e) => $"{TagToElementrsEnum(e.Value.elementObj.tag)} - {e.Value.count}")
               .ToArray()
            );

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
        
    }

    public void ExcSpell()
    {
        if (elementsHand[0] == ElementsEnum.Earth && elementsHand[1] == null) { ShootProjectile(ElementsEnum.Earth, 2, 15); }
        else if (elementsHand[0] == ElementsEnum.Fire && elementsHand[1] == null) {  ShootProjectile(ElementsEnum.Fire, 2, 5, false, true); }
        else if (elementsHand[0] == ElementsEnum.Wind && elementsHand[1] == null) { ShootProjectile(ElementsEnum.Wind, 4, 30); }
        else if (elementsHand[0] == ElementsEnum.Fire && elementsHand[1] == ElementsEnum.Wind) { ShootProjectile(ElementsEnum.Fire, 1, 10f, true, true); }
        else if (elementsHand[0] == ElementsEnum.Wind && elementsHand[1] == ElementsEnum.Fire) { MakeParryState(); }
        else if (elementsHand[0] == ElementsEnum.Fire && elementsHand[1] == ElementsEnum.Earth) { for(int i = 0; i<5;i++) { ShootProjectile(ElementsEnum.Earth, 3, 10); } }
        else if (elementsHand[0] == ElementsEnum.Earth && elementsHand[1] == ElementsEnum.Wind) { MakeShield(); }
        else if (elementsHand[0] == ElementsEnum.Wind && elementsHand[1] == ElementsEnum.Earth) { enemyDragon.TempIncressSpeedMultiplier(); }
        else if (elementsHand[0] == ElementsEnum.Earth && elementsHand[1] == ElementsEnum.Fire) { MakeAura(); }

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

    public void ShootProjectile(ElementsEnum type, float speed, float damage, bool autoAim = false, bool isDurational = false)
    {
        Debug.Log("Shooted");
        var shootedProjectile = Instantiate<GameObject>(projectilePrefab, shieldList[0].transform.position + new Vector3(0, 2, 0), shieldList[0].transform.rotation).GetComponent<Projectile>();
        shootedProjectile.speed = speed;
        shootedProjectile.damage = damage * damageMultiplier;
        shootedProjectile.type = type;
        shootedProjectile.autoAim = autoAim;
        shootedProjectile.isDurational = isDurational;
    }

    public void MakeShield()
    {
        isDamageBlocked = true;
        Invoke("ToggleBlock", 5);
    }

    public void MakeAura()
    {
        damageMultiplier = 2;
        Invoke("RemoveAura", 3);
    }

    public void MakeParryState() 
    {
        isParryState = true;
        Invoke("RemoveParryState", 1);
    }
}
