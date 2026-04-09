using UnityEngine;

public class playerEQ : MonoBehaviour
{
    [Header("Cash")]
    public float points = 0;

    [Header("Equipment")]
    public int clotchEfficience;
    public float clotchSize;
    public float sprinkleEfficience;
    public float sprinkleSize;
    [SerializeField] private GameObject sprinkWater;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        GameObject.Find("cloth").GetComponent<clothScript>().efficience = clotchEfficience;
        GameObject.Find("cloth").GetComponent<RectTransform>().localScale = new Vector3(1*clotchSize,1*clotchSize,1*clotchSize);
        sprinkWater.GetComponent<sprinkleWater>().efficience = sprinkleEfficience;
        sprinkWater.GetComponent<Transform>().localScale = new Vector3(1 * sprinkleSize, .8f * sprinkleSize, 1 * sprinkleSize);
    }
}
