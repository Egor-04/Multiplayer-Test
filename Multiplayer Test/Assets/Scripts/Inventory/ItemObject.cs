using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public Item ItemScriptableObject;

    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}