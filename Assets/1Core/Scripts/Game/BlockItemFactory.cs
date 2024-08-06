using UnityEngine;

public class BlockItemFactory : MonoBehaviour
{
    [SerializeField] private Factory<SpriteRenderer> _blockItemFactory;

    public SpriteRenderer Create(Transform target)
    {
        var item = _blockItemFactory.Create(target.position, target.rotation, target);
        return item;
    }
}