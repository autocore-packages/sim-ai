using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public abstract class PedestrianObj : ElementObject
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
        public override void SetObjAttbutes(ElementAttbutes attbutes)
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

        public override string NameLogic
        {
            get
            {
                var logic = ElementsManager.Instance.pedestrianManager.Models[model].Logic;
                if (logic != null) return logic;
                else return "HumanLogic";
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
            Debug.Log(PosList.Count + "" + PedIndex);
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