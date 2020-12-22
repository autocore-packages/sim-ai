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
    public abstract class ObjPedestrian : ElementObject
    {
        public override ElementAttbutes GetObjAttbutes()
        {
            ElementAttbutes ea = new ElementAttbutes(true, false, false, false, false, true, false, CanDelete)
            {
                IsRepeat = isHumanRepeat,
                Name = transform.name,
                Speed = speedObjTarget,
                PosArray = GetPosList(),
                TransformData = new TransformData(transform)
            };
            return ea;
        }
        public override void SetObjWithAttbute(ElementAttbutes attbutes)
        {
            model = attbutes.Model;
            speedObjTarget = attbutes.Speed;
            SetPedRepeat(attbutes.IsRepeat);
            SetPosList(attbutes.PosArray);
        }
        protected override void Start()
        {
            base.Start();
        }
        public override bool CanDelete => true;

        public override bool CanDrag => false;

        public override bool CanScale => false;
        #region data


        private List<Vector3> posList;
        public List<Vector3> PosList
        {
            get
            {
                if (posList == null)
                {
                    posList = new List<Vector3> { };
                }
                return posList;
            }
        }
        public bool isHumanRepeat = true;
        public bool isHumanWait = false;
        #endregion

        public void AddPedPos(Vector3 pos)
        {
            PosList.Add(pos);
            if (isPedStop) OnReachTarget();
        }

        public float currentSpeed;
        protected bool isReachTarget = false;
        private bool isPedStop = false;
        protected float RemainDistance
        {
            get
            {
                return Vector3.Distance(transform.position, AimPos);
            }
        }
        public int PedIndex = 0;


        protected Vector3 AimPos
        {
            get
            {
                if (PosList.Count < 1) return transform.position;
                else
                {
                    if (PedIndex >= PosList.Count) return PosList[0];
                    return PosList[PedIndex];
                }
            }
        }

        public override GameObject LogicPrefab
        {
            get
            {
                var logic = ElementsManager.Instance.PedstrainModels[model].LogicPrefab;
                return logic;
            }
        }



        private void SetPosList(List<Vec3> vec3s)
        {
            PosList.Clear();
            foreach (Vec3 item in vec3s)
            {
                PosList.Add(item.GetVector3());
            }
        }
        private List<Vec3> GetPosList()
        {
            List<Vec3> vec3s = new List<Vec3>();
            foreach (var item in PosList)
            {
                vec3s.Add(new Vec3(item));
            }
            return vec3s;
        }

        protected void OnReachTarget()
        {
            isReachTarget = true;
            PedIndex++;
            if (PedIndex >= PosList.Count)
            {
                if (!isHumanRepeat || PosList.Count <= 1)
                {
                    PedIndex = PosList.Count - 1;
                    SetPedstrianStop();
                    return;
                }
                PedIndex = 0;
            }
            isReachTarget = false;
            SetPedstrianAim();
            isPedStop = false;
        }
        public abstract void SetPedstrianAim();
        public virtual void SetPedstrianStop()
        {
            isPedStop = true;
        }
        public void SetPoslist(int index, Vector3 pos)
        {
            PosList[index] = pos;
        }

        public void SetPedRepeat(bool value)
        {
            isHumanRepeat = value;
            if (isPedStop) OnReachTarget();
        }
        public override void ElementReset()
        {
            base.ElementReset();
            transform.position = PosList[0];
            PedIndex = 0;
        }

    }

}