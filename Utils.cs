using Godot;

public static class Utils
{
    // Method to clear all children from a node
    public static void ClearChildren(Node parentNode)
    {
        if (parentNode == null)
        {
            GD.PrintErr("Parent node is null.");
            return;
        }

        foreach (Node child in parentNode.GetChildren())
        {
            child.QueueFree();
        }
    }
}
