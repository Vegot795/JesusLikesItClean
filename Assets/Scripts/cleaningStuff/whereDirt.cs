using UnityEngine;

public class whereDirt : MonoBehaviour
{
    private GameObject magicEye;

    private GameObject glass1;
    private GameObject glass2;
    private GameObject glass3;
    private GameObject glass4;
    
    private bool isActive1 = false;
    private bool isActive2 = false;
    private bool isActive3 = false;
    private bool isActive4 = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        glass1 = GameObject.Find("Glass1");
        glass2 = GameObject.Find("Glass2");
        glass3 = GameObject.Find("Glass3");
        glass4 = GameObject.Find("Glass4");
        magicEye = GameObject.Find("whereDirt");
        magicEye.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (glass1.GetComponent<WindowScript>().isCleaned == false)
        {
            if (glass1.transform.childCount <= 50 && isActive1 == false)
            {
                isActive1 = true;
                magicEye.SetActive(true);
            }
            else if (glass1.transform.childCount > 50)
            {
                isActive1 = false;
                magicEye.SetActive(false);
            }
        }
        else if (glass2.GetComponent<WindowScript>().isCleaned == false)
        {
            if (glass2.transform.childCount <= 50 && isActive2 == false)
            {
                isActive2 = true;
                magicEye.SetActive(true);
            }
            else if (glass2.transform.childCount > 50)
            {
                isActive2 = false;
                magicEye.SetActive(false);
            }
        }
        else if (glass3.GetComponent<WindowScript>().isCleaned == false)
        {
            if (glass3.transform.childCount <= 50 && isActive3 == false)
            {
                isActive3 = true;
                magicEye.SetActive(true);
            }
            else if (glass3.transform.childCount > 50)
            {
                isActive3 = false;
                magicEye.SetActive(false);
            }
        }
        else if (glass4.GetComponent<WindowScript>().isCleaned == false)
        {
            if (glass4.transform.childCount <= 50 && isActive4 == false)
            {
                isActive4 = true;
                magicEye.SetActive(true);
            }
            else if (glass4.transform.childCount > 50)
            {
                isActive4 = false;
                magicEye.SetActive(false);
            }
        }
    }

    private void OnMouseDown()
    {
        if (isActive1)
        {
            foreach (Transform child in glass1.transform)
            {
                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                if (sr.color.a < .35f)
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
            }
        }
        else if (isActive2)
        {
            foreach (Transform child in glass2.transform)
            {
                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                if (sr.color.a < .35f)
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
            }
        }
        else if (isActive3)
        {
            foreach (Transform child in glass3.transform)
            {
                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                if (sr.color.a < .35f)
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
            }
        }
        else if (isActive4)
        {
            foreach (Transform child in glass4.transform)
            {
                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                if (sr.color.a < .35f)
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
            }
        }
    }

    private void OnMouseUp()
    {
        if (isActive1)
        {
            foreach (Transform child in glass1.transform)
            {
                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                if (sr.color.a == 1f)
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.1f);
            }
        }
        else if (isActive2)
        {
            foreach (Transform child in glass2.transform)
            {
                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                if (sr.color.a == 1f)
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.1f);
            }
        }
        else if (isActive3)
        {
            foreach (Transform child in glass3.transform)
            {
                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                if (sr.color.a == 1f)
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.1f);
            }
        }
        else if (isActive4)
        {
            foreach (Transform child in glass4.transform)
            {
                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                if (sr.color.a == 1f)
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.1f);
            }
        }
    }

}
