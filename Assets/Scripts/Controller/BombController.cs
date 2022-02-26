using System;
using UnityEngine;


namespace Controller
{
    public class BombController : MonoBehaviour
    {
        public Action<BombController> OnExploded;
        
        [SerializeField] private float damage;

        public float Damage => damage;


        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Bomb"))
            {
                Explode();
            }
        }


        private void Explode()
        {
            OnExploded?.Invoke(this);
        }
    }
}