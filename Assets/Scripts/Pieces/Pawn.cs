using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TH {
    public class Pawn : Piece {
        public bool firstMove = true;

        private void Awake() {
            // If we need to do something in Awake, do not forget to call the base.Awake()
            base.Awake();
        }

        public override List<Cell> GetAvailableCells() {
            List<Cell> cells = new List<Cell>();

            // Get moves
            List<Cell> moves = GetAvailableCellsToMove();
            if (moves != null && moves.Count > 0) {
                foreach (Cell move in moves) {
                    cells.Add(move);
                }
            }

            // Get eatable pieces
            // if an opponent piece is diagonal forward, the pawn can eat it
            List<Cell> eats = GetAvailableCellsToEat();
            if (eats != null && eats.Count > 0) {
                foreach (Cell eat in eats) {
                    cells.Add(eat);
                }
            }

            return cells;
        }

        /***
             * We need to test the two diagonals
             * isLight == true
             *              [  P  ]
             *       [  p1 ][     ][  p2 ]
             *       
             * isLight == false
             *       [  p1 ][     ][  p2 ]
             *              [  P  ]
             */
        private List<Cell> GetAvailableCellsToEat() {
            List<Cell> cells = new List<Cell>();
            Cell cell = null;

            // TODO: we need to fix the origin to handle the case where we have two pieces in front of a pawn. 
            // rn, he does not detect the one in diagonal.

            // Test the diagonal left
            Vector3 direction = isLight ? Vector3.down : Vector3.up;
            direction.x = -1;
            float distance = 25; // We need a longer distance as we search in diagonal
            direction *= distance;

            cell = IsThereAnOpponentPieceInDiagonalAt(direction, distance);
            if (cell != null) {
                cells.Add(cell);
            }

            // Test the diagonal right
            direction = isLight ? Vector3.down : Vector3.up;
            direction.x = 1;
            direction *= distance;

            cell = IsThereAnOpponentPieceInDiagonalAt(direction, distance);
            if (cell != null) {
                cells.Add(cell);
            }

            return cells;
        }

        private Cell IsThereAnOpponentPieceInDiagonalAt(Vector3 direction, float distance) {
            Vector3 origin = transform.position;
            // Position ourself on the edge of the cell to avoid hitting ourself
            if (isLight) {
                origin.y -= 50;
            } else {
                origin.y += 50;
            }

            // Position ourself at the edge of the origin cell
            if (direction.x < 0) {
                // We are searching for the diagonal left
                origin.x -= 50;
            } else if (direction.x > 0) {
                // We are searching for the diagonal right
                origin.x += 50;
            }

            RaycastHit2D hit;

            /*if (!isLight) {
                Debug.DrawRay(origin, direction, Color.red, 10000f);
            }*/

            // Test the available moves
            hit = Physics2D.Raycast(origin, direction, distance);

            // Something's on the way, check where it is located to determine
            // the number of available cells
            // Check the hit.distance is gt 50 to avoid the piece in front of us and be sure that we check
            // the one in diagonal
            if (hit.collider != null && hit.collider.CompareTag("Piece")) {
                Piece piece = board.GetPieceAtScreenPos(hit.point);
                Debug.Log($"Hit {piece.role} located at {piece.col}{piece.row}");

                if (piece.isLight == isLight) {
                    // The two pieces are of the same color, then we have nothing to add
                } else {
                    if (isLight) {
                        return board.GetCell(piece.col, piece.row);
                    } else {
                        return board.GetCell(piece.col, piece.row);
                    }
                }
            } else {
                Debug.Log("Hit nothing or something but not a piece");
            }
            return null;
        }

        private List<Cell> GetAvailableCellsToMove() {
            List<Cell> cells = new List<Cell>(); 
            
            Vector3 origin = transform.position;
            // Position ourself on the edge of the cell to avoid hitting ourself
            if (isLight) {
                origin.y -= 50;
            } else {
                origin.y += 50;
            }

            Vector3 direction = isLight ? Vector3.down : Vector3.up;
            float distance = firstMove ? 150 : 50;
            direction *= distance;

            RaycastHit2D hit;

            // Test the available moves
            hit = Physics2D.Raycast(origin, direction, distance);

            // Something's on the way, check where it is located to determine
            // the number of available cells
            if (hit.collider != null && hit.collider.CompareTag("Piece")) {
                Piece piece = board.GetPieceAtScreenPos(hit.point);

                Debug.Log($"Hit {piece.role} located at {piece.col}{piece.row}");

                /* Distance to the hit will be :
                 *   < 50 if the piece hit is sit one cell next to our
                 *   > 50 if the piece hit is sit two cells next to our
                 *   If it's the first move, the pawn can move two cells,
                 *   then we have to check
                 */
                if (firstMove && hit.distance > 50) {
                    if (isLight) {
                        Cell oneCellDown = board.GetOneCellDown(col, row);
                        if (oneCellDown != null) {
                            cells.Add(oneCellDown);
                        }
                    } else {
                        Cell oneCellUp = board.GetOneCellUp(col, row);
                        if (oneCellUp != null) {
                            cells.Add(oneCellUp);
                        }
                    }
                } else {
                    // No empty cell to add
                }

            } else {
                // Nothing's hit, clear to add all available cells
                Debug.Log("Hit nothing or something but not a piece");
                if (isLight) {
                    Cell oneCellDown = board.GetOneCellDown(col, row);
                    if (oneCellDown != null) {
                        cells.Add(oneCellDown);
                    }
                    if (firstMove) {
                        Cell twoCellsDown = board.GetOneCellDown(oneCellDown.col, oneCellDown.row);
                        if (twoCellsDown != null) {
                            cells.Add(twoCellsDown);
                        }
                    }
                } else {
                    Cell oneCellUp = board.GetOneCellUp(col, row);
                    if (oneCellUp != null) {
                        cells.Add(oneCellUp);
                    }
                    if (firstMove) {
                        Cell twoCellsUp = board.GetOneCellUp(oneCellUp.col, oneCellUp.row);
                        if (twoCellsUp != null) {
                            cells.Add(twoCellsUp);
                        }
                    }
                }
            }
            return cells;
        }

        public override void Move(int destinationCol, int destinationRow) {
            firstMove = false;
            base.Move(destinationCol, destinationRow);
        }
    }
}
