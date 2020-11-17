﻿#region License
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
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class TrafficlightManager : MonoBehaviour
    {
        public List<TrafficLightObj> TrafficLightList = new List<TrafficLightObj>();

        public void SetTrafficlight(ElementAttbutes attbutes,string name)
        {
            if (TrafficLightList.Count == 0)
            {
                TrafficLightList= FindObjectsOfType<TrafficLightObj>().ToList();
            }
            foreach (TrafficLightObj trafficLight in TrafficLightList)
            {
                if (trafficLight.name == name)
                {
                    trafficLight.objAttbutes = attbutes;

                }
            }
        }

    }

}
