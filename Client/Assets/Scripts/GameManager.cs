using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Colyseus;
using Game.Models;
using Game.Scripts.StateHandlers;
using UnityEngine.SceneManagement;

namespace Game.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public TextMeshProUGUI ServerText; 
        public GameObject PrefabUnit;
        public GameObject PrefabFlag;
        public Dictionary<string, GameObject> Units = new Dictionary<string, GameObject>();
        public Dictionary<string, GameObject> Flags = new Dictionary<string, GameObject>();
        public Dictionary<string, Player> Players = new Dictionary<string, Player>();
        public Room<State> GameRoom;
        private Client _client;
        public async void Start()
        {
            Debug.Log("Game Manager Start");

            var serverip = GetArg("-serverip");
            serverip = serverip != null ? serverip : "localhost";
            Debug.Log(serverip);

            var serverport = GetArg("-serverport");
            serverport = serverport != null ? serverport : "3000";
            Debug.Log(serverport);

            var roomname = GetArg("-roomname");
            roomname = roomname != null ? roomname : "match";
            Debug.Log(roomname);

            var token = GetArg("-token");
            Debug.Log(token);

            _client = new Client("ws://" + serverip + ":" + serverport);

            try {
                var options = new Dictionary<string, object>();
                options.Add("token", token);
                options.Add("map", SceneManager.GetActiveScene().name);
                GameRoom = await _client.JoinOrCreate<State>("match", options);
            } catch {
                ServerText.text = "Connection failed";
                return;
            }

            
            ServerText.text = serverip + ":" + serverport + " room: " + roomname;

            InitStateHandler();
        }

        private void InitStateHandler() {
            var stateHandlerPlayers = new StateHandlerPlayers(GameRoom.State.statePlayers, this);
            var stateHandlerUnits = new StateHandlerUnits(GameRoom.State.stateUnits, this);
            var stateHandlerFlags = new StateHandlerFlags(GameRoom.State.stateFlags, this);
        }

        private async void OnApplicationQuit()
        {
            Debug.Log("Leave");
            await GameRoom.Leave();
        }

        private static string GetArg(string name)
        {
            var args = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == name && args.Length > i + 1)
                {
                    return args[i + 1];
                }
            }
            return null;
        }
    }
}