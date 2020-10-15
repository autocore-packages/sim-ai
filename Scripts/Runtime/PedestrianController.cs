using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public abstract class PedestrianController :ElementObject
    {
        public override ElementAttbutes GetObjAttbutes()
        {
            ElementAttbutes ea = new ElementAttbutes(true, false, false, false, true, false, false, CanDelete)
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
            isHumanRepeat = attbutes.IsRepeat;
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
                if (posList==null || posList.Count <= 0)
                {
                    posList = new List<Vector3> { transform.position };
                }
                return posList;
            }
        }
        public bool isHumanRepeat = true;
        #endregion

        public float currentSpeed;
        protected bool isReachTarget = false;

        protected float RemainDistance
        {
            get
            {
                return Vector3.Distance(transform.position, AimPos);
            }
        }
        private int PedIndex = 0;
        

        protected Vector3 AimPos
        {
            get
            {
                if (PosList.Count < 1) return transform.position;
                else
                {
                    if (PedIndex >= PosList.Count) return PosList[PosList.Count - 1];
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
            List<Vec3> vec3s=new List<Vec3>();
            foreach (var item in PosList)
            {
                vec3s.Add(new Vec3(item));
            }
            return vec3s;
        }

        protected void OnReachTarget()
        {
            PedIndex++;
            if (PedIndex >= PosList.Count)
            {
                if (!isHumanRepeat)
                {
                    PedIndex = PosList.Count - 1;
                }
                else
                {
                    PedIndex = 0;
                }
            }
            isReachTarget = false;
        }
        public abstract void SetPedstrianAim();
        public void SetPoslist(int index, Vector3 pos)
        {
            PosList[index] = pos;
        }

        public void SetRepeat(bool value)
        {
            isHumanRepeat = value;
        }
        public override void ElementReset()
        {
            base.ElementReset();
            transform.position = PosList[0];
            PedIndex = 0;
        }

    }

}