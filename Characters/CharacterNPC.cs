using Godot;

public partial class CharacterNPC : Character
{
    private enum NPCState { Wander };
    private NPCState currentState = NPCState.Wander;

    // Movement settings
    [Export] public int speed = 75;
    // Boundaries of the region
    [Export] public Vector2 regionTopLeft = new Vector2(0, 0);
    [Export] public Vector2 regionBottomRight = new Vector2(1000, 600);

    private Vector2 direction = new Vector2();
    private double timeUntilChange = GD.Randf() * 2.0f + 1;
    private double timeToIdle = GD.Randf() * 2.0f + 1;
    private Vector2 destination = new Vector2();

    public override void _Ready()
    {
        currentState = NPCState.Wander;
        ChooseNewDirection();
    }

    public override void _Process(double delta)
    {
        switch (currentState)
        {
            case NPCState.Wander:
                Wander(delta);
                break;
        }
    }

    private void Wander(double delta)
    {
        timeUntilChange -= delta;

        if (timeUntilChange <= 0.0f)
        {
            Velocity = Vector2.Zero;
            timeToIdle -= delta;
        }

        if (timeToIdle <= 0.0f)
        {
            timeUntilChange = GD.Randf() * 2.0f + 1;
            timeToIdle = GD.Randf() * 2.0f + 1;

            ChooseNewDirection();
            Velocity = direction * speed;
        }

        if (Velocity != Vector2.Zero)
        {
            Vector2 potentialNewPosition = GlobalPosition + direction * speed * (float)delta;

            // Check if the new position is within the boundaries
            if (potentialNewPosition.X < regionTopLeft.X ||
                potentialNewPosition.X > regionBottomRight.X ||
                potentialNewPosition.Y < regionTopLeft.Y ||
                potentialNewPosition.Y > regionBottomRight.Y)
            {
                // Adjust direction to stay within boundaries
                direction = -direction;
                Velocity = direction * speed;
            }
        }

        MoveAndSlide();
    }

    private void ChooseNewDirection()
    {
        if (GD.Randf() < 0.5f)
        {
            direction = new Vector2((float)GD.RandRange(-1.0, 1.0), 0.0f).Normalized();
        }
        else
        {
            direction = new Vector2(0.0f, (float)GD.RandRange(-1.0, 1.0)).Normalized();
        }
    }
}
