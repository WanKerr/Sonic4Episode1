using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

class MonoGameController : Controller
{
    public int index;
    private bool doVibrate = true;
    private KeyboardController keyboardController;

    public MonoGameController(int index)
    {
        this.index = index;

        if (this.index == 0)
        {
            this.keyboardController = new KeyboardController();
        }
    }

    public override void SetVibrationEnabled(bool enabled)
    {
        this.doVibrate = enabled;
    }

    public override void SetVibration(ushort left, ushort right)
    {
        if (this.doVibrate)
            GamePad.SetVibration(index, (float)left / ushort.MaxValue, (float)right / ushort.MaxValue);
    }

    public override void UpdateControllerReading(ref ControllerReading reading)
    {
        var state = GamePad.GetState(index, GamePadDeadZone.Circular);

        if (this.keyboardController != null)
        {
            keyboardController.UpdateControllerReading(ref reading);
        }

        reading.alx = (short)(state.ThumbSticks.Left.X * short.MaxValue);
        reading.aly = (short)(state.ThumbSticks.Left.Y * short.MaxValue);

        reading.arx = (short)(state.ThumbSticks.Right.X * short.MaxValue);
        reading.ary = (short)(state.ThumbSticks.Right.Y * short.MaxValue);

        if (state.Buttons.A == ButtonState.Pressed)
            reading.direction |= (ControllerConsts.JUMP_BUTTON | ControllerConsts.CONFIRM | ControllerConsts.A);
        if (state.Buttons.B == ButtonState.Pressed)
            reading.direction |= (ControllerConsts.JUMP_BUTTON | ControllerConsts.CANCEL | ControllerConsts.B);

        if (state.Buttons.Y == ButtonState.Pressed)
            reading.direction |= (ControllerConsts.SUPER_SONIC | ControllerConsts.Y);
        if (state.Buttons.X == ButtonState.Pressed)
            reading.direction |= (ControllerConsts.SUPER_SONIC | ControllerConsts.X);

        if (state.Buttons.LeftShoulder == ButtonState.Pressed)
            reading.direction |= ControllerConsts.L;

        if (state.Buttons.LeftShoulder == ButtonState.Pressed)
            reading.direction |= ControllerConsts.R;

        if (state.Buttons.Start == ButtonState.Pressed)
            reading.direction |= ControllerConsts.START;

        if (state.DPad.Left == ButtonState.Pressed)
            reading.direction |= ControllerConsts.LEFT;
        if (state.DPad.Up == ButtonState.Pressed)
            reading.direction |= ControllerConsts.UP;
        if (state.DPad.Down == ButtonState.Pressed)
            reading.direction |= ControllerConsts.DOWN;
        if (state.DPad.Right == ButtonState.Pressed)
            reading.direction |= ControllerConsts.RIGHT;
    }
}

class ControllerSource
{
    private List<Controller> controllers;
    private bool hasKeyboard = false;
    private int controllerIndex = 0;

    public Controller this[int index] => controllers.ElementAtOrDefault(index);
    public int Count => controllers.Count;

    public ControllerSource()
    {
        controllers = new List<Controller>(4);
        for (int i = 0; i < 4; i++)
        {
            controllers.Add(new MonoGameController(i));
        }
    }

    public void Update()
    {
        foreach (var item in controllers)
        {
            item?.UpdateControllerReading();
        }
    }
}