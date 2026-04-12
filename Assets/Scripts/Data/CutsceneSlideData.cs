using UnityEngine;

[CreateAssetMenu(fileName = "CutsceneSlideData", menuName = "ScriptableObjects/CutsceneSlideData", order = 1)]
public class CutsceneSlideData : ScriptableObject
{
    public Sprite image;
    [TextArea] public string dialogueText;
    public Vector2 TextAnchoredPosition;
    public Vector2 TextAnchoredSize;
    public float displayDuration = 5f;
}
