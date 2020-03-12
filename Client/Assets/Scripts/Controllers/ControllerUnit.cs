using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class ControllerUnit : MonoBehaviour
    {
        public Vector3 DesiredPosition;
        public Vector3 DesiredRotation;
        public float SpeedLerp = .085f;
        public float HealthCurrent;
        public float HealthMax;
        public float EnergyCurrent;
        public float EnergyMax;
        public string Id = null;

        private ControllerTag _controllerTag;

        public void Start()
        {
            DesiredPosition = transform.position;

            GameObject gameObjectBody = this.transform.Find("Body").gameObject;
            Color newColor = Random.ColorHSV();
            Material newMaterial = new Material(Shader.Find("Standard"));
            Renderer rendererBody = gameObjectBody.GetComponent<Renderer>();
            rendererBody.material.SetColor("_Color", newColor);
            
            _controllerTag = GetComponentInChildren<ControllerTag>();
        }
        public void Update()
        {
            if (DesiredPosition != null)
            {
                var t = Time.deltaTime / SpeedLerp;
                transform.position = Vector3.Lerp(transform.position, DesiredPosition, t);
            }

            if (DesiredRotation != null)
            {
                var t = Time.deltaTime / SpeedLerp;
                
                float angle = Mathf.LerpAngle(transform.eulerAngles.y, DesiredRotation.y, t);
                transform.eulerAngles = new Vector3(0, angle, 0);
            }

            if (_controllerTag != null)
            {
                _controllerTag.HealthPercent = Mathf.Clamp01(HealthCurrent / HealthMax);
                _controllerTag.EnergyPercent = Mathf.Clamp01(EnergyCurrent / EnergyMax);
            }
        }
    }
}