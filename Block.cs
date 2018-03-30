using System;

namespace Tetris
{
    public class Block
    {
        private int location;
        private BlockType type;
        private bool ghostMode;
        
        private int currentRotation;

        public Block(BlockType t, bool g) 
            : this(t, 4)
        {
            ghostMode = g;
        }

        public Block(BlockType t, int loc){
            type = t;
            location = loc;
            currentRotation = 0;
        }

        public Block(BlockType t)
        : this(t,4){}

        public void SetLocation(int loc){
            if(location < Config.GetFieldSize())
                location = loc;
        }

        public int GetLocation(){
            return location;
        }

        public void MoveLeft(int count){
            location-=count;
        }

        public bool GhostModeEnabled() { return ghostMode; }

        public void MoveRight(int count){
            location+=count;
        }

        public BlockType GetBlockType() { return type; }

        public int GetRotation()
        {
            return currentRotation;
        }

        public void SetRotation(int r)
        {
            currentRotation = r >= 0 && r < type.GetRotationCount() ? r : currentRotation;
        }

        public void MoveUp(int count){
            location-=Config.GetFieldWidth();
        }

        public void MoveDown(int count){
            location+=Config.GetFieldWidth();
        }

        public void RotateRight(){
            currentRotation++;
            if(currentRotation >= type.GetRotationCount())
                currentRotation = 0;
        }

        public void RotateLeft(){
            if(currentRotation>0)
                currentRotation--;
            else
                currentRotation = type.GetRotationCount() - 1;
        }

        public int[] GetBlockArray(){
            return ghostMode ? type.GetGhostLayout(currentRotation) : type.GetLayout(currentRotation);
        }


        public override string ToString(){
            string r = "";

            int[] arr = ghostMode ? type.GetGhostLayout(currentRotation) : type.GetLayout(currentRotation);
            for(int yyy = 0; yyy < 4; yyy++){
                for(int xxx = 0; xxx < 4; xxx++){
                    r+=arr[yyy*4+xxx];
                }
                r+="\n";
            }
            r += "Ghost mode: " + ghostMode + "\n";
            r += "Location: " + location+ "\n";
            return r;
        }

    
    }
}