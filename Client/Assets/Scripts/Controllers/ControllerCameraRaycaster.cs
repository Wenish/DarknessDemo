using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Models;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts.Controllers
{
    public class ControllerCameraRaycaster : MonoBehaviour
    {
        public Texture2D DefaultCursor = null;
        public Texture2D AttackCursor = null;
        public Vector2 CursorHotspot = new Vector2(0, 0);
        public GameObject MoveToIndicator = null;

        private GameManager _gameManager = null;
        private GameObject _lastMoveToIndicator = null;
        Rect _screenRectAtStartGame = new Rect(0, 0, Screen.width, Screen.height);
        // Start is called before the first frame update
        void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // TODO Implement UI interaction
            } else
            {
                PerformRaycasts();
            };
        }
        async void PerformRaycasts()
        {
            if(_screenRectAtStartGame.Contains(Input.mousePosition))
            {
                RaycastHit hitInfo;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
                {
                    if (await IsRaycastHitOnPlayer(hitInfo)) return;
                    MoveToRaycastHit(hitInfo);
                }
                Cursor.SetCursor(DefaultCursor, CursorHotspot, CursorMode.Auto);
            }
        }

        private async Task<bool> IsRaycastHitOnPlayer (RaycastHit hitInfo)
        {
            var gameObjectHit = hitInfo.collider.gameObject;
            var controllerUnit = gameObjectHit.GetComponent<ControllerUnit>();
            if (controllerUnit)
            {
                Cursor.SetCursor(AttackCursor, CursorHotspot, CursorMode.Auto);

                if (Input.GetMouseButtonDown(1) && _gameManager.GameRoom != null)
                {
                    await _gameManager.GameRoom.Send(new
                    {
                        ACTION_TYPE = "TARGET_PLAYER",
                        payload = new
                        {
                            idUnit = controllerUnit.Id
                        }
                    });
                }

                return true;
            }

            return false;
        }

        private async void MoveToRaycastHit (RaycastHit hitInfo)
        {
            if(Input.GetMouseButtonDown(1) && _gameManager.GameRoom != null)
            {
                Debug.Log(hitInfo.point);

                Player player = _gameManager.Players[_gameManager.GameRoom.SessionId];
                    await _gameManager.GameRoom.Send(new
                    {
                        ACTION_TYPE = "UNIT_MOVE_TO",
                        payload = new
                        {
                            hitInfo.point.x,
                            hitInfo.point.z,
                            player.idUnit
                        }
                    });

                
                if (_lastMoveToIndicator)
                {
                    Destroy(_lastMoveToIndicator);
                }
                if (MoveToIndicator != null)
                {
                    _lastMoveToIndicator = Instantiate(MoveToIndicator, new Vector3(hitInfo.point.x, 0, hitInfo.point.z), Quaternion.identity);
                    Destroy(_lastMoveToIndicator, 1);
                }
            }
        }
    }
}