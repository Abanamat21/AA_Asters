using UnityEngine;

namespace Assets.Scripts.Exstentions
{
    static class ObjectCooperationExstentions
    {
        public static void WallEnteringLineCast(this GameObject gameObject, Vector3 fromPos, Vector3 toPos)
        {
            RaycastHit hit;
            if (Physics.Linecast(fromPos, toPos, out hit))
            {
                if (hit.collider.gameObject.TryGetComponent(out IWall wall))
                {
                    wall.WarpIt(gameObject);
                }
            }
        }
    }
}
