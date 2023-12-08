using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Newronizer
{
    public class BaseButtonAttendant : MonoBehaviour
    {
        protected Button buttonComponent => GetComponent<Button>();

        public void Click() => buttonComponent.onClick?.Invoke();
    }
}