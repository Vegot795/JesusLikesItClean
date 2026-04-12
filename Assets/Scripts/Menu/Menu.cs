using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject creditsView;
    [SerializeField] private GameObject optionsView;

    public void Awake()
    {
        ChangeTo(0); // ustawienie widoku menu
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ChangeTo(int opt) 
    {
        if (opt == 0) // widkou menu
        {
            mainMenu.SetActive(true);
            creditsView.SetActive(false);
            optionsView.SetActive(false);
        }
        else if (opt == 1) // widok credits
        {
            mainMenu.SetActive(false);
            creditsView.SetActive(true);
            optionsView.SetActive(false);
        }
        else if (opt == 2) // widok options
        {
            mainMenu.SetActive(false);
            creditsView.SetActive(false);
            optionsView.SetActive(true);
        }
    }

     public void QuitGame()
    {
        Application.Quit();
    }
}
