using System.Collections;
using System;


namespace Tetris
{
    public class Field
    {
        int[] field;

        private Block activeBlock;
        private Block ghostBlock;
        private Block holdBlock;
        private Block holdGhostBlock;
        private bool holdSwitchAvailable;
        private ArrayList blockBag;
        private Random rand;
        private readonly string BLOCKDISTRIB = "IJLOSTZ";
        private int linesCleared;
        private bool gameOver;
        private bool ghostBlockActive;
        
        public Field(){
            //initialize new empty field
            InitializeField();

            //initialize random number generator with current time as starting seed
            rand = new Random(System.DateTime.Now.Millisecond);

            //initialize blog bag
            blockBag = new ArrayList();
            GenerateBlockBag();
            NextBlock();
        }

        private void InitializeField(){
            ghostBlockActive = true;
            holdSwitchAvailable = true;
            //borders left and right -> that makes the collision check really easy
            field = new int[Config.GetFieldHeight()*Config.GetFieldWidth()];
            for(int iii = 0; iii < Config.GetFieldHeight(); iii++){
                field[iii*Config.GetFieldWidth()] = 9;
                field[iii*Config.GetFieldWidth()+Config.GetFieldWidth()-1] = 9;
            }            

            //3 fake rows on bottom to avoid index out of bounds exception
            for(int iii = 0; iii < Config.GetFieldWidth(); iii++){
                field[(Config.GetFieldHeight()-1)*Config.GetFieldWidth()+iii] = 9;
                field[(Config.GetFieldHeight()-2)*Config.GetFieldWidth()+iii] = 9;
                field[(Config.GetFieldHeight()-3)*Config.GetFieldWidth()+iii] = 9;
            }
        }

        //Temporary - please remove!!!!!!
        public Block GetGhostBlock()
        {
            return ghostBlock;
        }

        public void SetGhost(bool b) { ghostBlockActive = b; }
        public bool GetGhost() { return ghostBlockActive; }

        private void GenerateBlockBag(){
            if(blockBag.Count > 0) blockBag.Clear();

            string remainingBlocks = BLOCKDISTRIB;

            while(remainingBlocks.Length > 0){

                int randomNumber = rand.Next()%remainingBlocks.Length;
                char newBlockChar = remainingBlocks[randomNumber];

                blockBag.Add(new Block(Config.GetBlockType(newBlockChar)));

                //remove added block from list of available types
                remainingBlocks = remainingBlocks.Replace(newBlockChar.ToString(),"");     

            }
        }

        private void NextBlock(){
            activeBlock = (Block)blockBag[0];
            ghostBlock = new Block(activeBlock.GetBlockType(), true);
            blockBag.RemoveAt(0);
            if(blockBag.Count == 0)
                GenerateBlockBag();
            UpdateGhostBlock();
        }

        private Block GetNextBlock(){
            return (Block)blockBag[0];
        }

        public int[] GetFieldArray(){
            return GetMergedArray();
        }

        public int GetClearedLines(){return linesCleared;}


        private bool CollisionCheck(Block b)
        {
            return CollisionCheck(b, false);
        }

