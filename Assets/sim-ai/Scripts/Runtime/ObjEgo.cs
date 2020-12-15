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

    public class ObjEgo : ElementObject
    {
        public ITrafficLight CurrentTL { get; set; }

        public override bool CanDelete => false;

        public override bool CanDrag => false;

        public override bool CanScale => false;

        public override GameObject LogicPrefab
        {
            get
            {
                var logic = ElementsManager.Instance.EgoModels[model].LogicPrefab;
                if (logic == null) Debug.Log("No logic");
                return logic;
            }
        }

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
            transform.position = attbutes.TransformData.V3Pos.GetVector3();
            transform.rotation = Quaternion.Euler(attbutes.TransformData.V3Pos.GetVector3());
        }
        protected override void Start()
        {
            base.Start();
        }
        public override void ElementReset()
        {
            base.ElementReset();
        }
    }
}