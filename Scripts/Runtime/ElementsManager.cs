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


using Assets.Scripts.SimuUI;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Element
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
        public enum EditMode
        {
            Null = 0,
            SetCarPose = 1,
            SetStatic = 2,
            SetHuman = 3,
            SetCarAI = 4,
            SetCheckPoint = 5
        }
        public EditMode editMode;
        private int indexMode = 0;
        public List<ObjObstacle> ObstacleList = new List<ObjObstacle>();
        public List<ObjHuman> HumanList = new List<ObjHuman>();
        public List<ElementObject> CarList = new List<ElementObject>();
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
        private ObjHuman ObjHuman
        {
            get
            {
                if (SelectedElement != null)
                {
                    var objHuman = SelectedElement.GetComponent<ObjHuman>();
                    if (objHuman != null)
                        return SelectedElement.GetComponent<ObjHuman>();
                }
                return null;
            }
        }
        public ObjAICar ObjAiCar
        {
            get
            {
                if (SelectedElement != null)
                {
                    var objAICar = SelectedElement.GetComponent<ObjAICar>();
                    if (objAICar != null)
                        return SelectedElement.GetComponent<ObjAICar>();
                }
                return null;
            }
        }

        public ObjTrafficLight ObjTL
        {
            get
            {
                if (SelectedElement != null)
                {
                    var objTL = SelectedElement.GetComponent<ObjTrafficLight>();
                    if (objTL != null)
                        return SelectedElement.GetComponent<ObjTrafficLight>();
                }
                return null;
            }
        }
        private GameObject objTemp;

        Vector3 mousePos;

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
                        PanelInspector.Instance.InspectorInit(_elementObject.GetObjAttbutes());
                    }
                }
            }
        }

        private void Awake()
        {
            _instance = this;
        }

        private bool isShowLine;
        private Vector3[] LinePoses;

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

            var lr = transform.GetComponent<LineRenderer>();
            if (lr == null) lr = gameObject.AddComponent<LineRenderer>();
            if (lineRenderer == null) lineRenderer = lr;
            lineRenderer.enabled = false;

            PanelInspector.Instance.button_AddPos.onClick.AddListener(() =>
            {
                SetHumanPoses();
            });
            PanelInspector.Instance.button_changeLeft.onClick.AddListener(() =>
            {
                if (ObjAiCar.CanChangeLaneLeft()) ObjAiCar.ChangeLane();
            });
            PanelInspector.Instance.button_changeRight.onClick.AddListener(() =>
            {
                if (ObjAiCar.CanChangeLaneRight()) ObjAiCar.ChangeLane();
            });
            PanelInspector.Instance.button_SwitchLight.onClick.AddListener(() =>
            {
                ObjTL.SwitchLight();
            });
            PanelInspector.Instance.button_SetAim.onClick.AddListener(() =>
            {
                SetEditMode(EditMode.SetCarAI, 3);
            });
            PanelInspector.Instance.btn_DeleteObj.onClick.AddListener(() =>
            {
                RemoveElement();
            });
        }
        private void Update()
        {
            mousePos = OverLookCamera.Instance.MouseWorldPos;
            if (SelectedElement != null) PanelInspector.Instance.InspectorUpdate(SelectedElement.GetObjAttbutes());
            switch (editMode)
            {
                case EditMode.Null:
                    indexMode = 0;
                    break;
                case EditMode.SetCarPose:
                    switch (indexMode)
                    {
                        case 0:
                            PanelOther.Instance.SetTipText("Click to set vehicle position，right click to cancel");
                            indexMode = 1;
                            break;
                        case 1:
                            if (Input.GetMouseButtonDown(0))
                            {
                                PanelOther.Instance.SetTipText("Click to set vehicle orientation");
                                EgoVehicle.Instance.transform.position = mousePos + Vector3.up * 0.5f;
                                TestDataManager.Instance.WriteTestData("Set ego vehicle position:" + mousePos);
                                indexMode = 2;
                            }
                            else if (MouseInputBase.Button1Down)
                            {
                                editMode = EditMode.Null;
                            }
                            break;
                        case 2:
                            if (Vector3.Distance(EgoVehicle.Instance.transform.position, mousePos) > 1)
                                EgoVehicle.Instance.transform.LookAt(mousePos, Vector3.up);
                            if (Input.GetMouseButtonDown(0))
                            {
                                editMode = EditMode.Null;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case EditMode.SetStatic:
                    switch (indexMode)
                    {
                        case 0:
                            PanelOther.Instance.SetTipText("Click to set obstacle position，left ctrl+ mouse wheel to set obstacle size，right click to cancel");
                            objTemp = Instantiate(Obstalce, ObstacleParent);
                            SelectedElement = objTemp.GetComponent<ElementObject>();
                            indexMode = 1;
                            break;
                        case 1:
                            if (MouseInputBase.Button0Down)
                            {
                                objTemp.tag = "Obstacle";
                                objTemp.name = "Obstacle" + ObstacleList.Count.ToString();
                                objTemp.transform.SetParent(ObstacleParent);
                                TestDataManager.Instance.WriteTestData("Set Static Obstacle,Position:" + mousePos + "Scale:" + objTemp.transform.localScale.x);
                                editMode = EditMode.Null;
                            }
                            else if (MouseInputBase.Button1Down)
                            {
                                RemoveElement(objTemp);
                                editMode = EditMode.Null;
                            }
                            objTemp.transform.rotation = Quaternion.identity;
                            objTemp.GetComponent<ObjObstacle>().FollowMouse();
                            break;
                        default:
                            break;
                    }
                    break;
                case EditMode.SetHuman:
                    switch (indexMode)
                    {
                        case 0:
                            PanelOther.Instance.SetTipText("Click to set pedestrian starting position");
                            indexMode = 1;
                            break;
                        case 1:
                            if (MouseInputBase.Button0Down)
                            {
                                PanelOther.Instance.SetTipText("Click to add target position for pedestrian, right click to cancel");
                                SelectedElement = AddHuman(mousePos + Vector3.up * 0.1f);
                                ObjHuman.PosList.Add(mousePos);
                                TestDataManager.Instance.WriteTestData("Set Human,Position:" + mousePos.ToString());
                                indexMode = 2;
                            }
                            else if (MouseInputBase.Button1Down)
                            {
                                editMode = EditMode.Null;
                            }
                            break;
                        case 2:
                            isShowLine = true;
                            LinePoses = ObjHuman.PosList.ToArray();
                            LinePoses = new Vector3[ObjHuman.PosList.Count + 1];
                            ObjHuman.PosList.CopyTo(LinePoses);
                            LinePoses[LinePoses.Length - 1] = mousePos;
                            if (MouseInputBase.Button0Down)
                            {
                                if (ObjHuman.PosList.Count > 10)
                                {
                                    PanelOther.Instance.SetTipText("Can't set target positon more than 10");
                                }
                                else
                                {
                                    ObjHuman.PosList.Add(mousePos);
                                    PanelOther.Instance.SetTipText("Click to add target position for pedestrian, right click to cancel");
                                }
                            }
                            else if (MouseInputBase.Button1Down)
                            {
                                editMode = EditMode.Null;
                                isShowLine = false;
                                PanelOther.Instance.SetTipText("cancelled");
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case EditMode.SetCarAI:
                    switch (indexMode)
                    {
                        case 0:
                            PanelOther.Instance.SetTipText("Click to set AI vehicle init position");
                            indexMode = 1;
                            break;
                        case 1:
                            if (Input.GetMouseButtonDown(0))
                            {
                                SelectedElement = AddCarAI(mousePos);
                                ObjAiCar.posInit = mousePos;
                                PanelOther.Instance.SetTipText("Click to set AI vehicle starting position");
                                indexMode = 2;
                            }
                            else if (Input.GetMouseButtonDown(1))
                            {
                                editMode = EditMode.Null;
                            }
                            break;
                        case 2:
                            isShowLine = true;
                            LinePoses = new Vector3[2] { ObjAiCar.transform.position, mousePos };
                            LaneData laneTemp = MapManager.Instance.SearchNearestPos2Lane(out int index, mousePos);
                            Vector3 posStart = laneTemp.List_pos[index].GetVector3();
                            ObjAiCar.transform.LookAt(posStart);
                            if (Input.GetMouseButtonDown(0))
                            {
                                isShowLine = false;
                                ObjAiCar.posStart = posStart;
                                TestDataManager.Instance.WriteTestData("Set AI vehicle Init Position:" + ObjAiCar.posInit + "Start Position:" + posStart);
                                ObjAiCar.CarInit();
                                editMode = EditMode.Null;
                            }
                            else if (Input.GetMouseButtonDown(1))
                            {
                                isShowLine = false;
                                Destroy(ObjAiCar.gameObject);
                                editMode = EditMode.Null;
                            }
                            break;
                        case 3:
                            PanelOther.Instance.SetTipText("Click to set AI vehicle target position");
                            indexMode = 4;
                            break;
                        case 4:
                            if (Input.GetMouseButtonDown(0))
                            {
                                PanelOther.Instance.SetTipText("AI vehicle settled");
                                SelectedElement.GetComponent<ObjAICar>().SetTarget(mousePos);
                                editMode = EditMode.Null;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case EditMode.SetCheckPoint:
                    switch (indexMode)
                    {
                        case 0:
                            PanelOther.Instance.SetTipText("Click to set checkpoint position，left ctrl+ mouse wheel to set checkpoint size，right click to cancel");
                            objTemp = Instantiate(CheckPoint, CheckPointParent);
                            objTemp.transform.SetParent(CheckPointParent);
                            SelectedElement = objTemp.GetComponent<ElementObject>();
                            indexMode = 1;
                            break;
                        case 1:
                            if (MouseInputBase.Button0Down)
                            {
                                if (CheckPointList.Count == 1)
                                {
                                    CheckPointController.Instance.SwitchCheckPoint();
                                }
                                TestDataManager.Instance.WriteTestData("Set CheckPoint,Position:" + mousePos + "Scale:" + objTemp.transform.localScale.x);
                                editMode = EditMode.Null;
                            }
                            else if (MouseInputBase.Button1Down)
                            {
                                RemoveElement(objTemp);
                                editMode = EditMode.Null;
                            }
                            objTemp.transform.rotation = Quaternion.identity;
                            objTemp.GetComponent<ObjCheckPoint>().FollowMouse();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            if (isShowLine)
            {
                SetLineRenderer(LinePoses);
            }
            else if (lineRenderer.enabled) lineRenderer.enabled = false;
        }
        public void SetEditMode(EditMode mode, int index = 0)
        {
            editMode = mode;
            indexMode = index;
        }
        public void SetCarSetMode()
        {
            SetEditMode(EditMode.SetCarPose);
        }
        public void SetAddCarAI()
        {
            SetEditMode(EditMode.SetCarAI);
        }
        public void SetAddHuman()
        {
            SetEditMode(EditMode.SetHuman);
        }
        public void SetHumanPoses()
        {
            SetEditMode(EditMode.SetHuman, 2);
        }
        public void SetAddObstacle()
        {
            SetEditMode(EditMode.SetStatic);
        }
        public void SetAddCheckPoint()
        {
            SetEditMode(EditMode.SetCheckPoint);
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
        public ObjHuman AddHuman(Vector3 pos, string name)
        {
            objTemp = Instantiate(Human, pos, Quaternion.identity, HumanParent);
            objTemp.name = name;
            return objTemp.GetComponent<ObjHuman>();
        }
        public ObjObstacle AddObstacle(Vector3 pos, Vector3 rot, Vector3 scale, string name)
        {
            objTemp = Instantiate(Obstalce, pos, Quaternion.Euler(rot), ObstacleParent);
            objTemp.name = name;
            return objTemp.GetComponent<ObjObstacle>();
        }

        public ObjCheckPoint AddCheckPoint(Vector3 pos, Vector3 rot, Vector3 scale, string name)
        {
            objTemp = Instantiate(CheckPoint, pos, Quaternion.Euler(rot), CheckPointParent);
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
        private void RemoveElementFromList(ElementObject elementObject)
        {
            if (!elementObject.CanDelete) return;
            if (elementObject.GetComponent<ObjObstacle>() != null) ObstacleList.Remove((ObjObstacle)elementObject);
            else if (elementObject.GetComponent<ObjCheckPoint>() != null) CheckPointList.Remove((ObjCheckPoint)elementObject);
            else if (elementObject.GetComponent<ObjHuman>() != null) HumanList.Remove((ObjHuman)elementObject);
            else if (elementObject.GetComponent<ObjAICar>() != null) CarList.Remove(elementObject);
            else if (elementObject.GetComponent<ObjTrafficLight>() != null) TrafficLightList.Remove((ObjTrafficLight)elementObject);
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
            CarList.Add(EgoVehicle.Instance.TestCar);
            SelectedElement = null;
        }

        public void AddCarElement(ElementObject obj)
        {
            CarList.Add(obj);
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

        private LineRenderer lineRenderer;
        private void SetLineRenderer(Vector3[] postions)
        {
            Vector3[] Poses = new Vector3[postions.Length];
            for (int i = 0; i < postions.Length; i++)
            {
                Poses[i] = postions[i] + Vector3.up * 3;
            }
            if (!lineRenderer.enabled) lineRenderer.enabled = true;
            lineRenderer.positionCount = Poses.Length;
            lineRenderer.SetPositions(Poses);
        }
        private void SetLineRendererWithMousePos(Vector3[] postions)
        {
            if (lineRenderer == null) lineRenderer = transform.GetComponent<LineRenderer>();
            if (!lineRenderer.enabled) lineRenderer.enabled = true;
            lineRenderer.positionCount = postions.Length + 1;
            lineRenderer.SetPositions(postions);
            lineRenderer.SetPosition(postions.Length, mousePos);
        }
    }
}

