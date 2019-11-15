// ColliderGen2D by iviachupichu.
// 2015.02.10 - Version 1.02

using UnityEngine;
using System.Collections;
using System.Collections.Generic;	// For Lists
using System.Linq;					// For List operations.

public class ColliderGen2D : MonoBehaviour {
	public Camera cam;							// The Collider will be generated from this camera's viewpoint.
	public PhysicsMaterial2D material;			// 
	public bool isTrigger = false;				//
	public bool collideChildren = true;			// If true then this script will recursively instance itself in all children.
	public bool simpleMode = false;				// If true then an orthographic XY with zPlane = 0 is used instead of a camera. Enabling improves speed.
	public bool edgeCollider = false;			// If true then creates an EdgeCollider2D instead of a PolygonCollider2D.
	public bool intersectionOnly = false;		// If true then only keeps the points that are within the bounding box of the 3D mesh.
	public float zPlane = 0f;					// The 2D plane of motion being used. Controls the z-coordinate of the wall being collided to calculate vertices.
	public float cutoffDistance = -1f;			// If a calculated point is within this distance to another point then it is ignored.
	public int roundingDigits = 2;				// This will round the origin/direction of generated rays before casting them.
	private BoxCollider2D wall;					// Rays are drawn from the camera to this wall to calculate collider vertices.
	private Collider2D myCollider;				// Keep a reference to collider so we can update its vertices.
	private GameObject rootObject;				// This will hold all GameObjects created by this script.

	void Start() {
		rootObject = GameObject.Find("ColliderGen2D");// Search for pre-existing root object.
        // Create rootObject if not found.
        if(!rootObject) {
            rootObject = new GameObject();
            rootObject.name = ("ColliderGen2D");
        }
		if (!cam) cam = Camera.main;																// Default to Main Camera.
		if (collideChildren){																		// Recursively add the script to children and copies all preferences.
			for (int i = 0; i < transform.childCount; i++){
				GameObject child = transform.GetChild(i).gameObject;
				if (child.GetComponent<ColliderGen2D>()) { continue; }								// If child already has this script then don't change it.
				copyTo(child); } }
		Transform wallContainer = rootObject.transform.Find("Wall2D"+zPlane.ToString()+"Z");		// Search for existing Wall of correct depth.
		if (wallContainer){ wall = wallContainer.gameObject.GetComponent<BoxCollider2D>(); }		// Use existing wall if found.
		else {																						// Otherwise create a new wall.
			GameObject o = new GameObject(); wallContainer = o.transform;
			wallContainer.parent = rootObject.transform; wallContainer.name = "Wall2D"+zPlane.ToString()+"Z";
			wallContainer.Translate(Vector3.forward*zPlane);
			wall = wallContainer.gameObject.AddComponent<BoxCollider2D>();
			wall.size = new Vector2(2492f,2492f); wall.isTrigger = true; wall.enabled = false; }
		GameObject collider2D = new GameObject();// Create GameObject for the Collider2D.
        collider2D.layer = 8;
		collider2D.transform.parent = rootObject.transform; collider2D.name = transform.name+"2D";
		if (edgeCollider) { myCollider = collider2D.AddComponent<EdgeCollider2D>();}
		else { myCollider = collider2D.AddComponent<PolygonCollider2D>(); }
		myCollider.sharedMaterial = material;
		myCollider.isTrigger = isTrigger;
		ColliderUpdate();
	}
	// Recalculates the vertices of the 2D Collider.
	void ColliderUpdate() {
        List<Vector2> vertices = ProjectVertices(gameObject,cam);							// Draw a line to every vertex and find where it intersects the wall.
		vertices = ConvexHull(vertices);													// Find the minimum polygon that bounds these vertices.
		if (vertices.Count == 0) { myCollider.enabled = false; return; }
		myCollider.enabled = true;
		if (edgeCollider) { ((EdgeCollider2D)myCollider).points = vertices.ToArray();  }
		else { ((PolygonCollider2D)myCollider).points = vertices.ToArray(); }
	}
	void FixedUpdate() { if (Input.GetMouseButtonDown(0)) { ColliderUpdate(); } }			// Debug code to update colliders on mouse click.
	void OnDisable() { if (myCollider) { Destroy(myCollider.gameObject); } }				// Destroy collider on object death. Wall/RootObject are preserved.

