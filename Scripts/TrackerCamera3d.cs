using Godot;
using System;

public partial class TrackerCamera3d : Camera3D
{
    [Export] Node3D trackedTarget;

    public override void _Process(double delta)
    {
        LookAt(trackedTarget.GlobalPosition, Vector3.Up);
    }
}// EOF CLASS

