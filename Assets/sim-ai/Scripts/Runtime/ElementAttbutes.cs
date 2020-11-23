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

using System.Collections.Generic;

namespace Assets.Scripts.simai
{
    public class ElementAttbutes
    {
        public ElementAttbutes() { }

        public ElementAttbutes(
            bool name,
            bool pos,
            bool rot,
            bool scale,
            bool npc,
            bool ped,
            bool light,
            bool delete
            )
        {
            IsShowName = name;
            IsShowPos = pos;
            IsShowRot = rot;
            IsShowSca = scale;
            IsShowCarAI = npc;
            IsShowHuman = ped;
            IsShowTraffic = light;
            IsShowDelete = delete;
        }

        public bool IsShowName { get; set; }
        public bool IsShowPos { get; set; }
        public bool IsShowRot { get; set; }
        public bool IsShowSca { get; set; }
        public bool IsShowCarAI { get; set; }
        public bool IsShowHuman { get; set; }
        public bool IsShowTraffic { get; set; }
        public bool IsShowDelete { get; set; }
        public bool IsAuto { get; set; }
        public float WaitTime { get; set; }
        public float SwitchTime { get; set; }
        public bool IsRepeat { get; set; }
        public List<Vec3> PosArray { get; set; }
        public TransformData TransformData { get; set; }
        public Vec3 PosEnd { get; set; }
        public Vec3 PosStart { get; set; }
        public int LightMode { get; set; }
        public float Speed { get; set; }
        public string Name { get; set; }
        public Vec3 PosInit { get; set; }
        public int Model { get; set; }
    }
}
