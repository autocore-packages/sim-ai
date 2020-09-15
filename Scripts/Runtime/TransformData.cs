using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.simai
{

    public class TransformData
    {
        public Vec3 V3Pos { get; set; }
        public Vec3 V3Rot { get; set; }
        public Vec3 V3Sca { get; set; }
        public TransformData()
        {

        }
        public TransformData(Vec3 pos, Vec3 rot, Vec3 sca)
        {
            V3Pos = pos;
            V3Rot = rot;
            V3Sca = sca;
        }
        public TransformData(Transform tran)
        {
            V3Pos = new Vec3(tran.position);
            V3Rot = new Vec3(tran.rotation.eulerAngles);
            V3Sca = new Vec3(tran.localScale);
        }
    }
}
