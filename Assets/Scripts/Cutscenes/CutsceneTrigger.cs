using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CutsceneTrigger : MonoBehaviour
{
    [Tooltip("Leave empty to use the mid cutscene from CutsceneManager")]
    public CutsceneSO overrideCutscene;

    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        if (overrideCutscene != null)
            CutsceneManager.Instance.player.Play(overrideCutscene);
        else
            CutsceneManager.Instance.PlayMid();

        gameObject.SetActive(false); // prevent re-trigger
    }
}