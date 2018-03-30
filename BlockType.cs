using System;

namespace Tetris
{
    public class BlockType
    {
        private char name;
        private int[][] layoutRotations;
        private int[][] layoutRotationsGhostStyle;
        private int rotations;

        private bool locked = false;

        public char GetName(){
            return name;
        }

        public BlockType(char n)
        {
            layoutRotations = new int[4][];
            layoutRotationsGhostStyle = new int[4][];
            rotations = 0;
            name = n;
        }

        public void AddBlockRotationArray(int[] r){
            if(!locked){
                layoutRotations[rotations] = r;

                int[] ghostStyleArray = (int[])r.Clone();
                for(int iii = 0; iii < ghostStyleArray.Length; iii++)
                {
                    ghostStyleArray[iii] *= -1; 
                }
                layoutRotationsGhostStyle[rotations] = ghostStyleArray;

                rotations++;
            }
        }

        public void LockBlockType(){
            locked = true;
        }

        public int GetRotationCount(){
            return rotations;
        }

        public int[] GetLayout(int rotation)
        {
            return layoutRotations[rotation >= rotations ? rotations : rotation];
        }

        public int[] GetGhostLayout(int rotation)
        {
            return layoutRotationsGhostStyle[rotation >= rotations ? rotations : rotation];
        }

        public override string ToString(){
            string returnString = "BlockType [name]: " + name + " / [rotation count] " + rotations + "\n";

            foreach(int[] s in layoutRotations){
                if(s is null) break;
                for(int iii = 0; iii < 16; iii++){
                    if(iii > 0 && iii % 4 == 0)
                        returnString += "\n";
                    returnString += s[iii];
                }
                returnString += "\n#####\n";
            }

            return returnString;
        }

    }
}
