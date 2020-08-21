using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAddNode : FuncNode {

    public MusicAddNode() {
        // Initial function default parameters
    }

    public override void Interpret(Context context) {
        context.SkipToken("Add");
        InterpretPart(context);
    }

    public override void Execute() {
        ComicBookManager.Console.MusicAdd(paraDic["path"], float.Parse(paraDic["crossfade"]));
    }
}
