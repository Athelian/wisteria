using System;
using System.Collections.Generic;
using Godot;

public abstract partial class Character : CharacterBody2D
{
    [Export] public int Speed { get; set; } = 400;
    public AnimationPlayer animationPlayer;
    public List<Item> inventory = new List<Item>();

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("./Sprite2D/Animation");
        animationPlayer.Play("idle");
    }

    public virtual void AddToInventory(Item item)
    {
        GD.Print("Added to inventory");
        inventory.Add(item);
    }

    public virtual void RemoveFromInventory(Item item)
    {
        GD.Print("Removed from inventory");
        inventory.Remove(item);
    }
}