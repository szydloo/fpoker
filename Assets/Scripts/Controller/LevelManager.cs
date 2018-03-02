using FunnyPoker.Scripts.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace FunnyPoker.Scripts.Controller
{
    public class LevelManager : MonoBehaviour
    {
        public void LoadLevel(string sceneName)
        {
            if(EventSystem.current?.currentSelectedGameObject?.name == "btnMainMenu")
            {
                var c = FindObjectOfType<Client>();
                var s = FindObjectOfType<Server>();

                if(c != null)
                {
                    Destroy(c);
                }
                if(s != null)
                {
                    Destroy(s);
                }

            }
            SceneManager.LoadScene(sceneName);
        }

        public void QuitRequest()
        {
            Application.Quit();
            Debug.Log("Quit request");
        }
    }
}
