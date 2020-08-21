using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicBookReader : MonoBehaviour {
    ComicBookManager cbm;
    // Use this for initialization
    void Start() {
        cbm = ComicBookManager.ComicBook;
        cbm.RaiseComicBook(ComicBookManager.chapter2);
        //ComicBookManager.ComicBook.console.PrintHello();
        cbm.RoyalFlare();
    }

}

