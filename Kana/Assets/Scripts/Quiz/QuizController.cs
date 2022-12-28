using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VD.Quiz
{
    public class QuizController : MonoBehaviour
    {

        // In case the stars align and the loop becomes infinite
        private const int MaxLoopCount = 100;
        
        public KanaLibrary kanaLibrary;
        [Tooltip("How many questions to wait before the same question can be asked")]
        public int recencyBuffer;
        [Tooltip("How many times the player must answer correctly in a row before the question is removed")]
        public int correctCount;

        [Header("UI")]
        public TextMeshProUGUI symbolDisplay;
        public TextMeshProUGUI correctAnswer;
        public TMP_InputField inputField;
        public Button submitButton;
        
        [Header("Prompts")]
        public GameObject correctPrompt;
        public GameObject wrongPrompt;

        private int _currentKana;
        private int[] _points;
        private Queue<int> _recentKana;
        private List<int> _learnedKana;
        
        private void Start()
        {
            _points = new int[kanaLibrary.Count];
            
            _recentKana = new Queue<int>();
            _learnedKana = new List<int>();
            
            LoadNextKana();
        }

        private void Update()
        {
            if(Input.GetButtonDown("Submit"))
                SubmitAnswer();
        }

        private void LoadNextKana()
        {
            int rand = UnityEngine.Random.Range(0, kanaLibrary.Count - 1);

            int i = 0;
            while (_recentKana.Contains(rand) || _learnedKana.Contains(rand) || i >= MaxLoopCount)
            {
                rand = UnityEngine.Random.Range(0, kanaLibrary.Count - 1);
                i++;
            }

            _recentKana.Enqueue(rand);

            if (_recentKana.Count > recencyBuffer)
                _recentKana.Dequeue();

            _currentKana = rand;
            ShowKana(kanaLibrary[rand].symbol);
        }

        private void ShowKana(in string symbol)
        {
            symbolDisplay.text = symbol;
            inputField.text = "";
            correctAnswer.text = "";

            correctPrompt.SetActive(false);
            wrongPrompt.SetActive(false);
            
            submitButton.interactable = true;
            inputField.interactable = true;
            inputField.Select();
        }

        public void SubmitAnswer()
        {
            if(string.IsNullOrEmpty(inputField.text)) return;
            
            submitButton.interactable = false;
            inputField.interactable = false;

            bool correct = string.Equals(inputField.text.Trim(), kanaLibrary[_currentKana].answer, StringComparison.CurrentCultureIgnoreCase);
            
            correctPrompt.SetActive(correct);
            wrongPrompt.SetActive(!correct);
            correctAnswer.text = kanaLibrary[_currentKana].answer;

            _points[_currentKana] = correct ? (_points[_currentKana] + 1) : 0;
            if(_points[_currentKana] >= correctCount)
                _learnedKana.Add(_currentKana);

            StartCoroutine(WaitAfterAnswer());
        }

        private IEnumerator WaitAfterAnswer()
        {
            yield return new WaitForSeconds(1);
            LoadNextKana();
        }

    }
}
