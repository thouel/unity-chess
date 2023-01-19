using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH {
    public class Rook : Piece {

        public override List<Cell> GetAvailableCells() {
            List<Cell> cells = new List<Cell>();

            /***
             * We need to search for available cells to move or eat
             * in 4 directions : up, down, left, right
             * The Rook is free to move from any number of cells across 
             * the board
             */
            List<Cell> c = SearchUp();
            foreach (Cell cell in c) {
                cells.Add(cell);
            }

            c = SearchDown();
            foreach (Cell cell in c) {
                cells.Add(cell);
            }

            c = SearchLeft();
            foreach (Cell cell in c) {
                cells.Add(cell);
            }

            c = SearchRight();
            foreach (Cell cell in c) {
                cells.Add(cell);
            }

            return cells;
        }

        private List<Cell> DetectPossibleRoques() {
            // If the Rook is on his starting row
            // And the next Piece on the row is the King (that nevers moved)
            // Then the Roque is a legal move
            return null;
        }
    }
}
