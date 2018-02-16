using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

//For saving level data of each occupiable space
public struct Tile {
	public float x;
	public float y;
	public bool isWall;
	public bool isJump;
	public bool isOccupied;
	public bool isPlayer;

	public Tile(float xAxis, float yAxis, bool hasWall, bool canJump, bool occupied, bool player){
		x = xAxis;
		y = yAxis;
		isWall = hasWall;
		isJump = canJump;
		isOccupied = occupied;
		isPlayer = player;
	}
}

//Data for passing on level data to player and enemy robot controller
[System.Serializable]
public struct TileGridData {
	public int verticalTileCnt;
	public int horizontalTileCnt;
	public float tileWidth;
	public float tileHeight;
	public float robotFootingOffset;

	public TileGridData (int VerticalTileCnt, int HorizontalTileCnt, float TileWidth, float TileHeight, float RobotFootingOffset){
		verticalTileCnt = VerticalTileCnt;
		horizontalTileCnt = HorizontalTileCnt;
		tileWidth = TileWidth;
		tileHeight = TileHeight;
		robotFootingOffset = RobotFootingOffset;
	}
}

public class LevelGenerator : MonoBehaviour {
	/* Level Generator Process Summary
	 * Initialize Level --> Read randomly selected text file 
	 * --> Progressively Store level data --> Instantiate enemies 
	 * --> Instantiate Level 
	 */
	public static LevelGenerator _levelGeneratorInstance;


	[SerializeField] TileGridData _tileGridData = new TileGridData(5, 9, 1.32f, 1.32f, -0.33f);

	[SerializeField] Vector3 _initialPos;

	[Header("Level Tile Prefabs")]
	[SerializeField] GameObject _tileFloorPrefab;
	[SerializeField] GameObject _tileWallPrefab, _tileJumpPrefab, _tileJumpWallPrefab;

	[Header("Enemy Robot Prefabs")]
	[SerializeField] GameObject _enemyRobotPrefab;
	EnemyRobotController _tempEnemyRobotController;

	//Nested Array of Tiles to store level data
	Tile[,] _tiles;
	GameObject[,] _tileGameObject;
	List<EnemyRobotController> _enemyRobotControllers = new List<EnemyRobotController>();

	Quaternion _identityQuaternion = new Quaternion(0f, 0f, 0f, 1f);
	Quaternion _enemyRobotFacingLeftRotation = new Quaternion (0f, -0.707f, 0f, 0.707f);

	[SerializeField] string[] _paths;
	char[] _delimiter = new char[1]{'|'};

	[Header("Sprite References for destructable walls")]
	[SerializeField] Sprite _floorSprite;
	[SerializeField] Sprite _jumpSprite;

	//keep record for resetting the same scene
	bool _isInitialized = false;

	void Awake(){
		_levelGeneratorInstance = this;
	}
		
	void Start(){
		_tiles = new Tile[_tileGridData.horizontalTileCnt, _tileGridData.verticalTileCnt];
		_tileGameObject = new GameObject[_tileGridData.horizontalTileCnt, _tileGridData.verticalTileCnt];

		//Create a default version of the level expected to be populated later
		InitializeLevel ();
		_isInitialized = true;

		//select a random path to Parse
		ReadTextFile(_paths[Random.Range (0, _paths.Length)]);

		GenerateLevel ();
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode){
		// Regenerate a level upon reset
		if (scene.buildIndex == 1 && _isInitialized) {
			//select a random path to Parse
			ReadTextFile (_paths [Random.Range (0, _paths.Length)]);
			GenerateLevel ();
		}
	}

	//intialize the nested Array of tiles to be used for generating level
	void InitializeLevel() {
		for (int i = 0; i < _tileGridData.verticalTileCnt; i++) {
			for (int j = 0; j < _tileGridData.horizontalTileCnt; j++) {
				float xAxis = (j) * _tileGridData.tileHeight + _initialPos.x;
				float yAxis = (i) * _tileGridData.tileWidth + _initialPos.y;
				_tiles [j, i] = new Tile (xAxis, yAxis, false, false, false, false);
			}
		}
		// Player will always default to position 0,0 upon start
		_tiles [0, 0].isPlayer = true;
	}


