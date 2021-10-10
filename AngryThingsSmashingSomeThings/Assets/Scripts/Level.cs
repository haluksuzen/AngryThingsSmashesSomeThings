using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Level
{
   private int highscore;
   private int currentScore;
   private bool unlocked;
   public int oneStarReq;
   public int twoStarReq;
   public int threeStarReq;
   public Level()
   {
       highscore = 0;
       currentScore = 0;
       unlocked = false;
       oneStarReq = 10000;
       twoStarReq = 20000;
       threeStarReq = 30000;
   }
   public int CurrentScore
   {
       get{ return currentScore;}
       set{ currentScore = value;}
   }
   public int CurrStarScore
   {
       get
       {
           if (currentScore >= threeStarReq)
           {
               return 3;
           }
           if (currentScore >= twoStarReq)
           {
               return 2;
           }
           if (currentScore >= oneStarReq)
           {
               return 1;
           }
           return 0;
       }
   }
   public bool Unlocked
   {
       get{ return unlocked;}
       set{ unlocked = value;}
   }
   public bool Defeated
   {
       get
       {
           if (highscore > oneStarReq)
           {
               return true;
           }
           return false;
       }
   }
   public bool CurrentDefeated
   {
       get
       {
           if (currentScore >= oneStarReq)
           {
               return true;
           }
           return false;
       }
   }
   public int HighStarScore
   {
       get
       {
           if (highscore >= threeStarReq)
           {
               return 3;
           }
           if (highscore >= twoStarReq)
           {
               return 2;
           }
           if (highscore >= oneStarReq)
           {
               return 1;
           }
           return 0;
       }
   }
   public int Highscore
   {
       get{return highscore;}
       set{highscore = value;}
   }
}
