using UnityEngine;

namespace Controller
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        
        
        private void OnEnable()
        {
            audioSource.Play();
        }

        
        private void OnDisable()
        {
            audioSource.Stop();
        }
    }
}


