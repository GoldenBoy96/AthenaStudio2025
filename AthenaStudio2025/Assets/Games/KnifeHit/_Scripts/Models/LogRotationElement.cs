using System;
using System.ComponentModel;
using UnityEngine;

namespace KnifeHit
{
    [Serializable]
    public class LogRotationElement
    {
        [Description("Max speed: " +
            "\n+ positive for clockwise rotation" +
            "\n- negative for counterclockwise rotation")]
        [SerializeField] private float maxSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float duration;

        public LogRotationElement(float maxSpeed, float acceleration, float duration)
        {
            this.maxSpeed = maxSpeed;
            this.acceleration = acceleration;
            this.duration = duration;
        }

        public float MaxSpeed { get => maxSpeed; }
        public float Acceleration { get => acceleration; }
        public float Duration { get => duration; }
    }
}