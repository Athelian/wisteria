using Godot;

public partial class CharacterNPC : Character
{
    double timeStationary = 0.0f;

    public override void _Process(double delta)
    {
        if (timeStationary > 1.0f)
        {
            Velocity = new Vector2(1, 0) * Speed;
            timeStationary = 0.0f;
        }
        else
        {
            timeStationary += delta;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
        AfterPhysics();
    }
}
