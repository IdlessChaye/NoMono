using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRemoveNode : FuncNode {

    public CharacterRemoveNode() {
        // Initial function default parameters
    }

    public override void Interpret(Context context) {
        context.SkipToken("Remove");
        InterpretPart(context);
    }

    public override void Execute() {
        ComicBookManager.Console.CharacterRemove(paraDic["which"]);
    }
}
