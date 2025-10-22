﻿using System;
using UnityEngine;

namespace Ftech.Lib.Common.UnityInspector.Editor {
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SpriteFieldAttribute : PropertyAttribute {
        public readonly float size;

        public SpriteFieldAttribute(float size = 64f) {
            this.size = size;
        }
    }
}