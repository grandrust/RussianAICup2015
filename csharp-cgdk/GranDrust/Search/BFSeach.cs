using System;
using System.Collections.Generic;
using System.Diagnostics;
using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;
using GranDrust.GameEntities;

namespace GranDrust.Search
{
    // ReSharper disable once InconsistentNaming
    public class BFSearch
    {
        public class Cell
        {
            public int X;
            public int Y;

            public override string ToString()
            {
                return string.Format("Cell({0},{1})", X, Y);
            }

            [DebuggerStepThrough]
            public Cell()
            {
                X = -1;
                Y = -1;
            }

            [DebuggerStepThrough]
            public Cell(int x, int y)
            {
                X = x;
                Y = y;
            }

            public override bool Equals(object obj)
            {
                var cell = obj as Cell;
                if (cell == null) return false;

                return (X == cell.X) && (Y == cell.Y);
            }

            public override int GetHashCode()
            {
                return X ^ Y;
            }
        }


        IList<Cell> _queue;
        IList<int> _parentIndexs;
        Cell terminatePoint;
        Cell startPoint;

        int gridWidth;
        int gridHeight;
        
        private Vehicle _vehicle;

        public BFSearch(Vehicle vehicle, Cell start, Cell terminate)
        {
            _vehicle = vehicle;
            terminatePoint = terminate;
            startPoint = start;

            gridWidth = vehicle.World.Width;
            gridHeight = vehicle.World.Height;
        }

        public IList<Cell> Search(int limit = 400)
        {
            var searchLimit = limit;

            var current = startPoint;
            var index = 0;

            _queue = new List<Cell> { current };
            _parentIndexs = new List<int> { index };

            while (!current.Equals(terminatePoint) && index < searchLimit)
            {
                if (Math.Abs(terminatePoint.X - startPoint.X) > Math.Abs(terminatePoint.Y - startPoint.Y))
                {
                    AddRight(index, current);
                    AddTop(index, current);
                    AddLeft(index, current);
                    AddBottom(index, current);
                }
                else
                {
                    AddTop(index, current);
                    AddRight(index, current);
                    AddBottom(index, current);
                    AddLeft(index, current);
                }

                index++;

                if (index >= _queue.Count) break;

                current = _queue[index];
            }

            return CreateRoute(index);
        }

        private void AddTop(int index, Cell current)
        {
            switch (_vehicle.World.TilesXY[current.X][current.Y])
            {
                case TileType.Vertical:
                case TileType.Crossroads:
                case TileType.LeftBottomCorner:
                case TileType.RightBottomCorner:
                case TileType.TopHeadedT:
                case TileType.LeftHeadedT:
                case TileType.RightHeadedT:
                    AddCellToMap(index, new Cell {X = current.X, Y = current.Y - 1});
                    break;
            }
        }

        private void AddLeft(int index, Cell current)
        {
            switch (_vehicle.World.TilesXY[current.X][current.Y])
            {
                case TileType.Horizontal:
                case TileType.Crossroads:
                case TileType.RightTopCorner:
                case TileType.RightBottomCorner:
                case TileType.TopHeadedT:
                case TileType.LeftHeadedT:
                case TileType.BottomHeadedT:
                    AddCellToMap(index, new Cell { X = current.X - 1, Y = current.Y });
                    break;
            }
        }

        private void AddBottom(int index, Cell current)
        {
            switch (_vehicle.World.TilesXY[current.X][current.Y])
            {
                case TileType.Vertical:
                case TileType.Crossroads:
                case TileType.RightTopCorner:
                case TileType.LeftTopCorner:
                case TileType.RightHeadedT:
                case TileType.LeftHeadedT:
                case TileType.BottomHeadedT:
                    AddCellToMap(index, new Cell { X = current.X, Y = current.Y + 1 });
                    break;
            }

        }

        private void AddRight(int index, Cell current)
        {
            switch (_vehicle.World.TilesXY[current.X][current.Y])
            {
                case TileType.Horizontal:
                case TileType.Crossroads:
                case TileType.LeftTopCorner:
                case TileType.LeftBottomCorner:
                case TileType.RightHeadedT:
                case TileType.TopHeadedT:
                case TileType.BottomHeadedT:
                    AddCellToMap(index, new Cell { X = current.X + 1, Y = current.Y });
                    break;
            }
        }

        private IList<Cell> CreateRoute(int indx)
        {
            var route = new List<Cell>();
            var index = (indx > 0 && indx < _queue.Count) ? indx : _queue.Count - 1;

            while (index > 0)
            {
                route.Add(_queue[index]);
                index = _parentIndexs[index];
            }

            route.Reverse();
            return route;
        }

        private void AddCellToMap(int index, Cell newCell)
        {
            if (CanAddCell(newCell) && !_queue.Contains(newCell))
            {
                _queue.Add(newCell);
                _parentIndexs.Add(index);
            }
        }

        private bool CanAddCell(Cell newCell)
        {
            return InGridRange(newCell);
        }

        private bool InGridRange(Cell cell)
        {
            if (cell.X < 0 || cell.X >= gridWidth) return false;
            if (cell.Y < 0 || cell.Y >= gridHeight) return false;

            return true;
        }
    }
}
