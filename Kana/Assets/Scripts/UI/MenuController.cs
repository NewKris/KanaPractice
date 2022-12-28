using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VD.UI
{
    public class MenuController : MonoBehaviour
    {

        public void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
        
        public void LoadHiraganaScene()
        {
            SceneManager.LoadScene("Hiragana");
        }

        public void LoadKatakanaScene()
        {
            //SceneManager.LoadScene("Katakana");
        }
        
        public void ExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        
    }
}
