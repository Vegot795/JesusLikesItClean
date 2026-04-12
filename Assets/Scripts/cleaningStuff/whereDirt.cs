using UnityEngine;

public class whereDirt : MonoBehaviour
{
    private GameObject magicEye;
    private GameUI gameUI;

    public WindowScript[] windows;

    public SpriteRenderer spriteRenderer;

    void Start()
    {
        gameUI = GameObject.Find("UI").GetComponent<GameUI>();
        magicEye = GameObject.Find("whereDirt");
        spriteRenderer.enabled = false;
    }

    void Update()
    {
        int lvl = GameObject.Find("Game").GetComponent<GameLoad>().licznik;
        //Debug.Log(windows[lvl].clearingProgress);
        if (windows[lvl].clearingProgress >= 0.9f)
        {
            spriteRenderer.enabled = true;
        }
        else if (windows[lvl].clearingProgress < 0.9f)
        {
            spriteRenderer.enabled = false;
        }

    }

    private void OnMouseDown()
    { 
        foreach (GameObject cell in gameUI.currentWindow.stainedCells)
        {
            SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
            if (sr.color.a < .35f)
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        }
    }

    private void OnMouseUp()
    {
        foreach (GameObject cell in gameUI.currentWindow.stainedCells)
        {
            SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
            if (sr.color.a == 1f)
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.1f);
        }
    }
}
