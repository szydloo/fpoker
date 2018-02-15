using FunnyPoker.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FunnyPoker.src.Controller
{
    public class PlayersUIManager : MonoBehaviour
    {
        private List<Text> txtPlayerNames;

        // Use this for initialization
        void Start()
        {
            txtPlayerNames = new List<Text>();

            //var playersNameObj = GameObject.Find("PlayersNames");
            //foreach(GameObject child in playersNameObj.transform )
            //{
            //    var go = child.GetComponentInChildren<Text>();
            //    go.gameObject.SetActive(true);
            //    txtPlayerNames.Add(go);
            //}
        }

        public void UpdatePlayersUI(List<GameObject> players)
        {
            int i = 0;
            foreach(var p in players)
            {
                txtPlayerNames[i].text = p.GetComponent<Player>().Name;
            }
        }
        

        // 
        public void DeactivateIndicatorForPlayer(List<GameObject> players, int playerNumInList)
        {
            var player = players[playerNumInList];
            if (player.GetComponent<Player>() == null)
            {
                throw new System.Exception("Invalid object to set indicator for.");
            }
            player.transform.Find("PlayerPointer").gameObject.SetActive(false);
        }

        public void ActivateIndicatorForPlayer(List<GameObject> players, int playerNumInList)
        {
            var player = players[playerNumInList];

            if (player.GetComponent<Player>() == null)
            {
                throw new System.Exception("Invalid object to set indicator for.");
            }
            player.transform.Find("PlayerPointer").gameObject.SetActive(true);
        }
    }
}