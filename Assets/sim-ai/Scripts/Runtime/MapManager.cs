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


using Assets.Scripts.SimuUI;
using UnityEngine;

namespace Assets.Scripts.Element
{
    public class MapManager:MonoBehaviour
    {
        private static MapManager _instance;
        public static MapManager Instance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError("No MapManager");
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
                    ElementsManager.Instance.AddCheckPoint(attrubute.TransformData.V3Pos,attrubute.TransformData.V3Rot,attrubute.TransformData.V3Sca, attrubute.Name);

                }
            }
            if (testMode.ObstacleAtts != null)
            {
                foreach (ElementAttbutes attrubute in testMode.ObstacleAtts)
                {
                    ElementsManager.Instance.AddObstacle(attrubute.TransformData.V3Pos, attrubute.TransformData.V3Rot, attrubute.TransformData.V3Sca, attrubute.Name);
                }
            }
            if (testMode.CarAIAtts != null)
            {
                foreach (ElementAttbutes attrubute in testMode.CarAIAtts)
                {
                    ElementsManager.Instance.AddCarAI(attrubute.PosInit.GetVector3(), attrubute.Name).CarInit();
                }
            }
            if (testMode.HumanAtts != null)
            {
                foreach (ElementAttbutes attrubute in testMode.HumanAtts)
                {
                    ElementsManager.Instance.AddHuman(attrubute.PosArray[0],attrubute.Name).SetObjAttbutes(attrubute);
                }
            }
            if (testMode.TrafficLightAtts != null)
            {
                foreach (ElementAttbutes attrubute in testMode.TrafficLightAtts)
                {
                    foreach (ObjTrafficLight item in ElementsManager.Instance.TrafficLightList)
                    {
                        if (item.name == attrubute.Name)
                        {
                            item.SetObjAttbutes(attrubute);
                        }
                    }
                }
            }
            //if (TestConfig.TestMode.VoyageTestConfig != null)
            //{
            //    VoyageTestManager.Instance.SetVoyageTestConfig(TestConfig.TestMode.VoyageTestConfig);
            //}
        }
        public void ResetMapElements()
        {
            if (testMode.TestCarStart != null) ElementsManager.Instance.testCar.ElementReset();
            foreach (ElementObject obj in ElementsManager.Instance.CarList)
            {
                var objCarAI = obj.GetComponent<ObjAICar>();
                if (objCarAI != null) objCarAI.ElementReset();
            }
            foreach (ElementObject obj in ElementsManager.Instance.HumanList)
            {
                var objHuman = obj.GetComponent<ObjHuman>();
                if (objHuman != null) objHuman.ElementReset();
            }
            foreach (ElementObject obj in ElementsManager.Instance.ObstacleList)
            {
                var objObstacle = obj.GetComponent<ObjObstacle>();
                if (objObstacle != null) objObstacle.ElementReset();
            }
            foreach (ElementObject obj in ElementsManager.Instance.CheckPointList)
            {
                var objCheckPoint = obj.GetComponent<ObjCheckPoint>();
                if (objCheckPoint != null) objCheckPoint.ElementReset();
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
