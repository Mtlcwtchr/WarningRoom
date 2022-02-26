using System;
using Abstractions;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Controller
{
    public class CharacterController : MonoBehaviour, ITrackingObject
    {
        public Action<GameObject> OnPositionChanged { get; set; }
        
        [SerializeField] private CameraController trackingCamera;
        [SerializeField] private Rigidbody rigidbody;
    
        [SerializeField] private float velocity;
        [SerializeField] private float maxHealth;
    
        private float _health;
    
        
        private void OnEnable()
        {
            _health = maxHealth;
            
            trackingCamera.Initialize(this);
        }
    
    
        private void OnDisable()
        {
            trackingCamera.Deinitialize();
        }
    
        
        private void Update()
        {
            if (Input.GetKey(KeyCode.W))
            {
                Move(Vector3.forward);
            } 
            else if (Input.GetKey(KeyCode.S))
            {
                Move(Vector3.back);
            }
            
            if (Input.GetKey(KeyCode.A))
            {
                Move(Vector3.left);
            } 
            else if (Input.GetKey(KeyCode.D))
            {
                Move(Vector3.right);
            }
        }
    
        
        private void Move(Vector3 movementVector)
        {
            rigidbody.AddForce(movementVector * velocity);
    
            OnPositionChanged?.Invoke(gameObject);
        }
    
        
        private void OnCollisionEnter(Collision other)
        {
            BombController bombController = other.gameObject.GetComponent<BombController>();
            if (bombController != null)
            {
                HandleDamage(bombController.Damage);
            }
        }
    
        
        private void HandleDamage(float damage)
        {
            if (_health <= damage)
            {
                Die();
            }
            else
            {
                _health -= damage;
            }
    
            if (_health >= maxHealth)
            {
                _health = maxHealth;
            }
        }
    
        
        private void Die()
        {
            Destroy(gameObject);

            SceneManager.LoadScene("Scenes/Main");
        }
    
    }
}