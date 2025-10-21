using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wigi.Actions
{
    public interface IActionUI
    {
        public abstract bool IsActionEnable();
        public abstract void DoShowUI();
        public abstract void DoHideUI();
    }
}
