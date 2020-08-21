using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComicBookUIConsole : MonoBehaviour {
    public GameObject comicBookView;
    private GameObject canvas;
    private Animator ani;
    private Image imageBG;
    private Text textContext;
    private Image characterLeft;
    private Image characterRight;
    private Text nameLeft;
    private Text nameRight;
    private Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();
    private float pauseTime;
    private bool isTyping;
    private List<GameObject> hideUIList = new List<GameObject>();

    void Start() {
        canvas = GameObject.Find("Canvas");
        if(canvas == null)
            throw new System.Exception("Cant Find Canvas!");
        comicBookView = Instantiate(Resources.Load<GameObject>("View/ComicBookView")) as GameObject;
        comicBookView.transform.SetParent(canvas.transform, false);
        ani = comicBookView.GetComponent<Animator>();
        imageBG = GameObject.Find("ImageBG").GetComponent<Image>();
        textContext = GameObject.Find("TextContext").GetComponent<Text>();
        characterLeft = GameObject.Find("CharacterLeft").GetComponent<Image>();
        characterRight = GameObject.Find("CharacterRight").GetComponent<Image>();
        nameLeft = GameObject.Find("NameLeft").GetComponent<Text>();
        nameRight = GameObject.Find("NameRight").GetComponent<Text>();
        pauseTime = 0.03f;
        isTyping = false;
    }

    void Update() {
        if(isTyping == true) {
            if(Input.GetKeyDown(KeyCode.Z) || Input.GetKey(KeyCode.LeftControl)) {
                isTyping = false;
            }
        }
        if(isTyping == true) {
            ComicBookManager.ComicBook.LockComicBook();
        } else {
            ComicBookManager.ComicBook.UnLockComicBook();
        }
    }



    public void GameInitial() {
        // MusicClear();
        PictureClear();
        CharacterClear();
        TextClear();
        DownMenuShow();
    }

    public void GameWait(float time) { //ms
        StartCoroutine(LockComicBookForTime(time));
    }

    public void GamePause() {
        ComicBookManager.ComicBook.Pause();
    }

    public void GameStop() {
        DownMenuHide();
        ComicBookManager.ComicBook.CloseComicBook();
    }

    public void GameEvent(string eventname) {
        if(eventname.Equals("GameStart", System.StringComparison.OrdinalIgnoreCase)) {
            //FullGameFlowManager.Instance.SendMessage("GameStartConfirm");
        } else if(eventname.Equals("MainSceneOver", System.StringComparison.OrdinalIgnoreCase)) {
            //FullGameFlowManager.Instance.SendMessage("MainSceneOver");
        }
    }

    IEnumerator LockComicBookForTime(float time) {
        ComicBookManager.ComicBook.LockComicBook();
        yield return new WaitForSeconds(time / 1000f);
        ComicBookManager.ComicBook.UnLockComicBook();
    }


    
    public void DownMenuShow() {
        ani.SetTrigger("OnEnter");
        foreach(Transform tf in canvas.transform) {
            if(tf.gameObject == comicBookView)
                continue;
            if(tf.gameObject.activeInHierarchy == false)
                continue;
            tf.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
            hideUIList.Add(tf.gameObject);
        }
    }

    public void DownMenuHide() {
        ani.SetTrigger("OnExit");
        foreach(GameObject go in hideUIList) {
            go.GetComponent<CanvasGroup>().alpha = 1f;
        }
    }



    public void TextSetShowPause(float pauseTime) {
        this.pauseTime = pauseTime;
    }

    public void TextAdd(string context) {
        StartCoroutine(TypeText(true, context));
    }

    public void TextChange(string context) {
        StartCoroutine(TypeText(false, context));
    }

    public void TextClear() {
        textContext.text = "";
    }

    IEnumerator TypeText(bool add, string context) {
        isTyping = true;
        string currentText = "";
        if(add)
            currentText = textContext.text;

        foreach(char letter in context.ToCharArray()) {
            currentText += letter;
            textContext.text = currentText;
            if(isTyping)
                yield return new WaitForSeconds(pauseTime);
        }
        isTyping = false;
    }



    public void CharacterAdd(string which, string path, string name) {
        if(which.Equals("left", System.StringComparison.OrdinalIgnoreCase)) {
            characterLeft.sprite = GetSingleSprite(path);
            characterLeft.color = Color.white;
            nameLeft.text = name;
        } else if(which.Equals("right", System.StringComparison.OrdinalIgnoreCase)) {
            characterRight.sprite = GetSingleSprite(path);
            characterRight.color = Color.white;
            nameRight.text = name;
        } else {
            throw new System.Exception("Error In CharacterAddOrChange!");
        }
    }

    public void CharacterChange(string which, string path, string name) {
        CharacterAdd(which, path, name);
    }

    public void CharacterRemove(string which) {
        if(which.Equals("left", System.StringComparison.OrdinalIgnoreCase)) {
            characterLeft.sprite = null;
            characterLeft.color = Color.clear;
            nameLeft.text = "";
        } else if(which.Equals("right", System.StringComparison.OrdinalIgnoreCase)) {
            characterRight.sprite = null;
            characterRight.color = Color.clear;
            nameRight.text = "";
        } else {
            throw new System.Exception("Error In CharacterRemove!");
        }
    }

    public void CharacterClear() {
        CharacterRemove("left");
        CharacterRemove("right");
    }


    public void PictureSetPosition(float width, float height, float x, float y) {

    }

    public void PictureAdd(string path, float crossfade) {

    }

    public void PictureChange(string oldPath, string path, float crossfade) {

    }

    public void PictureRemove(string path, float crossfade) {

    }

    public void PictureClear() {
        if (imageBG != null)
            imageBG.sprite = null;
    }



    public void MusicPlay(string name, float crossfade) {
        //FullMusicManager.Instance.Play(name, crossfade);
    }

    public void MusicStop(float crossfade) {
        //FullMusicManager.Instance.Stop(crossfade);
    }

    public void MusicAdd(string name, float crossfade) {
        MusicPlay(name, crossfade);
    }

    public void MusicRemove(string oldname, float crossfade) {
        MusicStop(crossfade);
    }

    public void MusicChange(string oldname, string name, float crossfade) {
        //FullMusicManager.Instance.Change(name, crossfade);
    }

    public void MusicClear(float crossfade = 1500f) {
        MusicStop(crossfade);
    }



    public Sprite GetSingleSprite(string path) {
        if(spriteDict.ContainsKey(path) == false || spriteDict[path] == null) {
            Sprite sprite = Instantiate(Resources.Load<Sprite>(path)) as Sprite;
            AddOrReplaceSprite(path, sprite);
            return sprite;
        }
        return spriteDict[path];
    }

    public void DestroySingleSprite(string path) {
        if(spriteDict.ContainsKey(path) == false)
            return;
        if(spriteDict[path] == null) {
            spriteDict.Remove(path);
            return;
        }
        GameObject.Destroy(spriteDict[path]);
        spriteDict.Remove(path);
        return;
    }

    private void AddOrReplaceSprite(string path, Sprite sprite) {
        if(spriteDict.ContainsKey(path) == false) {
            spriteDict.Add(path, sprite);
            return;
        }
        if(spriteDict[path] == null) {
            spriteDict[path] = sprite;
            return;
        }
    }
}
