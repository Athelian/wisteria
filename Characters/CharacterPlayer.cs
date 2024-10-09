using Godot;

public partial class CharacterPlayer : Character
{
    [Export] public new int Speed { get; set; } = 300;
    [Export] private NodePath inventoryUIPath;
    [Export] private NodePath spritePath;
    private CanvasLayer inventoryUI;

    public override void _Ready()
    {
        base._Ready();
        inventoryUI = GetNode<CanvasLayer>(inventoryUIPath);
        inventoryUI.Visible = false;
        UpdateInventoryUI();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_inventory"))
        {
            inventoryUI.Visible = !inventoryUI.Visible;
        }
        if (@event.IsActionPressed("interact"))
        {
            Interact();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
        Velocity = inputDirection * Speed;

        MoveAndSlide();
        AfterPhysics();
    }

    public override void AddToInventory(Item item)
    {
        base.AddToInventory(item);
        UpdateInventoryUI();
    }

    public override void RemoveFromInventory(Item item)
    {
        base.RemoveFromInventory(item);
        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        GridContainer inventoryGrid = inventoryUI.GetNode<GridContainer>("./CenteredContainer/GridContainer");

        Utils.ClearChildren(inventoryGrid);

        for (int i = 0; i < Constants.INVENTORY_SIZE; i++)
        {
            Item item = i < inventory.Count ? inventory[i] : null;
            if (item != null)
            {
                TextureButton textureButton = new TextureButton();
                inventoryGrid.AddChild(textureButton);
                textureButton.TextureNormal = item.sprite.Texture;
            }
            else
            {
                Button button = new Button();
                button.CustomMinimumSize = new Vector2(64, 64);
                inventoryGrid.AddChild(button);
            }
        }
    }

    public void Interact()
    {
        // Create a ray to detect objects in front of the player (for example)
        var spaceState = GetWorld2D().DirectSpaceState;
        var rayOrigin = GlobalPosition;
        var currentDirection = GetNode<Sprite2D>(spritePath).GlobalRotation;
        var rayEnd = GlobalPosition + new Vector2(1, 0).Rotated(currentDirection) * 64;

        // Perform the raycast
        var result = spaceState.IntersectRay(PhysicsRayQueryParameters2D.Create(rayOrigin, rayEnd));

        if (result != null && result.ContainsKey("collider"))
        {
            var collider = (Node2D)result["collider"];

            // Check if the object is in the "interactable" group and call its interaction method
            if (collider.IsInGroup("interactable"))
            {
                collider.Call("Interact");
                GD.Print("Interacted with " + collider.Name);
            }
        }
    }
}
