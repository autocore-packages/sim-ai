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

using UnityEngine;


namespace Assets.Scripts.simai
{
    public class TrafficLightObj : ElementObject
    {
        public override ElementAttbutes GetObjAttbutes()
        {
            ElementAttbutes ea = new ElementAttbutes(true, false, false, false, false, false, true, CanDelete)
            {
                Name = transform.name,
                LightMode = (int)trafficMode,
                SwitchTime = switchTime,
                WaitTime = waitTime
            };
            return ea;
        }
        public override void SetObjAttbutes(ElementAttbutes attbutes)
        {
            model = attbutes.Model;
            waitTime = attbutes.WaitTime;
            switchTime = attbutes.SwitchTime;
            trafficMode = (TrafficMode)attbutes.LightMode;
        }
        public override bool CanDelete => false;

        public override bool CanDrag => false;

        public override bool CanScale => false;
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

        public override string NameLogic => "TrafficLightLogic";

        protected override void Start()
        {
            base.Start();
            ltl = logicObject.GetComponent<LogicTrafficLight>();
            trafficLightGroupA = transform.GetChild(0).GetComponentsInChildren<ITrafficLight>();
            trafficLightGroupB = transform.GetChild(1).GetComponentsInChildren<ITrafficLight>();
            SetLights();
            UpdateElementAttributes();
        }

        private void Update()
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
