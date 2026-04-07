using UnityEngine;

public class DirtBird : MonoBehaviour
{

    [SerializeField] DirtData dirtType;

    [SerializeField] private float dur;
    private float alpha;
    private bool cleared = false;
    private bool watered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Collider2D col = GetComponent<Collider2D>();
        dur = dirtType.durability;
        alpha = dirtType.alpha;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name==("cloth") && !cleared && watered)
        {
            dur -= collision.gameObject.GetComponent<clothScript>().efficience;
            if (dur <= 0)
            {
                cleared = true;
                dur = 0;
            }
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            Color c = sr.color;
            c.a = (float)(dur / dirtType.durability);
            sr.color = c;
        }

        if (collision.gameObject.name == ("Water(Clone)") && !cleared && !watered)
        {
            dur -= dirtType.durability * collision.gameObject.GetComponent<sprinkleWater>().efficience;
            if (dur <= 0)
            {
                cleared = true;
                dur = 0;
            }
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            Color c = sr.color;
            c.a = c.a * (float)(dur / dirtType.durability);
            sr.color = c;
            watered = true;
        }
    }
}
