using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDownMenuNode : InterpreterBaseNode {
    private List<InterpreterBaseNode> gameNodeList = new List<InterpreterBaseNode>();

    public override void Interpret(Context context) {
        context.SkipToken("DownMenu");
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

        if(token.Equals("Hide", System.StringComparison.OrdinalIgnoreCase)) {
            node = new DownMenuHideNode();
        } else if(token.Equals("Show", System.StringComparison.OrdinalIgnoreCase)) {
            node = new DownMenuShowNode();
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
