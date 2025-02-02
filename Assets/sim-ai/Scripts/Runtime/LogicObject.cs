﻿#region License
/*
 * Copyright (c) 2018 AutoCore
 */
#endregion
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class LogicObject : MonoBehaviour
    {
        public ElementObject elementObject;
        public LogicButton logicButton;
        public virtual void Start()
        {
            if (logicButton == null) logicButton = GetComponentInChildren<LogicButton>();
            logicButton.SetButtonObj(elementObject);
        }
    }
}
