using UnityEngine;

public class whereDirt : MonoBehaviour
{
    private GameObject magicEye;
    private GameObject glass;
    private bool isActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        glass = GameObject.Find("Glass");
        magicEye = GameObject.Find("whereDirt");
        magicEye.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (glass.transform.childCount <= 50 && isActive == false)
        {
            isActive = true;
            magicEye.SetActive(true);
        }
        else if (glass.transform.childCount > 50 )
        {
            isActive = false;
            magicEye.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        if (isActive)
        {
            foreach (Transform child in glass.transform)
            {
                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                if (sr.color.a < .35f)
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
            }
        }
    }

    private void OnMouseUp()
    {
        if (isActive)
        {
            foreach (Transform child in glass.transform)
            {
                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                if (sr.color.a == 1f)
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.1f);
            }
        }
    }

}