        private bool CollisionCheck(Block b, bool down){
            int[] bArr = b.GetBlockArray();

            for(int xxx = 0; xxx < 4; xxx++){
                for(int yyy = 0; yyy < 4; yyy++){
                    if(bArr[yyy*4+xxx] != 0 && field[b.GetLocation()+xxx+yyy*Config.GetFieldWidth()] > 0){
                        if(down && !b.GhostModeEnabled() && b.GetLocation()/Config.GetFieldWidth() <= 2)
                            gameOver = true;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool GetGameOver(){return gameOver;}

        public void Restart(){
            InitializeField();
            GenerateBlockBag();
            NextBlock();
            linesCleared = 0;
            gameOver = false;
        }

        public bool MoveDown(){
            activeBlock.MoveDown(1);
            if(CollisionCheck(activeBlock,true)){
                activeBlock.MoveUp(1);
                MergeBlockWithField(activeBlock);
                NextBlock();
                return false;
            }
            return true;                
        }

        public bool MoveLeft(){
            activeBlock.MoveLeft(1);
            if(CollisionCheck(activeBlock)){
                activeBlock.MoveRight(1);
                return false;
            }
            if (ghostBlockActive) UpdateGhostBlock();
            return true;        
        }

        public bool MoveRight(){
            activeBlock.MoveRight(1);
            if(CollisionCheck(activeBlock)){
                activeBlock.MoveLeft(1);
                return false;
            }
            if(ghostBlockActive) UpdateGhostBlock();
            return true;        
        }

        public bool RotateRight(){
            activeBlock.RotateRight();
            if(CollisionCheck(activeBlock)){
                activeBlock.RotateLeft();
                return false;
            }
            if (ghostBlockActive) UpdateGhostBlock();
            return true;        
        }

        public bool RotateLeft()
        {
            activeBlock.RotateLeft();
            if (CollisionCheck(activeBlock))
            {
                activeBlock.RotateRight();
                return false;
            }
            if (ghostBlockActive) UpdateGhostBlock();
            return true;
        }

        public void LockBlock()
        {
            while (MoveDown()) ;
        }

        private void UpdateGhostBlock()
        {
            ghostBlock.SetLocation(activeBlock.GetLocation());
            ghostBlock.SetRotation(activeBlock.GetRotation());
            bool movedDown = false;
            while (!CollisionCheck(ghostBlock))
            {
                ghostBlock.MoveDown(1);
                movedDown = true;
            }
            if(movedDown)
                ghostBlock.MoveUp(1);
        }

        private void MergeBlockWithField(Block b){
            int[] bArr = b.GetBlockArray();

            for(int xxx = 0; xxx < 4; xxx++){
                for(int yyy = 0; yyy < 4; yyy++){
                    if(bArr[yyy * 4 + xxx] != 0)
                        field[b.GetLocation()+xxx+yyy*Config.GetFieldWidth()] = bArr[yyy*4+xxx];
                }
            }

            linesCleared = 0;
            int lineIndex;
            for(int yyy = 0; yyy < 4; yyy++){
                lineIndex = b.GetLocation()/Config.GetFieldWidth() + yyy;
                if(LineFull(lineIndex)){
                    RemoveLine(lineIndex);
                    linesCleared++;
                }
            }

            holdSwitchAvailable = true;
        }

        public bool SwitchActiveBlockWithHold()
        {
            if (holdSwitchAvailable)
            {
                if(holdBlock is null)
                {
                    holdBlock = activeBlock;
                    if (ghostBlockActive) holdGhostBlock = ghostBlock;
                    NextBlock();
                    holdSwitchAvailable = false;
                    holdBlock.SetLocation(4);
                    return true;
                }
                else
                {
                    Block tempBlock = activeBlock;
                    activeBlock = holdBlock;
                    holdBlock = tempBlock;

                    if (ghostBlockActive)
                    {
                        Block tempGhostBlock = ghostBlock;
                        ghostBlock = holdGhostBlock;
                        holdGhostBlock = tempGhostBlock;
                        UpdateGhostBlock();
                    }


                    holdSwitchAvailable = false;
                    holdBlock.SetLocation(4);
                    return true;
                }
            }
            return false;
        }

        private bool LineFull(int index){
            if(index >= Config.GetFieldHeight() - 3)
                return false;
            for(int xxx = 1; xxx <= 10; xxx++){
                if(field[Config.GetFieldWidth()*index+xxx] == 0){
                    return false;
                }
            }
            return true;
        }

        //removes a line - all lines above move one down
        public void RemoveLine(int index){
            for(int iii = index; iii > 0; iii--){
                for(int xxx = 1; xxx <= 10; xxx++){
                    field[iii*Config.GetFieldWidth()+xxx] = field[(iii-1)*Config.GetFieldWidth()+xxx];
                }
            }
        }

        private int[] GetMergedArray(){
            int[] bArr = activeBlock.GetBlockArray();
            int[] returnArray = (int[])field.Clone();
            int[] gArr = ghostBlock.GetBlockArray();

            for (int xxx = 0; xxx < 4; xxx++){
                for(int yyy = 0; yyy < 4; yyy++)
                {
                    if (ghostBlockActive && gArr[yyy * 4 + xxx] != 0)
                        returnArray[ghostBlock.GetLocation() + xxx + yyy * Config.GetFieldWidth()] = gArr[yyy * 4 + xxx];
                    if (bArr[yyy * 4 + xxx] != 0)
                        returnArray[activeBlock.GetLocation() + xxx + yyy * Config.GetFieldWidth()] = bArr[yyy * 4 + xxx];
                }
            }



            return returnArray;
        }


        //translates the 4x4 block int array into a complete field array representation of the block (only the block)
        private int[] TranslateBlock(Block b){
            int[] r = new int[Config.GetFieldHeight()*Config.GetFieldWidth()];
            int[] bArr = b.GetBlockArray();
            for(int xxx = 0; xxx < 4; xxx++){
                for(int yyy = 0; yyy < 4; yyy++){
                    r[b.GetLocation()+xxx+yyy*Config.GetFieldWidth()] = bArr[yyy*4+xxx];
                }
            }
            return r;
        }

        //method not safe, use with caution
        private int GetVal(int x, int y){
            return field[y*Config.GetFieldWidth()+x];
        }

        //returns the field + block
        public override string ToString(){
            string r = "";
            int[] mergedArray = GetMergedArray();
            for(int yyy = 0; yyy < Config.GetFieldHeight(); yyy++){
                for(int xxx = 0; xxx < Config.GetFieldWidth(); xxx++){
                    r+=mergedArray[yyy * Config.GetFieldWidth() + xxx];
                }
                r+="\n";
            }
            return r;
        }

    }
}