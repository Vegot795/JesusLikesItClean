using UnityEngine;

[CreateAssetMenu(fileName = "CutsceneSO", menuName = "Cutscene/Cutscene")]
public class CutsceneSO : ScriptableObject
{
    public CutsceneSlideData[] slides;

    public enum OnFinish {  EnableGameplay, LoadNextScene, FireEventOnly }
    public OnFinish onFinish;

    [Tooltip("Only used if onFinish = LoadNextScene")]
    public string nextSceneName;
}
