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
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class PedestrianManager: MonoBehaviour
    {
        public List<PedestrianObj> PedestrainList = new List<PedestrianObj>();
        public Model[] Models;
        public void AddPedestrian(ElementAttbutes attbutes)
        {
            AddPedestrian(attbutes.Model).SetObjAttbutes(attbutes);
        }
        public PedestrianObj AddPedestrian(int model = 0)
        {
            PedestrianObj pedestrianController = Instantiate(Models[model].Prefab, transform).GetComponent<PedestrianObj>();
            pedestrianController.model = model;
            PedestrainList.Add(pedestrianController);
            return pedestrianController;
        }
    }

}
