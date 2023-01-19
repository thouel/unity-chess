using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH {
    public class Knight: Piece {

        public override List<Cell> GetAvailableCells() {
            List<Cell> cells = new List<Cell>();
            /**
             * We need to search in the following cells :
             *   up + diagonal left
             *   up + diagonal right
             *   left + diagonal left
             *   left + diagonal right
             *   down + diagonal left
             *   down + diagonal right
             *   right + diagonal left
             *   right + diagonal right
             */

            // up + diagonal left
            if (row + 1 < 8 && row + 2 < 8 && col - 1 >= 0) {
                Piece p = board.GetPieceAt(col - 1, row + 2);
                if (p == null) {
                    // The cell is unoccupied, we can add it
                    cells.Add(board.GetCell(col - 1, row + 2));
                } else {
                    if (!IsOneOfMine(p)) {
                        // The cell is occupied by an opponent piece,
                        // we can add it to the available moves
                        cells.Add(board.GetCell(col - 1, row + 2));
                    }
                }
            }

            // up + diagonal right
            if (row + 1 < 8 && row + 2 < 8 && col + 1 < 8) {
                Piece p = board.GetPieceAt(col + 1, row + 2);
                if (p == null) {
                    // The cell is unoccupied, we can add it
                    cells.Add(board.GetCell(col + 1, row + 2));
                } else {
                    if (!IsOneOfMine(p)) {
                        // The cell is occupied by an opponent piece,
                        // we can add it to the available moves
                        cells.Add(board.GetCell(col + 1, row + 2));
                    }
                }
            }

            // left + diagonal left
            if (col - 1 >= 0 && col - 2 >= 0 && row - 1 >= 0) {
                Piece p = board.GetPieceAt(col - 2, row - 1);
                if (p == null) {
                    // The cell is unoccupied, we can add it
                    cells.Add(board.GetCell(col - 2, row - 1));
                } else {
                    if (!IsOneOfMine(p)) {
                        // The cell is occupied by an opponent piece,
                        // we can add it to the available moves
                        cells.Add(board.GetCell(col - 2, row - 1));
                    }
                }
            }

            // left + diagonal right
            if (col - 1 >= 0 && col - 2 >= 0 && row + 1 < 8) {
                Piece p = board.GetPieceAt(col - 2, row + 1);
                if (p == null) {
                    // The cell is unoccupied, we can add it
                    cells.Add(board.GetCell(col - 2, row + 1));
                } else {
                    if (!IsOneOfMine(p)) {
                        // The cell is occupied by an opponent piece,
                        // we can add it to the available moves
                        cells.Add(board.GetCell(col - 2, row + 1));
                    }
                }
            }

            // down + diagonal left
            if (row - 1 >= 0 && col - 1 >= 0 && row - 2 >= 0) {
                Piece p = board.GetPieceAt(col - 1, row - 2);
                if (p == null) {
                    // The cell is unoccupied, we can add it
                    cells.Add(board.GetCell(col - 1, row - 2));
                } else {
                    if (!IsOneOfMine(p)) {
                        // The cell is occupied by an opponent piece,
                        // we can add it to the available moves
                        cells.Add(board.GetCell(col - 1, row - 2));
                    }
                }
            }

            // down + diagonal right
            if (row - 1 >= 0 && col + 1 < 8 && row - 2 >= 0) {
                Piece p = board.GetPieceAt(col + 1, row - 2);
                if (p == null) {
                    // The cell is unoccupied, we can add it
                    cells.Add(board.GetCell(col + 1, row - 2));
                } else {
                    if (!IsOneOfMine(p)) {
                        // The cell is occupied by an opponent piece,
                        // we can add it to the available moves
                        cells.Add(board.GetCell(col + 1, row - 2));
                    }
                }
            }

            // right + diagonal left
            if (col + 1 < 8 && col + 2 < 8 && row + 1 < 8) {
                Piece p = board.GetPieceAt(col + 2, row + 1);
                if (p == null) {
                    // The cell is unoccupied, we can add it
                    cells.Add(board.GetCell(col + 2, row + 1));
                } else {
                    if (!IsOneOfMine(p)) {
                        // The cell is occupied by an opponent piece,
                        // we can add it to the available moves
                        cells.Add(board.GetCell(col + 2, row + 1));
                    }
                }
            }

            // right + diagonal right
            if (col + 1 < 8 && col + 2 < 8 && row - 1 >= 0) {
                Piece p = board.GetPieceAt(col + 2, row - 1);
                if (p == null) {
                    // The cell is unoccupied, we can add it
                    cells.Add(board.GetCell(col + 2, row - 1));
                } else {
                    if (!IsOneOfMine(p)) {
                        // The cell is occupied by an opponent piece,
                        // we can add it to the available moves
                        cells.Add(board.GetCell(col + 2, row - 1));
                    }
                }
            }

            return cells;
        }
    }
}
