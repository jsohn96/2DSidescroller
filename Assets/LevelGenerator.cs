using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct Tile {
	public float x;
	public float y;
	public bool isWall;
	public bool isJump;
	public bool isOccupied;

	public Tile(float xAxis, float yAxis, bool hasWall, bool canJump, bool occupied){
		x = xAxis;
		y = yAxis;
		isWall = hasWall;
		isJump = canJump;
		isOccupied = occupied;
	}
}

public class LevelGenerator : MonoBehaviour {

	[SerializeField] int _verticalTileCnt = 5;
	[SerializeField] int _horizontalTileCnt = 9;

	[SerializeField] float _tileWidth = 1.32f;
	[SerializeField] float _tileHeight = 1.32f;

	float _robotFootingOffset = -0.33f;

	[SerializeField] Vector3 _initialPos;

	[SerializeField] GameObject _tileFloorPrefab;

	Tile[,] _tiles;

	Quaternion _identityQuaternion = new Quaternion(0f, 0f, 0f, 1f);


	void Start(){
		_tiles = new Tile[_horizontalTileCnt, _verticalTileCnt];
		GenerateLevel ();
	}


	//Generate the platforming stages in a grid
	void GenerateLevel() {
		for (int i = 0; i < _verticalTileCnt; i++) {
			for (int j = 0; j < _horizontalTileCnt; j++) {
				float xAxis = (j) * _tileHeight + _initialPos.x;
				float yAxis = (i) * _tileWidth + _initialPos.y;
				_tiles [j, i] = new Tile (xAxis, yAxis, false, false, false);
				Vector3 tempPos = _initialPos;
				tempPos.x = xAxis;
				tempPos.y = yAxis;
				Instantiate (_tileFloorPrefab, tempPos, _identityQuaternion, this.transform);
			}
		}
	}


	// Allow Robots to Get Tile information to determine whether action is valid
	public Tile GetTile(int hCnt, int vCnt){
		return _tiles [hCnt, vCnt];
	}

	void ReadTextFile(string path){
		StreamReader stream = new StreamReader (path);

		while (!stream.EndOfStream) {
			string line = stream.ReadLine ();
		}
		stream.Close ();
	}
}
