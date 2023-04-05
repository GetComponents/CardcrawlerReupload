using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MenueHandler : MonoBehaviour
    {
        [SerializeField]
        private string m_gameScene = "Overworld";
        [SerializeField]
        private GameObject info_page;
        [SerializeField]
        private GameObject option_Page;
        [SerializeField]
        private GameObject credit_Page;

        private void Start()
        {
            info_page.SetActive(true);
            option_Page.SetActive(false);
            credit_Page.SetActive(false);
        }

        public void Play_ButtonClick()
        {
            SceneManager.LoadScene(m_gameScene);
        }

        public void Options_ButtonClick()
        {
            Debug.Log("Options Click");
            option_Page.SetActive(true);
            info_page.SetActive(false);
            credit_Page.SetActive(false);
        }

        public void Credits_ButtonClick()
        {
            Debug.Log("Credits Click");
            credit_Page.SetActive(true);
            info_page.SetActive(false);
            option_Page.SetActive(false);
        }

        public void Exit_ButtonClick()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// Go Back to the Mainmenu
        /// </summary>
        public void Leave_ButtonClick()
        {
            option_Page.SetActive(false);
            credit_Page.SetActive(false);
        }
    }
}