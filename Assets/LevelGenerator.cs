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

	[Header("Level Tile Prefabs")]
	[SerializeField] GameObject _tileFloorPrefab;
	[SerializeField] GameObject _tileWallPrefab, _tileJumpPrefab, _tileJumpWallPrefab;

	[Header("Enemy Robot Prefabs")]
	[SerializeField] GameObject _enemyRobotPrefab;
	EnemyRobotController _tempEnemyRobotController;

	Tile[,] _tiles;

	Quaternion _identityQuaternion = new Quaternion(0f, 0f, 0f, 1f);

	[SerializeField] string[] _paths;
	char[] _delimiter = new char[1]{'|'};


	void Start(){
		_tiles = new Tile[_horizontalTileCnt, _verticalTileCnt];

		//Create a default version of the level expected to be populated later
		InitializeLevel ();

		//select a random path to Parse
		ReadTextFile(_paths[Random.Range (0, _paths.Length)]);

		GenerateLevel ();
	}


	//intialize the nested Array of tiles to be used for generating level
	void InitializeLevel() {
		for (int i = 0; i < _verticalTileCnt; i++) {
			for (int j = 0; j < _horizontalTileCnt; j++) {
				float xAxis = (j) * _tileHeight + _initialPos.x;
				float yAxis = (i) * _tileWidth + _initialPos.y;
				_tiles [j, i] = new Tile (xAxis, yAxis, false, false, false);
//				Vector3 tempPos = _initialPos;
//				tempPos.x = xAxis;
//				tempPos.y = yAxis;
//				Instantiate (_tileFloorPrefab, tempPos, _identityQuaternion, this.transform);
			}
		}
	}

	void GenerateLevel(){
		for (int i = 0; i < _verticalTileCnt; i++) {
			for (int j = 0; j < _horizontalTileCnt; j++) {
				Tile tempTile = _tiles [j, i];
				Vector3 tempPos = _initialPos;
				tempPos.x = tempTile.x;
				tempPos.y = tempTile.y;

				if (tempTile.isJump) {
					if (tempTile.isWall) {
						Instantiate (_tileJumpWallPrefab, tempPos, _identityQuaternion, this.transform);
					} else {
						Instantiate (_tileJumpPrefab, tempPos, _identityQuaternion, this.transform);
					}
				} else if (tempTile.isWall) {
					Instantiate (_tileWallPrefab, tempPos, _identityQuaternion, this.transform);
				} else {
					Instantiate (_tileFloorPrefab, tempPos, _identityQuaternion, this.transform);
				}
			}
		}
	}

	//Instantiate Enemy Prefabs while parsing occurs
	void GenerateEnemy(int horizontal, int vertical, PlayerMoveSet[] enemyMoveSet){
		Vector3 tempPos = _initialPos;
		tempPos.x = _tiles [horizontal, vertical].x;
		tempPos.y = _tiles [horizontal, vertical].y + _robotFootingOffset;
		GameObject tempEnemyObject = Instantiate (_enemyRobotPrefab, tempPos, _identityQuaternion, this.transform);
		_tempEnemyRobotController = tempEnemyObject.GetComponent<EnemyRobotController>();
		_tempEnemyRobotController.InitializeEnemy (enemyMoveSet);
	}


	// Allow Robots to Get Tile information to determine whether action is valid
	public Tile GetTile(int hCnt, int vCnt){
		return _tiles [hCnt, vCnt];
	}
		

	void ReadTextFile(string path){
		StreamReader stream = new StreamReader (path);
		int lineCnt = 0;
		while (!stream.EndOfStream) {
			string line = stream.ReadLine ();
			ParseString (line, lineCnt);
			lineCnt++;
		}
		stream.Close ();
	}


	/* Current level Parsing Logic (Line 0 - 4: Level Composition)
	 * F = floor
	 * W = wall
	 * J = jump
	 * L = jump Wall
	 * */

	//Parse a line from the text document
	void ParseString(string line, int lineCnt){
		if (lineCnt < 5) {
			int length = line.Length;
			int y = 4 - lineCnt;
			int x = length < _horizontalTileCnt ? length : _horizontalTileCnt;
			for (int i = 0; i < x; i++) {
				Tile tempTile = _tiles [i, y];
				switch (line [i]) {
				case 'H':
					break;
				case 'W':
					tempTile.isWall = true;
					break;
				case 'J':
					tempTile.isJump = true;
					break;
				case 'L':
					tempTile.isWall = true;
					tempTile.isJump = true;
					break;
				default:
					break;
				}
				_tiles [i, y] = tempTile;
			}
		} else {
			string[] tempString = line.Split (_delimiter);
			Debug.Log (tempString [0]);
			int enemyH = int.Parse (tempString [0]);
			int enemyV = int.Parse (tempString [1]);

			int length = tempString [2].Length;
			PlayerMoveSet[] tempEnemyMoveset = new PlayerMoveSet[length];

			for (int i = 0; i < length; i++) {
				switch (tempString [2] [i]) {
				case 'C':
					tempEnemyMoveset[i] = PlayerMoveSet.MoveLeft;
					break;
				case 'D':
					tempEnemyMoveset[i] = PlayerMoveSet.MoveRight;
					break;
				case 'E':
					tempEnemyMoveset[i] = PlayerMoveSet.JumpUp;
					break;
				case 'F':
					tempEnemyMoveset[i] = PlayerMoveSet.JumpDown;
					break;
				case 'G':
					tempEnemyMoveset[i] = PlayerMoveSet.Rest;
					break;
				default:
					break;
				}
			}
			GenerateEnemy (enemyH, enemyV, tempEnemyMoveset);
		}
	}
}
