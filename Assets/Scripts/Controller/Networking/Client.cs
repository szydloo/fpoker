using FunnyPoker.Scripts.Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using FunnyPoker.Models;
using FunnyPoker.Extensions;

namespace FunnyPoker.Scripts.Networking
{
    // Not rly SOLID my god what have i done 
    public class Client : MonoBehaviour
    {
        public string ClientName;
        public bool IsHost;
        public int Id;
        public GameObject player; // ??

        private bool socketReady;
        private TcpClient socket;
        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;
        private LevelManager LevelManager;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            LevelManager = FindObjectOfType<LevelManager>();
        }

        private void Update()
        {
            if (socketReady)
            {
                if (stream.DataAvailable)
                {
                    string data = reader.ReadLine();
                    if (data != null)
                    {
                        OnIncomingMessage(data);
                    }
                }
            }
        }
        
        public void ConnectToServer(string host, int port)
        {
            if (socketReady)
                return;

            try
            {
                socket = new TcpClient(host, port);
                stream = socket.GetStream();
                writer = new StreamWriter(stream);
                reader = new StreamReader(stream);

                socketReady = true;
            }
            catch (Exception e)
            {
                Debug.Log("Connect to server error: " + e.Message);
            }
        }

        public void OnIncomingMessage(string data)
        {
            var msg = data.Split('|');

            Debug.Log("Client : " + data);

            switch (msg[0])
            {
                // Initialization 
                case "SWHO": // On server asking who connected send clients name and whether he's a host 
                    {
                        Send("CWHO|" + ClientName + '|' + ((IsHost) ? 1 : 0));
                        break;
                    }
                case "SGID":
                    {
                        this.Id = int.Parse(msg[1]);
                        Send("CGID"); // Send confirmation that u got id
                        break;
                    }
                case "SSGM": // On server message to start game load game
                    {
                        LevelManager.LoadLevel("02_Game");
                        break;
                    }
                case "SGIN": // Synchronization of init Game variables accross players
                    { // SGIN|NumOfCardsToLose|NumOfPlayers|NumOfStartingCards
                        Game.Instance.NumOfCardsToLose = int.Parse(msg[1]);
                        Game.Instance.NumOfPlayers = int.Parse(msg[2]);
                        Game.Instance.NumOfStartingCards = int.Parse(msg[3]);
                        Game.Instance.SetPlayers();
                        break;
                    }
                case "SENDTURNME": // End of turn signalization
                    { // SENDTURNME|ID|
                        int id = int.Parse(msg[1]);
                        if (id != this.Id) break;
                        int numOfCards = int.Parse(msg[2]);
                        int k = 0;

                        for (int i = 0; i < numOfCards; i++)
                        {
                            string cardsInString = msg[3];
                            string card = cardsInString.Substring(k, 2);
                            player.GetComponent<Player>().CurrentCards.Add(card.ToCard());
                            k += 2;
                        }
                        Game.Instance.AddCardPicforPlayer(this.Id, numOfCards);

                        break;
                    }
                case "SENDTURNOTHER":
                    {
                        
                        int numOfCards = int.Parse(msg[1]);
                        int senderClId = int.Parse(msg[2]);
                        if (senderClId == this.Id) break;
                        Game.Instance.AddCardPicforPlayer(senderClId, numOfCards);
                        break;
                    }
            }
        }

        public void Send(string data)
        {
            if (!socketReady)
                return;

            writer.WriteLine(data);
            writer.Flush();
        }

        public void CloseSocket()
        {
            if (!socketReady)
            {
                return;
            }

            stream.Dispose();
            reader.Dispose();
            writer.Dispose();
            socket.Close();

            socketReady = false;
        }

        private void OnApplicationQuit()
        {
            CloseSocket();
        }

        private void OnDisable()
        {
            CloseSocket();
        }

    }
}