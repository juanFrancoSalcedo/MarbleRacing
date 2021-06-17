using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyStorageEventHandler : MonoBehaviour
{
    [Tooltip("otherwise is taken as KeyCache class")]
    [SerializeField] bool useKeyStorage = true;
    public string keyStorage =null;
    public string value = null;
    public UnityEvent onKeyEquals = null;
    public UnityEvent onKeyDontEquals = null;

    private void Start()
    {
        string valueObtained = GetValueConverted();
        if (!useKeyStorage)
        {
            //KeyCache.dictionary.TryGetValue(keyStorage, out valueObtained);

            if (valueObtained == value)
                onKeyEquals?.Invoke();
            else
                onKeyDontEquals?.Invoke();
        }
        else
        {
            if (valueObtained == value)
                onKeyEquals?.Invoke();
            else
                onKeyDontEquals?.Invoke();
        }
    }

    private string GetValueConverted() 
    {
        char lastChar = keyStorage[keyStorage.Length - 1];
        switch(lastChar)
        {
            case 'I':
                return RaceController.Instance.dataManager.GetSpecificKeyInt(System.Type.GetType("KeyStorage").GetField(keyStorage).GetValue(null).ToString()).ToString();
            case 'S':
                return RaceController.Instance.dataManager.GetSpecificKeyString(System.Type.GetType("KeyStorage").GetField(keyStorage).GetValue(null).ToString());
        }
        return null;
    }

    public void SetValueKey(int _value)
    {
        if (useKeyStorage)
            RaceController.Instance.dataManager.SetSpecificKeyInt(keyStorage, _value);
        else
        {
            //if (KeyCache.dictionary.ContainsKey(keyStorage))
            //{
            //    KeyCache.dictionary[keyStorage] = _value.ToString();
            //}

        }
    }

    public void SetValueKey(string _value)
    {
        if (useKeyStorage)
            RaceController.Instance.dataManager.SetSpecificKeyInt(keyStorage, int.Parse(_value));
        else
        {
            //if (KeyCache.dictionary.ContainsKey(keyStorage))
            //{
            //    KeyCache.dictionary[keyStorage] = _value;
            //}

        }
    }
}
