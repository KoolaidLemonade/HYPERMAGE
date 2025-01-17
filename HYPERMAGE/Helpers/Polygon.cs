using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.Helpers
{
    // from https://github.com/tedigc/SeparatingAxisTheorem modified with a few additional methods 
    // allows for things like hitbox rotation and circle hitboxes
    public class Shape
    {
        public bool IntersectsWith(Shape other)
        {
            if (GetType() == typeof(Polygon) && other.GetType() == typeof(Polygon))
            {
                return Intersect((Polygon)this, (Polygon)other);
            }

            if (GetType() == typeof(Polygon) && other.GetType() == typeof(Circle))
            {
                return Intersect((Polygon)this, (Circle)other);
            }

            if (GetType() == typeof(Circle) && other.GetType() == typeof(Polygon))
            {
                return Intersect((Polygon)other, (Circle)this);
            }

            if (GetType() == typeof(Circle) && other.GetType() == typeof(Circle))
            {
                return Intersect((Circle)this, (Circle)other);
            }

            Console.WriteLine("Warning: shape types not recognised");
            return false;
        }

        private static bool Intersect(Circle circle1, Circle circle2)
        {
            Vector2 dv = circle2.GetPosition() - circle1.GetPosition();
            return dv.X * dv.X + dv.Y * dv.Y <= MathF.Pow(circle1.GetRadius() + circle2.GetRadius(), 2);
        }

        private static bool Intersect(Polygon polygon, Circle circle)
        {
            List<Vector2> normals = new List<Vector2>();
            normals.AddRange(polygon.GetEdgeNormals());
            normals.Add(GetPolygonCircleAxis(polygon, circle));
            foreach (Vector2 axis in normals)
            {
                var (min1, max1) = GetMinMaxProjections(polygon, axis);
                var (min2, max2) = GetMinMaxProjections(circle, axis);
                float intervalDistance = min1 < min2 ? min2 - max1 : min1 - max2;
                if (intervalDistance >= 0) return false;
            }
            return true;
        }

        private static bool Intersect(Polygon polygon1, Polygon polygon2)
        {
            List<Vector2> normals = new List<Vector2>();
            normals.AddRange(polygon1.GetEdgeNormals());
            normals.AddRange(polygon2.GetEdgeNormals());
            foreach (Vector2 axis in normals)
            {
                var (min1, max1) = GetMinMaxProjections(polygon1, axis);
                var (min2, max2) = GetMinMaxProjections(polygon2, axis);
                float intervalDistance = min1 < min2 ? min2 - max1 : min1 - max2;
                if (intervalDistance >= 0) return false;
            }
            return true;
        }

        private static (float, float) GetMinMaxProjections(Polygon polygon, Vector2 axis)
        {
            float min = int.MaxValue;
            float max = int.MinValue;
            foreach (Vector2 vertex in polygon.GetVertices())
            {
                Vector2 projection = Project(vertex, axis);
                float scalar = Scalar(projection, axis);
                if (scalar < min) min = scalar;
                if (scalar > max) max = scalar;
            }
            return (min, max);
        }

        private static (float, float) GetMinMaxProjections(Circle circle, Vector2 axis)
        {
            Vector2 v1 = circle.GetPosition() - Vector2.Normalize(axis) * circle.GetRadius();
            Vector2 v2 = circle.GetPosition() + Vector2.Normalize(axis) * circle.GetRadius();
            Vector2 p1 = Project(v1, axis);
            Vector2 p2 = Project(v2, axis);
            float s1 = Scalar(p1, axis);
            float s2 = Scalar(p2, axis);
            return s1 > s2 ? (s2, s1) : (s1, s2);
        }

        private static Vector2 Project(Vector2 vertex, Vector2 axis)
        {
            float dot = Vector2.Dot(vertex, axis);
            float mag2 = axis.LengthSquared();
            return dot / mag2 * axis;
        }

        private static float Scalar(Vector2 vertex, Vector2 axis)
        {
            return Vector2.Dot(vertex, axis);
        }

        private static Vector2 GetPolygonCircleAxis(Polygon polygon, Circle circle)
        {
            Vector2 nearestVertex = FindClosestVertex(polygon, circle.GetPosition());
            Vector2 axis = circle.GetPosition() - nearestVertex;
            Vector2 perp = new Vector2(axis.Y, -axis.X);
            return perp;
        }

        private static Vector2 FindClosestVertex(Polygon polygon, Vector2 vertex)
        {
            float shortestDistance = int.MaxValue;
            Vector2 closestVertex = polygon.GetVertex(0);
            foreach (Vector2 polygonVertex in polygon.GetVertices())
            {
                float currentDistance = Vector2.DistanceSquared(vertex, polygonVertex);
                if (currentDistance < shortestDistance)
                {
                    closestVertex = polygonVertex;
                    shortestDistance = currentDistance;
                }
            }
            return closestVertex;
        }

    }
    public static class PolygonFactory
    {

        public static Polygon CreateRectangle(int x, int y, int width, int height)
        {
            return CreateRectangle(x, y, width, height, 0);
        }

        public static Polygon CreateRectangle(int x, int y, int width, int height, float angle)
        {
            Vector2 origin = new Vector2(width * .5f, height * .5f);
            return CreateRectangle(x, y, width, height, angle, origin);
        }

        public static Polygon CreateRectangle(int x, int y, int width, int height, float angle, Vector2 origin)
        {
            return new Polygon(
                new[] {
                new Vector2(x, y),
                new Vector2(x + width, y),
                new Vector2(x + width, y + height),
                new Vector2(x, y + height)
                },
                origin,
                angle
            );
        }

    }

    public class Polygon : Shape
    {

        private readonly Vector2[] vertices;
        private float angle;
        private Vector2 originalPosition;
        private readonly Vector2 origin;
        private readonly int edgeCount;

        public Polygon(Vector2[] vertices, Vector2 origin) : this(vertices, origin, 0) { }
        public Polygon(Vector2[] vertices, Vector2 origin = new Vector2(), float angle = 0)
        {
            this.origin = origin;
            this.vertices = vertices.Select(vertex => vertex - origin).ToArray();
            originalPosition = vertices[0];
            edgeCount = vertices.Length;
            SetAngle(angle);
        }

        public List<float> GetVerticesX()
        {
            List<float> floats = [];

            for (int i = 0; i < vertices.Length; i++)
            {
                floats.Add(vertices[i].X);
            }

            return floats;
        }
        public List<float> GetVerticesY()
        {
            List<float> floats = [];

            for (int i = 0; i < vertices.Length; i++)
            {
                floats.Add(vertices[i].Y);
            }

            return floats;
        }
        public Vector2[] GetVertices()
        {
            return vertices;
        }

        public Vector2 GetVertex(int index)
        {
            return vertices[index];
        }

        public Vector2 GetEdge(int index)
        {
            Vector2 v1 = vertices[index];
            Vector2 v2 = vertices[(index + 1) % vertices.Length];
            return v1 - v2;
        }

        public Vector2 GetEdgeNormal(int index)
        {
            Vector2 edge = GetEdge(index);
            return new Vector2(edge.Y, -edge.X);
        }

        public List<Vector2> GetEdges()
        {
            List<Vector2> edges = new List<Vector2>();
            for (int i = 0; i < edgeCount; i++)
            {
                edges.Add(GetEdge(i));
            }
            return edges;
        }

        public List<Vector2> GetEdgeNormals()
        {
            List<Vector2> normals = new List<Vector2>();
            for (int i = 0; i < edgeCount; i++)
            {
                normals.Add(GetEdgeNormal(i));
            }
            return normals;
        }

        public void SetPosition(Vector2 position)
        {
            Vector2 diff = position - (originalPosition + origin);

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] += diff;
            }

            originalPosition += diff;
        }

        public void SetAngle(float angle)
        {
            float diff = angle - angle;
            this.angle = angle;

            Vector2 offset = originalPosition + origin;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = Vector2.Transform(vertices[i] - offset, Matrix.CreateRotationZ(diff)) + offset;
            }
        }

        public Vector2 GetOrigin()
        {
            return origin;
        }

        public Vector2 GetPosition()
        {
            return originalPosition + origin;
        }

    }

    public class Circle : Shape
    {

        private Vector2 position;
        private float radius;

        public Circle(Vector2 position, float radius)
        {
            this.position = position;
            this.radius = radius;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public float GetRadius()
        {
            return radius;
        }

        public void SetPosition(float x, float y)
        {
            position.X = x;
            position.Y = y;
        }

    }
}
