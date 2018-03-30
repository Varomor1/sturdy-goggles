using System;

namespace Tetris
{
    public static class Config
    {
        private static BlockType[] blockTypes;
        private static bool configInitialized = false;

        private static int fieldHeight;
        private static int fieldWidth;

        public static BlockType GetBlockType(int index){
            return configInitialized && index >= 0 && index < blockTypes.Length ? blockTypes[index] : null;
        }

        public static BlockType GetBlockType(char c){
            if(configInitialized){
                foreach(BlockType bt in blockTypes){
                    if(bt.GetName().Equals(c))
                        return bt;
                }
            }
            return null;
        }

        public static int GetFieldWidth(){
            return fieldWidth;
        }

        public static int GetFieldHeight(){
            return fieldHeight;
        }

        public static int GetFieldSize(){
            return (int)(fieldHeight *fieldWidth);
        }

        #region init
        public static void Initialize(){
            int[] arr = new int[16];
            blockTypes = new BlockType[7];
        //Field parameters
        //Width + 2 for collision check
                    fieldHeight = 25;
                    fieldWidth = 12;
        //BlockType I
        
                    BlockType t = new BlockType('I');
                    
                    arr = new int[]{
                        0,1,0,0,
                        0,1,0,0,
                        0,1,0,0,
                        0,1,0,0};
                    t.AddBlockRotationArray(arr);
                    arr = new int[]{
                        0,0,0,0,
                        1,1,1,1,
                        0,0,0,0,
                        0,0,0,0};
                    t.AddBlockRotationArray(arr);
            arr = new int[]{
                        0,0,1,0,
                        0,0,1,0,
                        0,0,1,0,
                        0,0,1,0};
            t.AddBlockRotationArray(arr);
            arr = new int[]{
                        0,0,0,0,
                        0,0,0,0,
                        1,1,1,1,
                        0,0,0,0};
            t.AddBlockRotationArray(arr);

            blockTypes[0] = t;

        //BlockType O

                    t = new BlockType('O');
                                
                    arr = new int[]{
                        2,2,0,0,
                        2,2,0,0,
                        0,0,0,0,
                        0,0,0,0};
                    t.AddBlockRotationArray(arr);
                
                    blockTypes[1] = t;

        //BlockType T
        
                    t = new BlockType('T');
                    
                    arr = new int[]{
                        0,0,0,0,
                        3,3,3,0,
                        0,3,0,0,
                        0,0,0,0};
                    t.AddBlockRotationArray(arr);
                    arr = new int[]{
                        0,3,0,0,
                        3,3,0,0,
                        0,3,0,0,
                        0,0,0,0};
                    t.AddBlockRotationArray(arr);
                    arr = new int[]{
                        0,3,0,0,
                        3,3,3,0,
                        0,0,0,0,
                        0,0,0,0};
                    t.AddBlockRotationArray(arr);
                    arr = new int[]{
                        0,3,0,0,
                        0,3,3,0,
                        0,3,0,0,
                        0,0,0,0};
                    t.AddBlockRotationArray(arr);

                    blockTypes[2] = t;

        //BlockType S
        
                    t = new BlockType('S');
                    
                    arr = new int[]{
                        0,0,0,0,
                        0,4,4,0,
                        4,4,0,0,
                        0,0,0,0};
                    t.AddBlockRotationArray(arr);            
                    arr = new int[]{
                        4,0,0,0,
                        4,4,0,0,
                        0,4,0,0,
                        0,0,0,0};
                    t.AddBlockRotationArray(arr);
                    blockTypes[3] = t;

        //BlockType Z
        
                    t = new BlockType('Z');
                    
                    arr = new int[]{
                        0,0,0,0,
                        5,5,0,0,
                        0,5,5,0,
                        0,0,0,0};
                    t.AddBlockRotationArray(arr);            
                    arr = new int[]{
                        0,5,0,0,
                        5,5,0,0,
                        5,0,0,0,
                        0,0,0,0};
                    t.AddBlockRotationArray(arr);
                    blockTypes[4] = t;

        //BlockType J
        
                    t = new BlockType('J');
                    
                    arr = new int[]{
                        0,6,0,0,
                        0,6,0,0,
                        6,6,0,0,
                        0,0,0,0};
                    t.AddBlockRotationArray(arr);            
                    arr = new int[]{
                        6,0,0,0,
                        6,6,6,0,
                        0,0,0,0,
                        0,0,0,0};
                    t.AddBlockRotationArray(arr);            
                    arr = new int[]{
                        0,6,6,0,
                        0,6,0,0,
                        0,6,0,0,
                        0,0,0,0};
                    t.AddBlockRotationArray(arr);            
                    arr = new int[]{
                        0,0,0,0,
                        6,6,6,0,
                        0,0,6,0,
                        0,0,0,0};
                    t.AddBlockRotationArray(arr);
                    blockTypes[5] = t;

        //BlockType L

                    t = new BlockType('L');
            
                    arr = new int[]{
                        0,7,0,0,
                        0,7,0,0,
                        0,7,7,0,
                        0,0,0,0};
                    t.AddBlockRotationArray(arr);            
                    arr = new int[]{
                        0,0,0,0,
                        7,7,7,0,
                        7,0,0,0,
                        0,0,0,0};
                    t.AddBlockRotationArray(arr);            
                    arr = new int[]{
                        7,7,0,0,
                        0,7,0,0,
                        0,7,0,0,
                        0,0,0,0};
                    t.AddBlockRotationArray(arr);            
                    arr = new int[]{
                        0,0,7,0,
                        7,7,7,0,
                        0,0,0,0,
                        0,0,0,0};
                    t.AddBlockRotationArray(arr);
                    blockTypes[6] = t;

            configInitialized = true;
        }
#endregion
    }
}