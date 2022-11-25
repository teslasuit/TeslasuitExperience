using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Antilatency;

public class AltEnvironment : MonoBehaviour {

    [Tooltip("Is bars will be shown. To contol this behavior at runtime use SetBarsVisible(bool visible) or ToggleBarsVisibility() methods instead of directly apply value to this property")]
    public bool DrawBars;
    [Tooltip("Is contours will be shown. To contol this behavior at runtime use SetContoursVisible(bool visible) or ToggleContoursVisibility() methods instead of directly apply value to this property")]
    public bool DrawContours;
    [Tooltip("Enables offset for contours")]
    public bool EnableContourOffset = false;
    [Tooltip("Offset of contours. Positive value shall move contours inside the playable area and vice versa")]
    public float ContourOffset;

    private const float ContoursHeight = 2.0f;

    private Material _barMaterial;
    private Mesh _barMesh;

    private Transform _contoursTarget;
    private Material _contoursMaterial;
    private string _propName = "_UserPosition";

    private Dictionary<int, List<Vector2>> _contourPoints = new Dictionary<int, List<Vector2>>();
    private List<GameObject> _contours = new List<GameObject>();
    private List<Mesh> _contourMeshes = new List<Mesh>();

    private List<GameObject> _barObjects = new List<GameObject>();

    private readonly Vector3 _barOffset = new Vector3(0.0f, 0.02f, 0.0f);

    private Environment _environment;

    public Environment Environment {
        get {
            if (_environment == null && Application.isPlaying) {
                _environment = Environment.Create();
            }
            return _environment;
        }
    }

    public void ToggleBarsVisibility() {
        SetBarsVisible(!DrawBars);
    }

    public void SetBarsVisible(bool visible) {
        if(DrawBars == visible) { return; }

        DrawBars = visible;

        foreach (var bar in _barObjects) {
            bar.SetActive(DrawBars);
        }
    }

    public void ToggleContoursVisibility() {
        SetContoursVisible(!DrawContours);
    }

    public void SetContoursVisible(bool visible) {
        if (DrawContours == visible) { return; }

        DrawContours = visible;

        foreach (var contour in _contours) {
            contour.SetActive(DrawContours);
        }
    }

    private void Awake() {
        _barMaterial = Resources.Load<Material>("AltTrackingBars");
        if (_contoursMaterial == null) {
            _contoursMaterial = Resources.Load<Material>("AltTrackingBoundaries");
        }
    }

    private Vector3 To3DSpace(Vector2 p) {
        return new Vector3(p.x, 0, p.y);
    }

    private void Start() {
        var barTexture = _barMaterial.GetTexture("_MainTex");
        _barMesh = CreateBarsMesh(barTexture);
    }

    public void SetContoursTarget(Transform target) {
        _contoursTarget = target;
    }

    private void Update() {
        if (Environment != null) {
            if (DrawBars) {
                if (Environment.IsValid) {
                    using (var bars = Environment.GetBars()) {
                        //add more
                        if (bars.Count > _barObjects.Count) {
                            for (var i = _barObjects.Count; i < bars.Count; ++i) {
                                var bar = new GameObject("Bar " + i);
                                var barMeshFilter = bar.AddComponent<MeshFilter>();
                                var barMeshRenderer = bar.AddComponent<MeshRenderer>();
                                barMeshFilter.sharedMesh = _barMesh;
                                barMeshRenderer.sharedMaterial = _barMaterial;
                                _barObjects.Add(bar);
                                bar.transform.SetParent(transform);
                            }
                        }
                        //remove
                        while (_barObjects.Count > bars.Count) {
                            Destroy(_barObjects[0]);
                            _barObjects.RemoveAt(0);
                        }

                        //move bars
                        for (var i = 0; i < bars.Count; ++i) {
                            using (var bar = bars[i]) {
                                var barObject = _barObjects[i];

                                var position = new AltTracking.float2();
                                var direction = new AltTracking.float2();
                                bar.GetPositionAndDirection(ref position, ref direction);
                                barObject.transform.localPosition = To3DSpace(position.ToVector2()) + _barOffset;
                                barObject.transform.localRotation =
                                    Quaternion.FromToRotation(Vector3.right, To3DSpace(direction.ToVector2()));
                            }
                        }
                    }
                } else {
                    
                }
            }

            if (DrawContours) {
                if (Environment.IsValid) {
                    var contoursCount = Environment.GetContoursCount();
                    if (contoursCount > 0) {
                        var contourPoints = new Dictionary<int, List<Vector2>>();

                        for (uint i = 0; i < contoursCount; i++) {

                            var points = Environment.GetContourPointsCount(i);
                            var pointsList = new List<Vector2>((int)points);

                            for (uint j = 0; j < points; j++) {
                                var point = Environment.GetContourPoint(i, j).ToVector2();
                                pointsList.Add(point);
                            }

                            contourPoints.Add((int)i, pointsList);
                        }

                        var equal = true;

                        if (_contourPoints.Keys.Count == contoursCount) {
                            for (var i = 0; i < contourPoints.Keys.Count; i++) {
                                if (_contourPoints[i].Count == contourPoints[i].Count) {
                                    equal = !_contourPoints[i].Except(contourPoints[i]).Any() && equal;
                                } else {
                                    equal = false;
                                }
                            }
                        } else {
                            equal = false;
                        }

                        if (_contoursMaterial != null) {
                            if (!equal) {
                                CreateContoursMesh(contourPoints);
                            }
                        }
                    }
                }
            }

            if (_contoursTarget != null && _contoursMaterial != null) {
                _contoursMaterial.SetVector(_propName, _contoursTarget.position);
            }
        }
    }

