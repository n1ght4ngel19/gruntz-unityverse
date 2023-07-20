using GruntzUnityverse.Enumz;

namespace GruntzUnityverse.Objectz.Itemz {
  public abstract class Tool : Item {
    public ToolName toolName;


    protected override void Start() {
      base.Start();
      switch (spriteRenderer.sprite.name) {
        case "Barehandz":
          toolName = ToolName.Barehandz;
          break;
        case "Warpstone":
          toolName = ToolName.Warpstone;
          break;
        case "Shovel":
          toolName = ToolName.Shovel;
          break;
        default:
          throw new System.Exception("Unknown tool name: " + spriteRenderer.sprite.name);
      };
    }
  }
}
