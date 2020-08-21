using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChangeNode : FuncNode {

    public CharacterChangeNode() {
        // Initial function default parameters
    }

    public override void Interpret(Context context) {
        context.SkipToken("Change");
        InterpretPart(context);
    }

    public override void Execute() {
        ComicBookManager.Console.CharacterChange(paraDic["which"], paraDic["path"], paraDic["name"]);
    }
}
