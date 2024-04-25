using Microsoft.Xna.Framework.Input;

public class KeyboardController : Controller
{
    public override void SetVibration(ushort left, ushort right) { }
    public override void SetVibrationEnabled(bool enabled) { }

    public override void UpdateControllerReading(ref ControllerReading reading)
    {
        var state = Keyboard.GetState();
        if (state.IsKeyDown(Keys.Up))
            reading.direction |= ControllerConsts.UP;
        if (state.IsKeyDown(Keys.Down))
            reading.direction |= ControllerConsts.DOWN;
        if (state.IsKeyDown(Keys.Left))
            reading.direction |= ControllerConsts.LEFT;
        if (state.IsKeyDown(Keys.Right))
            reading.direction |= ControllerConsts.RIGHT;
        if (state.IsKeyDown(Keys.A))
            reading.direction |= ControllerConsts.A;
        if (state.IsKeyDown(Keys.S))
            reading.direction |= ControllerConsts.B;
        if (state.IsKeyDown(Keys.D))
            reading.direction |= ControllerConsts.X;
        if (state.IsKeyDown(Keys.Q))
            reading.direction |= ControllerConsts.L;
        if (state.IsKeyDown(Keys.W))
            reading.direction |= ControllerConsts.Y;
        if (state.IsKeyDown(Keys.E))
            reading.direction |= ControllerConsts.R;
        if (state.IsKeyDown(Keys.Escape) || state.IsKeyDown(Keys.Enter))
            reading.direction |= ControllerConsts.START;
    }
}