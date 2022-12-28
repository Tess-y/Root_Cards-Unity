using HarmonyLib;

[HarmonyPatch(typeof(GeneralInput), "Update")]
internal class GeneralInputPatchUpdate {
    public static float vel = 10;
    private static void Postfix(GeneralInput __instance) {
        try {
            CharacterData data = __instance.GetComponent<CharacterData>();
            Dark_Queen dark_Queen = __instance.GetComponentInChildren<Dark_Queen>();
            if(dark_Queen==null)
                return;
            if(data.playerActions.Up.IsPressed) {
                __instance.transform.position+=UnityEngine.Vector3.up*vel*data.stats.movementSpeed*TimeHandler.deltaTime;
            }
            if(data.playerActions.Down.IsPressed) {
                __instance.transform.position+=UnityEngine.Vector3.down*vel*data.stats.movementSpeed*TimeHandler.deltaTime;
            }
            if(data.playerActions.Left.IsPressed) {
                __instance.transform.position+=UnityEngine.Vector3.left*vel*data.stats.movementSpeed*TimeHandler.deltaTime;
            }
            if(data.playerActions.Right.IsPressed) {
                __instance.transform.position+=UnityEngine.Vector3.right*vel*data.stats.movementSpeed*TimeHandler.deltaTime;
            }
            if(data.playerActions.Fire.WasPressed) {
                dark_Queen.Telleport();
            }
            __instance.shootIsPressed=false;
            __instance.shootWasPressed=false;
            __instance.shootWasReleased=false;
        } catch { }
    }
}