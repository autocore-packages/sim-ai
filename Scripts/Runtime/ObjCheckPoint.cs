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

using System;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class ObjCheckPoint : ElementObject
    {
        public override ElementAttbutes GetObjAttbutes()
        {
            ElementAttbutes ea = new ElementAttbutes(true, true, false, false, false, false, false, CanDelete)
            {
                Name = transform.name,
                TransformData = new TransformData(transform),
                Model = model
            };
            return ea;
        }
        public override void SetObjAttbutes(ElementAttbutes attbutes)
        {
            model = attbutes.Model;
            TransformData data = attbutes.TransformData;
            transform.position = data.V3Pos.GetVector3();
            transform.rotation = Quaternion.Euler(data.V3Pos.GetVector3());
            transform.localScale = data.V3Sca.GetVector3();
        }
        protected override void Start()
        {
            base.Start();
        }
        public override Vector3 OffsetPos => new Vector3(0, -0.5f * V3Scale.y, 0);

        public override GameObject LogicPrefab
        {
            get
            {
                var logic = ElementsManager.Instance.CheckPointModels[model].LogicPrefab;
                return logic;
            }
        }

        public override bool CanDelete => true;

        public override bool CanDrag => true;

        public override bool CanScale => true;

        public override void ElementReset()
        {
            base.ElementReset(); 
        }
    }
}
