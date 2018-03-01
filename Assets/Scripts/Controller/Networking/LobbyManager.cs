using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FunnyPoker.Scripts.Networking
{

    public class LobbyManager : MonoBehaviour
    {
        public GameObject MainMenu;
        public GameObject ConnectMenu;
        public GameObject HostMenu;
        public GameObject Waiting;

        public GameObject ServerPrefab;
        public GameObject ClientPrefab;

        public InputField NameInputHost;
        public InputField NameInputCon;
        public InputField AddressInput;
        public InputField PortInput;
        public Button btnHostStart;

        public void ConnectMenuButton()
        {
            MainMenu.SetActive(false);
            HostMenu.SetActive(false);
            ConnectMenu.SetActive(true);
        }

        public void HostMenuButton()
        {
            MainMenu.SetActive(false);
            HostMenu.SetActive(true);
            ConnectMenu.SetActive(false);
        }

        public void CancelButton()
        {
            MainMenu.SetActive(true);
            HostMenu.SetActive(false);
            ConnectMenu.SetActive(false);
        }

        public void HostButton()
        {
            try
            {
                Server s = Instantiate(ServerPrefab).GetComponent<Server>();
                s.Init();

                Client c = Instantiate(ClientPrefab).GetComponent<Client>();
                c.IsHost = true;
                c.ClientName = NameInputHost.text;
                if(c.ClientName == "")
                {
                    c.ClientName = "Host";
                }

                c.ConnectToServer("127.0.0.1", 5500); // default

                HostMenu.SetActive(false);
                Waiting.SetActive(true);
            }
            catch (Exception e)
            {
                Debug.Log("Error in hosting game : " + e.Message); 
            }
        }

        public void ActivateHostStartBtn()
        {
            btnHostStart.onClick.AddListener(HostStartButton);

            ColorBlock cb = btnHostStart.colors;
            cb.normalColor = Color.green;
            btnHostStart.colors = cb;

        }

        public void HostStartButton()
        {
            
        }

        public void ConnectToServerButton()
        {
            string hostAddress = AddressInput.text;
            string portAddress = PortInput.text;

            if (hostAddress == "")
                hostAddress = "127.0.0.1";

            if (portAddress == "")
                portAddress = "5500";

            try
            {
                Client c = Instantiate(ClientPrefab).GetComponent<Client>();

                c.ClientName = NameInputHost.text;
                c.IsHost = false;
                if (c.ClientName == "")
                {
                    c.ClientName = "Guest";
                }

                c.ConnectToServer(hostAddress, Int32.Parse(portAddress));
                ConnectMenu.SetActive(false);
                Waiting.SetActive(true);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        public void CancelInWaitingButton()
        {
            var c = FindObjectOfType<Client>();
            if (c != null)
            {
                Destroy(c.gameObject);
            }

            var s = FindObjectOfType<Server>();
            if(s != null)
            {
                Destroy(s.gameObject);
            }
            MainMenu.SetActive(true);
            ConnectMenu.SetActive(false);
            HostMenu.SetActive(false);
            Waiting.SetActive(false);
            btnHostStart.gameObject.SetActive(c.IsHost ? true : false);

        }


    }
}