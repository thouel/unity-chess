using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH {
    public class King: Piece {

        public List<Piece> threats = new List<Piece>();
        public List<Cell> forbiddenCells = new List<Cell>();

        public bool firstMove = true;

        public void UpdateThreats() {
            threats.Clear();
            forbiddenCells.Clear();

            // Find threats in all directions
            #region Find threats in all directions
            // Search up
            if (row + 1 < 8) {
                for (int j = row + 1; j < 8; j++) {
                    List<Cell> fc = GetForbiddenCells(col, j);
                    if (fc != null && fc.Count > 0) {
                        threats.Add(board.GetPieceAt(col, j));
                        foreach (Cell c in fc) {
                            forbiddenCells.Add(c);
                        }
                    }
                }
            }

            // Search up-right
            if (row + 1 < 8 && col + 1 < 8) {
                for (int i = col + 1, j = row + 1; i < 8 && j < 8; i++, j++) {
                    List<Cell> fc = GetForbiddenCells(i, j);
                    if (fc != null && fc.Count > 0) {
                        threats.Add(board.GetPieceAt(i, j));
                        foreach (Cell c in fc) {
                            forbiddenCells.Add(c);
                        }
                    }
                }
            }

            // Search right
            if (col + 1 < 8) {
                for (int i = col + 1; i < 8; i++) {
                    List<Cell> fc = GetForbiddenCells(i, row);
                    if (fc != null && fc.Count > 0) {
                        threats.Add(board.GetPieceAt(i, row));
                        foreach (Cell c in fc) {
                            forbiddenCells.Add(c);
                        }
                    }
                }
            }

            // Search down-right
            if (row - 1 >= 0 && col + 1 < 8) {
                for (int i = col + 1, j = row - 1; i < 8 && j >= 0; i++, j--) {
                    List<Cell> fc = GetForbiddenCells(i, j);
                    if (fc != null && fc.Count > 0) {
                        threats.Add(board.GetPieceAt(i, j));
                        foreach (Cell c in fc) {
                            forbiddenCells.Add(c);
                        }
                    }
                }
            }

            // Search down
            if (row - 1 >= 0) {
                for (int j = row - 1; j >= 0; j--) {
                    List<Cell> fc = GetForbiddenCells(col, j);
                    if (fc != null && fc.Count > 0) {
                        threats.Add(board.GetPieceAt(col, j));
                        foreach (Cell c in fc) {
                            forbiddenCells.Add(c);
                        }
                    }
                }
            }

            // Search down-left
            if (row - 1 >= 0 && col - 1 >= 0) {
                for (int i = col - 1, j = row - 1; i >= 0 && j >= 0; i--, j--) {
                    List<Cell> fc = GetForbiddenCells(i, j);
                    if (fc != null && fc.Count > 0) {
                        threats.Add(board.GetPieceAt(i, j));
                        foreach (Cell c in fc) {
                            forbiddenCells.Add(c);
                        }
                    }
                }
            }

            // Search left
            if (col - 1 >= 0) {
                for (int i = col - 1; i >= 0; i--) {
                    List<Cell> fc = GetForbiddenCells(i, row);
                    if (fc != null && fc.Count > 0) {
                        threats.Add(board.GetPieceAt(i, row));
                        foreach (Cell c in fc) {
                            forbiddenCells.Add(c);
                        }
                    }
                }
            }



            // Search up-left
            if (row + 1 < 8 && col - 1 >= 0) {
                for (int i = col - 1, j = row + 1; i >= 0 && j < 8; i--, j++) {
                    List<Cell> fc = GetForbiddenCells(i, j);
                    if (fc != null && fc.Count > 0) {
                        threats.Add(board.GetPieceAt(i, j));
                        foreach (Cell c in fc) {
                            forbiddenCells.Add(c);
                        }
                    }
                }
            }
            #endregion
            #region Find threats in all Knight's directions
            // up + diagonal left
            if (row + 1 < 8 && row + 2 < 8 && col - 1 >= 0) {
                Piece p = board.GetPieceAt(col - 1, row + 2);
                if (p != null && !IsOneOfMine(p) && p.IsKnight()) {
                    threats.Add(p);
                    forbiddenCells.Add(board.GetCell(p));
                }
            }

            // up + diagonal right
            if (row + 1 < 8 && row + 2 < 8 && col + 1 < 8) {
                Piece p = board.GetPieceAt(col + 1, row + 2);
                if (p != null && !IsOneOfMine(p) && p.IsKnight()) {
                    threats.Add(p);
                    forbiddenCells.Add(board.GetCell(p));
                }
            }

            // left + diagonal left
            if (col - 1 >= 0 && col - 2 >= 0 && row - 1 >= 0) {
                Piece p = board.GetPieceAt(col - 2, row - 1);
                if (p != null && !IsOneOfMine(p) && p.IsKnight()) {
                    threats.Add(p);
                    forbiddenCells.Add(board.GetCell(p));
                }
            }

            // left + diagonal right
            if (col - 1 >= 0 && col - 2 >= 0 && row + 1 < 8) {
                Piece p = board.GetPieceAt(col - 2, row + 1);
                if (p != null && !IsOneOfMine(p) && p.IsKnight()) {
                    threats.Add(p);
                    forbiddenCells.Add(board.GetCell(p));
                }
            }

            // down + diagonal left
            if (row - 1 >= 0 && col - 1 >= 0 && row - 2 >= 0) {
                Piece p = board.GetPieceAt(col - 1, row - 2);
                if (p != null && !IsOneOfMine(p) && p.IsKnight()) {
                    threats.Add(p);
                    forbiddenCells.Add(board.GetCell(p));
                }
            }

            // down + diagonal right
            if (row - 1 >= 0 && col + 1 < 8 && row - 2 >= 0) {
                Piece p = board.GetPieceAt(col + 1, row - 2);
                if (p != null && !IsOneOfMine(p) && p.IsKnight()) {
                    threats.Add(p);
                    forbiddenCells.Add(board.GetCell(p));
                }
            }

            // right + diagonal left
            if (col + 1 < 8 && col + 2 < 8 && row + 1 < 8) {
                Piece p = board.GetPieceAt(col + 2, row + 1);
                if (p != null && !IsOneOfMine(p) && p.IsKnight()) {
                    threats.Add(p);
                    forbiddenCells.Add(board.GetCell(p));
                }
            }

            // right + diagonal right
            if (col + 1 < 8 && col + 2 < 8 && row - 1 >= 0) {
                Piece p = board.GetPieceAt(col + 2, row - 1);
                if (p != null && !IsOneOfMine(p) && p.IsKnight()) {
                    threats.Add(p);
                    forbiddenCells.Add(board.GetCell(p));
                }
            }
            #endregion
        }

        private List<Cell> GetForbiddenCells(int pCol, int pRow) {
            Piece p = board.GetPieceAt(pCol, pRow);
            List<Cell> fc = new List<Cell>();

            // There's a piece and it's an opponent
            if (p != null && !IsOneOfMine(p)) {

                // Get all his available cells
                List<Cell> availableCells = p.GetAvailableCells();
                bool kingIsAccessible = false;

                // Iterate through those to check if our King's cell
                // is accessible. If it does, then this Piece is a Threat
                // for our King
                foreach (Cell c in availableCells) {
                    if (c.col == col && c.row == row) {
                        kingIsAccessible = true;
                        break;
                    }
                }

                if (kingIsAccessible) {
                    foreach (Cell c in availableCells) {
                        fc.Add(c);
                    }
                }
            }

            return fc;
        }

        public bool IsThreatened() {
            return threats.Count > 0;
        }

        public override List<Cell> GetAvailableCells() {
            List<Cell> cells = new List<Cell>();

            /**
             * We need to check if there is any piece threating the King
             */
            // Launch Raycasts in every direction and check if the found piece can eat ?
            //   if launch raycasts, need to add specific check for Knights
            // Or
            // From all opponent's pieces, try to eat the king ?


            /**
             * We need to search in every direction possible
             * at one cell distance
             */
            // Search up
            if (row + 1 < 8) {
                if (IsCellAvailable(col, row + 1)) {
                    cells.Add(board.GetCell(col, row + 1));
                }
            }

            // Search up-left
            if (row + 1 < 8 && col - 1 >= 0) {
                if (IsCellAvailable(col - 1, row + 1)) {
                    cells.Add(board.GetCell(col - 1, row + 1));
                }
            }

            // Search up-right
            if (row + 1 < 8 && col + 1 < 8) {
                if (IsCellAvailable(col + 1, row + 1)) {
                    cells.Add(board.GetCell(col + 1, row + 1));
                }
            }

            // Search down
            if (row - 1 >= 0) {
                if (IsCellAvailable(col, row - 1)) {
                    cells.Add(board.GetCell(col, row - 1));
                }
            }

            // Search down-left
            if (row - 1 >= 0 && col - 1 >= 0) {
                if (IsCellAvailable(col - 1, row - 1)) {
                    cells.Add(board.GetCell(col - 1, row - 1));
                }
            }

            // Search down-right
            if (row - 1 >= 0 && col + 1 < 8) {
                if (IsCellAvailable(col + 1, row - 1)) {
                    cells.Add(board.GetCell(col + 1, row - 1));
                }
            }

            // Search left
            if (col - 1 >= 0) {
                if (IsCellAvailable(col - 1, row)) {
                    cells.Add(board.GetCell(col - 1, row));
                }
            }

            // search right
            if (col + 1 < 8) {
                if (IsCellAvailable(col + 1, row)) {
                    cells.Add(board.GetCell(col + 1, row));
                }
            }

            return cells;
        }
        
        protected bool IsCellAvailable(int col, int row) {
            Piece p = board.GetPieceAt(col, row);
            if (p == null) {
                // The cell is unoccupied, we can add it
                return true;
            } else {
                if (!IsOneOfMine(p)) {
                    // The cell is occupied by an opponent piece,
                    // we can add it to the available moves
                    return true;
                }
            }
            return false;
        }
    }
}
