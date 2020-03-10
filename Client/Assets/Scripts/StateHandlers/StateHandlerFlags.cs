using Game.Models;
using UnityEngine;

namespace Game.Scripts.StateHandlers
{
    public class StateHandlerFlags
    {
        private StateFlags _stateFlags;
        private GameManager _gameManager;

        public StateHandlerFlags(StateFlags stateFlags, GameManager gameManager)
        {
            _stateFlags = stateFlags;
            _gameManager = gameManager;
            _stateFlags.flags.OnAdd += OnAdd;
            //_statePlayers.players.OnRemove += OnRemove;
            //_statePlayers.players.OnChange += OnChange;
        }

        private void OnAdd(Flag flag, string key)
        {
            GameObject gameObjectFlag = Object.Instantiate(
                _gameManager.PrefabFlag,
                new Vector3(flag.position.x, flag.position.y, flag.position.z),
                new Quaternion());
            _gameManager.Flags.Add(key, gameObjectFlag);
            Debug.Log("Flag Add");

            flag.OnChange += (changes) =>
            {
                changes.ForEach((obj) =>
                {
                    switch(obj.Field)
                    {
                        case "position":
                        {
                            Position position = obj.Value as Position;
                            gameObjectFlag.transform.position = new Vector3(position.x, position.y, position.z);
                            Debug.Log("position changed");
                            break;
                        }
                    }
                });
            };
        }
        
    }
}