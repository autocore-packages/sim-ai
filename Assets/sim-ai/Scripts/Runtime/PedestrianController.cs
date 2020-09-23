using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class PedestrianController :ElementObject
    {
        public override ElementAttbutes GetObjAttbutes()
        {
            ElementAttbutes ea = new ElementAttbutes(true, false, false, false, true, false, false, CanDelete)
            {
                IsRepeat = isHumanRepeat,
                Name = transform.name,
                Speed = speedObjTarget,
                IsWait = stopTime != 0.1f,
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
            stopTime = attbutes.IsWait ? 1 : 0.1f;
        }
        #region data

        public List<Vector3> PosList = new List<Vector3>();
        public bool isHumanRepeat = true;


        #endregion

        public float stopTime = 1;//人物停顿时间
        private bool IsWait 
        {
            get;
            set;
        }
        public float currentSpeed;
        private bool isReachTarget = false;

        private float remainDistance;//与当前目标点的距离

        private int currentIndex = 0;
        

        protected Vector3 AimPos
        {
            get
            {
                if (PosList.Count < 1) return transform.position;
                else
                {
                    return PosList[currentIndex];
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

        protected override void Start()
        {
            base.Start();
            CanScale = false;
            CanDrag = false;
            CanDelete = true;
        }
        protected override void Update()
        {
            base.Update();
            remainDistance = Vector3.Distance(transform.position,  AimPos);

        }


        private void SetPosList(List<Vec3> vec3s)
        {
            PosList.Clear();
            foreach (Vec3 item in vec3s)
            {
                PosList.Add(item.GetVector3());
            }

            if (!isReachTarget&&remainDistance<0.2f)
            {
                isReachTarget = true;

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
        IEnumerator SetNextTarget()
        {
            IsWait = true;
            if (PosList.Count < 2)
            {
                yield return new WaitForSeconds(1);
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
                        yield return new WaitForSeconds(1);
                        StartCoroutine(SetNextTarget());
                        yield break;
                    }
                    else
                    {
                        currentIndex = 0;
                    }
                }
                yield return new WaitForSeconds(stopTime);
                IsWait = false;
                isReachTarget = false;
            }
        }

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
            currentIndex = 0;
            StopCoroutine(SetNextTarget());
        }

    }

}