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

        public static event Action<bool> OnAnswered;
        
        public KanaLibrary kanaLibrary;

        [Header("UI")]
        public TextMeshProUGUI symbolDisplay;
        public TextMeshProUGUI correctAnswer;
        public TMP_InputField inputField;
        public Button submitButton;
        public EventSystem eventSystem;
        
        [Header("Prompts")]
        public GameObject correctPrompt;
        public GameObject wrongPrompt;

        private Queue<int> _recentKana;
        private Kana _currentKana;
        
        private void Start()
        {
            _recentKana = new Queue<int>();
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

            if (_recentKana.Count != 0)
            {
                while (_recentKana.Contains(rand))
                    rand = UnityEngine.Random.Range(0, kanaLibrary.Count - 1);   
                
                _recentKana.Dequeue();
            }

            _recentKana.Enqueue(rand);

            _currentKana = kanaLibrary[rand];
            ShowKana(_currentKana.symbol);
        }

        private void ShowKana(in string symbol)
        {
            symbolDisplay.text = symbol;
            inputField.text = "";
            correctAnswer.text = "";

            correctPrompt.SetActive(false);
            wrongPrompt.SetActive(false);
            
            //eventSystem.SetSelectedGameObject(inputField.gameObject);
            submitButton.interactable = true;
            inputField.interactable = true;
            inputField.Select();
        }

        public void SubmitAnswer()
        {
            if(string.IsNullOrEmpty(inputField.text)) return;
            
            submitButton.interactable = false;
            inputField.interactable = false;

            bool correct = string.Equals(inputField.text.Trim(), _currentKana.answer, StringComparison.CurrentCultureIgnoreCase);
            
            correctPrompt.SetActive(correct);
            wrongPrompt.SetActive(!correct);
            correctAnswer.text = _currentKana.answer;
            
            OnAnswered?.Invoke(correct);
            StartCoroutine(WaitAfterAnswer());
        }

        private IEnumerator WaitAfterAnswer()
        {
            yield return new WaitForSeconds(1);
            LoadNextKana();
        }

    }
}
