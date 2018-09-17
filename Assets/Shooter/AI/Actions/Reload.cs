using UtilityAI;

public class Reload : Action {
    public override void Execute(IContext context) {
        var c = (HaloAIContext)context;
        c.controller.SetAIStateText("Reload");

        c.agent.Reload();
    }

    public override void Stop(IContext context) {
    }
}