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
    public class ObstacleManager : MonoBehaviour
    {
        public List<ObjObstacle> ObstacleList = new List<ObjObstacle>();
        public Model[] ObstacleModels;
        public void AddObstacle(ElementAttbutes attbutes)
        {
            AddObstacle(attbutes.Model).SetObjAttbutes(attbutes);
        }
        public ObjObstacle AddObstacle(int model = 0)
        { 
            ObjObstacle obstacleController = Instantiate(ObstacleModels[model].GOPrefab, transform).GetComponent<ObjObstacle>();
            obstacleController.model = model;
            ObstacleList.Add(obstacleController);
            return obstacleController;
        }
    }
}
