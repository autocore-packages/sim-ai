﻿#region License
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



using Assets.Scripts.SimuUI;

namespace Assets.Scripts.Element
{

    public class ObjTestCar : ElementObject
    {
        public TrafficLight CurrentTL { get; set; }

        public override ElementAttbutes GetObjAttbutes()
        {
            return new ElementAttbutes
            {
                attributes = new bool[8] { true, true ,false, false, false, false, false, false },
                name = transform.name,
                pos = transform.position,
                rot = transform.rotation.eulerAngles.y,
                canDelete = CanDelete
            };
        }
        public override void SetObjAttbutes(ElementAttbutes attbutes)
        {
            if (ElementsManager.Instance.SelectedElement != this) return;
            base.SetObjAttbutes(attbutes);
            transform.position = attbutes.pos;
        }
        protected override void Start()
        {
            nameLogic = "BlueCarLogic";
            base.Start();
            CanScale = false;
            CanDrag = false;
            CanDelete = false;
        }
        protected override void Update()
        {
            base.Update();
        }
        public override void ElementReset()
        {
            base.ElementReset();
        }
    }
}