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
    [ExecuteInEditMode]
    public class ElementsManager : MonoBehaviour
    {
        private static ElementsManager _instance = null;
        public static ElementsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ElementsManager>();
                    Debug.Log("No ElementManager");
                }
                return _instance;
            }
        }

        

        public bool IsInEdit { get; set; } = false;

        [HideInInspector]
        public EgoVehicleController testCar;
        public List<ElementObject> ElementList = new List<ElementObject>();

        public GameObject GONPCManager;
        public NPCManager nPCManager;

        public GameObject GOPedestrianManager;
        public PedestrianManager pedestrianManager;

        public GameObject GOObstacleManager;
        public ObstacleManager obstacleManager;

        public GameObject GOTrafficlghtManager;
        public TrafficlightManager trafficlightManager;

        public GameObject GOCheckPointManager;
        public CheckPointManager checkPointManager;


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
            elementObject.transform.position =MouseWorldPos + elementObject.OffsetPos;
        }

        private void Awake()
        {
            _instance = this;
            if (nPCManager == null) nPCManager = Instantiate(GONPCManager, transform).GetComponent<NPCManager>();
            if (pedestrianManager == null) pedestrianManager = Instantiate(GOPedestrianManager, transform).GetComponent<PedestrianManager>();
            if (obstacleManager == null) obstacleManager = Instantiate(GOObstacleManager, transform).GetComponent<ObstacleManager>();
            if (checkPointManager == null) checkPointManager = Instantiate(checkPointManager, transform).GetComponent<CheckPointManager>();
            if (trafficlightManager == null) trafficlightManager = Instantiate(GOTrafficlghtManager, transform).GetComponent<TrafficlightManager>();
        }

        public bool isShowLine;
        public Vector3[] LinePoses;

        bool isCursorSeted = false;
        private void Start()
        {
            locusLR.enabled = false;
        }
        private void Update()
        {
            if (isShowLine)
            {
                SetLineRenderer(LinePoses);
            }
            else if (locusLR.enabled) locusLR.enabled = false;
        }


        public void RemoveSelectedElement()
        {
            if (!SelectedElement.CanDelete) return;
            Destroy(SelectedElement.gameObject);
        }
        public void RemoveElement(GameObject obj) 
        {
            var eleObj = obj.GetComponent<ElementObject>();

            RemoveElementFromList(eleObj);
        }
        public void RemoveElementFromList(ElementObject elementObject)
        {
            if (!elementObject.CanDelete) return;
            Destroy(elementObject.gameObject);
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
            SelectedElement = null;
        }

        public LineRenderer locusLR;
        private void SetLineRenderer(Vector3[] postions)
        {
            if (locusLR == null) locusLR = GetComponent<LineRenderer>();
            Vector3[] Poses = new Vector3[postions.Length];
            for (int i = 0; i < postions.Length; i++)
            {
                Poses[i] = postions[i] + Vector3.up * 3;
            }
            if (!locusLR.enabled) locusLR.enabled = true;
            locusLR.positionCount = Poses.Length;
            locusLR.SetPositions(Poses);
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

