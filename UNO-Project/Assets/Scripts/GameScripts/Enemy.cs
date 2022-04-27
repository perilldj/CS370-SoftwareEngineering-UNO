using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomNames {
    private static List<string> names = new List<string> {"Sophia", "Aiden", "Emma", "Jackson", "Isabella",
                                                          "Olivia", "Liam", "Ava", "Jacob", "Madison", "Noah",
                                                          "Emily", "Riley", "Ryan", "Alexander", "Layla", "Jack"};

    private static List<int> usedNames;

    private static bool isNameUsed(int index) {
        for(int i = 0; i < usedNames.Count; i++)
            if(usedNames[i] == index)
                return true;
        return false;
    }

    public static void clearUsedNames() {
        usedNames.Clear();
    }

    public static string getRandomName() {

        System.Random ran = new System.Random();
        int randomIndex = ran.Next(0, names.Count);

        int attempts = 5; //Insurance this doesn't get stuck in an infinite loop
        while(attempts > 0) {
            attempts--;
            if(isNameUsed(randomIndex))
                randomIndex = ran.Next(0, names.Count);
            else
                return names[randomIndex];
        }
        
        return ":)"; 

    }

}

public class Enemy {
    
    private bool isAI = true;

    private string name;

    private GameControl gameController;
    private GameObject enemyObject;
    private Hand hand;

    private const float HAND_OFFSET_X = 0.0f;
    private const float HAND_OFFSET_Y = 0.0f;

    public Enemy(Vector2 pos) {
        //if(isAI)
    }

}
