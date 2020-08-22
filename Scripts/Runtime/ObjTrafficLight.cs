using UnityEngine;

using Assets.Scripts.SimuUI;
using UnityEditor;

namespace Assets.Scripts.Element
{
    public class ObjTrafficLight : ElementObject
    {
        public override ElementAttbutes GetObjAttbutes()
        {
            ElementAttbutes ea = new ElementAttbutes();
            ea.isShowCarAI = false;
            ea.isShowName = true;
            ea.isShowHuman = false;
            ea.isShowPos = false;
            ea.isShowRot = false;
            ea.isShowSca = false;
            ea.isShowTraffic = true;
            ea.isShowDelete = CanDelete;
            ea.Name = transform.name;
            ea.WaitTime = waitTime;
            ea.SwitchTime = switchTime;
            ea.lightMode = (int)trafficMode;
            return ea;
        }
        public override void SetObjAttbutes(ElementAttbutes attbutes)
        {
            if (ElementsManager.Instance.SelectedElement != this) return;
            base.SetObjAttbutes(attbutes);
        }
        public enum TrafficMode
        {
            Wait = 0,
            Apass = 1,
            Bpass = 2
        }
        public TrafficMode trafficMode = TrafficMode.Apass;
        public ITrafficLight[] trafficLightGroupA;
        public ITrafficLight[] trafficLightGroupB;
        public float switchTime = 10;
        public float waitTime = 3;//黄灯时间
        private float tempSwtichTime=0;
        public float tempTime = 0;
        private bool isApass;
        private LogicTrafficLight ltl;
        protected override void Start()
        {
            nameLogic = "TrafficLightLogic";
            base.Start();
            if(!ElementsManager.Instance.TrafficLightList.Contains(this))
            ElementsManager.Instance.TrafficLightList.Add(this);
            CanScale = false;
            CanDrag = false;
            CanDelete = false;
            ltl = logicObject.GetComponent<LogicTrafficLight>();
            trafficLightGroupA = transform.GetChild(0).GetComponentsInChildren<ITrafficLight>();
            trafficLightGroupB = transform.GetChild(1).GetComponentsInChildren<ITrafficLight>();
            SetLights();
        }

        protected override void Update()
        {
            #region 红绿灯时间计算
            tempTime += Time.deltaTime;
            ltl?.SetLogicText((int)(tempSwtichTime - tempTime) + 1);
            if (tempTime > tempSwtichTime)
            {
                SwitchLight();
            }
            #endregion
        }
        public void SwitchLight()
        {
            switch (trafficMode)
            {
                case TrafficMode.Wait:
                    tempSwtichTime = switchTime;
                    if (isApass) trafficMode = TrafficMode.Bpass;
                    else trafficMode = TrafficMode.Apass;
                    isApass = !isApass;
                    break;
                case TrafficMode.Apass:
                    tempSwtichTime = waitTime;
                    trafficMode = TrafficMode.Wait;
                    break;
                case TrafficMode.Bpass:
                    tempSwtichTime = waitTime;
                    trafficMode = TrafficMode.Wait;
                    break;
                default:
                    break;
            }
            SetLights();
            tempTime = 0;
        }
        private void SetLights()
        {
            switch (trafficMode)
            {
                case TrafficMode.Wait:
                    foreach (ITrafficLight light in trafficLightGroupA)
                    { 
                        light.SetLight(2);
                    }
                    foreach (ITrafficLight light in trafficLightGroupB)
                    {
                        light.SetLight(2);
                    }
                    break;
                case TrafficMode.Apass:
                    foreach (ITrafficLight light in trafficLightGroupA)
                    {
                        light.SetLight(1);
                    }
                    foreach (ITrafficLight light in trafficLightGroupB)
                    {
                        light.SetLight(3);
                    }
                    break;
                case TrafficMode.Bpass:
                    foreach (ITrafficLight light in trafficLightGroupA)
                    {
                        light.SetLight(3);
                    }
                    foreach (ITrafficLight light in trafficLightGroupB)
                    {
                        light.SetLight(1);
                    }
                    break;
                default:
                    break;
            }
            ltl.SetLogicTrafficLight((int)trafficMode);
        }
    }
}
