using FunnyPoker.Scripts.Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace FunnyPoker.Scripts.Networking
{
    public class Server : MonoBehaviour
    {
        public int Port = 5500;

        private List<ServerClient> _clients;
        private List<ServerClient> _disconectedList;

        private TcpListener server;
        private bool isServerStarted;


        public void Init()
        {
            DontDestroyOnLoad(gameObject);

            _clients = new List<ServerClient>();
            _disconectedList = new List<ServerClient>();

            try
            {
                server = new TcpListener(IPAddress.Any, Port);
                server.Start();
                StartListening();
                isServerStarted = true;
            }
            catch (Exception e)
            {
                Debug.Log("Server error. " + e.Message);
            }
        }

        private void OnDestroy()
        {
            GameNetworkManager.Instance.ResetNetworkVariables();
            server.Stop();
        }

        private void OnApplicationQuit()
        {
            server.Stop();
        }

        private void Update()
        {
            if (!isServerStarted)
                return;

            foreach (var c in _clients)
            {
                // Is the client still connected
                if (!IsConnected(c.Tcp))
                {
                    c.Tcp.Close();
                    _disconectedList.Add(c);
                }
                // Check for message from the client
                else
                {
                    NetworkStream stream = c.Tcp.GetStream();
                    if (stream.DataAvailable)
                    {
                        StreamReader reader = new StreamReader(stream, true);
                        string data = reader.ReadLine();

                        if (data != null)
                        {
                            OnIncommingData(c, data);
                        }
                    }
                }
            }

            for (int i = 0; i < _disconectedList.Count; i++)
            {
                _clients.Remove(_disconectedList[i]);
                _disconectedList.RemoveAt(i);
            }
        }

        private bool IsConnected(TcpClient c)
        {
            try
            {
                if (c != null && c.Client != null && c.Client.Connected)
                {
                    if (c.Client.Poll(0, SelectMode.SelectRead))
                    {
                        return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                    }
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        private void StartListening()
        {
            server.BeginAcceptTcpClient(AcceptTcpClient, server);
        }

        private void AcceptTcpClient(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;

            string allUsers = "";
            foreach (ServerClient i in _clients)
            {
                allUsers += i.ClientName + '|';
            }

            ServerClient sc = new ServerClient(listener.EndAcceptTcpClient(ar));

            _clients.Add(sc);
            StartListening();

            Broadcast("SWHO|" + allUsers, _clients[_clients.Count - 1]);

        }

        // Server read
        private void OnIncommingData(ServerClient c, string data)
        {
            var msg = data.Split('|');

            Debug.Log("Server : " + data);

            switch (msg[0])
            {
                case "CWHO":
                    { 
                        c.ClientName = msg[1];
                        c.IsHost = (msg[2] == "0") ? false : true;
                        c.Id = GameNetworkManager.Instance.ClientId;
                        Broadcast("SGID|" + (GameNetworkManager.Instance.ClientId).ToString(), _clients.Where(x => x.Id == GameNetworkManager.Instance.ClientId).ToList());
                        GameNetworkManager.Instance.ClientId++;

                        break;
                    }
                case "CGID": // Client's msg that he got id and is ready to 
                    {
                        if (_clients.Count == PlayerPrefsManager.GetNumberOfPlayers()) 
                        {
                            Broadcast("SSGM", _clients);
                        }
                        break;
                    }
                case "CSGM": // sync game conditions
                    {
                        foreach (var i in _clients)
                        {
                            Debug.Log(i.Id + " " + i.ClientName);
                        }
                        Broadcast("SGIN|" + PlayerPrefsManager.GetLoseCondition() + "|" + PlayerPrefsManager.GetNumberOfPlayers()
                            + "|" + PlayerPrefsManager.GetNumberOfStartingCards(), _clients);

                        break;
                    }
                case "CENDTURN": // On end turn deal cards
                    { // CENTRUEN|ClientsNumCards|ClientsId
                        var clientsNumCards = int.Parse(msg[1]);
                        var clientsId = int.Parse(msg[2]);
                        StringBuilder strBuild = new StringBuilder();
                        for (int i = 0; i < clientsNumCards; i++)
                        {
                            var card = GameNetworkManager.deck.DrawRandomCard();
                            strBuild.Append(card.ToString());
                        }

                        // TODO Think of better ideas for msg names
                        Broadcast("SENDTURNME|"+ clientsId + "|" + clientsNumCards + "|" + strBuild.ToString(), _clients.Where(x => x.Id == clientsId).ToList()); // Sending actual cards to client
                        break;
                    }
                case "CENDTURNME":
                    {
                        var clientsNumCards = int.Parse(msg[1]);
                        var clientsId = int.Parse(msg[2]);

                        Broadcast("SENDTURNOTHER|" + clientsNumCards + "|" + clientsId, _clients.Where(x => x.Id != clientsId).ToList()); // Sending just num of cards, the client has, to others
                        break;
                    }
            }
        }

        // Server write
        private void Broadcast(string data, ServerClient c)
        {
            List<ServerClient> sc = new List<ServerClient> { c };
            Broadcast(data, sc);
        }
        // Server write
        private void Broadcast(string data, List<ServerClient> clients)
        {
            foreach (var c in clients)
            {
                try
                {
                    var writer = new StreamWriter(c.Tcp.GetStream());
                    writer.WriteLine(data);
                    writer.Flush();
                }
                catch (Exception e)
                {
                    Debug.Log("Writing error on server side: " + e.Message);
                }
            }
        }
    }

    public class ServerClient
    {
        public string ClientName { get; set; }
        public TcpClient Tcp { get; set; }
        public bool IsHost { get; set; }
        public int Id { get; set; }

        public ServerClient(TcpClient _tcp)
        {
            Tcp = _tcp;
        }
    }
}