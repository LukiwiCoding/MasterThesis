public class BookShelfRiddleComponent : InteractableObject
{
    private BookShelfRiddle parent = null;
    private bool isPulled = false;
    public bool IsPulled { get { return isPulled; } }

    public void SetIsPulled(bool isPulled) => this.isPulled = isPulled;
    public void Awake()
    {
        parent = transform.parent.GetComponent<BookShelfRiddle>();
    }
    public override void Interact()
    {
        parent.InteractionServerRPC(ObjectID);
    }
}
