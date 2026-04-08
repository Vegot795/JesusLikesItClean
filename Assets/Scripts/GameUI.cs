using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public void LoadScene(int scena) // #0 menu startowe, #1 gra, #2 sklep
    {
        SceneManager.LoadScene(scena);
    }
}
