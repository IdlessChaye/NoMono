using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMusicNode : InterpreterBaseNode {
    private List<InterpreterBaseNode> gameNodeList = new List<InterpreterBaseNode>();

    public override void Interpret(Context context) {
        context.SkipToken("Music");
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

        if(token.Equals("Play", System.StringComparison.OrdinalIgnoreCase)) {
            node = new MusicPlayNode();
        } else if(token.Equals("Stop", System.StringComparison.OrdinalIgnoreCase)) {
            node = new MusicStopNode();
        } else if(token.Equals("Add", System.StringComparison.OrdinalIgnoreCase)) {
            node = new MusicAddNode();
        } else if(token.Equals("Remove", System.StringComparison.OrdinalIgnoreCase)) {
            node = new MusicRemoveNode();
        } else if(token.Equals("Change", System.StringComparison.OrdinalIgnoreCase)) {
            node = new MusicChangeNode();
        } else if(token.Equals("Clear", System.StringComparison.OrdinalIgnoreCase)) {
            node = new MusicClearNode();
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
