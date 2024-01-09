using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct BasicPlayer : IComponentData
{
    public Entity ControlledCharacter;
    public Entity ControlledCamera;
    public double lastTime;
    public double fireInterval;
}

[Serializable]
public struct BasicPlayerInputs : IComponentData
{
    public float2 MoveInput;
    public float2 CameraLookInput;
    public float CameraZoomInput;
    public FixedInputEvent JumpPressed;
    public bool aimPressed;
    public FixedInputEvent primaryPressed;
    public FixedInputEvent secondaryPressed;
    public FixedInputEvent handPressed;
    public bool firePressed;
}
