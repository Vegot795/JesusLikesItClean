using UnityEngine;

public class whereDirt : MonoBehaviour
{
    private GameObject magicEye;
    private GameUI gameUI;

    void Start()
    {
        gameUI = GameObject.Find("UI").GetComponent<GameUI>();
        magicEye = GameObject.Find("whereDirt");
        magicEye.SetActive(false);
    }

    void Update()
    {
        if (gameUI.currentWindow.GetComponent<WindowScript>().isCleaned == false)
        {
            if (gameUI.currentWindow.clearingProgress >= 0.9f)
            {
                magicEye.SetActive(true);
            }
            else if (gameUI.currentWindow.clearingProgress < 0.9f)
            {
                magicEye.SetActive(false);
            }
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
