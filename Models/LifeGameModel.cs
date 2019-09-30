namespace BayshoreCodingChallenge.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class LifeGameModel
    {
        #region Properties

        public int GenerationNumber { get; set; }

        public bool[] CellBoard { get; set; }

        #endregion

        #region Constants

        public const int GAMEBOARDSIZE = 5;

        public const int CELL_UNDERPOPULATIONLIMIT = 2;

        public const int CELL_OVERPOPULATIONLIMIT = 3;

        #endregion

        #region Static Fields

        public static int GameBoardTotalCells = GAMEBOARDSIZE * GAMEBOARDSIZE;

        private static int GameBoardFirstIndex = 0;

        private static int GameBoardLastIndex = GameBoardTotalCells - 1;

        #endregion

        #region Domain Methods

        /// <summary>
        /// Creates a new game board with random alive and dead cell inputs.
        /// </summary>
        public void GenerateNewCellBoard()
        {
            var newBoard = new bool[GameBoardTotalCells];

            for (int i = 0; i < GameBoardTotalCells; i++)
            {
                newBoard[i] = new Random().NextDouble() >= 0.5;
            }

            CellBoard = newBoard;
            GenerationNumber = 1;
        }

        /// <summary>
        /// Iterates through each cell and evolves them based on neighbors, then sets the board to the new one.
        /// </summary>
        public void EvolveGenerations()
        {
            var evolvedBoard = new bool[GameBoardTotalCells];

            for (int i = 0; i < CellBoard.Length; i++)
            {
                evolvedBoard[i] = GetNewEvolvedCell(CellBoard[i], i);
            }

            CellBoard = evolvedBoard;
            GenerationNumber++;
        }

        /// <summary>
        /// Identity neighbors for cell, then determine it's outcome of whether it should be alive or not
        /// due to underpopulation, overcrowding, suvival, or reproduction.
        /// </summary>
        private bool GetNewEvolvedCell(bool isCellAlive, int currentCellIndex)
        {
            var neighborCells = buildNeighborCellArray(currentCellIndex);
            int numAliveNeighborCells = neighborCells.Count(x => x == true);
            bool shouldCellBeAlive = false;

            // Underpopulation - Any live cell with fewer than two live neighbours dies.
            if (isCellAlive && numAliveNeighborCells < CELL_UNDERPOPULATIONLIMIT)
                shouldCellBeAlive = false;
            // Overcrowding - Any live cell with more than three live neighbours dies.
            else if (isCellAlive && numAliveNeighborCells > CELL_OVERPOPULATIONLIMIT)
                shouldCellBeAlive = false;
            // Survival - Any live cell with two or three live neighbours lives on to the next generation.
            else if (isCellAlive && numAliveNeighborCells >= CELL_UNDERPOPULATIONLIMIT && numAliveNeighborCells <= CELL_OVERPOPULATIONLIMIT)
                shouldCellBeAlive = true;
            // Reproduction - Any dead cell with exactly three live neighbours becomes a live cell.
            else if (isCellAlive == false && numAliveNeighborCells == CELL_OVERPOPULATIONLIMIT)
                shouldCellBeAlive = true;

            return shouldCellBeAlive;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Builds a list of all the surrounding neighbor cells based on the index of the current cell.
        /// </summary>
        private bool[] buildNeighborCellArray(int index)
        {
            var neighborCells = new List<bool>();

            // Determine if the cell is on the east or west edge of the board.
            bool isEastEdge = (index + 1) % GAMEBOARDSIZE == 0;
            bool isWestEdge = index % GAMEBOARDSIZE == 0;

            // Determine if a cell will exist in it's neighboring cells based on the direction
            // of the cell's neighbors - N, NE, E, SE, S, SW, W, NW.
            bool northCellExists = NeighborCellIndex_North() >= GameBoardFirstIndex;
            bool northeastCellExists = NeighborCellIndex_Northeast() >= GameBoardFirstIndex;
            bool eastCellExists = NeighborCellIndex_East() <= GameBoardLastIndex && !isEastEdge;
            bool southeastCellExists = NeighborCellIndex_Southeast() <= GameBoardLastIndex;
            bool southCellExists = NeighborCellIndex_South() <= GameBoardLastIndex;
            bool southwestCellExists = NeighborCellIndex_Southwest() <= GameBoardLastIndex;
            bool westCellExists = NeighborCellIndex_West() >= GameBoardFirstIndex && !isWestEdge;
            bool northwestCellExists = NeighborCellIndex_Northwest() >= GameBoardFirstIndex;

            // North Cell
            if (northCellExists)
                neighborCells.Add(CellBoard[NeighborCellIndex_North()]);

            // East Cell
            if (eastCellExists)
                neighborCells.Add(CellBoard[NeighborCellIndex_East()]);

            // South Cell
            if (southCellExists)
                neighborCells.Add(CellBoard[NeighborCellIndex_South()]);

            // West Cell
            if (westCellExists)
                neighborCells.Add(CellBoard[NeighborCellIndex_West()]);

            // Northeast Cell
            if ((northCellExists && eastCellExists) && northeastCellExists)
                neighborCells.Add(CellBoard[NeighborCellIndex_Northeast()]);

            // Northwest Cell
            if (northCellExists && westCellExists && northwestCellExists)
                neighborCells.Add(CellBoard[NeighborCellIndex_Northwest()]);

            // Southwest Cell
            if (southCellExists && westCellExists && southwestCellExists)
                neighborCells.Add(CellBoard[NeighborCellIndex_Southwest()]);

            // Southeast Cell
            if (southCellExists && eastCellExists && southeastCellExists)
                neighborCells.Add(CellBoard[NeighborCellIndex_Southeast()]);

            int NeighborCellIndex_North() => index - GAMEBOARDSIZE;

            int NeighborCellIndex_Northeast() => index - (GAMEBOARDSIZE - 1);

            int NeighborCellIndex_East() => index + 1;

            int NeighborCellIndex_Southeast() => index + (GAMEBOARDSIZE + 1);

            int NeighborCellIndex_South() => index + GAMEBOARDSIZE;

            int NeighborCellIndex_Southwest() => index + (GAMEBOARDSIZE - 1);

            int NeighborCellIndex_West() => index - 1;

            int NeighborCellIndex_Northwest() => index - (GAMEBOARDSIZE + 1);

            return neighborCells.ToArray();
        }

        #endregion
    }
}