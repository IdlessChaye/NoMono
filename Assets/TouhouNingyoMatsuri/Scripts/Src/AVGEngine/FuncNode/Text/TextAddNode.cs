using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAddNode : FuncNode {

    public TextAddNode() {
        // Initial function default parameters
    }

    public override void Interpret(Context context) {
        context.SkipToken("Add");
        InterpretPart(context);
    }

    public override void Execute() {
        ComicBookManager.Console.TextAdd(paraDic["context"]);
    }
}
