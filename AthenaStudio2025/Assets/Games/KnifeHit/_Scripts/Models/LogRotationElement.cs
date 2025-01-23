using Newtonsoft.Json;
using System;
using System.ComponentModel;
using UnityEngine;

namespace KnifeHit
{
    [Serializable]
    public class LogRotationElement
    {
        [Description("Max speed:" +
            "\n+ positive for clockwise rotation" +
            "\n- negative for counterclockwise rotation")]
        [SerializeField] private float maxSpeed; 
        [Description("Acceleration: increased degree per second, use absolute value")]
        [SerializeField] private float acceleration;
        [Description("Use seconds")]
        [SerializeField] private float duration;

        public LogRotationElement(float maxSpeed, float acceleration, float duration)
        {
            this.maxSpeed = maxSpeed; 
            this.acceleration = Math.Abs(acceleration);
            this.duration = duration;
        }

        public float MaxSpeed { get => maxSpeed; }
        public float Acceleration { get => acceleration; }
        public float Duration { get => duration; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}