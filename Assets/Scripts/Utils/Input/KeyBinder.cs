using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyBinder
{
    private readonly KeyListener _keyListener;
    private Dictionary<InputTypes, Dictionary<KeyCode, UnityAction>> _keyCommands = new Dictionary<InputTypes, Dictionary<KeyCode, UnityAction>>();

    public KeyBinder(KeyListener keyListener)
    {
        _keyListener = keyListener;

        _keyListener.KeyPressed += ExecuteCommandOnKeyPressed;
        _keyListener.KeyUp += ExecuteCommandOnKeyUp;
        _keyListener.KeyDown += ExecuteCommandOnKeyDown;
    }

    ~KeyBinder()
    {
        _keyListener.KeyPressed -= ExecuteCommandOnKeyPressed;
        _keyListener.KeyUp -= ExecuteCommandOnKeyUp;
        _keyListener.KeyDown -= ExecuteCommandOnKeyDown;
    }

    private void ExecuteCommandOnKeyPressed(KeyCode keyCode) => ExecuteCommand(InputTypes.KeyPressed, keyCode);

    private void ExecuteCommandOnKeyUp(KeyCode keyCode) => ExecuteCommand(InputTypes.KeyUp, keyCode);

    private void ExecuteCommandOnKeyDown(KeyCode keyCode) => ExecuteCommand(InputTypes.KeyDown, keyCode);

    private void ExecuteCommand(InputTypes inputType, KeyCode keyCode)
    {
        if (_keyCommands.ContainsKey(inputType))
        {
            if (_keyCommands[inputType].ContainsKey(keyCode))
            {
                _keyCommands[inputType][keyCode]();
            }
        }
    }

    public void BindCommand(InputTypes inputType, KeyCode keyCode, UnityAction command)
    {
        if (_keyCommands.ContainsKey(inputType))
        {
            if (_keyCommands[inputType].ContainsKey(keyCode))
            {
                _keyCommands[inputType][keyCode] = command;
            }
            else
            {
                _keyCommands[inputType].Add(keyCode, command);
            }
        }
        else
        {
            _keyCommands.Add(inputType, new Dictionary<KeyCode, UnityAction>());
            _keyCommands[inputType].Add(keyCode, command);
        }
    }

    public void UnbindAllCommands()
    {
        _keyCommands.Clear();
    }
}