	//Traverse the nested array of tiles to instantiate the level tiles
	void GenerateLevel(){
		for (int i = 0; i < _tileGridData.verticalTileCnt; i++) {
			for (int j = 0; j < _tileGridData.horizontalTileCnt; j++) {
				Tile tempTile = _tiles [j, i];
				Vector3 tempPos = _initialPos;
				tempPos.x = tempTile.x;
				tempPos.y = tempTile.y;

				if (tempTile.isJump) {
					if (tempTile.isWall) {
						_tileGameObject[j,i] = Instantiate (_tileJumpWallPrefab, tempPos, _identityQuaternion, this.transform);
					} else {
						_tileGameObject[j,i] = Instantiate (_tileJumpPrefab, tempPos, _identityQuaternion, this.transform);
					}
				} else if (tempTile.isWall) {
					_tileGameObject[j,i] = Instantiate (_tileWallPrefab, tempPos, _identityQuaternion, this.transform);
				} else {
					_tileGameObject[j,i] = Instantiate (_tileFloorPrefab, tempPos, _identityQuaternion, this.transform);
				}
			}
		}
	}

	//Instantiate Enemy Prefabs while parsing occurs
	void GenerateEnemy(int horizontal, int vertical, PlayerMoveSet[] enemyMoveSet){
		//Occupy the spot
		_tiles[horizontal, vertical].isOccupied = true;
		//get position data for enemy
		Vector3 tempPos;
		tempPos.x = _tiles [horizontal, vertical].x;
		tempPos.y = _tiles [horizontal, vertical].y + _tileGridData.robotFootingOffset;
		tempPos.z = -1f;
		GameObject tempEnemyObject = Instantiate (_enemyRobotPrefab, tempPos, _enemyRobotFacingLeftRotation, this.transform);
		_tempEnemyRobotController = tempEnemyObject.GetComponent<EnemyRobotController>();
		_enemyRobotControllers.Add (_tempEnemyRobotController);
		_tempEnemyRobotController.InitializeEnemy (horizontal, vertical, enemyMoveSet);
	}


	// Allow Robots to Get Tile information to determine whether action is valid
	public Tile GetTile(int hCnt, int vCnt){
		return _tiles [hCnt, vCnt];
	}

	public void SetTile(int hCnt, int vCnt, Tile tile){
		_tiles [hCnt, vCnt] = tile;
	}

	public TileGridData GetTileGridData(){
		return _tileGridData;
	}

	//Called by playerRobotController.cs to find the correct enemy to destroy
	public void DestroyEnemy(int hCnt, int vCnt){
		bool enemyToDestroyFound = false;
		IEnumerator<EnemyRobotController> iEnum = _enemyRobotControllers.GetEnumerator ();
		iEnum.Reset ();
		while (iEnum.MoveNext() && !enemyToDestroyFound) {
			EnemyRobotController tempEnemyController = iEnum.Current;
			if (tempEnemyController.CurrentHorizontalIndex == hCnt) {
				if (tempEnemyController.CurrentVerticalIndex == vCnt) {
					tempEnemyController.DestroyEnemyRobot ();
					enemyToDestroyFound = true;
				}
			}
		}
	}

	//Swap the sprites of broken wall to sprite without wall
	public IEnumerator BreakWall(int hCnt, int vCnt){
		float duration = 1.4f;
		yield return new WaitForSeconds (duration);
		if (_tiles [hCnt, vCnt].isJump) {
			_tileGameObject [hCnt, vCnt].GetComponent<SpriteRenderer> ().sprite = _jumpSprite;
		} else {
			_tileGameObject [hCnt, vCnt].GetComponent<SpriteRenderer> ().sprite = _floorSprite;
		}
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
			int x = length < _tileGridData.horizontalTileCnt ? length : _tileGridData.horizontalTileCnt;
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
			// Split string by delimiter
			string[] tempString = line.Split (_delimiter);
			// 0 - horizontal index
			int enemyH = int.Parse (tempString [0]);
			// 1 - vertical index
			int enemyV = int.Parse (tempString [1]);

			// 2 - enemy moveset
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
			//Call to instantiate enemy
			GenerateEnemy (enemyH, enemyV, tempEnemyMoveset);
		}
	}

	void OnEnable(){
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable(){
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}
