using System;
using System.Collections.Generic;

namespace Match3
{
    // Basic Match-3 board
    public class Match3Board
    {
        public int Rows;        // number of rows
        public int Cols;        // number of columns
        public int ColorsCount; // how many different tile colors

        private int[,] grid;    // the board itself
        private Random rng;     // random generator
        private const int EMPTY = -1;

        public Match3Board(int rows, int cols, int colorsCount, int? seed = null)
        {
            Rows = rows;
            Cols = cols;
            ColorsCount = colorsCount;

            grid = new int[Rows, Cols];
            rng = seed.HasValue ? new Random(seed.Value) : new Random();

            FillStartBoard();
        }

        // return the board state
        public int[,] GetBoard() => (int[,])grid.Clone();

        // try swapping two cells
        public bool TrySwap(Coord a, Coord b, out int cleared, out int cascades)
        {
            cleared = 0;
            cascades = 0;

            if (!AreAdjacent(a, b)) return false;

            Swap(a, b);

            var matches = FindMatches();
            if (matches.Count == 0)
            {
                // no match -> revert
                Swap(a, b);
                return false;
            }

            // keep clearing until stable
            while (matches.Count > 0)
            {
                cleared += Clear(matches);
                ApplyGravity();
                Refill();
                cascades++;

                matches = FindMatches();
            }

            return true;
        }

        // --- Board creation and refill ---

        private void FillStartBoard()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    grid[r, c] = PickSafeColor(r, c);
                }
            }
        }

        private void Refill()
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Cols; c++)
                    if (grid[r, c] == EMPTY)
                        grid[r, c] = rng.Next(ColorsCount);
        }

        // --- Matching / clearing / gravity ---

        private List<Coord> FindMatches()
        {
            var toClear = new List<Coord>();

            // horizontal
            for (int r = 0; r < Rows; r++)
            {
                int run = 1;
                for (int c = 1; c < Cols; c++)
                {
                    if (grid[r, c] != EMPTY && grid[r, c] == grid[r, c - 1])
                    {
                        run++;
                        if (c == Cols - 1 && run >= 3)
                            for (int k = c - run + 1; k <= c; k++) toClear.Add(new Coord(r, k));
                    }
                    else
                    {
                        if (run >= 3)
                            for (int k = c - run; k < c; k++) toClear.Add(new Coord(r, k));
                        run = 1;
                    }
                }
            }

            // vertical
            for (int c = 0; c < Cols; c++)
            {
                int run = 1;
                for (int r = 1; r < Rows; r++)
                {
                    if (grid[r, c] != EMPTY && grid[r, c] == grid[r - 1, c])
                    {
                        run++;
                        if (r == Rows - 1 && run >= 3)
                            for (int k = r - run + 1; k <= r; k++) toClear.Add(new Coord(k, c));
                    }
                    else
                    {
                        if (run >= 3)
                            for (int k = r - run; k < r; k++) toClear.Add(new Coord(k, c));
                        run = 1;
                    }
                }
            }

            return toClear;
        }

        private int Clear(List<Coord> matches)
        {
            int count = 0;
            foreach (var pos in matches)
            {
                if (grid[pos.R, pos.C] != EMPTY)
                {
                    grid[pos.R, pos.C] = EMPTY;
                    count++;
                }
            }
            return count;
        }

        private void ApplyGravity()
        {
            for (int c = 0; c < Cols; c++)
            {
                int writeRow = Rows - 1;
                for (int r = Rows - 1; r >= 0; r--)
                {
                    if (grid[r, c] != EMPTY)
                    {
                        grid[writeRow, c] = grid[r, c];
                        if (writeRow != r) grid[r, c] = EMPTY;
                        writeRow--;
                    }
                }
            }
        }

        // --- Helpers ---

        private int PickSafeColor(int r, int c)
        {
            while (true)
            {
                int color = rng.Next(ColorsCount);

                // avoid starting with a 3 in a row
                if (c >= 2 && grid[r, c - 1] == color && grid[r, c - 2] == color)
                    continue;
                if (r >= 2 && grid[r - 1, c] == color && grid[r - 2, c] == color)
                    continue;

                return color;
            }
        }

        private bool AreAdjacent(Coord a, Coord b) =>
            Math.Abs(a.R - b.R) + Math.Abs(a.C - b.C) == 1;

        private void Swap(Coord a, Coord b)
        {
            int tmp = grid[a.R, a.C];
            grid[a.R, a.C] = grid[b.R, b.C];
            grid[b.R, b.C] = tmp;
        }
    }

    // simple coordinate struct
    public struct Coord
    {
        public int R, C;
        public Coord(int r, int c) { R = r; C = c; }
        public override string ToString() => $"({R},{C})";
    }
}

