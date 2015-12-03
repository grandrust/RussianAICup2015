using System;
using System.Collections.Generic;
using System.Diagnostics;
using Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk.Model;
using GranDrust.GameEntities;
using GranDrust.Helpers;

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


        private IList<Cell> _queue;
        private IList<int> _parentIndexs;
        private readonly Cell _terminatePoint;
        private readonly Cell _startPoint;

        private Cell _excludeCell;

        private readonly int _gridWidth;
        private readonly int _gridHeight;
        
        private readonly Vehicle _vehicle;

        private int StartIndex
        {
            get { return _excludeCell == null ? 0 : 1; }
        }

        public BFSearch(Vehicle vehicle, Cell start, Cell terminate)
        {
            _vehicle = vehicle;
            _terminatePoint = terminate;
            _startPoint = start;

            _gridWidth = vehicle.World.Width;
            _gridHeight = vehicle.World.Height;
        }

        public IList<Cell> Search(int limit = 400, Cell excludeCell = null)
        {
            var searchLimit = limit;
            _excludeCell = excludeCell;

            var current = _startPoint;
            var index = StartIndex;

            _queue = index > 0 
                        ? new List<Cell> { _excludeCell, current } 
                        : new List<Cell> { current };

            _parentIndexs = index > 0
                ? new List<int> {0, index}
                : new List<int> {index};

            var carCenter = _vehicle.Self.CurrentPoint();
            var nosePoint = _vehicle.NosePoint();
            var isRight = carCenter.X - nosePoint.X < 0.0D;
            var isBottom = carCenter.Y - nosePoint.Y < 0.0D;

            if (_terminatePoint.Equals(new Cell(1,3)))
                Console.WriteLine();


            while (!current.Equals(_terminatePoint) && index < searchLimit)
            {
                if (Math.Abs(_vehicle.Self.SpeedX) > Math.Abs(_vehicle.Self.SpeedY))
                    HorizontalStep(isRight, isBottom, index, current);
                else
                    VerticalStep(isRight, isBottom, index, current);

                index++;

                if (index >= _queue.Count) break;

                current = _queue[index];
            }

            return CreateRoute(index);
        }

        private void VerticalStep(bool isRight, bool isBottom, int index, Cell current)
        {
            if (isBottom)
                AddBottom(index, current);
            else
                AddTop(index, current);
            
            if (isRight)
                AddRight(index, current);
            else
                AddLeft(index, current);


            if (!isBottom)
                AddBottom(index, current);
            else
                AddTop(index, current);

            if (!isRight)
                AddRight(index, current);
            else
                AddLeft(index, current);

        }

        private void HorizontalStep(bool isRight, bool isBottom, int index, Cell current)
        {
            if (isRight)
                AddRight(index, current);
            else
                AddLeft(index, current);

            if (isBottom)
                AddBottom(index, current);
            else
                AddTop(index, current);


            if (!isRight)
                AddRight(index, current);
            else
                AddLeft(index, current);

            if (!isBottom)
                AddBottom(index, current);
            else
                AddTop(index, current);
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
            var index = (indx > StartIndex && indx < _queue.Count) ? indx : _queue.Count - 1;

            while (index > StartIndex)
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
            if (newCell.Equals(_excludeCell))
            {
                _queue[0] = new Cell(-1,-1);
            }
        }

        private bool CanAddCell(Cell newCell)
        {
            return InGridRange(newCell);
        }

        private bool InGridRange(Cell cell)
        {
            if (cell.X < 0 || cell.X >= _gridWidth) return false;
            if (cell.Y < 0 || cell.Y >= _gridHeight) return false;

            return true;
        }
    }
}
