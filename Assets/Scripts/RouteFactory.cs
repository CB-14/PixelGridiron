using UnityEngine;

public static class RouteFactory
{
    public static Vector3[] GenerateRoute(Vector3 startPos, RouteType type)
    {
        float z = startPos.z; // Keep all route points on same depth

        switch (type)
        {
            case RouteType.Slant:
                return new Vector3[]
                {
                    startPos,
                    new Vector3(startPos.x + 5f, startPos.y, z),
                    new Vector3(startPos.x + 12f, startPos.y + 3f, z)
                };

            case RouteType.Go:
                return new Vector3[]
                {
                    startPos,
                    new Vector3(startPos.x + 12f, startPos.y, z)
                };

            case RouteType.Post:
                return new Vector3[]
                {
                    startPos,
                    new Vector3(startPos.x + 8f, startPos.y, z),
                    new Vector3(startPos.x + 12f, startPos.y + 4f, z)
                };

            case RouteType.Out:
                return new Vector3[]
                {
                    startPos,
                    new Vector3(startPos.x + 6f, startPos.y, z),
                    new Vector3(startPos.x + 6f, startPos.y + 2f, z),
                    new Vector3(startPos.x + 9f, startPos.y + 2f, z)
                };

            default:
                return new Vector3[] { startPos };
        }
    }
}
