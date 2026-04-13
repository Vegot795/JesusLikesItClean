using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;

public class playerEQ : MonoBehaviour
{
    [Header("Cash")]
    public float points = 0;

    [Header("Equipment")]
    [Tooltip("0 - lvl 1, 1 - lvl 2, 2 - lvl 3, 3 - lvl 4, 4 - lvl 5")]
    public int startLvl = 0;
    
    [Header("Cloth")]
    public GameObject cloth;
    public int clothLvlEf;
    public int clothEfficience;
    public int clothLvlSi;
    public float clothSize = 5;
    
    [Header("Sprinkle")]
    public GameObject sprinkle;
    public int sprinkleLvlEf;
    public float sprinkleEfficience;
    public int sprinkleLvlSi;
    public float sprinkleSize;
    [SerializeField] private GameObject sprinkWater;

    [Header("Level First Try")]
    public bool firstTry = true;

    public float time = 0;

    public static playerEQ instance;
    public List<Sprite> clothSprites = new List<Sprite>();
    public List<Sprite> sprinkleSprites = new List<Sprite>();

    [SerializeField] public ShopData[] shopUPG;
    //0 - clothEffi
    //1 - clothSize
    //2 - sprinkleEffi
    //3 - sprinkleSize

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Debug.Log($"New run started with cloth lvl: {clothLvlEf} and sprinkle lvl {sprinkleLvlEf}");
    }

    private void Start()
    {
        SetObjectsStartLvl(startLvl);

        cloth = GameObject.Find("cloth");
        sprinkle = GameObject.Find("sprinkle");
        cloth.GetComponent<clothScript>().efficience = clothEfficience;
        cloth.GetComponent<clothScript>().size = clothSize;
        sprinkWater.GetComponent<sprinkleWater>().efficience = sprinkleEfficience;
        sprinkWater.GetComponent<Transform>().localScale = new Vector3(1 * sprinkleSize, .8f * sprinkleSize, 1 * sprinkleSize);
    }

    private void SetObjectsStartLvl(int startLvl)
    {
        clothLvlEf = startLvl;
        clothEfficience = (int)shopUPG[0].upgradeValue[clothLvlEf];
        clothLvlSi = startLvl;
        clothSize = (float)shopUPG[1].upgradeValue[clothLvlSi];
        sprinkleLvlEf = startLvl;
        sprinkleEfficience = (float)shopUPG[2].upgradeValue[sprinkleLvlEf];
        sprinkleLvlSi = startLvl;
        sprinkleSize = (float)shopUPG[3].upgradeValue[sprinkleLvlSi];
    }
}
