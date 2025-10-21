using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wigi.Actions
{
    public static class DOActionUtils
    {
        public static DOActionBase GetDOActionByLabel(Component component, string label)
        {
            var listAction = component.GetComponents<DOActionBase>();
            foreach (var action in listAction)
            {
                if (action.LabelAction == label)
                    return action;
            }
            return null;
        }

        public static Component GetTargetDOAction(Object obj, ActionBase action)
        {
            if (obj is GameObject) {
                var listDO = ((GameObject)obj).GetComponents<DOActionBase>();
                foreach (var actionDO in listDO)
                {
                    if (actionDO.HasAction(action))
                        return actionDO.target;
                }
            }
            return null;
        }
    }
}