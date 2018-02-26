using FunnyPoker.Scripts.Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;


namespace FunnyPoker.Scripts.Networking
{
    public class Client : MonoBehaviour
    {
        public string ClientName;
        public bool IsHost;
        public LevelManager LevelManager;

        private int id;
        private bool socketReady;
        private TcpClient socket;
        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;

        private List<GameClient> players = new List<GameClient>();

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            LevelManager = FindObjectOfType<LevelManager>();
        }

        private void Update()
        {
            Debug.Log(players.Count);

            if(socketReady)
            {
                if(stream.DataAvailable)
                {
                    string data = reader.ReadLine();
                    if(data != null)
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

            Debug.Log("Client:" + data);


            switch (msg[0])
            {
                case "SWHO":
                    for(int i = 1; i < msg.Length; i++)
                    {
                        UserConnected(msg[i], false);
                    }
                break;
                case "SBID":
                    Game.Instance.OnCalledBid();
                 break;
                case "SCHK":
                    Game.Instance.OnCalledCheck();
                    break;

                    // case "SDCA": // SDCA|Host|
                    // GameManager.Instance.GotCard(playerName, card);
                    // break;
            }
        }

        private void UserConnected(string name, bool v2)
        {
            GameClient c = new GameClient();
            c.name = name;
            id = players.Count + 1;

            players.Add(c);
            if(players.Count == PlayerPrefsManager.GetNumberOfPlayers())
            {
                LevelManager.LoadLevel("02_Game");
            }

        }

        public void Send(string data)
        {
            if (!socketReady)
                return;

            writer.WriteLine();
            writer.Flush();
        }

        public void CloseSocket()
        {
            if(!socketReady)
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

    public class GameClient
    {
        public string name;
        public bool isHost;
    }
}