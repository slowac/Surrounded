public interface ILootable
{
    void OnStartLook(); // Called when the player starts looking at an item
    void OnInteract(); // Called when the player interacts with an item
    void OnEndLook(); // Called when the player stops looking at an item
}