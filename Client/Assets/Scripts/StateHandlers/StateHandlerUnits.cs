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
            controllerUnit.Id = key;
            controllerUnit.DesiredRotation.y = unit.rotation;
            controllerUnit.HealthCurrent = unit.health.current;
            controllerUnit.HealthMax = unit.health.max;
            controllerUnit.EnergyCurrent = unit.energy.current;
            controllerUnit.EnergyMax = unit.energy.max;

            _gameManager.Units.Add(key, gameObjectUnit);
            Debug.Log("Player Add");

            unit.OnChange += (changes) =>
            {
                changes.ForEach((obj) =>
                {
                    switch(obj.Field)
                    {
                        case "position":
                        {
                            Position position = obj.Value as Position;
                            controllerUnit.DesiredPosition.x = position.x;
                            controllerUnit.DesiredPosition.z = position.z;
                            break;
                        }
                        case "rotation":
                        {
                            controllerUnit.DesiredRotation.y = float.Parse(obj.Value.ToString());
                            break;
                        }
                        case "health":
                        {
                            Bar bar = obj.Value as Bar;
                            controllerUnit.HealthCurrent = bar.current;
                            controllerUnit.HealthMax = bar.max;
                            break;
                        }
                        case "energy":
                        {
                            Bar bar = obj.Value as Bar;
                            controllerUnit.EnergyCurrent = bar.current;
                            controllerUnit.EnergyMax = bar.max;
                            break;
                        }
                        case "locomotionAnimationSpeedPercent":
                        {
                            controllerUnit.LocomotionAnimationSpeedPercent = float.Parse(obj.Value.ToString());
                            break;
                        }
                        case "isAlive":
                        {
                            controllerUnit.IsAlive = bool.Parse(obj.Value.ToString());
                            break;
                        }
                        case "weaponLoadout":
                        {
                            Debug.Log(obj.Value.ToString());
                            break;
                        }
                    }
                });
            };
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
            /*
            string idUnit = key;
            GameObject gameObjectUnit = _gameManager.Units[idUnit];
            ControllerUnit controllerUnit = gameObjectUnit.GetComponent<ControllerUnit>();
            controllerUnit.Unit = unit;
            */
        }
    }
}