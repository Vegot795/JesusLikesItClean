using UnityEngine;
using UnityEngine.UI;

public class CursorScript : MonoBehaviour
{
    public RectTransform cursorUI;
    public Animator CursorAnimator;
    public float holdThreshold = 0.5f;

    private bool isHolding = false;
    private float holdTimer = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        cursorUI.position = Input.mousePosition;
    }
}
