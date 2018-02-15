using System;
using System.Collections;
using System.Collections.Generic;
using FunnyPoker.Models;
using UnityEngine;
using UnityEngine.UI;

namespace FunnyPoker.src.Controller
{
    public class PlayersCreator : MonoBehaviour
    {
        private PlayersUIManager _playersUIManager;
        private List<GameObject> _players;
        private CardDealer _cardDealer;
        private GameObject _panel;
        private GameObject _playerPrefab;
        private Button _btnEnterInput;
        private InputField _inputUsername;
        private InputField _inputName;
        private Text _txtInputTitle;
        private string _username;
        private string _name;
        private readonly string NAME_OF_PLAYER_POS_PARENT = "PlayerPos";
        private readonly string PANEL_NAME = "UIPlayerInputPanel";
        private int _nameCount = 1;
        private int _numOfPlayers;
        private bool _arePlayersInit;

        private void Start()
        {
            _numOfPlayers = PlayerPrefsManager.GetNumberOfPlayers();
            _cardDealer = FindObjectOfType<CardDealer>();
            _playersUIManager = FindObjectOfType<PlayersUIManager>();

            _playerPrefab = Resources.Load("Prefabs\\Player") as GameObject;
        }

        private void Update()
        {

            if(_arePlayersInit)
            {
                ClosePanel();
                _cardDealer.DealFirstCard(_players);
                // Activate indicator for first player
                _playersUIManager.ActivateIndicatorForPlayer(_players, 0);
                Destroy(gameObject);
            }
        }

        private void InitialisePlayers()
        {
            if (_numOfPlayers > _players.Count)
            {
                GetDataFromInputFields();
                ClearInputFields();

                var parent = GameObject.Find(NAME_OF_PLAYER_POS_PARENT + _nameCount).transform;
                var player = Instantiate(_playerPrefab, parent).GetComponent<Player>();

                player.SetName(_name);
                player.SetUsername(_username);

                _players.Add(player.gameObject);

                if(_numOfPlayers <= _players.Count)
                {
                    _arePlayersInit = true;
                }
                _nameCount++;
                _txtInputTitle.text = "Player " + _nameCount;
            }
        }

        private void SetPlayersName()
        {
            int i = 1;
            foreach(var pObj in _players)
            {
                pObj.name = "Player " + i;
                i++;
            }
        }

        private void ClearInputFields()
        {
            _inputName.text = "";
            _inputUsername.text = "";
        }

        private void ClosePanel()
        {
            if (_panel != null)
            {
                Destroy(_panel);
            }
        }

        private void GetDataFromInputFields()
        {
            _name = _inputName.text;
            _username = _inputUsername.text;
        }

        private void InitializeInputField()
        {
            _btnEnterInput = GameObject.Find("btnEnterInput").GetComponent<Button>();
            _inputUsername = GameObject.Find("inputUsername").GetComponent<InputField>();
            _inputName = GameObject.Find("inputName").GetComponent<InputField>();
            _txtInputTitle = GameObject.Find("txtInputTitle").GetComponent<Text>();
            _txtInputTitle.text = "Player " + _nameCount;

            _btnEnterInput.onClick.AddListener(InitialisePlayers);
        }

        public void CreatePlayers(List<GameObject> players)
        {
            _players = players;
            // Create input panel for username and name
            var c = FindObjectOfType<Canvas>();

            _panel = (GameObject)Instantiate(Resources.Load("Prefabs\\PlayersNameInput"));
            _panel.name = PANEL_NAME;
            _panel.transform.SetParent(c.transform, false);
            InitializeInputField();
           
        }
    }
}