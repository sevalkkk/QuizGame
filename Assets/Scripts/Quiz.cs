using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;
    [SerializeField] TextMeshProUGUI questionText;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    [SerializeField] private int correctAnswerIndex;
    bool hasAnsweredEarly;

    [Header("Button Colors")]
    [SerializeField] private Sprite correctAnswerSprite;
    [SerializeField] private Sprite defaultAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

   


    private void Start()
    {
        currentQuestion = questions[0];
        timer = FindObjectOfType<Timer>();
        
        //DisplayQuestion();
    }

    private void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
        if (timer.loadNextQuestion)
        {
            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;

        }
        else if (!hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            DisplayAnswer(-1);
            SetButtonState(false);
        }



    }

    private void DisplayAnswer(int index)
    {
        Image buttonImage;
        if (index == currentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "Correct!!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;

        }

        else
        {
            correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
            string correctAnswer = currentQuestion.GetAnswer(correctAnswerIndex);
            questionText.text = "Sorry ,the correct answer was :\n " + correctAnswer;
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
        SetButtonState(false);
    }
    public void OnAnswerSelected(int index)
    {
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.CancelTimer();


       
    }

  

    void GetNextQuestion()
    {
        if(questions.Count>0)
        {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestions();
            DisplayQuestion();
        }
       
    }

    private void GetRandomQuestions()
    {
        int randIndex = UnityEngine.Random.Range(1, questions.Count);
        currentQuestion = questions[randIndex];
        if(questions.Contains(currentQuestion))
        {
            questions.Remove(currentQuestion);
        }
        
    }

    private void SetDefaultButtonSprites()
    {
       for(int i=0;i<answerButtons.Length;i++)
        {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }

    void DisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();
        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.GetAnswer(i);
        }

    }
    void SetButtonState(bool state)
    {
        for(int i=0;i<answerButtons.Length;i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

}
