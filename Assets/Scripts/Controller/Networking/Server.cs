using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

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

    private void Update()
    {
        if (!isServerStarted)
            return;

        foreach (var c in _clients)
        {
            // Is the client still connected
            if(!IsConnected(c.tcp))
            {
                c.tcp.Close();
                _disconectedList.Add(c);
            }
            // Check for message from the client
            else
            {
                NetworkStream stream = c.tcp.GetStream();
                if(stream.DataAvailable)
                {
                    StreamReader reader = new StreamReader(stream, true);
                    string data = reader.ReadLine();

                    if(data != null)
                    {
                        OnIncommingData(c,data);
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

    private void OnIncommingData(ServerClient c, string data)
    {
        // datatypes from clients CWHO, BID-'WHATFIGUERE', CHECK, DEAL CARD
        var msg = data.Split('|');

        Debug.Log("Server:" + data);

        switch (msg[0])
        {
            case "CWHO":
                c.clientName = msg[1];
                c.isHost = (msg[2] == "0") ? false : true;
                Broadcast("SCNN|" + c.clientName, _clients);
                break;
            case "CBID":
                StringBuilder sbDataA = new StringBuilder(data);
                sbDataA[0] = 'S';
                Broadcast(sbDataA.ToString(), _clients);
                break;
            case "CCHK":
                StringBuilder sbDataB = new StringBuilder(data);
                sbDataB[0] = 'S';
                Broadcast(sbDataB.ToString(), _clients);
                break;
            case "CDCA": // Deal Card
                StringBuilder sbDataC = new StringBuilder(data);
                sbDataC[0] = 'S';
                Broadcast(sbDataC.ToString(), _clients);
                break;
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
            allUsers += i.clientName + '|';
        }

        ServerClient sc = new ServerClient(listener.EndAcceptTcpClient(ar));

        _clients.Add(sc);
        StartListening();

        // Send message to everyone, 
        Broadcast("SWHO|" + allUsers, _clients);

    }

    private void Broadcast(string data, List<ServerClient> clients)
    {
        foreach (var c in clients)
        {
            try
            {
                var writer = new StreamWriter(c.tcp.GetStream());
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
    public string clientName;
    public TcpClient tcp;
    public bool isHost;

    public ServerClient(TcpClient _tcp)
    {
        System.Random ran = new System.Random();
        clientName = "Guest" + ran.Next(1000, 90000);
        tcp = _tcp;
    }
}