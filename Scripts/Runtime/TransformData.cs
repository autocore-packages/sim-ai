#region License
/*
* Copyright 2018 AutoCore
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/
#endregion
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
