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

using System;
using UnityEngine;

namespace Assets.Scripts.simai
{
    [Serializable]
    public class Model
    {
        public GameObject Prefab;
        public string Logic;
    }
    public abstract class ElementObject : MonoBehaviour
    {
        public ElementAttbutes objAttbutes;
        public GameObject elementButton;
        public LogicObject logicObject;
        public Vector3 offsetLogic = Vector3.zero;
        public Vector3 v3Scale;
        public int model= 0;

        public abstract bool CanDelete { get; }
        public abstract bool CanDrag { get; }
        public abstract bool CanScale { get; }
        public abstract string NameLogic
        {
            get;
        }
        public virtual Vector3 OffsetPos
        {
            get
            {
                return Vector3.zero;
            }
        }
        public float speedObjTarget;
        public abstract ElementAttbutes GetObjAttbutes();
        public abstract void SetObjAttbutes(ElementAttbutes attbutes);
        public virtual void ElementInit()
        {
            if (!ElementsManager.Instance.ElementList.Contains(this))
            {
                ElementsManager.Instance.ElementList.Add(this);
            }
            SetElementName();
            SetLogicObj();
        }
        public void UpdateElementAttributes()
        {
            objAttbutes = GetObjAttbutes();
        }
        private void OnDestroy()
        {
            DestroyElement();
        }
        public virtual void DestroyElement()
        {
            if (ElementsManager.Instance == null) return;
            if (ElementsManager.Instance.ElementList.Contains(this))
            {
                ElementsManager.Instance.RemoveElement(gameObject);
            }
            if (this is NPCObj npc) ElementsManager.Instance.nPCManager.NPCList.Remove(npc);
            else if (this is PedestrianObj ped) ElementsManager.Instance.pedestrianManager.PedestrainList.Remove(ped);
            else if(this is ObstacleObj obs) ElementsManager.Instance.obstacleManager.ObstacleList.Remove(obs);
            else if(this is CheckPointObj che) ElementsManager.Instance.checkPointManager.CheckPointList.Remove(che);
            else if (this is TrafficLightObj tra) ElementsManager.Instance.trafficlightManager.TrafficLightList.Remove(tra);
            if (elementButton != null) Destroy(elementButton);
        }
        protected virtual void Awake()
        {
        }
        protected virtual void Start()
        {
            ElementInit();
        }

        private void SetLogicObj()
        {
            if (logicObject != null) return;
            GameObject logictemp = (GameObject)Resources.Load("LogicObjs/" + NameLogic);
            if (logictemp != null)
            {
                logicObject = Instantiate(logictemp, transform).GetComponent<LogicObject>();
                logicObject.elementObject = this;
                logicObject.transform.position = transform.position + offsetLogic;
            }
            else
            {
                Debug.LogError("LogicObj missing");
            }
        }
        public void SetObjScale(float value)
        {
            if (!CanScale) return;
            v3Scale = transform.localScale;
            v3Scale = new Vector3(v3Scale.x * value, v3Scale.y * value, v3Scale.z * value);
            transform.localScale = v3Scale;
        }
        private void SetElementName()
        {
            if (objAttbutes != null)
            {
                gameObject.name = objAttbutes.Name;
                return;
            }

            if (this is EgoVehicleObj)
            {
                gameObject.name = "EgoVehicle";
            }
            else if (this is ObstacleObj)
            {
                gameObject.name = "Obstacle" + ElementsManager.Instance.obstacleManager.ObstacleList.Count;
            }
            else if (this is PedestrianObj)
            {
                gameObject.name = "Pedestrian" + ElementsManager.Instance.pedestrianManager.PedestrainList.Count;
            }
            else if (this is TrafficLightObj)
            {
                gameObject.name = "Traffic Light" + ElementsManager.Instance.trafficlightManager.TrafficLightList.Count;
            }
            else if (this is NPCObj)
            {
                gameObject.name = "NPC Vehicle" + ElementsManager.Instance.nPCManager.NPCList.Count;
            }
            else if (this is CheckPointObj)
            {
                gameObject.name = "CheckPoint" + ElementsManager.Instance.checkPointManager.CheckPointList.Count;
            }
        }
        public virtual void ElementReset()
        {
            if (objAttbutes != null) UpdateElementAttributes();
            else Debug.Log("No att");
        }
    }
}