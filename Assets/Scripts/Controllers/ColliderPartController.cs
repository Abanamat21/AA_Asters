using UnityEngine;

public class ColliderPartController : MonoBehaviour, IStrikable, IHitable
{
    public void Hit(GameObject projectile, GameObject causer)
    {
        IHitable hitableParent = (IHitable)transform.parent.GetComponent<IHitable>();
        if(hitableParent != null)        
            hitableParent.Hit(projectile, causer);        
        else        
            throw new AAExceptions.ImportentComponentNotFound($"{transform.parent.name} controller not implement IHitable interface");        
    }

    public void Strike(GameObject causer)
    {
        IStrikable hitableParent = (IStrikable)transform.parent.GetComponent<IStrikable>();
        if (hitableParent != null)        
            hitableParent.Strike(causer);        
        else        
            throw new AAExceptions.ImportentComponentNotFound($"{transform.parent.name} controller not implement IStrikable interface");        
    }
}
