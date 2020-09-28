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
    public class MapManager:MonoBehaviour
    {
        private static MapManager _instance;
        public static MapManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<MapManager>();
                return _instance;
            }
        }
        public SimuTestMode testMode;
        private MapData mapData;
        public MapData MapData
        {
            get
            {
                if (mapData == null) Debug.LogError("No mapData");
                return mapData;
            }
            set
            {
                mapData = value;
            }
        }
        public bool isRepeat = false;
        void Awake()
        {
            _instance = this; 
        }
        void Start()
        {
            Mapinit();
        }
        public void Mapinit()
        {
            ElementsManager.Instance.RemoveAllElements();
            SetMapElements();
        }
        public void SetMapElements()
        {
            if (testMode.TestCarStart != null) ElementsManager.Instance.testCar.ElementReset();
            isRepeat = testMode.isRepeat;
            if (testMode.CheckPointAtts != null && testMode.CheckPointAtts.Count > 0)
            {
                foreach (ElementAttbutes attrubute in testMode.CheckPointAtts)
                {
                    ElementsManager.Instance.checkPointManager.AddCheckPoint(attrubute);
                }
            }
            if (testMode.ObstacleAtts != null)
            {
                foreach (ElementAttbutes attrubute in testMode.ObstacleAtts)
                {
                    ElementsManager.Instance.obstacleManager.AddObstacle(attrubute);
                }
            }
            if (testMode.CarAIAtts != null)
            {
                foreach (ElementAttbutes attrubute in testMode.CarAIAtts)
                {
                    ElementsManager.Instance.nPCManager.AddNPC(attrubute);
                }
            }
            if (testMode.HumanAtts != null)
            {
                foreach (ElementAttbutes attrubute in testMode.HumanAtts)
                {
                    ElementsManager.Instance.pedestrianManager.AddPedestrian(attrubute);
                }
            }
            if (testMode.TrafficLightAtts != null)
            {
                foreach (ElementAttbutes attrubute in testMode.TrafficLightAtts)
                {
                    ElementsManager.Instance.trafficlightManager.SetTrafficlight(attrubute, attrubute.Name);
                }
            }
        }

        public LaneData SearchNearestPos2Lane(out int index, Vector3 positon)
        {
            if (MapData == null) Debug.LogError("MapData Load fialed");
            float disMin = Mathf.Infinity;
            LaneData laneDataTemp = MapData.LanesData[0];
            int indexTemp = 0;
            foreach (LaneData lane in MapData.LanesData)
            { 
                foreach (Vec3 pos in lane.List_pos)
                {
                    float dis = Vector3.Distance(pos.GetVector3(), positon);
                    if (dis < disMin)
                    {
                        disMin = dis;
                        indexTemp = lane.List_pos.IndexOf(pos);
                        if (indexTemp == lane.List_pos.Count - 1) indexTemp--;
                        laneDataTemp = lane;
                    }
                }
            }
            index = indexTemp + 1;
            return laneDataTemp;
        }
    }
}
