using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandTextNode : InterpreterBaseNode {
    private List<InterpreterBaseNode> gameNodeList = new List<InterpreterBaseNode>();

    public override void Interpret(Context context) {
        context.SkipToken("Text");
        while(true) {
            string funcToken = context.GetCurrentToken();
            if(funcToken == null)
                break;

            if(!CanParse(funcToken, context))
                break;
        }
    }

    public override void Execute() {
        foreach(InterpreterBaseNode node in gameNodeList)
            node.Execute();
    }

    private bool CanParse(string token, Context context) {
        bool canParse = true;
        InterpreterBaseNode node = null;

        if(token.Equals("SetShowPause", System.StringComparison.OrdinalIgnoreCase)) {
            node = new TextSetShowPauseNode();
        } else if(token.Equals("Add", System.StringComparison.OrdinalIgnoreCase)) {
            node = new TextAddNode();
        } else if(token.Equals("Change", System.StringComparison.OrdinalIgnoreCase)) {
            node = new TextChangeNode();
        } else if(token.Equals("Clear", System.StringComparison.OrdinalIgnoreCase)) {
            node = new TextClearNode();
        } else {
            canParse = false;
        }

        if(node != null) {
            gameNodeList.Add(node);
            node.Interpret(context);
        }

        return canParse;
    }

}
