using UnityEngine;

public class WallController : MonoBehaviour, IWall, IHitable
{
    public GameObject Ground;
    public WallType Type;

    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.gameObject.GetComponent<IWall>() == null)
        //    warpIt(collision.gameObject);
    }

    public void WarpIt(GameObject gameObject)
    {
        Vector3 newPos = gameObject.transform.localPosition;

        switch (Type)
        {
            case WallType.top:
                newPos.y = Ground.transform.localScale.y - gameObject.transform.localPosition.y + 5;
                break;
            case WallType.bottom:
                newPos.y = Ground.transform.localScale.y - gameObject.transform.localPosition.y - 5;
                break;
            case WallType.left:
                newPos.x = Ground.transform.localScale.x - gameObject.transform.localPosition.x - 5;
                break;
            case WallType.right:
                newPos.x = Ground.transform.localScale.x - gameObject.transform.localPosition.x + 5;
                break;
        }

        gameObject.transform.localPosition = newPos;

    }

    public void Hit(GameObject projectile, GameObject causer)
    {
        WarpIt(projectile);
    }
}
