using Godot;
using System;

public partial class ExcavatorInput : Node3D
{
    [Export] float rotationSpeed = 1.0f;
    [Export] float targetDepthSpeed = 1.0f;
    [Export] float targetHeightSpeed = 1.0f;
    [Export] float curlSpeed = 1.0f;
    [Export] float maxBucketAngle = 65.0f;
    [Export] float minBucketAngle = -65.0f;


    [ExportGroup("References")]
    [Export] Node3D bucketIKTarget;
    [Export] Skeleton3D skeleton;
    [Export] Node3D bucketAttachment;

    int bucketBoneIDX;
    Plane levelPlane;
    Vector3 v3Min = new Vector3(float.MinValue,-2.0f,float.MinValue);
    Vector3 v3Max = new Vector3(float.MaxValue,2.7f,float.MaxValue);
    
    float MaxBucketAngle => Mathf.DegToRad(maxBucketAngle);
    float MinBucketAngle => Mathf.DegToRad(minBucketAngle);



    public override void _Ready()
    {
         for (int i = 0; i < skeleton.GetBoneCount(); i++)
        {
            if (skeleton.GetBoneName(i) == "Bucket")
            {
                bucketBoneIDX = i;
                break;
            }
        }
        levelPlane = new Plane(Vector3.Up, GlobalPosition);
        
    }

    public override void _Process(double delta)
    {
        //DoExcavatorInput((float)delta);
    }
    public override void _PhysicsProcess(double delta)
    {
        DoExcavatorInput((float)delta);
    }

    private void DoExcavatorInput(float delta)
    {
        // Rotation
        float rotDelta = rotationSpeed * Input.GetAxis("RotateRight", "RotateLeft");
        RotateY(rotDelta * delta);
        
        // bucket target depth
        float depthDelta = 0.0f;
        float currentDistance = GlobalPosition.DistanceTo(levelPlane.Project(bucketIKTarget.GlobalPosition));
        depthDelta += targetDepthSpeed * Input.GetAxis("TargetOut", "TargetIn");
        if (depthDelta > 0 && currentDistance > 1.0f) 
        {   
            // Move closer
            bucketIKTarget.Position += bucketIKTarget.Basis.Z * depthDelta * delta;
        }
        if(depthDelta < 0 && currentDistance < 6.0f)
        {
            // Move away 
            bucketIKTarget.Position += bucketIKTarget.Basis.Z * depthDelta * delta;
        }

        // bucket target height
        float heightDelta = targetHeightSpeed * Input.GetAxis("TargetDown", "TargetUp");
        bucketIKTarget.Position = (bucketIKTarget.Position + (bucketIKTarget.Basis.Y * heightDelta * delta)).Clamp(v3Min,v3Max);

        // if all but curl delta is zero, snap ik target to bucket
        if(rotDelta == 0.0f && depthDelta == 0.0f && heightDelta == 0.0f)
        {
            bucketIKTarget.GlobalPosition = bucketAttachment.GlobalPosition;
        }

        // Bucket curl
        float curlDelta = curlSpeed * Input.GetAxis("BucketCurlOut", "BucketCurlIn");
        Quaternion qRot = skeleton.GetBonePoseRotation(bucketBoneIDX);
        Quaternion newRot = qRot * new Quaternion(Vector3.Left, curlDelta * delta);
        float currentEulerX = newRot.Normalized().GetEuler().X;

        if (currentEulerX < MaxBucketAngle && currentEulerX > MinBucketAngle)
        {
            skeleton.SetBonePoseRotation(bucketBoneIDX, newRot);
        }
    }
}// EOF CLASS
