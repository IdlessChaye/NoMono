using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAddNode : FuncNode {

    public CharacterAddNode() {
        // Initial function default parameters
    }

    public override void Interpret(Context context) {
        context.SkipToken("Add");
        InterpretPart(context);
    }

    public override void Execute() {
        ComicBookManager.Console.CharacterAdd(paraDic["which"], paraDic["path"], paraDic["name"]);
    }
}
