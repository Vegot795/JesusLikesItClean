using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance { get; private set; }

    [Header("Cutscenes")]
    public CutsceneSO introCutscene;
    public CutsceneSO midCutscene;
    public CutsceneSO outroCutscene;

    [Header("References")]
    public CutscenePlayer player;
    public GameObject gameplayRoot;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void PlayIntro() => Play(introCutscene);
    public void PlayMid() => Play(midCutscene);
    public void PlayOutro() => Play(outroCutscene);

    void Play(CutsceneSO cutscene)
    {
        gameplayRoot.SetActive(false);
        player.Play(cutscene);
    }

    public void EnableGameplay()
    {
        gameplayRoot.SetActive(true);
    }
}