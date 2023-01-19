using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH {
    public class Queen : Piece {

        public override List<Cell> GetAvailableCells() {
            List<Cell> cells = new List<Cell>();

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

            c = SearchDiagonalUpLeft();
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
