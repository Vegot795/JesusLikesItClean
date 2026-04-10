using UnityEngine;

public class DirtSmog : MonoBehaviour
{
    [SerializeField] DirtData dirtType;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] private float dur;
    private float maxDur;
    private float alpha;
    private int lvl;
    private WindowScript windowScript;
    private bool watered = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        windowScript = GetComponentInParent<WindowScript>();
        lvl = windowScript.WindowLvl;
        dur = dirtType.durability * lvl;
        maxDur = dur;

        if (sr == null)
            sr = GetComponent<SpriteRenderer>();
    }

    public void AddPoints()
    {
        GameObject sc = GameObject.Find("SceneControl");
        if (sc == null) return;
        playerEQ eq = sc.GetComponent<playerEQ>();
        if (eq == null) return;
        if (dirtType == null) return;
        eq.points += dirtType.points;
        GameObject.Find("HolyPower").GetComponent<HolyPower>().AddHolyPower(dirtType.points);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (windowScript == null)
            windowScript = GetComponentInParent<WindowScript>();

        if (collision.gameObject.name == "cloth")
        {
            clothScript cloth = collision.gameObject.GetComponent<clothScript>();
            if (cloth == null) return;
            dur -= cloth.efficience;
            if (dur <= 0)
            {
                AddPoints();
                if (windowScript != null && windowScript.stainedCells != null)
                    windowScript.stainedCells.Remove(gameObject);
                Destroy(gameObject);
                return;
            }
            if (maxDur > 0)
                alpha = (dur / maxDur) + 0.5f;
            else
                alpha = 1f;
            if (sr != null)
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a * alpha);
        }

        if (collision.gameObject.name == "Water(Clone)" && !watered)
        {
            sprinkleWater sprinkle = collision.gameObject.GetComponent<sprinkleWater>();
            if (sprinkle == null) return;
            dur -= (dirtType != null ? dirtType.durability * sprinkle.efficience : 0f);
            if (dur <= 0)
            {
                AddPoints();
                if (windowScript != null && windowScript.stainedCells != null)
                    windowScript.stainedCells.Remove(gameObject);
                Destroy(gameObject);
                return;
            }
            if (maxDur > 0)
                alpha = (dur / maxDur);
            else
                alpha = 1f;
            if (sr != null)
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a * alpha);
            watered = true;
        }
    }
}
