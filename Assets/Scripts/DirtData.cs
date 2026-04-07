using UnityEngine;

[CreateAssetMenu(fileName = "DirtData", menuName = "Window/Dirt Type")]
public class DirtData : ScriptableObject
{
    public string dirtName;
    public int lvl;
    public float alpha;
    public GameObject dirtPrefab;
    public int durability;
}
