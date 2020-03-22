using Game.Models;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class ControllerUnit : MonoBehaviour
    {
        public float SpeedLerp = .085f;
        public float LocomotionAnimationSmoothTime = .1f;
        public string Id = null;
        public Unit Unit;
        private ControllerTag _controllerTag;
        private Animator _animator;

        public void Start()
        {
            /*
            GameObject gameObjectBody = this.transform.Find("Body").gameObject;
            Color newColor = Random.ColorHSV();
            Material newMaterial = new Material(Shader.Find("Standard"));
            Renderer rendererBody = gameObjectBody.GetComponent<Renderer>();
            rendererBody.material.SetColor("_Color", newColor);
            */
            
            _controllerTag = GetComponentInChildren<ControllerTag>();
            _animator = GetComponentInChildren<Animator>();
            Debug.Log(_animator);
        }
        public void Update()
        {
            if (Unit?.position != null)
            {
                var t = Time.deltaTime / SpeedLerp;
                Vector3 desiredPostion = new Vector3(Unit.position.x, Unit.position.y, Unit.position.z);
                transform.position = Vector3.Lerp(transform.position, desiredPostion, t);
                _animator.SetFloat("SpeedPercent", Unit.locomotionAnimationSpeedPercent, LocomotionAnimationSmoothTime, Time.deltaTime);
            }

            if (Unit?.rotation != null)
            {
                var t = Time.deltaTime / SpeedLerp;
                Vector3 desiredRotation = new Vector3(0, Unit.rotation, 0);
                float angle = Mathf.LerpAngle(transform.eulerAngles.y, desiredRotation.y, t);
                transform.eulerAngles = new Vector3(0, angle, 0);
            }

            if (_controllerTag != null)
            {
                _controllerTag.HealthPercent = Mathf.Clamp01(Unit.health.current / Unit.health.max);
                _controllerTag.EnergyPercent = Mathf.Clamp01(Unit.energy.current / Unit.energy.max);
            }

            if (_animator != null) {
                _animator.SetBool("IsAlive", Unit.isAlive);
            }
        }
    }
}