    private Mesh CreateBarsMesh(Texture barTexture) {
        var mesh = new Mesh();

        var meshWidth = barTexture.width * 1.5f / (barTexture.width - barTexture.height);
        var meshHeight = meshWidth * barTexture.height / barTexture.width;

        mesh.name = "Bar";

        var vertices = new Vector3[4];
        var uv = new Vector2[vertices.Length];

        vertices[0] = new Vector3(-meshWidth / 2.0f, 0.0f, -meshHeight / 2.0f);
        uv[0] = new Vector2(0.0f, 0.0f);
        vertices[1] = new Vector3(-meshWidth / 2.0f, 0.0f, meshHeight / 2.0f);
        uv[1] = new Vector2(0.0f, 1.0f);
        vertices[2] = new Vector3(meshWidth / 2.0f, 0.0f, meshHeight / 2.0f);
        uv[2] = new Vector2(1.0f, 1.0f);
        vertices[3] = new Vector3(meshWidth / 2.0f, 0.0f, -meshHeight / 2.0f);
        uv[3] = new Vector2(1.0f, 0.0f);

        var triangles = new int[6];

        triangles[0] = 0;
        triangles[3] = triangles[2] = 3;
        triangles[4] = triangles[1] = 1;
        triangles[5] = 2;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        return mesh;
    }

    private void CreateContoursMesh(Dictionary<int, List<Vector2>> contourPoints) {
        if (EnableContourOffset) {
            ApplyContourOffset(ref contourPoints);
        }

        //add more
        if (contourPoints.Keys.Count > _contours.Count) {
            for (var i = _contours.Count; i < contourPoints.Keys.Count; ++i) {

                var contour = new GameObject("Contour #" + i);
                var meshFilter = contour.AddComponent<MeshFilter>();

                var mesh = new Mesh();
                _contourMeshes.Add(mesh);
                meshFilter.mesh = mesh;
                
                var rnd = contour.AddComponent<MeshRenderer>();
                rnd.sharedMaterial = _contoursMaterial;
                _contours.Add(contour);
                contour.transform.SetParent(transform);
                contour.transform.localRotation = Quaternion.identity;
                contour.transform.localPosition = Vector3.zero;
            }
        }

        //remove
        while (_contours.Count > contourPoints.Keys.Count) {
            Destroy(_contourMeshes[0]);
            _contourMeshes.RemoveAt(0);
            Destroy(_contours[0]);
            _contours.RemoveAt(0);
        }

        //generate borders
        for (var i = 0; i < contourPoints.Keys.Count; i++) {
            _contourMeshes[i].name = "Contour #" + i + " mesh";

            var vertices = new List<Vector3>();
            var uvs = new List<Vector2>();
            var uvs2 = new List<Vector2>();
            var normals = new List<Vector3>();
            var tris = new List<int>();

            for (var j = 0; j < contourPoints[i].Count; ++j) {
                var pointA = contourPoints[i][j];
                var pointB = contourPoints[i][(j + 1) % contourPoints[i].Count];
                ConstructQuad(pointA, pointB, ref vertices, ref uvs, ref uvs2, ref normals, ref tris);
            }

            _contourMeshes[i].vertices = vertices.ToArray();
            _contourMeshes[i].uv = uvs.ToArray();
            _contourMeshes[i].uv2 = uvs2.ToArray();
            _contourMeshes[i].triangles = tris.ToArray();
            _contourMeshes[i].normals = normals.ToArray();
        }

        _contourPoints = contourPoints;
    }