	// ProjectVertices projects a 3D mesh into 2D space according to a camera viewport then returns the corresponding 2D vertex positions.
	// Fires a ray from the camera viewport through every vertex on the mesh and records where each ray intersects the 2D wall.
	public List<Vector2> ProjectVertices(GameObject obj, Camera cam)
	{
        List<Vector2> vertices = new List<Vector2>();
		Mesh mesh = getMesh(); if (!mesh) { return vertices; }													// Return early if no mesh is found.
		foreach (Vector3 vertex in mesh.vertices){
			Vector3 transvertex = transform.TransformPoint(vertex);												// Find the position of the vertex in world space.
			if (simpleMode && checkCutoff(vertices,transvertex)) { vertices.Add(transvertex); continue; }		// simpleMode assumes an XY-plane Orthographic camera with zPlane = 0.
			Vector3 viewpoint = cam.WorldToViewportPoint(transvertex);											// Convert world location to viewport position.
			Ray ray = cam.ViewportPointToRay(viewpoint);														// Draw a ray back through this viewport position.
			if (!(roundingDigits < 0)) {																		// Rounds the ray origin/direction by non-negative roundingDigits.
				ray.origin = RoundDec(ray.origin,roundingDigits);
				ray.direction = RoundDec(ray.direction,roundingDigits); }
			wall.enabled = true;																				// Turn the Wall on so we can collide it.
			RaycastHit2D[] hitbin = Physics2D.GetRayIntersectionAll(ray, cam.farClipPlane-cam.nearClipPlane);	// Raycast our ray onto all 2D colliders...
			foreach (RaycastHit2D hit in hitbin) {
				if (hit.transform.name == "Wall2D"+zPlane.ToString()+"Z" && checkCutoff(vertices,hit.point) && checkIntersection(hit.point)) {	// ...make sure we hit the wall and are not within cutoffDistance.
					vertices.Add(hit.point);
				}
			}
			wall.enabled = false;																				// Turn the Wall off so it doesn't bork other scripts.
		}
		return vertices;
	}
	
	// Convex Hull: Given a list of unordered 2D points, returns a clockwise-ordered list of vertices of the bounding polygon.
	public List<Vector2> ConvexHull(List<Vector2> vertices) {
		if (vertices.Count == 0) { return vertices; }			// Return early if no input.
		if (edgeCollider) {										// Special collinearity check for Edge Colliders.
			Vector2 leftBottom = Vector2.one*Mathf.Infinity; Vector2 rightTop = Vector2.one*Mathf.NegativeInfinity;
			for (int c=0;c<vertices.Count+1;c++) {
				if (c == vertices.Count) { List<Vector2> polygon = new List<Vector2>(); polygon.Add(leftBottom); polygon.Add(rightTop); return polygon; }	// Collinear! Return start/end.
				if (!(Mathf.Abs(isLeft(vertices[0],vertices[1],vertices[c])) < 0.01f)) { break; }													// Not collinear. Use actual Convex Hull algorithm.
				if (vertices[c].x < leftBottom.x || (vertices[c].x == leftBottom.x && vertices[c].y < leftBottom.y)) { leftBottom = vertices[c]; }	// Find leftmost point. Lower preferred.
				if (vertices[c].x > rightTop.x || (vertices[c].x == rightTop.x && vertices[c].y > rightTop.y)) { rightTop = vertices[c]; }			// Find rightmost point. Higher preferred.
			}
		}
		return GiftWrapping(vertices);
	}
	// Implementation of the gift-wrapping convex hull algorithm.
	public List<Vector2> GiftWrapping(List<Vector2> vertices)
	{
		List<Vector2> polygon = new List<Vector2>();
		Vector2 pointOnHull = Vector2.right*Mathf.Infinity;
		for (int m=0;m<vertices.Count;m++) { if (vertices[m].x < pointOnHull.x || (vertices[m].x == pointOnHull.x && vertices[m].y < pointOnHull.y)) { pointOnHull = vertices[m]; } }
		Vector2 edgeHold = pointOnHull;
		int i = 0;
		while(true)
		{
			polygon.Add(pointOnHull);
			Vector2 endpoint = vertices[0];
			for (int j = 1; j < vertices.Count; j++) {
				if (endpoint == pointOnHull || isLeft(polygon[i],endpoint,vertices[j]) < 0f) {
					endpoint = vertices[j]; } }
			pointOnHull = endpoint;
			i += 1;
			if (endpoint == polygon[0] && i != 0)
				break;
			if (i > 4984) { Debug.Log("Loop limit reached. Use other algorithm / Enable distance cutoff and edge collider to support collinear surfaces."); break; }
		}
		if (edgeCollider) { polygon.Add(edgeHold); }
		return polygon;
	}
	/*public List<Vector2> GrahamScan(List<Vector2> vertices)
	{
		// To be implemented.
	}*/

