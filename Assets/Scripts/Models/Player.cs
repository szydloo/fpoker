using FunnyPoker.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunnyPoker.Models
{

    public class Player : MonoBehaviour
    {
        public Guid UserId { get; private set; }
        public string Username { get; private set; }
        public string Name { get; private set; }
        public List<Card> CurrentCards { get; set; }
        public int NumOfCards { get; set; }
        public PlayerStatus Status;

        public void Awake()
        {
            CurrentCards = new List<Card>();
        }
        public void Start()
        {
            NumOfCards = 1; // TODO Set val from PlayerPrefsManager
            Status = PlayerStatus.Playing;
            UserId = Guid.NewGuid();
        }

        public void SetUsername(string username)
        {
            if (username.IsNullOrWhiteSpace())
            {
                Username = username;
            }
        }

        public void SetName(string name)
        {
            if (name.IsNullOrWhiteSpace())
            {
                Name = name;
            }
        }

    }

    public enum PlayerStatus
    {
        Playing,
        Defeated
    }
}