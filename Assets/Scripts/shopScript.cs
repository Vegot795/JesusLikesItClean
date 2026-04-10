using System;
using UnityEngine;

public class shopScript : MonoBehaviour
{

    private int clothEffLvl;
    private int clothSizeLvl;
    private int sprinkleEffLvl;
    private int sprinkleSizeLvl;

    [SerializeField] private ShopData[] shopUPG;



    private void Start()
    {
        playerEQ pEqLvl = GameObject.Find("SceneControl").GetComponent<playerEQ>();
        clothEffLvl = pEqLvl.clothLvlEf;
        clothSizeLvl = pEqLvl.clothLvlSi;
        sprinkleEffLvl = pEqLvl.sprinkleLvlEf;
        sprinkleSizeLvl = pEqLvl.sprinkleLvlSi;
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

    private void BuyUpgrade(int opcja)
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
            }
        }
        else if (opcja == 2)
        {
            if (sprinkleEffLvl + 1 < shopUPG[opcja].upgradeValue.Count && pEqLvl.points >= shopUPG[opcja].upgradeCost[(int)(sprinkleEffLvl + 1)]  )
            {
                pEqLvl.points -= shopUPG[opcja].upgradeCost[pEqLvl.sprinkleLvlEf + 1];
                pEqLvl.sprinkleLvlEf++;
                pEqLvl.sprinkleEfficience = (float)shopUPG[opcja].upgradeValue[pEqLvl.sprinkleLvlEf];
                sprinkleEffLvl = pEqLvl.sprinkleLvlEf;
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
            }
        }
    }
}
