using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FunnyPoker.src.Controller
{
    public class LevelManager : MonoBehaviour
    {
        public void LoadLevel(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void QuitRequest()
        {
            Application.Quit();
            Debug.Log("Quit request");
        }
    }
}
