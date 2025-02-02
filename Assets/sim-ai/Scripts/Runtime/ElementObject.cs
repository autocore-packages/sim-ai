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

using System;
using UnityEngine;

namespace Assets.Scripts.simai
{
    [Serializable]
    public class Model
    {
        public GameObject GOPrefab;
        public GameObject LogicPrefab;
    }
    public abstract class ElementObject : MonoBehaviour
    {
        public Transform m_transform;
        public ElementAttbutes objAttbutes;
        public GameObject elementButton;
        public LogicObject logicObject;
        public Vector3 offsetLogic = Vector3.zero;
        public Vector3 V3Scale
        {
            get
            {
                return m_transform.localScale;
            }
        }
        public int model= 0;

        public abstract bool CanDelete { get; }
        public abstract bool CanDrag { get; }
        public abstract bool CanScale { get; }
        public abstract GameObject LogicPrefab
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
        public abstract void SetObjWithAttbute(ElementAttbutes attbutes);
        public virtual void ElementInit()
        {
            if (!ElementsManager.Instance.ElementList.Contains(this)) ElementsManager.Instance.ElementList.Add(this);
            if (this is ObjNPC npc) ElementsManager.Instance.NPCList.Add(npc);
            else if (this is ObjEgo ego) ElementsManager.Instance.EgoList.Add(ego);
            else if (this is ObjPedestrian ped) ElementsManager.Instance.PedestrainList.Add(ped);
            else if (this is ObjObstacle obs) ElementsManager.Instance.ObstacleList.Add(obs);
            else if (this is ObjCheckPoint che) ElementsManager.Instance.CheckPointList.Add(che);
            else if (this is ObjTrafficLight tra) ElementsManager.Instance.TrafficLightList.Add(tra);
            SetElementName();
            SetLogicObj();
        }
        public void UpdateElement2Attributes()
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
            if (ElementsManager.Instance.ElementList.Contains(this)) ElementsManager.Instance.RemoveElement(this);
            if (this is ObjNPC npc) ElementsManager.Instance.NPCList.Remove(npc);
            else if (this is ObjEgo ego) ElementsManager.Instance.EgoList.Remove(ego);
            else if (this is ObjPedestrian ped) ElementsManager.Instance.PedestrainList.Remove(ped);
            else if (this is ObjObstacle obs) ElementsManager.Instance.ObstacleList.Remove(obs);
            else if (this is ObjCheckPoint che) ElementsManager.Instance.CheckPointList.Remove(che);
            else if (this is ObjTrafficLight tra) ElementsManager.Instance.TrafficLightList.Remove(tra);

            if (elementButton != null) Destroy(elementButton);
        }
        protected virtual void Awake()
        {
        }
        protected virtual void Start()
        {
            m_transform = transform;
            ElementInit();
        }

        public void SetObjScale(float value) 
        {
            if (!CanScale) return; 
            transform.localScale = V3Scale*value;
        }
        private void SetLogicObj()
        {
            if (logicObject != null) return;
            if (LogicPrefab != null)
            {
                logicObject = Instantiate(LogicPrefab, m_transform).GetComponent<LogicObject>();
                logicObject.elementObject = this;
                logicObject.transform.position = m_transform.position + offsetLogic;
            }
            else
            {
                Debug.LogError("logicObj missing");
            }
        }
        private void SetElementName()
        {
            if (objAttbutes != null)
            {
                gameObject.name = objAttbutes.Name;
                return;
            }
            if (this is ObjEgo objEgo)
            {
                gameObject.name = "EgoVehicle" + ElementsManager.Instance.EgoList.IndexOf(objEgo);
            }
            else if (this is ObjObstacle objObstacle)
            {
                gameObject.name = "Obstacle" + ElementsManager.Instance.ObstacleList.IndexOf(objObstacle);
            }
            else if (this is ObjPedestrian objPedestrian)
            {
                gameObject.name = "Pedestrian" + ElementsManager.Instance.PedestrainList.IndexOf(objPedestrian);
            }
            else if (this is ObjTrafficLight objTrafficLight)
            {
                gameObject.name = "Traffic Light" + ElementsManager.Instance.TrafficLightList.IndexOf(objTrafficLight);
            }
            else if (this is ObjNPC objNPC)
            {
                gameObject.name = "NPC Vehicle" + ElementsManager.Instance.NPCList.IndexOf(objNPC);
            }
            else if (this is ObjCheckPoint objCheckPoint)
            {
                gameObject.name = "CheckPoint" + ElementsManager.Instance.CheckPointList.IndexOf(objCheckPoint);
            }
        }
        public virtual void ElementReset()
        {
            if (objAttbutes != null) SetObjWithAttbute(objAttbutes);
            else Debug.Log("No att");
        }
    }
}