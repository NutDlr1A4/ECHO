public interface IInteractable
{
    void OnInteractTargetEnter();
    void OnInteractTargetExit();

    // Returns true if interaction was successfull, false otherwise
    bool OnInteract();
}
