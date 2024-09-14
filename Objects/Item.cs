using Godot;
using System;

public partial class Item : Area2D
{
    [Export] public float MoveSpeed = 200.0f;
    private CharacterPlayer targetBody;
    public Sprite2D sprite;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;

        sprite = GetNode<Sprite2D>("Sprite2D");

#if DEBUG
        if (sprite == null)
        {
            throw new Exception("Critical error: Sprite2D node not found.");
        }
#else
        if (sprite == null)
        {
            GD.Print("Critical error: Sprite2D node not found.");
        }
#endif
    }

    private void OnBodyEntered(Node body)
    {
        if (body is CharacterPlayer)
        {
            GD.Print("CharacterPlayer entered the area");
            targetBody = (CharacterPlayer)body;
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body is CharacterPlayer)
        {
            GD.Print("CharacterPlayer exited the area");
            targetBody = null;
        }
    }

    public override void _Process(double delta)
    {
        if (targetBody != null && sprite != null)
        {
            Vector2 spritePosition = sprite.GlobalPosition;
            Vector2 targetPosition = targetBody.GlobalPosition;

            if (spritePosition.DistanceTo(targetPosition) < 20)
            {
                targetBody.AddToInventory(this);
                QueueFree();
            }

            sprite.GlobalPosition = spritePosition.MoveToward(targetPosition, MoveSpeed * (float)delta);
        }
    }
}
