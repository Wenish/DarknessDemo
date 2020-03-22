using Game.Models;
using UnityEngine;
using Game.Scripts.Controllers;

namespace Game.Scripts.StateHandlers
{
    public class StateHandlerUnits
    {
        private StateUnits _stateUnits;
        private GameManager _gameManager;
        public StateHandlerUnits(StateUnits stateUnits, GameManager gameManager)
        {
            _stateUnits = stateUnits;
            _gameManager = gameManager;
            _stateUnits.units.OnAdd += OnAdd;
            _stateUnits.units.OnRemove += OnRemove;
            _stateUnits.units.OnChange += OnChange;
        }
        private void OnAdd(Unit unit, string key)
        {
            Debug.Log(key);
            GameObject gameObjectUnit = Object.Instantiate(
                _gameManager.PrefabUnit,
                new Vector3(unit.position.x, 0, unit.position.z),
                new Quaternion());

            ControllerUnit controllerUnit = gameObjectUnit.GetComponent<ControllerUnit>();
            controllerUnit.Unit = unit;
            controllerUnit.Id = key;

            _gameManager.Units.Add(key, gameObjectUnit);
            Debug.Log("Player Add");

            /*
            unit.OnChange += (changes) =>
            {
                changes.ForEach((obj) =>
                {
                    switch(obj.Field)
                    {
                        case "isAlive":
                        {
                            controllerUnit.IsAlive = bool.Parse(obj.Value.ToString());
                            break;
                        }
                    }
                });
            };
            */
        }

        private void OnRemove(Unit unit, string key)
        {
            string idUnit = key;
            GameObject gameObjectUnit = _gameManager.Units[idUnit];
            Object.Destroy(gameObjectUnit);
            _gameManager.Units.Remove(idUnit);
            Debug.Log("Player Remove");
        }

        private void OnChange(Unit unit, string key)
        {
            string idUnit = key;
            GameObject gameObjectUnit = _gameManager.Units[idUnit];
            ControllerUnit controllerUnit = gameObjectUnit.GetComponent<ControllerUnit>();
            controllerUnit.Unit = unit;
        }
    }
}