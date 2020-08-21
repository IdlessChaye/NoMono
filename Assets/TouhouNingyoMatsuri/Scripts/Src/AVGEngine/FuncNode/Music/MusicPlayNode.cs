using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayNode : FuncNode {

    public MusicPlayNode() {
        // Initial function default parameters
    }

    public override void Interpret(Context context) {
        context.SkipToken("Play");
        InterpretPart(context);
    }

    public override void Execute() {
        ComicBookManager.Console.MusicPlay(paraDic["path"], float.Parse(paraDic["crossfade"]));
    }
}
