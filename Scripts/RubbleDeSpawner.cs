using Godot;
using System;

public partial class RubbleDeSpawner : Area3D
{
    [Export] bool red = false;
    [Export] bool blue = false;
    public override void _Ready()
    {
        BodyEntered += WhenBodyEntered;
    }

    private void WhenBodyEntered(Node3D body)
    {
        if(body is RigidBody3D rb)
        {
            DeSpawn(rb);
        }
    }

    private void DeSpawn(RigidBody3D rb)
    {
        bool accepted = false;
        string path = rb.GetNode<MeshInstance3D>("MeshInstance3D").GetSurfaceOverrideMaterial(0).ResourcePath;
        if(!accepted && red && path.Contains("Red")){accepted = true; UIPointCounter.AddRed();}
        if(!accepted && blue && path.Contains("Blue")){accepted = true; UIPointCounter.AddBlue();}
        if(!accepted){ UIPointCounter.AddErr();}
        rb.QueueFree();
    }
}// EOF CLASS
