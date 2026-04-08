using UnityEngine;

[CreateAssetMenu(fileName = "DirtData", menuName = "Window/Dirt Type")]
public class DirtData : ScriptableObject
{
    public string dirtName;
    public float minAlpha;
    public float maxAlpha;
    public GameObject dirtPrefab;
    public int durability;
    public float points;
}
