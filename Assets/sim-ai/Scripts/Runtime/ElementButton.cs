﻿#region License
/*
 * Copyright (c) 2018 AutoCore
 */
#endregion
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.simai
{
    public class ElementButton : Button
    {
        public GameObject elementObj;
        protected override void Start()
        {
            GetComponent<Button>()?.onClick.AddListener(delegate ()
            {
                if (!ElementsManager.Instance.IsInEdit) ElementsManager.Instance.SelectedElement = elementObj.GetComponent<ElementObject>();
            });
        }
        protected override void OnDestroy()
        {
            if (elementObj != null) Destroy(elementObj);
        }
    }
}
