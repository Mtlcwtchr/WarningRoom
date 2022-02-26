using Abstractions;
using UnityEngine;


namespace Controller
{
    public class CameraController : MonoBehaviour
    {
        private ITrackingObject _trackingObject;
        private Vector3 _cameraOffset;

        private void OnEnable()
        {
            _cameraOffset = transform.localPosition;
        }

        public void Initialize(ITrackingObject trackingObject)
        {
            _trackingObject = trackingObject;
            _trackingObject.OnPositionChanged = TrackAttachment;
        }

    
        public void Deinitialize()
        {
            _trackingObject.OnPositionChanged -= TrackAttachment;
        }


        private void TrackAttachment(GameObject trackingObject)
        {
            transform.position = trackingObject.transform.position + _cameraOffset;
        }
    }
}
