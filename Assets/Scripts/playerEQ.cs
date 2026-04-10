using UnityEngine;

public class playerEQ : MonoBehaviour
{
    [Header("Cash")]
    public float points = 0;

    [Header("Equipment")]
    [Header("Cloth")]
    public int clothLvlEf;
    public int clotchEfficience;
    public int clothLvlSi;
    public float clotchSize;
    [Header("Sprinkle")]
    public int sprinkleLvlEf;
    public float sprinkleEfficience;
    public int sprinkleLvlSi;
    public float sprinkleSize;

    [SerializeField] private GameObject sprinkWater;

    public static playerEQ instance;

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
    }

    private void Start()
    {
        GameObject.Find("cloth").GetComponent<clothScript>().efficience = clotchEfficience;
        GameObject.Find("cloth").GetComponent<RectTransform>().localScale = new Vector3(1*clotchSize,1*clotchSize,1*clotchSize);
        sprinkWater.GetComponent<sprinkleWater>().efficience = sprinkleEfficience;
        sprinkWater.GetComponent<Transform>().localScale = new Vector3(1 * sprinkleSize, .8f * sprinkleSize, 1 * sprinkleSize);
    }
}
