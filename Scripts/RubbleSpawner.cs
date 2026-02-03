using Godot;
using System;

public partial class RubbleSpawner : Node3D
{
    [Export] int maxRubble = 50;
    [Export] float minInterval = 1.0f;
    [Export] float maxInterval = 5.0f;
    [Export] float pushForceOnSpawn = 20.0f;

    [Export] Material red;
    [Export] Material blue;
    [Export] Node3D rubbleContainer;
    [Export] PackedScene prefabRubble;

    float nextSpawnIn = 3.0f;

    int RubbleCount => rubbleContainer.GetChildCount();

    public override void _Process(double delta)
    {
        if (RubbleCount < maxRubble)
        {
            nextSpawnIn -= (float)delta;
            if (nextSpawnIn < 0.0f) { SpawnRubble(); ScheduleNextSpawn(); }
        }
    }

    private void ScheduleNextSpawn()
    {
        nextSpawnIn = (float)GD.RandRange(minInterval, maxInterval);
    }

    private void SpawnRubble()
    {
        RigidBody3D rubble = prefabRubble.Instantiate<RigidBody3D>();
        rubble.GetNode<MeshInstance3D>("MeshInstance3D").SetSurfaceOverrideMaterial(0, RandomMaterial());
        rubble.Transform = Transform;
        rubbleContainer.AddChild(rubble);
        rubble.ApplyImpulse(rubble.GlobalBasis.Z * -pushForceOnSpawn);
    }

    private Material RandomMaterial()
    {
        if (GD.RandRange(0, 100) < 50)
        {
            return blue;
        }
        return red;
    }
}// EOF CLASS
