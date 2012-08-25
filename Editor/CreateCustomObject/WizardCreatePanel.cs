using UnityEditor;
using UnityEngine;
using System.Collections;

public enum NormalDirction
{
	x_plus,
	x_minus,
	y_plus,
	y_minus,
	z_plus,
	z_minus,
};

public class WizardCreatePanel : ScriptableWizard
{
	
	public Texture2D texture;
	public NormalDirction normalDirction = NormalDirction.z_minus;
	public int sectionsX = 1;
	public int sectionsY = 1;
	public float sizeW = 1.0f;
	public float sizeH = 1.0f;

	[MenuItem ("Create Object/Panel")]
	static void Init ()
	{
		ScriptableWizard.DisplayWizard<WizardCreatePanel> ("Create Custom Panel", "Create");
	}
	
	void OnWizardCreate ()
	{
		CreatePanel ();
	}
	
	void CreatePanel ()
	{
		int x, y;
		int idx;
		GameObject newGameobject = new GameObject ("CustomPanel");
		
		MeshRenderer meshRenderer = newGameobject.AddComponent<MeshRenderer> ();
		meshRenderer.material = new Material (Shader.Find ("Diffuse"));
		if (texture != null) {
			meshRenderer.sharedMaterial.mainTexture = texture;
		}
			
		MeshFilter meshFilter = newGameobject.AddComponent<MeshFilter> ();
		
		meshFilter.mesh = new Mesh ();
		Mesh mesh = meshFilter.sharedMesh;
		mesh.name = "CustomPanel";
		
		float triangleW = sizeW / sectionsX;
		float triangleH = sizeH / sectionsY;
		float triangleUSize = 1f / sectionsX;
		float triangleVSize = 1f / sectionsY;
		Vector3[] vertices = new Vector3[(sectionsX + 1) * (sectionsY + 1)];
		Vector2[] uv = new Vector2[(sectionsX + 1) * (sectionsY + 1)];
		for (y=0; y<sectionsY+1; y++) {
			float posY = sizeH * .5f - triangleH * y;
			float posV = 1f - triangleVSize * y;
			for (x=0; x<sectionsX+1; x++) {
				float posX = -sizeW * .5f + triangleW * x;
				float posU = triangleUSize * x;
				idx = (y * (sectionsX + 1) + x);
				vertices [idx] = new Vector3 (posX, posY, 0);
				uv [idx] = new Vector2 (posU, posV);
			}
		}
		Quaternion quateRot = Quaternion.Euler (0.0f, 0.0f, 0.0f);
		switch (normalDirction) {
		case NormalDirction.x_plus:
			quateRot = Quaternion.Euler (0.0f, 270.0f, 0.0f);
			break;
		case NormalDirction.x_minus:
			quateRot = Quaternion.Euler (0.0f, 90.0f, 0.0f);
			break;
		case NormalDirction.y_plus:
			quateRot = Quaternion.Euler (90.0f, 0.0f, 0.0f);
			break;
		case NormalDirction.y_minus:
			quateRot = Quaternion.Euler (270.0f, 0.0f, 0.0f);
			break;
		case NormalDirction.z_plus:
			quateRot = Quaternion.Euler (0.0f, 180.0f, 0.0f);
			break;
		case NormalDirction.z_minus:
			quateRot = Quaternion.Euler (0.0f, 0.0f, 0.0f);
			break;
		}
		for (int v=0; v<vertices.Length; v++) {
			vertices [v] = quateRot * vertices [v];
		}
		mesh.vertices = vertices;
		mesh.uv = uv;
		
		int[] triangles = new int[3 * sectionsX * sectionsY * 2];
		for (y=0; y<sectionsY; y++) {
			for (x=0; x<sectionsX; x++) {
				idx = ((y * sectionsX) + x) * 6;
				int p0 = y * (sectionsX + 1) + x;
				int p1 = y * (sectionsX + 1) + x + 1;
				int p2 = (y + 1) * (sectionsX + 1) + x;
				int p3 = (y + 1) * (sectionsX + 1) + x + 1;
				triangles [idx + 0] = p0;
				triangles [idx + 1] = p1;
				triangles [idx + 2] = p2;

				triangles [idx + 3] = p1;
				triangles [idx + 4] = p3;
				triangles [idx + 5] = p2;
			}
		}
		mesh.triangles = triangles;
		
		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();
		mesh.Optimize ();
	}
}
