using UnityEngine;

public class sprinkleWater : MonoBehaviour
{
    public float efficience = .15f;
    [SerializeField] private float timeToLive = .7f;
    private float size = 1f;
    private Vector3 originalScale;

    private void Awake()
    {
        efficience = GameObject.Find("SceneControl").GetComponent<playerEQ>().sprinkleEfficience;
        size = GameObject.Find("SceneControl").GetComponent<playerEQ>().sprinkleSize;
        originalScale = transform.localScale;
        
        transform.localScale = new Vector3(originalScale.x * size, originalScale.y * size, originalScale.z * size);
    }
    // Update is called once per frame
    void Update()
    {
       timeToLive -= Time.deltaTime;
        if (timeToLive <= 0)
        {
            Destroy(gameObject);
        }
    }
}
