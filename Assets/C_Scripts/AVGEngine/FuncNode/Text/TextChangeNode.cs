using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextChangeNode : FuncNode {

    public TextChangeNode() {
        // Initial function default parameters
    }

    public override void Interpret(Context context) {
        context.SkipToken("Change");
        InterpretPart(context);
    }

    public override void Execute() {
        ComicBookManager.Console.TextChange(paraDic["context"]);
    }
}
