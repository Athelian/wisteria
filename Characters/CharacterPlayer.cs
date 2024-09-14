using Godot;

public partial class CharacterPlayer : Character
{
    [Export] private NodePath inventoryUIPath;
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
}
