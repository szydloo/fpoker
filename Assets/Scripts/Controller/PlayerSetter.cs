using FunnyPoker.Models;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace FunnyPoker.Scripts.Controller
{
    public class PlayerSetter : NetworkBehaviour
    {
        public void SetPlayers(List<GameObject> players)
        {
            var _playersScripts = FindObjectsOfType<Player>();
            int i = 1;
            foreach(var p in _playersScripts)
            {
                var parentPos = GameObject.Find("PlayerPos" + i);
                var pObj = p.gameObject;
                players.Add(pObj);
                pObj.transform.SetParent(parentPos.transform);
                //p.PlayerId = i;
                pObj.transform.SetPositionAndRotation(new Vector3(parentPos.transform.position.x,parentPos.transform.position.y,-3), parentPos.transform.rotation);

                i++;
            }

        }
    }
}