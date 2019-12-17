using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelAndRockHandler : MonoBehaviour {

	[SerializeField] GameObject terrainClearer1;
	[SerializeField] GameObject terrainClearer2;
	[SerializeField] GameObject terrainClearer3;
	[SerializeField] GameObject terrainClearer4;
	[SerializeField] GameObject terrainClearer5;
	[SerializeField] GameObject terrainClearer6;
	[SerializeField] GameObject rockCheck1;
	[SerializeField] GameObject rockCheck2;
	[SerializeField] GameObject rockCheck3;
	[SerializeField] GameObject rockCheck4;
	[SerializeField] GameObject rockCheck5;
	[SerializeField] GameObject rockCheck6;
	[SerializeField] GameObject rock1;
	[SerializeField] GameObject rock2;
	[SerializeField] GameObject rock3;
	[SerializeField] GameObject rock4;
	float coordY;
	float coordX;

	void Start() {

		GameObject[] tunnels = { terrainClearer1, terrainClearer2, terrainClearer3, terrainClearer4, terrainClearer5, terrainClearer6 };

		coordY = Random.Range(0.33f, 3.0f);
		terrainClearer1.transform.position = new Vector3(2.03f, coordY, 0.0f);

		coordY = Random.Range(-2.0f, -3.0f);
		coordX = Random.Range(2.5f, 4.0f);
		terrainClearer2.transform.position = new Vector3(coordX, coordY, 0.0f);

		coordY = Random.Range(-7.5f, -9.5f);
		coordX = Random.Range(5.5f, 7.5f);
		terrainClearer3.transform.position = new Vector3(coordX, coordY, 0.0f);

		coordY = Random.Range(2.0f, 3.5f);
		coordX = Random.Range(6.25f, 9.0f);
		terrainClearer4.transform.position = new Vector3(coordX, coordY, 0.0f);

		coordY = Random.Range(-6.7f, -8.0f);
		coordX = Random.Range(1.0f, 2.9f);
		terrainClearer5.transform.position = new Vector3(coordX, coordY, 0.0f);

		coordY = Random.Range(-2.9f, -6.5f);
		coordX = Random.Range(3.5f, 8.5f);
		terrainClearer6.transform.position = new Vector3(coordX, coordY, 0.0f);


		GameObject[] rocks = { rock1, rock2, rock3, rock4 };
		GameObject[] rockCheckers = { rockCheck1, rockCheck2, rockCheck3, rockCheck4, rockCheck5, rockCheck6 };

		coordX = Random.Range(0.5f, 2.25f);
		coordY = Random.Range(4.0f, 1.8f);
		rock1.transform.position = new Vector3(coordX, coordY, 0.0f);
		coordX = Random.Range(2.75f, 4.5f);
		coordY = Random.Range(1.3f, -0.9f);
		rock2.transform.position = new Vector3(coordX, coordY, 0.0f);
		coordX = Random.Range(5.0f, 6.75f);
		coordY = Random.Range(-1.4f, -3.6f);
		rock3.transform.position = new Vector3(coordX, coordY, 0.0f);
		coordX = Random.Range(7.25f, 9.0f);
		coordY = Random.Range(-4.1f, -6.3f);
		rock4.transform.position = new Vector3(coordX, coordY, 0.0f);

		/*
		public bool Intersects(Bounds bounds) {
			return min.x <= bounds.max.x && max.x >= bounds.min.x &&
				   min.y <= bounds.max.y && max.y >= bounds.min.y &&
				   min.z <= bounds.max.z && max.z >= bounds.min.z;
		}
		*/

		for (int z = 0; z < 4; z++) {
			for (int i = 0; i < 6; i++) {
				if (i < 3) {
					while (intersection(rocks[z], rockCheckers[i], true)) {
						if (z == 0) {
							coordX = Random.Range(0.5f, 2.25f);
							coordY = Random.Range(4.0f, 1.8f);
						} else if (z == 1) {
							coordX = Random.Range(2.75f, 4.5f);
							coordY = Random.Range(1.3f, -0.9f);
						} else if (z == 2) {
							coordX = Random.Range(5.0f, 6.75f);
							coordY = Random.Range(-1.4f, -3.6f);
						} else {
							coordX = Random.Range(7.25f, 9.0f);
							coordY = Random.Range(-4.1f, -6.3f);
						}
						rocks[z].transform.position = new Vector3(coordX, coordY, 0.0f);
					}
				} else {
					while (intersection(rocks[z], rockCheckers[i], false)) {
						if (z == 0) {
							coordX = Random.Range(0.5f, 2.25f);
							coordY = Random.Range(4.0f, 1.8f);
						} else if (z == 1) {
							coordX = Random.Range(2.75f, 4.5f);
							coordY = Random.Range(1.3f, -0.9f);
						} else if (z == 2) {
							coordX = Random.Range(5.0f, 6.75f);
							coordY = Random.Range(-1.4f, -3.6f);
						} else {
							coordX = Random.Range(7.25f, 9.0f);
							coordY = Random.Range(-4.1f, -6.3f);
						}
						rocks[z].transform.position = new Vector3(coordX, coordY, 0.0f);
					}
				}
			}
		}

		StartCoroutine(turnOff());

	}

	public bool intersection(GameObject rock, GameObject rockChecker, bool horizontal) {

		double rockMinX = rock.transform.position.x - 0.3;
		double rockMaxX = rock.transform.position.x + 0.3;
		double rockMinY = rock.transform.position.y - 0.25;
		double rockMaxY = rock.transform.position.y + 0.25;

		double checkMinX;
		double checkMaxX;
		double checkMinY;
		double checkMaxY;
		if (horizontal) {
			checkMinX = rockChecker.transform.position.x - 2.2;
			checkMaxX = rockChecker.transform.position.x + 2.2;
			checkMinY = rockChecker.transform.position.y - 0.4;
			checkMaxY = rockChecker.transform.position.y + 0.6;
		} else {
			checkMinX = rockChecker.transform.position.x - 0.4;
			checkMaxX = rockChecker.transform.position.x + 0.4;
			checkMinY = rockChecker.transform.position.y - 2.2;
			checkMaxY = rockChecker.transform.position.y + 2.5;
		}

		// check if intersection
		// first and second lines check if x or y intersect
		// third and fourth lines check if the box only intersects on one plane
		if ((
			( rockMinX < checkMaxX || rockMaxX > checkMinX ) || 
			( rockMinY < checkMaxY || rockMaxY > checkMinY )
			) && (
			!( rockMaxX < checkMinX || rockMinX > checkMaxX) && 
			!( rockMaxY < checkMinY || rockMinY > checkMaxY) )){
			return true;
		} else {
			return false;
		}

	}

	IEnumerator turnOff() {
		yield return new WaitForEndOfFrame();
		terrainClearer1.GetComponent<BoxCollider2D>().enabled = false;
		terrainClearer2.GetComponent<BoxCollider2D>().enabled = false;
		terrainClearer3.GetComponent<BoxCollider2D>().enabled = false;
		terrainClearer4.GetComponent<BoxCollider2D>().enabled = false;
		terrainClearer5.GetComponent<BoxCollider2D>().enabled = false;
		terrainClearer6.GetComponent<BoxCollider2D>().enabled = false;

		EnemySpawner.Instance.SpawnEnemy(terrainClearer1.transform.position);
		EnemySpawner.Instance.SpawnEnemy(terrainClearer3.transform.position);
		EnemySpawner.Instance.SpawnEnemy(terrainClearer4.transform.position);
		EnemySpawner.Instance.SpawnEnemy(terrainClearer6.transform.position);
	}

}