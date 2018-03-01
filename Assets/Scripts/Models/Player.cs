using FunnyPoker.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunnyPoker.Models
{

    public class Player : MonoBehaviour
    {
        public List<Card> CurrentCards { get; set; }
        public int NumOfCards { get; set; }
        public int Id { get; set; }

        public PlayerStatus Status;

        public void Awake()
        {
            CurrentCards = new List<Card>();
        }
        public void Start()
        {
            NumOfCards = 1; // TODO Set val from PlayerPrefsManager
            Status = PlayerStatus.Playing;
        }
    }

    public enum PlayerStatus
    {
        Playing,
        Defeated
    }
}