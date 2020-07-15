using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour, IWall, IHitable
{
    public GameObject ground;

    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.gameObject.GetComponent<IWall>() == null)
        //    warpIt(collision.gameObject);
    }

    public void warpIt(GameObject gameObject)
    {
        Vector3 newPos = gameObject.transform.localPosition;
        switch (name)
        {
            case "TopWall":
                newPos.y = ground.transform.localScale.y - gameObject.transform.localPosition.y + 5;
                break;
            case "BottomWall":
                newPos.y = ground.transform.localScale.y - gameObject.transform.localPosition.y - 5;
                break;
            case "RightWall":
                newPos.x = ground.transform.localScale.x - gameObject.transform.localPosition.x + 5;
                break;
            case "LeftWall":
                newPos.x = ground.transform.localScale.x - gameObject.transform.localPosition.x - 5;
                break;
        }

        gameObject.transform.localPosition = newPos;

    }

    public void hit(GameObject projectile, GameObject causer)
    {
        warpIt(projectile);
    }
}
