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
    Огонь,
    Земля,
    Ветер
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

    [SerializeField] private float energyShieldBottomY = -6f;
    [SerializeField] private float energyShieldRadius = 1.5f;
    [SerializeField] private float baseDamage = 5;
    [SerializeField] private float baseSpellSpeed = 5;
    [SerializeField] private GameObject energyShieldPrefab;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject blockShieldPrefab;
    [SerializeField] private GameObject parryShieldPrefab;
    [SerializeField] private GameObject auraPrefab;
    [SerializeField] private List<GameObject> shieldList;

    private float damageMultiplier = 1;
    private int timeBetweenSpawnElements = 3;
    private float time = 0;
    private bool isDamageBlocked = false;
    private bool isParryState = false;
    private Dictionary<ElementsEnum, ElementsListItem> elementsDict;
    private ElementsEnum?[] elementsHand = new ElementsEnum?[2];
    private NewBehaviourScript enemyDragon;
    private EndModal endModal;
    private GameObject blockShield;
    private GameObject parryShield;
    private GameObject aura;

    private TextMeshProUGUI playerNameGT;
    private TextMeshProUGUI elementsGT;
    private TextMeshProUGUI elementsHandGT;

    private ElementsEnum TagToElementrsEnum(string tag)
    {
        if (tag.Contains("Fire")) { return ElementsEnum.Огонь; }
        else if (tag.Contains("Earth")) { return ElementsEnum.Земля; }
        else if (tag.Contains("Wind")) { return ElementsEnum.Ветер; }
        else
        {
            throw new Exception(tag);
        }
    }

    private void ToggleBlock()
    {
        isDamageBlocked = !isDamageBlocked;
        Destroy(blockShield);
    }

    private void RemoveAura()
    {
        damageMultiplier = 1;
        Destroy(aura);
    }

    private void RemoveParryState() 
    {
        isParryState = false;
        Destroy(parryShield);
    }

    void Start()
    {
        shieldList = new List<GameObject>();

        for(int i =1; i<= YandexGame.savesData.numEnegryShield; i++)
        {
            GameObject tShieldGo = Instantiate<GameObject>(energyShieldPrefab);
            tShieldGo.transform.position = new Vector3(0, energyShieldBottomY, 0);
            tShieldGo.transform.localScale = new Vector3(1 * i, 1 * i, 1 * i);
            shieldList.Add(tShieldGo);
        }

        elementsDict = elementsList.ToDictionary(e => TagToElementrsEnum(e.tag), e => new ElementsListItem(e, 0));

        enemyDragon = GameObject.Find("Enemy").GetComponent<NewBehaviourScript>();
        endModal = GameObject.FindObjectOfType<EndModal>(true);

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
        if(blockShield is not null)
            blockShield.transform.position = shieldList[0].transform.position;
        if (aura is not null)
            aura.transform.position = shieldList[0].transform.position;
        if (parryShield is not null)
            parryShield.transform.position = shieldList[0].transform.position;
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
            endModal.ToggleEndModal(RootToEnd.PlayerIsDead);
            YGSaveData();
        }
    }

    public void ElementHit(string tag)
    {
        var newElement = TagToElementrsEnum(tag);
        elementsDict[newElement].count = Math.Clamp(elementsDict[newElement].count + 1, 0, YandexGame.savesData.elementsCopacity);
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
        var coordinatesHeight = r.Next(-4, 9);
        Instantiate(elementsList[element], new Vector3(coordinatesWidth, coordinatesHeight, 2), new Quaternion(0,0,0,0));
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
        if (elementsHand[0] == ElementsEnum.Земля && elementsHand[1] == null) { ShootProjectile(ElementsEnum.Земля, baseSpellSpeed, baseDamage*3); }
        else if (elementsHand[0] == ElementsEnum.Огонь && elementsHand[1] == null) {  ShootProjectile(ElementsEnum.Огонь, baseSpellSpeed, baseDamage, false, true); }
        else if (elementsHand[0] == ElementsEnum.Ветер && elementsHand[1] == null) { ShootProjectile(ElementsEnum.Ветер, baseSpellSpeed*2, baseDamage*2); }
        else if (elementsHand[0] == ElementsEnum.Огонь && elementsHand[1] == ElementsEnum.Ветер) { ShootProjectile(ElementsEnum.Огонь, baseSpellSpeed/2, baseDamage, true, true); }
        else if (elementsHand[0] == ElementsEnum.Ветер && elementsHand[1] == ElementsEnum.Огонь) { MakeParryState(); }
        else if (elementsHand[0] == ElementsEnum.Огонь && elementsHand[1] == ElementsEnum.Земля) { for(int i =0; i<5;i++) ShootProjectile(ElementsEnum.Земля, baseSpellSpeed*3, baseDamage);  }
        else if (elementsHand[0] == ElementsEnum.Земля && elementsHand[1] == ElementsEnum.Ветер) { MakeShield(); }
        else if (elementsHand[0] == ElementsEnum.Ветер && elementsHand[1] == ElementsEnum.Земля) { enemyDragon.TempIncressSpeedMultiplier(); }
        else if (elementsHand[0] == ElementsEnum.Земля && elementsHand[1] == ElementsEnum.Огонь) { MakeAura(); }

        elementsHand = new ElementsEnum?[elementsHand.Length];
        elementsHandGT.text = "";
    }

    public void GetYGSaves()
    {
        playerNameGT.text = YandexGame.playerName;
    }

    public void YGSaveData()
    {
        YandexGame.NewLeaderboardScores("TopPlayerScore", YandexGame.savesData.level);
        YandexGame.SaveProgress();
    }

    public void ShootProjectile(ElementsEnum type, float speed, float damage, bool autoAim = false, bool isDurational = false)
    {
        Debug.Log("Shooted");
        var shootedProjectile = Instantiate<GameObject>(projectilePrefab, shieldList[0].transform.position + new Vector3(0, 2, 0), shieldList[0].transform.rotation).GetComponent<Projectile>();
        shootedProjectile.speed = speed * YandexGame.savesData.speedScale;
        shootedProjectile.damage = damage * damageMultiplier * YandexGame.savesData.damageScale;
        shootedProjectile.type = type;
        shootedProjectile.autoAim = autoAim;
        shootedProjectile.isDurational = isDurational;
    }

    public void MakeShield()
    {
        isDamageBlocked = true;
        blockShield = Instantiate(blockShieldPrefab, shieldList[0].transform.position, shieldList[0].transform.rotation);
        Invoke("ToggleBlock", 5);
    }

    public void MakeAura()
    {
        damageMultiplier = 2;
        aura = Instantiate(auraPrefab, shieldList[0].transform.position, shieldList[0].transform.rotation);
        Invoke("RemoveAura", 3);
    }

    public void MakeParryState() 
    {
        isParryState = true;
        parryShield = Instantiate(parryShieldPrefab, shieldList[0].transform.position, shieldList[0].transform.rotation);
        Invoke("RemoveParryState", 1);
    }
}