    private void ConstructQuad(Vector2 pointA, Vector2 pointB, ref List<Vector3> vertices, ref List<Vector2> uvs, ref List<Vector2> uvs2, ref List<Vector3> normals, ref List<int> tris) {
        var firstVertex = vertices.Count;

        vertices.Add(new Vector3(pointA.x, 0.0f, pointA.y));
        vertices.Add(new Vector3(pointA.x, ContoursHeight, pointA.y));
        vertices.Add(new Vector3(pointB.x, 0.0f, pointB.y));
        vertices.Add(new Vector3(pointB.x, ContoursHeight, pointB.y));

        var normal = Vector3.Cross(vertices[firstVertex + 1] - vertices[firstVertex], vertices[firstVertex + 2] - vertices[firstVertex]).normalized;
        for (var i = 0; i < 4; ++i) {
            normals.Add(normal);
        }

        var quadLength = Vector3.Distance(vertices[firstVertex], vertices[firstVertex + 2]);
        var uvOffset = quadLength / ContoursHeight;

        uvs.Add(new Vector2(0.0f, 0.0f));
        uvs.Add(new Vector2(0.0f, 1.0f));
        uvs.Add(new Vector2(uvOffset, 0.0f));
        uvs.Add(new Vector2(uvOffset, 1.0f));

        uvs2.Add(new Vector2(quadLength, ContoursHeight));
        uvs2.Add(new Vector2(quadLength, -ContoursHeight));
        uvs2.Add(new Vector2(-quadLength, ContoursHeight));
        uvs2.Add(new Vector2(-quadLength, -ContoursHeight));

        tris.AddRange(new int[3] { firstVertex, firstVertex + 1, firstVertex + 3 });
        tris.AddRange(new int[3] { firstVertex, firstVertex + 3, firstVertex + 2 });
    }

    private void ApplyContourOffset(ref Dictionary<int, List<Vector2>> contourPoints) {
        for (int i = 0; i < contourPoints.Count; i++) {
            var movedPoints = new List<Vector2>(contourPoints[i]);
            for (int j = 0; j < contourPoints[i].Count; j++) {
                var p1 = contourPoints[i][j];
                var p2 = contourPoints[i][(j + 1) % contourPoints[i].Count];
                var p3 = contourPoints[i][(j + 2) % contourPoints[i].Count];

                var v1 = (p2 - p1).normalized;
                v1 = new Vector2(v1.y, -v1.x);
                var v2 = (p3 - p2).normalized;
                v2 = new Vector2(v2.y, -v2.x);

                var p1Moved = p1 + v1 * ContourOffset;
                var p2Moved = p2 + v1 * ContourOffset;
                var p3Moved = p2 + v2 * ContourOffset;
                var p4Moved = p3 + v2 * ContourOffset;

                var result = new Vector3(p2.x, 0.0f, p2.y);

                GetLinesIntersection(p1Moved, p2Moved, p3Moved, p4Moved, ref result);
                movedPoints[(j + 1) % contourPoints[i].Count] = result;

            }
            contourPoints[i] = movedPoints;
        }
    }

    private bool GetLinesIntersection(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, ref Vector3 result) {
        if (Vector3.Angle(v2 - v1, v4 - v3) < 0.01f) {
            return false;
        }

        var p1 = new Vector3(v2.x - v1.x, v2.y - v1.y, v2.z - v1.z);
        var p2 = new Vector3(v4.x - v3.x, v4.y - v3.y, v4.z - v3.z);

        var x = (v1.x * p1.y * p2.x - v3.x * p2.y * p1.x - v1.y * p1.x * p2.x + v3.y * p1.x * p2.x) / (p1.y * p2.x - p2.y * p1.x);
        var y = (v1.y * p1.x * p2.y - v3.y * p2.x * p1.y - v1.x * p1.y * p2.y + v3.x * p1.y * p2.y) / (p1.x * p2.y - p2.x * p1.y);
        var z = (v1.z * p1.y * p2.z - v3.z * p2.y * p1.z - v1.z * p1.z * p2.z + v3.y * p1.z * p2.z) / (p1.y - p2.z - p2.y * p1.z);

        result = new Vector3(x, y, z);

        return true;
    }

    private void OnDestroy() {
        if (_environment != null) {
            _environment.Dispose();
            _environment = null;
        }
    }
}
