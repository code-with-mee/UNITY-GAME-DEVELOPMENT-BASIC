using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject titlePanel;
    public GameObject categoryPanel;
    public GameObject playPanel;
    public GameObject pauseDialog;
    public GameObject scoreDialog;

    [Header("Fade Settings")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 0.5f;

    private GameObject currentPanel;

    void Start()
    {
        ShowScreen(titlePanel, true);
    }

    // === Public Panel Controls ===

    public void ShowTitle()
    {
        AudioManager.Instance.PlayClick();
        ShowScreen(titlePanel);
    }

    public void ShowCategory()
    {
        AudioManager.Instance.PlayClick();
        ShowScreen(categoryPanel);
    }

    public void ShowPlay()
    {
        AudioManager.Instance.PlayClick();
        ShowScreen(playPanel);
    }

    public void ShowPause()
    {
        AudioManager.Instance.PlayClick();
        ShowDialog(pauseDialog);
    }

    public void ShowScore()
    {
        AudioManager.Instance.PlayClick();
        ShowDialog(scoreDialog);
    }

    public void ShowDialog(GameObject dialog)
    {
        dialog.SetActive(true);
        currentPanel = dialog;
    }

    public void ShowScreen(GameObject targetPanel, bool instant = false)
    {
        if (targetPanel == currentPanel) return;
        StartCoroutine(TransitionTo(targetPanel, instant));
    }

    public void GoBack(GameObject current)
    {
        AudioManager.Instance.PlayClick();

        if (current == playPanel)
        {
            ShowCategory();
        }
        else if (current == categoryPanel)
        {
            ShowTitle();
        }
        else if (current == pauseDialog)
        {
            ClosePauseDialogToCategory();
        }
        else if (current == scoreDialog)
        {
            CloseScoreDialogToCategory();
        }
        else
        {
            ShowTitle();
        }
    }

    // === Wrapper Methods for Dialog Back/Close (UI Button compatible) ===

    public void ClosePauseDialogToPlay()
    {
        AudioManager.Instance.PlayClick();
        StartCoroutine(CloseDialogAndGoTo(pauseDialog, playPanel));
    }

    public void ClosePauseDialogToCategory()
    {
        AudioManager.Instance.PlayClick();
        StartCoroutine(CloseDialogAndGoTo(pauseDialog, categoryPanel));
    }

    public void CloseScoreDialogToPlay()
    {
        AudioManager.Instance.PlayClick();
        StartCoroutine(CloseDialogAndGoTo(scoreDialog, playPanel));
    }

    public void CloseScoreDialogToCategory()
    {
        AudioManager.Instance.PlayClick();
        StartCoroutine(CloseDialogAndGoTo(scoreDialog, categoryPanel));
    }

    // === Transition Handling ===

    private IEnumerator CloseDialogAndGoTo(GameObject dialog, GameObject toScreen)
    {
        yield return StartCoroutine(Fade(1f));

        // Hide all
        titlePanel.SetActive(false);
        categoryPanel.SetActive(false);
        playPanel.SetActive(false);
        pauseDialog.SetActive(false);
        scoreDialog.SetActive(false);

        dialog.SetActive(false);
        toScreen.SetActive(true);
        currentPanel = toScreen;

        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator TransitionTo(GameObject targetPanel, bool instant = false)
    {
        if (!instant)
            yield return StartCoroutine(Fade(1f));

        // Hide all
        titlePanel.SetActive(false);
        categoryPanel.SetActive(false);
        playPanel.SetActive(false);
        pauseDialog.SetActive(false);
        scoreDialog.SetActive(false);

        currentPanel = targetPanel;
        currentPanel.SetActive(true);

        if (!instant)
            yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        fadeCanvasGroup.blocksRaycasts = true;

        float startAlpha = fadeCanvasGroup.alpha;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
        fadeCanvasGroup.blocksRaycasts = false;
    }
}
