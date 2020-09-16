using UnityEngine;


namespace Assets.Scripts.simai
{
    public class TrafficLightController : ElementObject
    {
        public override ElementAttbutes GetObjAttbutes()
        {
            ElementAttbutes ea = new ElementAttbutes(true, false, false, false, false, false, true, CanDelete)
            {
                Name = transform.name,
                lightMode= (int)trafficMode
            };
            return ea;
        }
        public override void SetObjAttbutes(ElementAttbutes attbutes)
        {
            if (ElementsManager.Instance.SelectedElement != this) return;
            base.SetObjAttbutes(attbutes);
            waitTime = attbutes.WaitTime;
            switchTime = attbutes.SwitchTime;
            trafficMode = (TrafficMode)attbutes.lightMode;
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
            CanScale = false;
            CanDrag = false;
            CanDelete = false;
            ltl = logicObject.GetComponent<LogicTrafficLight>();
            trafficLightGroupA = transform.GetChild(0).GetComponentsInChildren<ITrafficLight>();
            trafficLightGroupB = transform.GetChild(1).GetComponentsInChildren<ITrafficLight>();
            SetLights();
            UpdateElementAttributes();
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

        public override void ElementReset()
        {
            base.ElementReset();
            SetObjAttbutes(objAttbutes);
        }
    }
}
