using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HolyPower : MonoBehaviour
{

    [SerializeField] GameObject holyBar;
    [SerializeField] GameObject holyPower;

    public int lvl = 1;
    public bool working = false;
    [SerializeField] float conversionPoints = 0.035f;
    [SerializeField] float conversionLosePower = 0.005f;

    [SerializeField] private float holyPowerPoints = 50f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        holyPower.GetComponent<RectTransform>().sizeDelta= new Vector2(30,holyPowerPoints) ;
        holyBar.GetComponent<Image>().enabled = false;
        holyPower.SetActive(false);
    }

    private void FixedUpdate()
    {
        holyPower.GetComponent<RectTransform>().sizeDelta = new Vector2(30, holyPowerPoints);
        if (working)
        {
            holyPowerPoints -= lvl * conversionLosePower;
        }
        if (holyPowerPoints <= 0)
        {
            SceneManager.LoadScene(2);
            Debug.Log("przeglananananana");
        }
        else if (holyPowerPoints >= 765)
        {
            Debug.Log("NIe zawiodles mnie Powstan Ponownie");
        }
    }

    public void ShowBar()
    {
        holyBar.GetComponent<Image>().enabled = true;
        holyPower.SetActive(true);
    }

    public void AddHolyPower(float points)
    {
        holyPowerPoints += points * conversionPoints;
    }
}
