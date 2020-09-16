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
using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.simai
{
    public class ElementObject : MonoBehaviour
    {
        public ElementAttbutes objAttbutes;
        public GameObject elementButton;
        public LogicObj logicObject;
        public string nameLogic;
        public bool CanDelete = true;
        public bool CanDrag = false;
        public bool IsDraging = false;
        public bool CanScale = false;
        public Vector3 offsetLogic = Vector3.zero;
        private Vector3 PosDragStart;
        private Vector3 MousePosDragStart;
        public Vector3 v3Scale;
        public Vector3 offsetPos;
        public float speedObjTarget;
        public virtual ElementAttbutes GetObjAttbutes()
        {
            return new ElementAttbutes();
        }
        public virtual void SetObjAttbutes(ElementAttbutes attbutes)
        {
        }
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
            if (elementButton != null) Destroy(elementButton);
        }
        protected virtual void Start()
        {
            ElementInit();
        }
        protected virtual void Update()
        {
        }

        private void SetLogicObj()
        {
            if (logicObject != null) return;
            GameObject logictemp = (GameObject)Resources.Load("LogicObjs/" + nameLogic);
            if (logictemp != null)
            {
                logicObject = Instantiate(logictemp, transform).GetComponent<LogicObj>();
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

            if (this is EgoController)
            {
                gameObject.name = "EgoVehicle";
            }
            else if (this is ObstacleController)
            {
                gameObject.name = "Static Obstacle" + ElementsManager.Instance.obstacleManager.ObstacleList.Count;
            }
            else if (this is PedestrianController)
            {
                gameObject.name = "PedestrianController" + ElementsManager.Instance.pedestrianManager.PedestrainList.Count;
            }
            else if (this is TrafficLightController)
            {
                gameObject.name = "Traffic Light" + ElementsManager.Instance.trafficlightManager.TrafficLightList.Count;
            }
            else if (this is NPCController)
            {
                gameObject.name = "NPC Vehicle" + ElementsManager.Instance.nPCManager.NPCList.Count;
            }
            else if (this is CheckPointController)
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