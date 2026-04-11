using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class shopScript : MonoBehaviour
{

    private int clothEffLvl;
    private int clothSizeLvl;
    private int sprinkleEffLvl;
    private int sprinkleSizeLvl;
    public GameObject shopSprinkle;
    public GameObject shopCloth;

    [SerializeField] private ShopData[] shopUPG;

    private TextMeshProUGUI costClothEF;
    private TextMeshProUGUI costClotchSI;
    private TextMeshProUGUI costSprinkleEF;
    private TextMeshProUGUI costSprinkleSI;
    private TextMeshProUGUI lvlClothEF;
    private TextMeshProUGUI lvlClotchSI;
    private TextMeshProUGUI lvlSprinkleEF;
    private TextMeshProUGUI lvlSprinkleSI;



    private void Start()
    {
        playerEQ pEqLvl = GameObject.Find("SceneControl").GetComponent<playerEQ>();

        clothEffLvl = pEqLvl.clothLvlEf;
        clothSizeLvl = pEqLvl.clothLvlSi;
        sprinkleEffLvl = pEqLvl.sprinkleLvlEf;
        sprinkleSizeLvl = pEqLvl.sprinkleLvlSi;
        shopCloth = GameObject.Find("cloth");
        shopSprinkle = GameObject.Find("sprinkle");

        lvlClothEF = GameObject.Find("Text1").GetComponent<TextMeshProUGUI>();
        costClothEF = GameObject.Find("Text2").GetComponent<TextMeshProUGUI>();
        lvlClotchSI = GameObject.Find("Text3").GetComponent<TextMeshProUGUI>();
        costClotchSI = GameObject.Find("Text4").GetComponent<TextMeshProUGUI>();
        lvlSprinkleEF = GameObject.Find("Text5").GetComponent<TextMeshProUGUI>();
        costSprinkleEF = GameObject.Find("Text6").GetComponent<TextMeshProUGUI>();
        lvlSprinkleSI = GameObject.Find("Text7").GetComponent<TextMeshProUGUI>();
        costSprinkleSI = GameObject.Find("Text8").GetComponent<TextMeshProUGUI>();

        lvlClothEF.text = "Level: " + (clothEffLvl+1);
        costClothEF.text = "Cost: " + (shopUPG[0].upgradeCost[clothEffLvl]);
        lvlClotchSI.text = "Level: " + (clothSizeLvl+1);
        costClotchSI.text = "Cost: " + (shopUPG[1].upgradeCost[clothSizeLvl]);
        lvlSprinkleEF.text = "Level: " + (sprinkleEffLvl+1);
        costSprinkleEF.text = "Cost: " + (shopUPG[2].upgradeCost[sprinkleEffLvl]);
        lvlSprinkleSI.text = "Level: " + (sprinkleSizeLvl + 1);
        costSprinkleSI.text = "Cost: " + (shopUPG[3].upgradeCost[sprinkleSizeLvl]);


    }

    private void OnMouseUp()
    {
        if (gameObject.name == "clothEffi")
        {
            BuyUpgrade(0);
        }
        else if (gameObject.name == "clothSize")
        {
            BuyUpgrade(1);
        }
        else if (gameObject.name == "sprinkleEffi")
        {
            BuyUpgrade(2);
        }
        else if (gameObject.name == "sprinkleSize")
        {
            BuyUpgrade(3);
        }
    }

    public void BuyUpgrade(int opcja)
    {
        playerEQ pEqLvl = GameObject.Find("SceneControl").GetComponent<playerEQ>();

        if (opcja == 0)
        {
            if (clothEffLvl + 1 < shopUPG[opcja].upgradeValue.Count && pEqLvl.points >= shopUPG[opcja].upgradeCost[(int)(clothEffLvl + 1)])
            {
                pEqLvl.points -= shopUPG[opcja].upgradeCost[pEqLvl.clothLvlEf + 1];
                pEqLvl.clothLvlEf++;
                pEqLvl.clotchEfficience = (int)shopUPG[opcja].upgradeValue[pEqLvl.clothLvlEf];
                clothEffLvl = pEqLvl.clothLvlEf;
                lvlClothEF.text = "Level: " + (clothEffLvl + 1);
                costClothEF.text = "Cost: " + (shopUPG[0].upgradeCost[clothEffLvl]);
                UpdateClothSpriteWithLvl(clothEffLvl);
            }
        }
        else if (opcja == 1)
        {
            if (clothSizeLvl + 1 < shopUPG[opcja].upgradeValue.Count && pEqLvl.points >= shopUPG[opcja].upgradeCost[(int)(clothSizeLvl + 1)] )
            {
                pEqLvl.points -= shopUPG[opcja].upgradeCost[pEqLvl.clothLvlSi + 1];
                pEqLvl.clothLvlSi++;
                pEqLvl.clotchSize = (float)shopUPG[opcja].upgradeValue[pEqLvl.clothLvlSi];
                clothSizeLvl = pEqLvl.clothLvlSi;
                lvlClotchSI.text = "Level: " + (clothSizeLvl + 1);
                costClotchSI.text = "Cost: " + (shopUPG[1].upgradeCost[clothSizeLvl]);
            }
        }
        else if (opcja == 2)
        {
            if (sprinkleEffLvl + 1 < shopUPG[opcja].upgradeValue.Count && pEqLvl.points >= shopUPG[opcja].upgradeCost[(int)(sprinkleEffLvl + 1)] )
            {
                pEqLvl.points -= shopUPG[opcja].upgradeCost[pEqLvl.sprinkleLvlEf + 1];
                pEqLvl.sprinkleLvlEf++;
                pEqLvl.sprinkleEfficience = (float)shopUPG[opcja].upgradeValue[pEqLvl.sprinkleLvlEf];
                sprinkleEffLvl = pEqLvl.sprinkleLvlEf;
                lvlSprinkleEF.text = "Level: " + (sprinkleEffLvl + 1);
                costSprinkleEF.text = "Cost: " + (shopUPG[2].upgradeCost[sprinkleEffLvl]);
                UpdateSprinkleSpriteWithLvl(sprinkleEffLvl);
            }
        }
        else if (opcja == 3)
        {
            if ( sprinkleSizeLvl + 1 < shopUPG[opcja].upgradeValue.Count && pEqLvl.points >= shopUPG[opcja].upgradeCost[(int)(sprinkleSizeLvl + 1)] )
            {
                pEqLvl.points -= shopUPG[opcja].upgradeCost[pEqLvl.sprinkleLvlSi + 1];
                pEqLvl.sprinkleLvlSi++;
                pEqLvl.sprinkleSize = (float)shopUPG[opcja].upgradeValue[pEqLvl.sprinkleLvlSi];
                sprinkleSizeLvl = pEqLvl.sprinkleLvlSi;
                lvlSprinkleSI.text = "Level: " + (sprinkleSizeLvl + 1);
                costSprinkleSI.text = "Cost: " + (shopUPG[3].upgradeCost[sprinkleSizeLvl]);
            }
        }
    }

    private void UpdateClothSpriteWithLvl(int lvl)
    {
        playerEQ pEqLvl = GameObject.Find("SceneControl").GetComponent<playerEQ>();
        Image clothSprite = shopCloth.GetComponent<Image>();
        clothSprite.sprite = pEqLvl.clothSprites[lvl];
    }

    private void UpdateSprinkleSpriteWithLvl(int lvl)
    {
        playerEQ pEqLvl = GameObject.Find("SceneControl").GetComponent<playerEQ>();
        Image sprinkleSprite = shopSprinkle.GetComponent<Image>();
        sprinkleSprite.sprite = pEqLvl.sprinkleSprites[lvl];
    }
}
