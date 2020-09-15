using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.simai
{


    public class Vec3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public Vec3()
        {

        }
        public Vec3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vec3(Vector3 v3)
        {
            X = v3.x;
            Y = v3.y;
            Z = v3.z;
        }

        public Vector3 GetVector3()
        {
            return new Vector3(X, Y, Z);
        }
    }
}