	// Utility Functions
	// Draws line from p0 to p1 and returns <0 if p2 is left of line, 0 if on line, and >0 if right of line.
	// Source: http://geomalgorithms.com/a10-_hull-1.html
	public float isLeft(Vector2 p0, Vector2 p1, Vector2 p2)
	{ return (p1.x - p0.x)*(p2.y-p0.y)-(p2.x - p0.x)*(p1.y-p0.y); }
	// Approximate float comparison for Vector2.
	public bool AEquals(Vector2 vec1, Vector2 vec2, float tol)
	{ return Mathf.Abs((vec1-vec2).magnitude)<tol; }
	// Rounds each point in the vector to the specified number of digits.
	public Vector3 RoundDec(Vector3 vec,int digits) {
		float f = Mathf.Pow(10f,(float)digits);
		return new Vector3(Mathf.Round(vec.x*f)/f,Mathf.Round(vec.y*f)/f,Mathf.Round(vec.z*f)/f); }
	// Copies this script to the target GameObject.
	public void copyTo(GameObject obj){
		ColliderGen2D script = obj.AddComponent<ColliderGen2D>();
		script.material = material; script.isTrigger = isTrigger;
		script.simpleMode = simpleMode; script.edgeCollider = edgeCollider; script.intersectionOnly = intersectionOnly;
		script.zPlane = zPlane; script.cutoffDistance = cutoffDistance; script.roundingDigits = roundingDigits; }
	// Returns true if the point is outside cutoffDistance from all other vertices.
	private bool checkCutoff(List<Vector2> vertices, Vector2 point)
	{ return (cutoffDistance < 0f || !vertices.Exists(v => AEquals(point,v,cutoffDistance))); }
	// Checks if a point is contained within the object's axis-aligned-bounding-box.
	private bool checkIntersection(Vector2 point) {
		if (!intersectionOnly) { return true; }
		Vector3 point3d = new Vector3(point.x,point.y,zPlane);
		Bounds bounds = GetComponent<Renderer>().bounds;
		return bounds.Contains(point3d); }
	// Returns the mesh of the parent GameObject.
	private Mesh getMesh() {
		MeshCollider collider = gameObject.GetComponent<MeshCollider>(); if (collider && collider.enabled) { return collider.sharedMesh; }
		SkinnedMeshRenderer renderer = gameObject.GetComponent<SkinnedMeshRenderer>(); if (renderer && renderer.enabled) { return renderer.sharedMesh; }
		MeshFilter filter = gameObject.GetComponent<MeshFilter>(); if (filter) { return filter.sharedMesh; }
		return null; }
}
