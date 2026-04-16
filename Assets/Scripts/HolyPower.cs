using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HolyPower : MonoBehaviour
{

    [SerializeField] GameObject holyBar;
    [SerializeField] GameObject holyPower;
    [SerializeField] GameObject bg;

    public int lvl = 1;
    public bool working = false;
    [SerializeField] float conversionPoints = 0.035f;
    [SerializeField] float conversionLosePower = 0.005f;

    public float holyPowerPoints = 50f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        holyPower.GetComponent<RectTransform>().sizeDelta= new Vector2(30,holyPowerPoints) ;
        holyBar.GetComponent<Image>().enabled = false;
        holyPower.SetActive(false);
        bg.SetActive(false);
    }

    private void FixedUpdate()
    {
        holyPower.GetComponent<RectTransform>().sizeDelta = new Vector2(30, holyPowerPoints);
        if (working)
        {
            holyPowerPoints -=lvl * conversionLosePower;
        }
        if (holyPowerPoints <= 0)
        {
            SceneManager.LoadScene(2);
            Debug.Log("przeglananananana");
        }
        else if (holyPowerPoints >= 765) // Zostaw tak ze wypierdala poza skale bo to takie smieszne 
        {
            Debug.Log("NIe zawiodles mnie Powstan Ponownie");
            working = false;
        }
    }

    public void ShowBar()
    {
        holyBar.GetComponent<Image>().enabled = true;
        holyPower.SetActive(true);
        bg.SetActive(true);
    }

    public void AddHolyPower(float points)
    {
        holyPowerPoints += points * (conversionPoints);
    }
}
