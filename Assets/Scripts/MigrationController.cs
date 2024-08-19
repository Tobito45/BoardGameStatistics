using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MigrationController
{
   public static void Migration(string version, ref List<GameData> data)
   {
        if(version == "0.3")
        {
            UpdateTo0_4(ref data);
            version = "0.4";
        }
   }

    private static void UpdateTo0_4(ref List<GameData> data)
    {
        foreach (GameData gameData in data)
            foreach (Game game in gameData.GetGames)
                game.CreateLists();
    }
}
