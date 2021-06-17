using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseButtonComponent : MonoBehaviour
{
    protected Button buttonComponent => GetComponent<Button>();
}
