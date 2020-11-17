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
using UnityEngine.UI;

namespace Assets.Scripts.simai
{
    public class LogicTrafficLight : LogicObject
    {
        public Image[] ImagesA;
        public Image[] ImagesB;
        public Text textSecond;
        public Vector3 offset;
        public override void Start()
        {
            base.Start();
            if (textSecond == null)
                textSecond = transform.GetComponentInChildren<Text>();
            ImagesA = transform.GetChild(0).GetChild(1).GetComponentsInChildren<Image>();
            ImagesB = transform.GetChild(0).GetChild(2).GetComponentsInChildren<Image>();
            ElementsManager.Instance.OnCameraRotate += SetLogicTextAngle;
        }
        private void SetLogicTextAngle(Vector3 angle)
        {
            textSecond.transform.rotation = Quaternion.Euler(angle + offset);
        }
        public void SetLogicTrafficLight(int mode)
        {
            switch (mode)
            {
                case 0:
                    foreach (var item in ImagesA)
                    {
                        item.color = Color.yellow;
                    }
                    foreach (var item in ImagesB)
                    {
                        item.color = Color.yellow;
                    }
                    break;
                case 1:
                    foreach (var item in ImagesA)
                    {
                        item.color = Color.green;
                    }
                    foreach (var item in ImagesB)
                    {
                        item.color = Color.red;
                    }
                    break;
                case 2:
                    foreach (var item in ImagesA)
                    {
                        item.color = Color.red;
                    }
                    foreach (var item in ImagesB)
                    {
                        item.color = Color.green;
                    }
                    break;
                default:
                    break;
            }
        }
        public void SetLogicText(int second)
        {
            textSecond.text = second.ToString();
        }
    }
}