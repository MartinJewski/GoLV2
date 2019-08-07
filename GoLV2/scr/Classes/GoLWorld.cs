using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLV2.scr.Classes
{
    class GoLWorld
    {
        private int[,] mFirstGen;
        private int[,] mSecGen;
        private int mRows;
        private int mColumns;

        /// <summary>
        /// Initiliazes a wold with size rows x columns
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        public GoLWorld(int rows, int columns)
        {
            mRows = rows;
            mColumns = columns;
            mFirstGen = new int[rows, columns];
            mSecGen = new int[rows, columns];

            initializeWorld();
        }

        /// <summary>
        /// Initialized a wold of size 30x30
        /// </summary>
        public GoLWorld()
        {
            mRows = 30;
            mColumns = 30;
            mFirstGen = new int[mRows, mColumns];
            mSecGen = new int[mRows, mColumns];

            initializeWorld();
        }
        /// <summary>
        /// fills both arrays with 0 for initialization
        /// </summary>
        private void initializeWorld()
        {
            int i;
            int j;
            for (i = 0; i < mRows; i++)
            {
                for (j = 0; j < mColumns; j++)
                {
                    mFirstGen[i, j] = 0;
                    mSecGen[i, j] = 0;
                }
            }
        }

        public void evolveFirstGen()
        {
            int i;
            int j;
            for (i = 0; i < mRows; i++)
            {
                for (j = 0; j < mColumns; j++)
                {
                    int c = checkNeighbours(i, j);
                    applyRuleSet(i, j, c);
                }
            }
        }

        /// <summary>
        /// Checks all potions around a given position and
        /// counts if the position value is 1
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public int checkNeighbours(int row, int column)
        {
            int neighbourCount = 0;
            if (isInsideWold(row - 1, column - 1))
            {
                if (mFirstGen[row - 1, column - 1] == 1) neighbourCount++;
            } //1
            if (isInsideWold(row - 1, column))
            {
                if (mFirstGen[row - 1, column] == 1) neighbourCount++;
            } //2
            if (isInsideWold(row - 1, column + 1))
            {
                if (mFirstGen[row - 1, column + 1] == 1) neighbourCount++;
            } //3
            if (isInsideWold(row, column - 1))
            {
                if (mFirstGen[row, column - 1] == 1) neighbourCount++;
            }//4

            if (isInsideWold(row, column + 1))
            {
                if (mFirstGen[row, column + 1] == 1) neighbourCount++;
            }//6
            if (isInsideWold(row + 1, column - 1))
            {
                if (mFirstGen[row + 1, column - 1] == 1) neighbourCount++;
            }//7
            if (isInsideWold(row + 1, column))
            {
                if (mFirstGen[row + 1, column] == 1) neighbourCount++;
            }//8
            if (isInsideWold(row + 1, column + 1))
            {
                if (mFirstGen[row + 1, column + 1] == 1) neighbourCount++;
            }//9
            // 1 | 2 | 3
            //-----------
            // 4 | x | 6
            //-----------
            // 7 | 8 | 9
            return neighbourCount;
        }

        /// <summary>
        /// applies the Game of Live rule set and saves the result inside the secGen
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="neighbours"></param>
        private void applyRuleSet(int row, int column, int neighbours)
        {
            //dead+3 neighbours => revive
            if (mFirstGen[row, column] == 0 && neighbours == 3)
            {
                mSecGen[row, column] = 1;
            }
            //die bc of loneliness
            if (mFirstGen[row, column] == 1 && neighbours < 2)
            {
                mSecGen[row, column] = 0;
            }
            //alive + 2or 3 neighbours => keep living
            if (mFirstGen[row, column] == 1 && (neighbours == 2 || neighbours == 3))
            {
                mSecGen[row, column] = 1;
            }
            //die because of overpopulation
            if (mFirstGen[row, column] == 1 && neighbours > 3)
            {
                mSecGen[row, column] = 0;
            }
        }

        /// <summary>
        /// Updates the current generation
        /// </summary>
        public void nextGen()
        {
            for (int i = 0; i < mRows; i++)
            {
                for (int j = 0; j < mColumns; j++)
                {
                    mFirstGen[i, j] = mSecGen[i, j];
                }
            }
        }


        /// <summary>
        /// Checks if a given position is inisde the wold
        /// </summary>
        /// <param name="row">row position</param>
        /// <param name="column">column position</param>
        /// <returns>true if inside, else false</returns>
        private bool isInsideWold(int row, int column)
        {
            if (row >= 0 && row < mRows)
            {
                if (column >= 0 && column < mColumns)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// displays the first or second generation
        /// </summary>
        /// <param name="gen">1 for first gen, 2 for second gen</param>
        public void showWorld(int gen)
        {
            int i;
            int j;

            System.Console.WriteLine("-------------------------");
            if (gen > 0 && gen < 3) //allow only 1 or 2
            {
                if (gen == 1)
                {
                    for (i = 0; i < mRows; i++)
                    {
                        for (j = 0; j < mColumns; j++)
                        {
                            System.Console.Write(string.Format("{0} ", mFirstGen[i, j]));
                        }
                        Console.Write(Environment.NewLine + Environment.NewLine);
                    }
                    i = 0;
                    j = 0;
                    //Console.ReadKey(); //dont let the console close itself
                }
                else
                {
                    for (i = 0; i < mRows; i++)
                    {
                        for (j = 0; j < mColumns; j++)
                        {
                            System.Console.Write(string.Format("{0} ", mSecGen[i, j]));
                        }
                        Console.Write(Environment.NewLine + Environment.NewLine);
                    }
                    i = 0;
                    j = 0;
                    //Console.ReadKey();//dont let the console close itself
                }
            }
        }
        /// <summary>
        /// Change the value at position [row,column] in the first generation
        /// </summary>
        /// <param name="row">row position</param>
        /// <param name="column">column position</param>
        /// <param name="val">value 0 or 1</param>
        public void setValue(int row, int column, int val)
        {
            //set only possible if row/column in range and value 0/1
            if (val == 0 || val == 1)
            {

                if (row < mRows)
                {
                    if (column < mColumns)
                    {
                        mFirstGen[row, column] = val;
                    }
                }
            }
        }

        /// <summary>
        /// Returns wolds max row length
        /// </summary>
        /// <returns>max row length</returns>
        public int getRowLength()
        {
            return mRows;
        }

        /// <summary>
        /// returns wolds max column length
        /// </summary>
        /// <returns>max column length</returns>
        public int getColumnLength()
        {
            return mColumns;
        }

        /// <summary>
        /// Returns the current generation
        /// </summary>
        /// <returns>current generation</returns>
        public int[,] getWold()
        {
            return mFirstGen;
        }
    }
}
