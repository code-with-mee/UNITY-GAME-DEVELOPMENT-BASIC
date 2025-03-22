using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    [Header("UI")]
    public Text questionText;
    public Button[] answerButtons; // Size = 3
    public Image timerImage;
    public Text timerText;

    [Header("Score Dialog")]
    public GameObject scoreDialog;
    public Text scoreText;

    [Header("Dependencies")]
    public ScreenManager screenManager;

    private int correctAnswer;
    private int score = 0;
    private Coroutine timerCoroutine;

    void Start()
    {
        GenerateQuestion();
    }

    public void GenerateQuestion()
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        int a = 0, b = 0, result = 0;
        string op = "";
        bool valid = false;

        while (!valid)
        {
            int opType = Random.Range(0, 4); // 0:+, 1:-, 2:*, 3:/

            switch (opType)
            {
                case 0: // Addition
                    a = Random.Range(5, 16);
                    b = Random.Range(1, 16);
                    result = a + b;
                    op = "+";
                    break;
                case 1: // Subtraction
                    a = Random.Range(10, 21);
                    b = Random.Range(1, a);
                    result = a - b;
                    op = "-";
                    break;
                case 2: // Multiplication
                    a = Random.Range(2, 7);
                    b = Random.Range(2, 7);
                    result = a * b;
                    op = "×";
                    break;
                case 3: // Division
                    b = Random.Range(2, 6);
                    result = Random.Range(1, 6);
                    a = b * result;
                    op = "÷";
                    break;
            }

            if (result > 0 && result < 20)
                valid = true;
        }

        questionText.text = $"{a} {op} {b} = ?";
        correctAnswer = result;

        int correctIndex = Random.Range(0, 3);
        HashSet<int> usedAnswers = new HashSet<int> { result };

        for (int i = 0; i < 3; i++)
        {
            int answer;

            if (i == correctIndex)
            {
                answer = result;
            }
            else
            {
                do
                {
                    answer = Random.Range(result - 5, result + 6);
                } while (usedAnswers.Contains(answer) || answer < 0 || answer >= 20);

                usedAnswers.Add(answer);
            }

            answerButtons[i].GetComponentInChildren<Text>().text = answer.ToString();

            int captured = answer;
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayClick(); // 🔊 Button click
                CheckAnswer(captured);
            });
        }

        timerCoroutine = StartCoroutine(StartQuestionTimer(10f));
    }

    public void CheckAnswer(int selected)
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        AudioManager.Instance.StopTicking(); // Stop tick if running

        if (selected == correctAnswer)
        {
            AudioManager.Instance.PlayCorrect(); // 🔊 Correct
            score++;
            GenerateQuestion();
        }
        else
        {
            AudioManager.Instance.PlayWrong(); // 🔊 Wrong
            ShowScoreDialog();
        }
    }

    private void ShowScoreDialog()
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        timerImage.fillAmount = 0f;
        timerText.text = "0";

        scoreText.text = $"Your Score: {score}";
        score = 0;

        screenManager.ShowScore();
    }

    private IEnumerator StartQuestionTimer(float duration)
    {
        float timeLeft = duration;
        timerImage.fillAmount = 1f;
        timerText.text = Mathf.CeilToInt(timeLeft).ToString();

        AudioManager.Instance.StopTicking();
        bool tickingStarted = false;

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerImage.fillAmount = timeLeft / duration;
            timerText.text = Mathf.CeilToInt(timeLeft).ToString();

            if (!tickingStarted && timeLeft <= 5f)
            {
                tickingStarted = true;
                AudioManager.Instance.StartTicking(); // 🔊 Ticking countdown
            }

            yield return null;
        }

        timerImage.fillAmount = 0f;
        timerText.text = "0";

        AudioManager.Instance.StopTicking();
        AudioManager.Instance.PlayWrong(); // 🔊 Time’s up = wrong
        ShowScoreDialog();
    }
}
