using UnityEngine;

public class DirtSmog : MonoBehaviour
{

    [SerializeField] DirtData dirtType;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] private float dur;
    private float maxDur;
    private float alpha;

    //private bool cleared = false;
    private bool watered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int lvl = GameObject.Find("Glass").GetComponent<WindowScript>().WindowLvl;
        dur = dirtType.durability * lvl;
        maxDur = dur;
    }

    public void AddPoints()
    {
        GameObject.Find("SceneControl").GetComponent<playerEQ>().points += dirtType.points;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == ("cloth"))
        {
            dur -= collision.gameObject.GetComponent<clothScript>().efficience;
            if (dur <= 0)
            {
                AddPoints();
                Destroy(gameObject);
            }
            alpha = (float)(dur / maxDur);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a*alpha);
        }

        if (collision.gameObject.name == ("Water(Clone)") && !watered)
        {
            dur -= dirtType.durability * collision.gameObject.GetComponent<sprinkleWater>().efficience;
            if (dur <= 0)
            {
                AddPoints();
                Destroy(gameObject);
            }
            alpha = (float)(dur / maxDur);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a * alpha);
            watered = true;
        }
    }
}
