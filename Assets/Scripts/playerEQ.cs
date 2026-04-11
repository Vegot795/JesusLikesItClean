using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;

public class playerEQ : MonoBehaviour
{
    [Header("Cash")]
    public float points = 0;
    [Header("Equipment")]
    [Header("Cloth")]
    public GameObject cloth;
    public int clothLvlEf;
    public int clotchEfficience;
    public int clothLvlSi;
    public float clotchSize = 5;
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
        GameObject.Find("cloth").GetComponent<clothScript>().efficience = clotchEfficience;
        GameObject.Find("cloth").GetComponent<Transform>().localScale = new Vector3(5+clotchSize,5+clotchSize,5+clotchSize);
        sprinkWater.GetComponent<sprinkleWater>().efficience = sprinkleEfficience;
        sprinkWater.GetComponent<Transform>().localScale = new Vector3(1 * sprinkleSize, .8f * sprinkleSize, 1 * sprinkleSize);
        cloth = GameObject.Find("cloth");
        sprinkle = GameObject.Find("sprinkle");
    }
}
