using UnityEngine;

public class sprinkleWater : MonoBehaviour
{
    public float efficience = .15f;
    [SerializeField] private float timeToLive = .7f;


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
