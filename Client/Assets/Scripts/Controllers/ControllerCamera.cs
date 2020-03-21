using UnityEngine;
using Game.Scripts;
using Game.Models;

namespace Game.Scripts.Controllers
{
    public class ControllerCamera : MonoBehaviour
    {
        public Vector3 OffsetCamera;
        public float SpeedCamera;
        public float BorderThickness = 10f;
        public bool IsFocusingPlayer = false;
        public Transform CameraTarget;
        public Terrain CameraLimit;
        public float ScrollSpeed = 20f;
        public float MinCameraDistance = 3f;
        public float MaxCameraDistance = 120f;
        private GameManager _gameManager;

        void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            CameraLimit = FindObjectOfType<Terrain>();
        }

        public async void Update()
        {
            // Exit Game  
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
            }

            // Change Camera Type
            if (Input.GetKeyDown(KeyCode.Z))
            {
                IsFocusingPlayer = !IsFocusingPlayer;
            }

            if (IsFocusingPlayer) {
                Player player = _gameManager.Players[_gameManager.GameRoom.SessionId];
                GameObject unit = _gameManager.Units[player.idUnit];
                if (unit) {
                    CameraTarget = unit.transform;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && _gameManager.GameRoom != null)
            {
                Player player = _gameManager.Players[_gameManager.GameRoom.SessionId];

                if (_gameManager.Units.ContainsKey(player.idUnit))
                {
                    GameObject gameObjectUnit = _gameManager.Units[player.idUnit];
                    
                    await _gameManager.GameRoom.Send(new
                    {
                        ACTION_TYPE = "FLAG_PLACE",
                        payload = new
                        {
                            gameObjectUnit.transform.position.x,
                            gameObjectUnit.transform.position.z
                        }
                    });
                }
                print("space key was pressed");
            }
        }

        public void LateUpdate()
        {
            if (IsFocusingPlayer)
            {
                if (CameraTarget != null)
                {
                    var desiredPosition = CameraTarget.position + OffsetCamera;
                    var t = Time.deltaTime / SpeedCamera;
                    transform.position = Vector3.Lerp(transform.position, desiredPosition, t);

                    //transform.LookAt(GameManager.CameraTarget);
                }
            } else {
                Vector3 pos = transform.position;
                if (Input.mousePosition.y >= Screen.height - BorderThickness)
                {
                    var t = Time.deltaTime / SpeedCamera;
                    pos.z += t;
                }
                if (Input.mousePosition.y <= BorderThickness)
                {
                    var t = Time.deltaTime / SpeedCamera;
                    pos.z -= t;
                }
                if (Input.mousePosition.x >= Screen.width - BorderThickness)
                {
                    var t = Time.deltaTime / SpeedCamera;
                    pos.x += t;
                }
                if (Input.mousePosition.x <= BorderThickness)
                {
                    var t = Time.deltaTime / SpeedCamera;
                    pos.x -= t;
                }

                float scroll = Input.GetAxis("Mouse ScrollWheel");
                pos.y += -scroll * ScrollSpeed * 100f * Time.deltaTime;

                pos.x = Mathf.Clamp(pos.x, CameraLimit.transform.position.x, CameraLimit.terrainData.size.x);
                pos.y = Mathf.Clamp(pos.y, MinCameraDistance, MaxCameraDistance);
                pos.z = Mathf.Clamp(pos.z, CameraLimit.transform.position.z, CameraLimit.terrainData.size.z);

                transform.position = pos;
            }
        }
    }
}