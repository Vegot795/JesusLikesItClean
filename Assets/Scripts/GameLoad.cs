using System.Collections;
using UnityEngine;

public class GameLoad : MonoBehaviour
{

    private GameObject glass1;
    private GameObject glass2;
    private GameObject glass3;
    private GameObject glass4;
    public bool loaded = false;
    private int licznik = 0;

    private playerEQ plEQ;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        glass1 = GameObject.Find("Glass1");
        glass2 = GameObject.Find("Glass2");
        glass3 = GameObject.Find("Glass3");
        glass4 = GameObject.Find("Glass4");

        plEQ = GameObject.Find("SceneControl").GetComponent<playerEQ>();
    }

    // Update is called once per frame
    void Update()
    {
        if (glass4.transform.childCount == 2 && glass4.GetComponent<WindowScript>().isCleaned == true && licznik == 3)
        {
            glass4.SetActive(false);
            licznik++;
        }
        else if (glass1.transform.childCount == 1 && glass1.GetComponent<WindowScript>().isCleaned == true && licznik == 0)
        {
            glass2.SetActive(true);
            StartCoroutine(NextWindow(0, 3f));
            Debug.Log("Glass 1 cleaned");
            licznik++;
        }
        else if (glass2.transform.childCount == 1 && glass2.GetComponent<WindowScript>().isCleaned == true && licznik == 1)
        {
            glass3.SetActive(true); 
            StartCoroutine(NextWindow(1, 3f));
            Debug.Log("Glass 2 cleaned");
            licznik++;
        }
        else if (glass3.transform.childCount == 2 && glass3.GetComponent<WindowScript>().isCleaned == true && licznik == 2)
        {
            glass4.SetActive(true); 
            StartCoroutine(NextWindow(2, 3f));
            Debug.Log("Glass 3 cleaned");
            licznik++;
        }
    }

    private void FixedUpdate()
    {

        if (!loaded && glass1.transform.childCount > 50 && glass2.transform.childCount > 50 && glass3.transform.childCount > 50 && glass4.transform.childCount > 50)
        {
            if (plEQ.firstTry == false)
            {
                Debug.Log("Hide windows");
                glass1.SetActive(true);
                glass2.SetActive(false);
                glass3.SetActive(false);
                glass4.SetActive(false);
                loaded = true;
                GameObject.Find("Loading").SetActive(false);
            }
            else
            {
                Debug.Log("Hide windows");
                glass1.SetActive(false);
                glass2.SetActive(false);
                glass3.SetActive(false);
                glass4.SetActive(true);
                loaded = true;
                GameObject.Find("Loading").SetActive(false);
            }
        }
    }

    private IEnumerator NextWindow(int option, float delay)
    {
        if (option == 0)
        {
            GameObject.Find("UI").GetComponent<GameUI>().MoveToNextLvl();
            yield return new WaitForSeconds(delay);
            glass1.SetActive(false);
            GameObject.Find("HolyPower").GetComponent<HolyPower>().lvl = 2;
        }
        else if (option == 1)
        {
            GameObject.Find("UI").GetComponent<GameUI>().MoveToNextLvl();
            yield return new WaitForSeconds(delay);
            glass2.SetActive(false);
            GameObject.Find("HolyPower").GetComponent<HolyPower>().lvl = 3;
        }
        else if (option == 2)
        {
            GameObject.Find("UI").GetComponent<GameUI>().MoveToNextLvl();
            yield return new WaitForSeconds(delay);
            glass3.SetActive(false);
            GameObject.Find("HolyPower").GetComponent<HolyPower>().lvl = 4;
        }
        else if (option == 3)
        {
            GameObject.Find("UI").GetComponent<GameUI>().MoveToNextLvl();
            yield return new WaitForSeconds(delay);
            glass4.SetActive(false);
        }
    }
}
