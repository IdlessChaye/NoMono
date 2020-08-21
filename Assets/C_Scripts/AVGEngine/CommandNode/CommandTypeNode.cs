using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandTypeNode : InterpreterBaseNode {
    private InterpreterBaseNode node;

    public override void Interpret(Context context) {
        string commandToken = context.GetCurrentToken();
        if(!CanParse(commandToken, context))
            throw new System.Exception("ERROR IN CommandTypeNode! NOT DEFINDED!" + commandToken);
    }

    public override void Execute() {
        node.Execute();
    }

    private bool CanParse(string commandToken, Context context) {
        bool canParse = true;
        
        if(commandToken.Equals("Game", System.StringComparison.OrdinalIgnoreCase)) {
            node = new CommandGameNode();
        } else if(commandToken.Equals("Music", System.StringComparison.OrdinalIgnoreCase)) {
            node = new CommandMusicNode();
        } else if(commandToken.Equals("DownMenu", System.StringComparison.OrdinalIgnoreCase)) {
            node = new CommandDownMenuNode();
        } else if(commandToken.Equals("Character", System.StringComparison.OrdinalIgnoreCase)) {
            node = new CommandCharacterNode();
        } else if(commandToken.Equals("Text", System.StringComparison.OrdinalIgnoreCase)) {
            node = new CommandTextNode();
        } else if(commandToken.Equals("Picture", System.StringComparison.OrdinalIgnoreCase)) {
            node = new CommandPictureNode();
        } else {
            canParse = false;
        }

        if(node != null)
            node.Interpret(context);

        return canParse;
    }
}
