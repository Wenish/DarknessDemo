using UnityEngine;
using Game.Scripts;
using Game.Models;

namespace Game.Scripts.Controllers
{
    public class ControllerCamera : MonoBehaviour
    {
        public GameManager GameManager;

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

            if(Input.GetMouseButtonDown(1) && GameManager.GameRoom != null)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Debug.Log(hit.point);
                    Player player = GameManager.Players[GameManager.GameRoom.SessionId];
                    await GameManager.GameRoom.Send(new
                    {
                        ACTION_TYPE = "UNIT_MOVE_TO",
                        payload = new
                        {
                            hit.point.x,
                            hit.point.z,
                            player.idUnit
                        }
                    });
                }
            }
            if (Input.GetKeyDown(KeyCode.Space) && GameManager.GameRoom != null)
            {
                Player player = GameManager.Players[GameManager.GameRoom.SessionId];

                if (GameManager.Units.ContainsKey(player.idUnit))
                {
                    GameObject gameObjectUnit = GameManager.Units[player.idUnit];
                    
                    await GameManager.GameRoom.Send(new
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
    }
}