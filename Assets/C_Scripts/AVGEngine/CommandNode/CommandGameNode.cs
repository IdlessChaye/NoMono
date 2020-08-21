using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandGameNode : InterpreterBaseNode {
    private List<InterpreterBaseNode> gameNodeList = new List<InterpreterBaseNode>();

    public override void Interpret(Context context) {
        context.SkipToken("Game");
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
        if(token.Equals("Initial", System.StringComparison.OrdinalIgnoreCase)) {
            node = new GameInitialNode();
        } else if(token.Equals("Wait", System.StringComparison.OrdinalIgnoreCase)) {
            node = new GameWaitNode();
        } else if(token.Equals("Pause", System.StringComparison.OrdinalIgnoreCase)) {
            node = new GamePauseNode();
        } else if(token.Equals("Stop", System.StringComparison.OrdinalIgnoreCase)) {
            node = new GameStopNode();
        } else if(token.Equals("Event", System.StringComparison.OrdinalIgnoreCase)) {
            node = new GameEventNode();
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
