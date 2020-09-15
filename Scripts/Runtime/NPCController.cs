﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class NPCController : ElementObject
    {
        protected override void Start()
        {
            nameLogic = "GreenCarLogic";
            base.Start();
            CanScale = false;
            CanDrag = false;
            CanDelete = true;
        }
        public Vector3 posInit;//Init position
        public Vector3 posStart;// first aim position
        public Vector3 posAim; //target position
        public Vector3 posAimTemp; //temp target postion

        private readonly float maxSpeed = 40;//最大速度
        public float speedCurrent;//车辆实际速度
        private float acceleration_Drive = 3;
        private float acceleration_Break = 5;
        private float speedAim;//逻辑目标速度

        public override ElementAttbutes GetObjAttbutes()
        {
            ElementAttbutes ea = new ElementAttbutes(true,false,false,false,true,false,false,CanDelete);
            ea.Name = transform.name;
            ea.Speed = speedObjTarget;
            return ea;
        }
        public override void SetObjAttbutes(ElementAttbutes attbutes)
        {
            base.SetObjAttbutes(attbutes);
            speedObjTarget = attbutes.Speed;
            if (attbutes.PosInit != null) posInit = attbutes.PosInit.GetVector3();
            if (attbutes.PosStart != null) posStart = attbutes.PosStart.GetVector3();
            if (attbutes.PosEnd != null) posAim = attbutes.PosEnd.GetVector3();
        }

        //车辆是否行驶
        private bool isCarDrive = false;
        //检查范围
        private float minCheckDistance = 2;
        private bool isWaitTLStop;
        //当前车长度宽度
        private float npc_Width = 2.2f;
        private float npc_extent = 4.3f;

        private LaneData laneCurrent;
        private int indexLane;
        public override void ElementInit()
        {
            base.ElementInit();
            laneCurrent = MapManager.Instance.SearchNearestPos2Lane(out int indexLaneFiset, posStart);
            posAimTemp = laneCurrent.List_pos[indexLaneFiset + 1].GetVector3();
            transform.position = posInit;
            indexLane = indexLaneFiset;
            isCarDrive = true;
        }
        protected override void Update()
        {
            base.Update();
            posAimTemp = laneCurrent.List_pos[indexLane].GetVector3(); 
            PositionCheck();
            ObstacleCheck();
            TrafficLightCheck(); 
            SpeedCaculator();
        }
        #region PositionCheck
        private float angle_Front2Aim, angle_Right2Aim;
        private bool isFront, isRight;
        //车辆与目标相对位置
        private Vector2 offSet;
        private Vector3 offset_V3;
        /// <summary>
        /// 判断目标点相对车辆的位置
        /// </summary>
        /// 
        void PositionCheck()
        {
            offset_V3 = posAimTemp - transform.position;
            angle_Front2Aim = Vector3.Angle(transform.forward, offset_V3);
            angle_Right2Aim = Vector3.Angle(transform.right, offset_V3);
            offSet = new Vector2(offset_V3.x, offset_V3.z);
            isFront = angle_Front2Aim < 90 ? true : false;
            isRight = angle_Right2Aim < 90 ? true : false;
        }
        #endregion

        #region DistanceCheck

        public float distanceBrake; //安全距离
        private float distance2Target;
        //检查范围
        private float checkDistance = 2;
        void DistanceCheck()
        {
            distanceBrake = speedCurrent * (speedCurrent / acceleration_Break) / 2;
            if (distanceBrake < npc_extent + 1) distanceBrake = npc_extent + 1;
            distance2Target = offset_V3.magnitude;
            if (distance2Target > checkDistance && angle_Front2Aim > 150)
            {
                indexLane += 2;
            }
            else if (distance2Target < checkDistance)
            {
                indexLane++;
                //if (isChangeLane) isChangeLane = false;
            }
            if (isHaveTarget && Mathf.Abs(indexLane - indexTarget) < 3 && laneCurrent.List_sameLanesID.Contains(laneTarget.LaneID))
            {
                isHaveTarget = false;
                //PanelOther.Instance.SetTipText("AI vehicle arrive at target position");
            }
            if (indexLane >= laneCurrent.List_pos.Count)
            {
                laneCurrent = SearchNextLane();
                indexLane = 0;
            }
        }

        List<LaneData> listNextLanes;
        /// <summary>
        /// 当路径结束时查找下一条lane
        /// </summary>
        /// <returns></returns>
        private LaneData SearchNextLane()
        {
            if (MapManager.Instance.MapData == null) Debug.Log("lanes is null");
            if (isHaveTarget)
            {
                int index = ListLane2Target.Count - 1;
                ListLane2Target.RemoveAt(index);
                return ListLane2Target[index - 1];
            }
            else
            {
                listNextLanes = new List<LaneData>();
                foreach (LaneData lane in MapManager.Instance.MapData.LanesData)
                {
                    float dis = Vector3.Distance(lane.PosStart.GetVector3(), laneCurrent.PosEnd.GetVector3());
                    if (dis == 0) listNextLanes.Add(lane);
                }
                if (listNextLanes.Count != 0)
                {
                    return listNextLanes[Random.Range(0, listNextLanes.Count)];
                }
                else
                {
                    Debug.Log("not find");
                    return null;
                }
            }
        }
        #endregion

        #region ObstacleCheck

        protected bool isObstacleFront = false;
        void ObstacleCheck()
        {
            isObstacleFront = false;
            Vector3 DirCarGo = indexLane - 1 > 0 ? posAimTemp - laneCurrent.List_pos[indexLane - 1].GetVector3() : transform.forward;
            Vector3 PosCarOrigin = posAimTemp + new Vector3(0, 0.5f, 0) - (npc_extent * DirCarGo.normalized);

            if (RayCheckCar(PosCarOrigin, DirCarGo, out ElementObject element))
            {
                var CarAI = element.GetComponent<ObjAICar>();
                if (CarAI != null)
                {
                    isObstacleFront = true;
                    return;
                }
                Vector3 PosLaneLeft = PosCarOrigin + Quaternion.AngleAxis(90, Vector3.up) * DirCarGo * 3;
                Vector3 PosLaneRight = PosCarOrigin + Quaternion.AngleAxis(-90, Vector3.up) * DirCarGo * 3;

                if (!RayCheckCar(PosLaneLeft, DirCarGo) && CanChangeLaneLeft())
                {
                    ChangeLane();
                }
                else if (!RayCheckCar(PosLaneRight, DirCarGo) && CanChangeLaneRight())
                {
                    ChangeLane();
                }
                else
                {
                    isObstacleFront = true;
                }
            }
        }
        bool ObstacleCheck(Vector3 direction)
        {
            float tempMinDis = distanceBrake;
            foreach (ElementObject element in ElementsManager.Instance.ElementList)
            {
                GameObject obj = element.gameObject;
                if (!obj.activeSelf || obj.transform == transform) continue;   //如果物体没有显示或者为自身
                float heightDiffrence = obj.transform.position.y - transform.position.y;
                if (Mathf.Abs(heightDiffrence) > 1.5f) continue;
                float tempDis = Vector2.Distance(new Vector2(obj.transform.position.x, obj.transform.position.z), new Vector2(transform.position.x, transform.position.z));
                if (tempDis >= tempMinDis) continue; //如果距离大于最小距离
                float foreAngle = Vector3.Angle(direction, obj.transform.position - transform.position);//物体与当前车前方的角度
                if (foreAngle > 60) continue;
                float dis2Front = tempDis * Mathf.Sin(foreAngle * Mathf.Deg2Rad);
                if (dis2Front < (npc_Width / 2 + 0.1f))
                {
                    return true;
                }
            }
            return false;
        }

        Ray rayElement;
        LayerMask maskElement = 1 << 9;
        bool RayCheckCar(Vector3 posOrigin, Vector3 direction)
        {
            rayElement = new Ray(posOrigin, direction);
            if (Physics.Raycast(rayElement, out RaycastHit hitInfo, distanceBrake + 2, maskElement))
            {
                var element = hitInfo.transform.GetComponentInParent<ElementObject>();
                if (element != null && element != this) return true;
            }
            return false;
        }
        bool RayCheckCar(Vector3 posOrigin, Vector3 direction, out ElementObject element)
        {
            rayElement = new Ray(posOrigin, direction);
            if (Physics.Raycast(rayElement, out RaycastHit hitInfo, distanceBrake + 2, maskElement))
            {
                element = hitInfo.transform.GetComponentInParent<ElementObject>();
                if (element != null && element != this) return true;
            }
            element = null;
            return false;
        }
        private LaneData laneChangeTarget;
        public bool CanChangeLaneLeft()
        {
            int index = laneCurrent.List_sameLanesID.IndexOf(laneCurrent.LaneID) - 1;
            if (index >= 0)
            {
                laneChangeTarget = MapManager.Instance.MapData.LanesData[laneCurrent.List_sameLanesID[index]];
                return true;
            }
            else return false;
        }
        public bool CanChangeLaneRight()
        {
            int index = laneCurrent.List_sameLanesID.IndexOf(laneCurrent.LaneID) + 1;
            if (index < laneCurrent.List_sameLanesID.Count)
            {
                laneChangeTarget = MapManager.Instance.MapData.LanesData[laneCurrent.List_sameLanesID[index]];
                return true;
            }
            else return false;
        }
        /// <summary>
        /// 朝目标lane变道
        /// </summary>
        public void ChangeLane()
        {
            if (laneChangeTarget == null) Debug.Log("no target lane");
            if (!laneCurrent.List_sameLanesID.Contains(laneChangeTarget.LaneID)) Debug.Log("not SameLane");
            Vector3 posCar = transform.position;
            float disMin = Vector3.Distance(laneChangeTarget.List_pos[0].GetVector3(), posCar);
            int index = 0;
            int countLane = laneChangeTarget.List_pos.Count;
            for (int i = 1; i < countLane; i++)
            {
                float disTemp = Vector3.Distance(laneChangeTarget.List_pos[i].GetVector3(), posCar);
                if (disTemp < disMin)
                {
                    disMin = disTemp;
                    index = i;
                }
            }
            int lenth = (int)(distanceBrake - npc_extent);
            if (lenth < 3) lenth = 3;
            index += lenth;
            if (index >= countLane) index = countLane - 1;
            Vector3 direction = laneChangeTarget.List_pos[index].GetVector3() - transform.position;
            if (!ObstacleCheck(direction))
            {
                laneCurrent = laneChangeTarget;
                indexLane = index;
            }
        }
        #endregion

        #region TrafficLight Check

        public ITrafficLight currentTL;//当前目标交通灯
        private float disRemain;//距离停止线的距离
        int currentPath2T;//0是有问题，1是APass，2是Bpath
        private float angle2TL;
        void TrafficLightCheck()
        {
            //当前路段没有红绿灯
            if (currentTL == null || currentTL.CanPass)
            {
                isWaitTLStop = false;
                return;
            }
            angle2TL = Vector3.Angle(currentTL.StopLine.forward, transform.forward);
            disRemain = Vector3.Distance(currentTL.StopLine.position, transform.position) * Mathf.Cos(angle2TL * Mathf.Deg2Rad);
            if (angle2TL > 45 && disRemain < 5)
            {
                isWaitTLStop = false;
                return;
            }
            else if (disRemain < distanceBrake)
            {
                isWaitTLStop = true;
            }
        }

        #endregion

        #region Speed Caculator

        private void SpeedCaculator()
        {
            if (speedObjTarget > maxSpeed) speedObjTarget = maxSpeed;
            if (isObstacleFront || isWaitTLStop) speedAim = 0;
            else speedAim = speedObjTarget;
        }
        #endregion

        #region Set target

        private bool isHaveTarget;
        LaneData laneTarget;
        int indexTarget;
        float dis2TargetMin;
        public List<LaneData> ListLane2Target;


        /// <summary>
        /// 查找去目标点的路
        /// </summary>
        /// <param name="point"></param>
        public void SetTarget(Vector3 point)
        {
            laneTarget = MapManager.Instance.SearchNearestPos2Lane(out int index, point);
            indexTarget = index;
            ListLane2Target = new List<LaneData> { laneTarget };
            if (laneTarget == laneCurrent)
            {
                if (indexTarget > indexLane)
                {
                    isHaveTarget = true;
                }
                else
                {
                    SearchWay2(ListLane2Target, 0);
                }
            }
            else
            {
                dis2TargetMin = Mathf.Infinity;
                SearchWay(ListLane2Target, 0);
            }
        }

        /// <summary>
        /// 递归查找
        /// </summary>
        /// <param name="ListLanes"></param>
        /// <param name="lenth"></param>
        private void SearchWay(List<LaneData> ListLanes, float lenth)
        {
            if (ListLanes.Count >= 30 || lenth > 10000) return;
            LaneData laneLast = ListLanes[ListLanes.Count - 1];
            foreach (LaneData lane in MapManager.Instance.MapData.LanesData)
            {
                if (lane.PosEnd != laneLast.PosStart) continue;//不连接的线跳过
                if (ListLanes.Contains(lane)) continue; //剔除掉重复的
                if ((lane.List_sameLanesID.Contains(laneCurrent.LaneID) || lane == laneCurrent) && lenth < dis2TargetMin)
                {
                    ListLanes.Add(lane);
                    dis2TargetMin = lenth;
                    ListLane2Target = ListLanes;
                    isHaveTarget = true;
                }
                else
                {
                    float lenth_temp = lenth + lane.LaneLength;
                    if (lenth_temp < dis2TargetMin)
                    {
                        SearchWay(new List<LaneData>(ListLanes) { lane }, lenth_temp);
                    }
                }
            }
        }
        /// <summary>
        /// 终点和起点同一lane
        /// </summary>
        /// <param name="ListLanes"></param>
        /// <param name="lenth"></param>
        private void SearchWay2(List<LaneData> ListLanes, float lenth)
        {
            if (ListLanes.Count >= 30 || lenth > 5000) return;
            LaneData laneLast = ListLanes[ListLanes.Count - 1];
            foreach (LaneData lane in MapManager.Instance.MapData.LanesData)
            {
                if (lane.PosEnd != laneLast.PosStart) continue;//不连接的线跳过
                if (ListLanes.Contains(lane) && lane != ListLanes[0]) continue; //剔除掉重复的
                if ((lane.List_sameLanesID.Contains(laneCurrent.LaneID) || lane == laneCurrent) && lenth < dis2TargetMin)
                {
                    ListLanes.Add(lane);
                    dis2TargetMin = lenth;
                    ListLane2Target = ListLanes;
                    isHaveTarget = true;
                }
                else
                {
                    float lenth_temp = lenth + lane.LaneLength;
                    if (lenth_temp < dis2TargetMin)
                    {
                        SearchWay(new List<LaneData>(ListLanes) { lane }, lenth_temp);
                    }
                }
            }
        }

        #endregion
    }
}
