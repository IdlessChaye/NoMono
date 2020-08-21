using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSetPositionNode : FuncNode {

    public CharacterSetPositionNode() {
        // Initial function default parameters
    }

    public override void Interpret(Context context) {
        context.SkipToken("SetPosition");
        InterpretPart(context);
    }

    public override void Execute() {
        // Need to be done
    }
}
