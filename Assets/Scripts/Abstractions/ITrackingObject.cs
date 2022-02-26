using System;
using UnityEngine;

namespace Abstractions
{
    public interface ITrackingObject
    {
        Action<GameObject> OnPositionChanged { get; set; }
    }
}