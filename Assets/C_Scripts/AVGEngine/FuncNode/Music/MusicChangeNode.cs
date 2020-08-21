using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChangeNode : FuncNode {

    public MusicChangeNode() {
        // Initial function default parameters
    }

    public override void Interpret(Context context) {
        context.SkipToken("change");
        InterpretPart(context);
    }
    
    public override void Execute() {
        ComicBookManager.Console.MusicChange("", paraDic["path"], float.Parse(paraDic["crossfade"]));
    }
}
