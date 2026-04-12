using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CutscenePlayer : MonoBehaviour
{
    [Header("UI References")]
    public Image slideImage;
    public TextMeshProUGUI dialogueText;
    public CanvasGroup fadeOverlay;

    [Header("Settings")]
    public float fadeDuration = 0.5f;

    public event Action OnCutsceneFinished;

    private CutsceneSO currentCutscene;
    private int currentIndex;
    private bool isTransitioning;
    private Coroutine autoAdvanceCoroutine;

    public void Play(CutsceneSO cutscene)
    {
        currentCutscene = cutscene;
        currentIndex = 0;
        gameObject.SetActive(true);
        ShowSlide(0);
    }

    void Update()
    {
        if (isTransitioning) return;
        if (Input.anyKeyDown) AdvanceSlide();
    }

    void ShowSlide(int index)
    {
        if (index >= currentCutscene.slides.Length) { EndCutscene(); return; }

        var slide = currentCutscene.slides[index];
        slideImage.sprite = slide.image;
        dialogueText.text = slide.dialogueText;

        if (autoAdvanceCoroutine != null) StopCoroutine(autoAdvanceCoroutine);
        autoAdvanceCoroutine = StartCoroutine(AutoAdvance(slide.displayDuration));
    }

    IEnumerator AutoAdvance(float delay)
    {
        yield return new WaitForSeconds(delay);
        AdvanceSlide();
    }

    void AdvanceSlide()
    {
        if (isTransitioning) return;
        currentIndex++;
        if (currentIndex >= currentCutscene.slides.Length) { EndCutscene(); return; }
        StartCoroutine(TransitionToSlide(currentIndex));
    }

    IEnumerator TransitionToSlide(int index)
    {
        isTransitioning = true;
        yield return StartCoroutine(Fade(0f, 1f));
        ShowSlide(index);
        yield return StartCoroutine(Fade(1f, 0f));
        isTransitioning = false;
    }

    IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        fadeOverlay.alpha = from;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeOverlay.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }
        fadeOverlay.alpha = to;
    }

    void EndCutscene()
    {
        if (autoAdvanceCoroutine != null) StopCoroutine(autoAdvanceCoroutine);
        StartCoroutine(FinishCutscene());
    }

    IEnumerator FinishCutscene()
    {
        yield return StartCoroutine(Fade(0f, 1f));
        gameObject.SetActive(false);
        OnCutsceneFinished?.Invoke();

        switch (currentCutscene.onFinish)
        {
            case CutsceneSO.OnFinish.LoadNextScene:
                SceneManager.LoadScene(currentCutscene.nextSceneName);
                break;
            case CutsceneSO.OnFinish.EnableGameplay:
                CutsceneManager.Instance.EnableGameplay();
                yield return StartCoroutine(Fade(1f, 0f));
                break;
                // FireEventOnly: subscribers on OnCutsceneFinished handle it
        }
    }
}