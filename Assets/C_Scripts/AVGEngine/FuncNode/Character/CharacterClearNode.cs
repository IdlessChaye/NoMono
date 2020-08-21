using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterClearNode : FuncNode {

    public CharacterClearNode() {
        // Initial function default parameters
    }

    public override void Interpret(Context context) {
        context.SkipToken("Clear");
        InterpretPart(context);
    }

    public override void Execute() {
        // Need to be done
    }
}
