using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH {
    public class Bishop: Piece {

        public override List<Cell> GetAvailableCells() {
            List<Cell> cells = new List<Cell>();

            /***
             * We need to search for available cells to move or eat
             * in 4 directions : up-left, up-right, down-left, down-right
             * The Bishop is free to move from any number of cells across 
             * the board
             */
            List<Cell> c = SearchDiagonalUpLeft();
            foreach (Cell cell in c) {
                cells.Add(cell);
            }

            c = SearchDiagonalUpRight();
            foreach (Cell cell in c) {
                cells.Add(cell);
            }

            c = SearchDiagonalDownLeft();
            foreach (Cell cell in c) {
                cells.Add(cell);
            }

            c = SearchDiagonalDownRight();
            foreach (Cell cell in c) {
                cells.Add(cell);
            }

            return cells;
        }
    }
}
