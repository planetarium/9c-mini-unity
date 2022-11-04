using System;
using UnityEngine;
using UnityEngine.Animations;

namespace Mini9C
{
    public class SingleAxisRotator : MonoBehaviour
    {
        [SerializeField]
        private Axis axis;

        [SerializeField]
        private float speed;

        private void Update()
        {
            var rotation = transform.eulerAngles;
            switch (axis)
            {
                case Axis.None:
                    break;
                case Axis.X:
                    rotation.x += speed * Time.deltaTime;
                    break;
                case Axis.Y:
                    rotation.y += speed * Time.deltaTime;
                    break;
                case Axis.Z:
                    rotation.z += speed * Time.deltaTime;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            transform.eulerAngles = rotation;
        }
    }
}
