using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyListener : MonoBehaviour
{
    public event UnityAction<KeyCode> KeyPressed;
    public event UnityAction<KeyCode> KeyUp;
    public event UnityAction<KeyCode> KeyDown;
    public event UnityAction AnyKeyPressed;

    private IList<KeyCode> _keys;
    private KeyCode _lastKey;

    private void Awake()
    {
        _keys = (IList<KeyCode>)Enum.GetValues(typeof(KeyCode));
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            AnyKeyPressed?.Invoke();

            foreach (KeyCode key in _keys)
            {
                if (Input.GetKey(key))
                {
                    KeyPressed?.Invoke(key);
                }

                if (Input.GetKeyDown(key))
                {
                    KeyDown?.Invoke(key);

                    _lastKey = key;
                    break;
                }
            }
        }

        if(_lastKey != KeyCode.None)
        {
            if (Input.GetKeyUp(_lastKey))
            {
                KeyUp?.Invoke(_lastKey);

                _lastKey = KeyCode.None;
            }
        }
    }
}
