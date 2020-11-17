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
using UnityEngine;

namespace Assets.Scripts.simai
{

    public class LaneData
    {
        public int LaneID { get; set; }
        public Vec3 PosStart { get; set; }
        public Vec3 PosEnd { get; set; }
        public float LaneLength { get; set; }
        public List<Vec3> List_pos { get; set; }
        public List<int> List_sameLanesID { get; set; }
        public LaneData() { }
        public LaneData(int id, Vector3 vStart, Vector3 vEnd, float length, List<Vector3> listPos, List<int> sameLanesID)
        {
            LaneID = id;
            PosStart = new Vec3(vStart);
            PosEnd = new Vec3(vEnd);
            LaneLength = length;
            List_pos = new List<Vec3>();
            foreach (Vector3 pos in listPos)
            {
                List_pos.Add(new Vec3(pos));
            }
            List_sameLanesID = sameLanesID;
        }
    }
    public class RoadsData
    {
        public List<LaneData> LanesData
        {
            get; set;
        }
        public RoadsData() { }
    }
}