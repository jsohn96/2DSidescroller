using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Fading : MonoBehaviour {
    [SerializeField] Texture2D _fadeTexture;
    [SerializeField] float _fadeSpeed = 1f;
    [SerializeField] bool _noStartFade = false;

    int _fadeDir = -1;
    float _alpha = 1.0f;
    int _drawDepth = -1000;

    void Awake(){
        if (_noStartFade) {
            _alpha = 0.0f;
        }
    }

    void Start(){
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    void OnLevelLoaded(Scene scene, LoadSceneMode sceneMode) {
        if (!_noStartFade) {
            BeginFade (-1);
        }
    }

    void OnGUI() {
        _alpha += _fadeDir * (Time.deltaTime / _fadeSpeed);
        _alpha = Mathf.Clamp01 (_alpha);

        GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, _alpha);
        GUI.depth = _drawDepth;
        GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), _fadeTexture);
    }

    public float BeginFade(int direction){
        _fadeDir = direction;
        return (_fadeSpeed);
    }
}