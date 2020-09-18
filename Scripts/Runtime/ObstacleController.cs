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


using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class ObstacleController : ElementObject
    {
        public override ElementAttbutes GetObjAttbutes()
        {
            ElementAttbutes ea = new ElementAttbutes(true, true, false, false, false, false, false, CanDelete)
            {
                Name = transform.name,
                TransformData = new TransformData(transform)
            };
            return ea;
        }
        public override void SetObjAttbutes(ElementAttbutes attbutes)
        {
            if (ElementsManager.Instance.SelectedElement != this) return;
            base.SetObjAttbutes(attbutes);
            transform.position = attbutes.TransformData.V3Pos.GetVector3();
            transform.rotation = Quaternion.Euler(attbutes.TransformData.V3Pos.GetVector3());
            transform.localScale = attbutes.TransformData.V3Sca.GetVector3();
        }
        protected override void Start()
        {
            nameLogic = "ObstacleLogic";
            base.Start();
            CanScale = true;
            CanDrag = true;
            CanDelete = true;
        }
        protected override void Update()
        {
            base.Update();
        }
        public override Vector3 OffsetPos => new Vector3(0, 0.5f * v3Scale.y, 0);
        public override void ElementReset()
        {
            base.ElementReset();
        }
    }
}
