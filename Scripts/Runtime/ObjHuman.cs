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
using Assets.Scripts.SimuUI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class ObjHuman : ElementObject
    {
        public override ElementAttbutes GetObjAttbutes()
        {
            ElementAttbutes ea = new ElementAttbutes();
            ea.IsShowCarAI = false;
            ea.IsShowDelete = CanDelete;
            ea.IsShowName = true;
            ea.IsShowHuman = true;
            ea.IsShowPos = false;
            ea.IsShowRot = false;
            ea.IsShowSca = false;
            ea.IsRepeat = isHumanRepeat;
            ea.Name = transform.name;
            ea.Speed = speedObjTarget;
            ea.IsWait = stopTime != 0.1f;
            ea.PosArray = PosList;
            return ea;
        }
        public override void SetObjAttbutes(ElementAttbutes attbutes)
        {
            if (ElementsManager.Instance.SelectedElement != this) return;
            base.SetObjAttbutes(attbutes);
            speedObjTarget = attbutes.Speed;
            isHumanRepeat = attbutes.IsRepeat;
            PosList = attbutes.PosArray;
            stopTime=attbutes.IsWait?1:0.1f;
        }
        private enum HumanMode
        {
            run = 0,
            walk = 1,
            stop = 2
        }
        private HumanMode humanCurrentMode;
        public List<Vec3> PosList=new List<Vec3>();
        public List<Vector3> Aims
        {
            get
            {
                List<Vector3> list = new List<Vector3>();
                foreach (var item in PosList)
                {
                    list.Add(item.GetVector3());
                }
                return list;

            }
        }
        public float stopTime = 2;//人物停顿时间
        public bool isHumanRepeat = true;
        private bool isMove = false;
        public bool isRun;//是否是跑
        public bool isviolateTL;//是否会无视红绿灯

        public float currentSpeed;
        public int currentIndex=0;
        private Animator humanAnim;
        private bool isReachTarget = false;
        public bool isWaitTL;
        public int pass;//0是没过，1是过A，2是过B
        private int path;//1是A，2是B
        //private Traffic_Light TC;
        protected override void Start()
        {
            nameLogic = "HumanLogic";
            speedObjTarget = 1;
            base.Start();
            CanScale = false;
            CanDrag = false;
            CanDelete = true;
            humanAnim = transform.GetComponent<Animator>();
            pass = 0;
        }

        private float distance_Target;//与当前目标点的距离
        Ray ray;
        private bool isObstacleAhead = false;
        protected override void Update()
        {
            base.Update();
            if (PosList.Count < 1) return;
            ray = new Ray(transform.position + Vector3.up, transform.forward);
            distance_Target = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(PosList[currentIndex].X, PosList[currentIndex].Z));

            if (distance_Target > 0.1f)
            {
                transform.LookAt(PosList[currentIndex].GetVector3());
            }
            else if (!isReachTarget)
            {
                isReachTarget = true;
                pass = 0;
                //currentCollider = null;
                StartCoroutine(SetNextTarget());
            }
            ////正在等待红绿灯并且当前红绿灯可以通过
            //if (!isviolateTL && isWaitTL && !TC.isWait && path == (int)TC.currentMode)
            //{
            //    isMove = true;
            //    isWaitTL = false;
            //    pass = path;
            //}
            //timeTemp += Time.deltaTime;
            //if (timeTemp > 1)
            //{
            //    isMove = (!ObstacleCheck()&&isReachTarget);
            //    timeTemp = 0;
            //}
            //Physics.Raycast(ray, walkSpeed + 1)
            if (Physics.Raycast(ray, out RaycastHit hit, 2))
            {
                var Element = hit.collider.GetComponentInParent<ElementObject>();
                if (Element != null)
                {
                    var objhuman = Element.GetComponent<ObjHuman>();
                    if (objhuman == null) isObstacleAhead = true;
                    else isObstacleAhead = false;
                }
                else isObstacleAhead = false;
            }
            else isObstacleAhead = false;
            currentSpeed = (isMove && !isObstacleAhead) ? speedObjTarget : 0;
            if (currentSpeed >= 3)
            {
                humanCurrentMode = HumanMode.run;
                humanAnim.SetFloat("Forward", 1f);
            }
            else if (currentSpeed > 0)
            {
                humanCurrentMode = HumanMode.walk;
                humanAnim.SetFloat("Forward", 0.5f);
            }
            else
            {
                humanCurrentMode = HumanMode.stop;
                humanAnim.SetFloat("Forward", 0);
            }
            if (humanCurrentMode != HumanMode.stop)
                transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
        }
        private bool ObstacleCheck()
        {
            foreach (ElementObject element in ElementsManager.Instance.CarList)
            {
                float tempMinDis = speedObjTarget * 1 + 2;
                GameObject obj = element.gameObject;
                if (!obj.activeSelf || obj.transform == transform) continue;   //如果物体没有显示或者为自身
                float tempDis = Vector3.Distance(obj.transform.position, transform.position);
                if (tempDis >= tempMinDis) continue; //如果距离大于最小距离
                float foreAngle = Vector3.Angle(transform.forward, obj.transform.position - transform.position);//物体与当前物体目标方向的角度
                if (foreAngle < 90 && (tempDis * Mathf.Sin(foreAngle * Mathf.Deg2Rad) < 2))
                {
                    return true;
                }
            }
            foreach (ElementObject element in ElementsManager.Instance.ObstacleList)
            {
                float tempMinDis = 2;
                GameObject obj = element.gameObject;
                if (!obj.activeSelf || obj.transform == transform) continue;   //如果物体没有显示或者为自身
                float tempDis = Vector3.Distance(obj.transform.position, transform.position);
                if (tempDis >= tempMinDis) continue; //如果距离大于最小距离
                float foreAngle = Vector3.Angle(transform.forward, obj.transform.position - transform.position);//物体与当前物体目标方向的角度
                if (foreAngle < 90 && (tempDis * Mathf.Sin(foreAngle * Mathf.Deg2Rad) < 2)) return true;
            }
            return false;
        }

        IEnumerator SetNextTarget()
        {
            isMove = false;
            if (PosList.Count < 2)
            {
                yield return new WaitForSeconds(stopTime);
                StartCoroutine(SetNextTarget());
            }
            else
            {
                currentIndex++;
                if (currentIndex >= PosList.Count)
                {
                    if (!isHumanRepeat)
                    {
                        currentIndex = PosList.Count - 1;
                        yield return new WaitForSeconds(stopTime);
                        StartCoroutine(SetNextTarget());
                        yield break;
                    }
                    else
                    {
                        currentIndex = 0;
                    }
                }
                yield return new WaitForSeconds(stopTime);
                isMove = true;
                isReachTarget = false;
            }
        }
        /// <summary>
        /// 获取最近的点
        /// </summary>
        /// <returns></returns>
        private int GetNearlyPoint()
        {
            Vector3 temPos = transform.position;
            float minDis = Vector3.Distance(temPos, PosList[0].GetVector3());
            int nearlyIndex = 0;
            for (int i = 1; i < PosList.Count; i++)
            {
                float tempDis = Vector3.Distance(temPos, PosList[i].GetVector3());
                if (minDis > tempDis)
                {
                    minDis = tempDis;
                    nearlyIndex = i;
                }
            }
            currentIndex = nearlyIndex;
            return nearlyIndex;
        }
        //private Collider currentCollider;
        //private void OnTriggerEnter(Collider collider)
        //{
        //    if (collider == currentCollider || isviolateTL || !collider.transform.parent.GetComponent<Traffic_Light>()) return;
        //    TC = collider.transform.parent.GetComponent<Traffic_Light>();
        //    currentCollider = collider;
        //    //判断要通过的路口是A还是B
        //    path = 0;
        //    if (TC.HPA.Contains(collider)) path = 1;
        //    else if (TC.HPB.Contains(collider)) path = 2;
        //    //开始过路口
        //    if (pass == 0)
        //    {
        //        if (TC.isWait || //黄灯
        //            path != (int)TC.currentMode) //当前红绿灯不让通过
        //        {
        //            isMove = false;
        //            isWaitTL = true;
        //        }
        //        else
        //        {
        //            isWaitTL = false;
        //            pass = path;
        //        }
        //    }
        //    //结束过路口
        //    else if (pass == path)
        //    {
        //        pass = 0;
        //    }
        //}

        public void SetPoslist(int index,Vector3 pos)
        {
            PosList[index] =new Vec3( pos);
        }
        public void SetRepeat(bool value)
        {
            isHumanRepeat = value;
        }
        public override void ElementReset()
        {
            base.ElementReset();
            transform.position = PosList[0].GetVector3();
            currentIndex = 0;
            StopCoroutine(SetNextTarget());
        }
    }
}
