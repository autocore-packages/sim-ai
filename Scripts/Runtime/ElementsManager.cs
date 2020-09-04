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
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class ElementsManager : MonoBehaviour
    {
        private static ElementsManager _instance = null;
        public static ElementsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.Log("No ElementManager");
                }
                return _instance;
            }
        }

        

        public bool IsInEdit { get; set; } = false;
        public ObjTestCar testCar;
        public List<ObjObstacle> ObstacleList = new List<ObjObstacle>();
        public List<ObjHuman> HumanList = new List<ObjHuman>();
        public List<ObjAICar> CarList = new List<ObjAICar>();
        public List<ObjTrafficLight> TrafficLightList = new List<ObjTrafficLight>();
        public List<ObjCheckPoint> CheckPointList = new List<ObjCheckPoint>();
        public List<ElementObject> ElementList = new List<ElementObject>();
        private Transform ObstacleParent;
        private Transform HumanParent;
        private Transform AIcarParent;
        private Transform CheckPointParent;
        private GameObject Human;
        private GameObject Obstalce;
        private GameObject AICar;
        private GameObject CheckPoint;

        private GameObject objTemp;

        public Texture2D textureTarget;
        public CursorMode cm = CursorMode.Auto;


        public ElementObject _elementObject;
        public ElementObject SelectedElement
        {
            get { return _elementObject; }
            set
            {
                if (value != _elementObject)
                {
                    _elementObject = value;
                    if (_elementObject == null)
                    {
                        PanelInspector.Instance.SetPanelActive(false);
                    }
                    else
                    {
                        //PanelInspector.Instance.InspectorInit(_elementObject.GetObjAttbutes());
                    }
                }
            }
        }

        public Vector3 MouseWorldPos;

        private Vector3 PosDragStart;
        private Vector3 MousePosDragStart;
        public void ElementStartDrag(ElementObject elementObject)
        {
            SelectedElement = elementObject;
            if (elementObject.CanDrag)
            {
                MousePosDragStart = MouseWorldPos;
                PosDragStart = elementObject.transform.position;
            }
        }
        public void ElementDraging()
        {
            if (SelectedElement.CanDrag)
            {
                SelectedElement.transform.position = PosDragStart + MouseWorldPos - MousePosDragStart;
            }
        }
        public void FollowMouse(ElementObject elementObject)
        {
            elementObject.transform.position =MouseWorldPos + elementObject.offsetPos;
        }

        private void Awake()
        {
            _instance = this;
        }

        public bool isShowLine;
        public Vector3[] LinePoses;

        bool isCursorSeted = false;
        private void Start()
        {
            GameObject GO = new GameObject("Obstacles");
            ObstacleParent = GO.transform;
            ObstacleParent.SetParent(transform);
            GameObject GH = new GameObject("Humans");
            HumanParent = GH.transform;
            HumanParent.SetParent(transform);
            GameObject GC = new GameObject("AICars");
            AIcarParent = GC.transform;
            AIcarParent.SetParent(transform);
            GameObject GCP = new GameObject("CheckPoints");
            CheckPointParent = GC.transform;
            CheckPointParent.SetParent(transform);
            Human = (GameObject)Resources.Load("Elements/AIHuman");
            AICar = (GameObject)Resources.Load("Elements/AutoDriveCar");
            CheckPoint = (GameObject)Resources.Load("Elements/CheckPoint");
            Obstalce = (GameObject)Resources.Load("Elements/Static");
            locusLR.enabled = false;
        }
        private void Update()
        {
            //if (SelectedElement != null) PanelInspector.Instance.InspectorUpdate(SelectedElement.GetObjAttbutes());
            if (isShowLine)
            {
                SetLineRenderer(LinePoses);
            }
            else if (locusLR.enabled) locusLR.enabled = false;
        }

        public ObjAICar AddCarAI(Vector3 pos)
        {
            objTemp = Instantiate(AICar, pos, Quaternion.identity, AIcarParent);
            objTemp.name = "AI Vehicle" + CarList.Count;
            return objTemp.GetComponent<ObjAICar>();
        }
        public ObjAICar AddCarAI(Vector3 pos, string name)
        {
            objTemp = Instantiate(AICar, pos, Quaternion.identity, AIcarParent);
            objTemp.name = name;
            return objTemp.GetComponent<ObjAICar>();
        }
        public ObjHuman AddHuman(Vector3 pos)
        {
            objTemp = Instantiate(Human, pos, Quaternion.identity, HumanParent);
            objTemp.name = "Pedestrian" + HumanList.Count;
            return objTemp.GetComponent<ObjHuman>();
        }
        public ObjHuman AddHuman(Vec3 pos, string name)
        {
            objTemp = Instantiate(Human, pos.GetVector3(), Quaternion.identity, HumanParent);
            objTemp.name = name;
            return objTemp.GetComponent<ObjHuman>();
        }
        public ObjObstacle AddObstacle(Vec3 pos, Vec3 rot, Vec3 scale, string name)
        {
            objTemp = Instantiate(Obstalce, pos.GetVector3(), Quaternion.Euler(rot.GetVector3()), ObstacleParent);
            objTemp.transform.localScale = scale.GetVector3();
            objTemp.name = name;
            objTemp.tag = "Obstacle";
            return objTemp.GetComponent<ObjObstacle>();
        }

        public ObjCheckPoint AddCheckPoint(Vec3 pos, Vec3 rot, Vec3 scale, string name)
        {
            objTemp = Instantiate(CheckPoint, pos.GetVector3(), Quaternion.Euler(rot.GetVector3()), CheckPointParent);
            objTemp.name = name;
            return objTemp.GetComponent<ObjCheckPoint>();
        }

        public void RemoveElement()
        {
            var obj = SelectedElement;
            RemoveElementFromList(obj);
        }
        public void RemoveElement(GameObject obj)
        {
            var eleObj = obj.GetComponent<ElementObject>();
            RemoveElementFromList(eleObj);
        }
        public void RemoveElementFromList(ElementObject elementObject)
        {
            if (!elementObject.CanDelete) return;
            if (elementObject is ObjObstacle obstacle) ObstacleList.Remove(obstacle);
            else if (elementObject is ObjCheckPoint point) CheckPointList.Remove(point);
            else if (elementObject is ObjHuman human) HumanList.Remove(human);
            else if (elementObject is ObjAICar npc) CarList.Remove(npc);
            else if (elementObject is ObjTrafficLight light1) TrafficLightList.Remove(light1);
            if (ElementList.Contains(elementObject)) ElementList.Remove(elementObject);
            Destroy(elementObject);
            SelectedElement = null;
        }

        public void RemoveAllElements()
        {
            for (int i = ElementList.Count - 1; i >= 0; i--)
            {
                ElementObject Element = ElementList[i];
                if (!Element.CanDelete) continue;
                Destroy(Element.gameObject);
            }
            ObstacleList.Clear();
            HumanList.Clear();
            CarList.Clear();
            CheckPointList.Clear();
            SelectedElement = null;
        }

        public void AddCarElement(ElementObject obj)
        {
            CarList.Add((ObjAICar)obj);
        }
        public void AddTrafficLightElement(ElementObject obj)
        {
            TrafficLightList.Add((ObjTrafficLight)obj);
        }
        public void AddHumanElement(ElementObject obj)
        {
            HumanList.Add((ObjHuman)obj);
        }
        public void AddObstacleElement(ElementObject obj)
        {
            ObstacleList.Add((ObjObstacle)obj);
        }
        public void AddCheckPointElement(ElementObject obj)
        {
            CheckPointList.Add((ObjCheckPoint)obj);
        }

        private LineRenderer locusLR;
        private LineRenderer LocusLR
        {
            get
            {
                if (locusLR == null) locusLR = GetComponent<LineRenderer>();
                return locusLR;
            }
        }
        private void SetLineRenderer(Vector3[] postions)
        {
            Vector3[] Poses = new Vector3[postions.Length];
            for (int i = 0; i < postions.Length; i++)
            {
                Poses[i] = postions[i] + Vector3.up * 3;
            }
            if (!LocusLR.enabled) LocusLR.enabled = true;
            LocusLR.positionCount = Poses.Length;
            LocusLR.SetPositions(Poses);
        }
        public void ResetAllElements()
        {
            foreach (ElementObject item in ElementList)
            {
                item.ElementReset();
            }
        }
    }
}

