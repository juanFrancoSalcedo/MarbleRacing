
using UnityEngine;

namespace Newronizer.HierarchyStates
{
    public static class NavigationRecordControllerExtensions 
    {
        public static StateSetter GetSetterById(this NavigationRecordController controller , StateReference _reference)
        {
            foreach (var item in controller.sceneSetters)
            {
                bool f = (item.reference.UniqueId.Equals(_reference.UniqueId));
            }
            //wholeSetters.ForEach(t => print(t.reference.uniqueId));
            var element = controller.sceneSetters.Find(e => e.reference.UniqueId.Equals(_reference.UniqueId));
            return element;
        }
    }
}
