using UnityEngine;

[CreateAssetMenu(fileName = "CutsceneSlideData", menuName = "ScriptableObjects/CutsceneSlideData", order = 1)]
public class CutsceneSlideData : ScriptableObject
{
    public Sprite image;
    [TextArea] public string dialogueText;
    public float displayDuration = 10f;
}
