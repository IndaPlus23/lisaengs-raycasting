using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ray9006
{
    public class Player
    {
        private int PlayerX;
        private int PlayerY;

        private int PlayerDirection = 0;
        private int PlayerPitch = 0;

        public Player(int x, int y) {
            // Set the player's position
            PlayerX = x;
            PlayerY = y;
        }

        // Get the player info
        public int GetPlayerX() {
            return PlayerX;
        }
        public int GetPlayerY() {
            return PlayerY;
        }
        public int[] GetPlayerPos() {
            return new int[] {PlayerX, PlayerY};
        }
        public int GetPlayerDirection() {
            return PlayerDirection;
        }
        public int GetPlayerPitch() {
            return PlayerPitch;
        }
    }
}